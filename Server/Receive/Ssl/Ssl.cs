using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace server.receive
{
    public sealed class SslShell : Controller.LocalField<Setting.Ssl>
    {
        public const string NAME = "SslShell";

        private const uint NUMBER_OF_RESTART_ATTEMPTS = 15;
        private uint _currentAttempts = 0;

        public struct BUS
        {
            public struct Impuls
            {
                public const string RESTART = NAME + ":Restart";
                public const string START = NAME + ":Start";
            }
        }

        IInput<int, string> i_systemLogger;

        void Construction()
        {
            send_message<int, string>
                (ref i_systemLogger, Logger.Type.SYSTEM);

            listen_impuls(BUS.Impuls.RESTART)
                .output_to((info) =>
                {
                    if (StateInformation.IsDestroy == false)
                    {
                        if ((_currentAttempts++) <= NUMBER_OF_RESTART_ATTEMPTS)
                        {
                            SystemLogger("SLLSocket запросил перезапуск" + 
                                $"[{_currentAttempts}/{NUMBER_OF_RESTART_ATTEMPTS}].");

                            if (try_obj<Ssl>(Ssl.NAME, out Ssl obj))
                            {
                                obj.destroy();

                                obj<Ssl>(Ssl.NAME, Field);
                            }
                            else throw new Exception("");
                        }
                        else
                        {
                            SystemLogger("SLLSocket запросил перезапуск, но " + 
                                "количесво возможных попыток подошло к концу.");
                            
                            destroy();
                        }
                    }
                },
                Header.Events.SYSTEM);

            listen_impuls(BUS.Impuls.START)
                .output_to((info) =>
                {
                    if (StateInformation.IsDestroy == false)
                    {
                        _currentAttempts = 0;

                        SystemLogger("SLLSocket отчитался о начале своей работы.");
                    }
                },
                Header.Events.SYSTEM);
        }

        void Start()
        {
            SystemLogger("Start");

            obj<Ssl>(Ssl.NAME, Field);
        }

        void Stop()
        {
            SystemLogger("Stop");
        }

        void SystemLogger(string info)
        {
            if (StateInformation.IsCallConstruction)
                i_systemLogger.To(Logger.INFO, $"{NAME}:{info}");
        }

        private sealed class Ssl : Controller.Board.LocalField<Setting.Ssl>
        {
            public const string NAME = "Ssl";

            IInput<int, string> i_systemLogger;

            IInput i_start, i_restart;

            SslSocket _sslListen;

            void Construction()
            {
                _sslListen = new SslSocket(Field.Address, Field.Port, Destroy);

                send_message<int, string>(ref i_systemLogger,
                    Logger.Type.SYSTEM);

                send_message(ref _sslListen.I_sendNewClient,
                    ClientsManager.BUS.Message.LISTEN_CONNECT_CLIENTS);

                send_impuls(ref i_start, SslShell.BUS.Impuls.START);
                send_impuls(ref i_restart, SslShell.BUS.Impuls.RESTART);

                add_event(Header.Events.RECEIVE_NEW_CONNECT, 100, _sslListen.Update);
            }

            void Destroy(string info)
            {
                SystemLogger(info);

                i_restart.To();
            }

            void Start()
            {
                SystemLogger("Start");

                i_start.To();

                if (_sslListen.Start())
                {
                    SystemLogger("Listen access.");
                }
                else SystemLogger("Listen error");
            }

            void Stop()
            {
                if (StateInformation.IsCallConfigurate)
                {
                    if (_sslListen.Stop())
                    {
                        SystemLogger("Stop access.");
                    }
                    else SystemLogger("Stop error.");
                }
            }

            void Destroying()
            {
                _sslListen.IsRunning = false;
            }

            void SystemLogger(string info)
            {
                if (StateInformation.IsCallConstruction)
                    i_systemLogger.To(Logger.INFO, $"{NAME}:{info}");
            }

            private sealed class SslSocket : TcpListener
            {
                private readonly Action<string> _destroy;

                public bool IsRunning = false;

                public IInput<Socket> I_sendNewClient;

                public SslSocket(string localaddr, int port, Action<string> destroy) 
                    : base(IPAddress.Parse(localaddr), port)
                { 
                    _destroy = destroy; 
                }

                public void Update()
                {
                    if (IsRunning)
                    {
                        try
                        {
                            if (Pending())
                            {
                                do
                                {
                                    I_sendNewClient.To(AcceptSocket());
                                }
                                while (Pending());
                            }
                        }
                        catch (Exception ex)
                        {
                            IsRunning = false;

                            _destroy.Invoke(ex.ToString());
                        }
                    }
                }

                public new bool Start()
                {
                    try 
                    {
                        base.Start();

                        IsRunning = true;

                        return true;
                    }
                    catch(Exception ex)
                    {
                        _destroy(ex.ToString());

                        return false;
                    }
                }

                public new bool Stop()
                {
                    try 
                    {
                        base.Stop();

                        return true;
                    }
                    catch(Exception ex)
                    {
                        _destroy(ex.ToString());

                        return false;
                    }
                }
            }
        }
    }
}
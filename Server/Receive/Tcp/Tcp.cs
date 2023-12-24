using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace server.receive
{
    public sealed class TcpShell : Controller.LocalField<Setting.Tcp>
    {
        public const string NAME = "TcpShell";

        const uint NUMBER_OF_RESTART_ATTEMPTS = 15;
        uint _currentAttempts = 0;

        public struct BUS
        {
            public struct Impuls
            {
                public const string RESTART = NAME + ":Restart";
                public const string START = NAME + ":Start";
            }

            public struct Echo
            {
                public const string SUBSCRIBE = NAME + ":Subscribe";
                public const string UNSUBSCRIBE = NAME + ":Unsubscribe";
            }
        }

        readonly Dictionary<int, client.ConnectController.ITcpConnectionReceive> _clients
            = new();

        IInput<int, string> i_systemLogger;


        void Construction()
        {
            send_message<int, string>
                (ref i_systemLogger, Logger.Type.SYSTEM);

            listen_echo_2_1<int, client.ConnectController.ITcpConnectionReceive, bool>
                (BUS.Echo.SUBSCRIBE)
                    .output_to((key, newClient, @return) =>
                    {
                        if (_clients.TryGetValue(key,
                            out client.ConnectController.ITcpConnectionReceive client))
                        {
                            SystemLoggerInfo
                                ($"Неудалось подписать клиента {@return.GetKey()}, " +
                                $"так как по ключу[{key}], уже подписан {client.GetKey()}.");

                            @return.To(false);
                        }
                        else
                        {
                            SystemLoggerInfo
                                ($"Клиент {@return.GetKey()} подписался по ключу {key}.");

                            _clients.Add(key, newClient);

                            @return.To(true);
                        }
                    },
                    Header.Events.RECEIVE_NEW_CONNECT);

            listen_echo_1_1<int, bool>(BUS.Echo.UNSUBSCRIBE)
                .output_to((key, @return) =>
                {
                    if (_clients.Remove(key))
                    {
                        SystemLoggerInfo
                            ($"Клиент {@return.GetKey()} подписался по ключу {key}.");

                        @return.To(true);
                    }
                    else
                    {
                        SystemLoggerInfo
                            ($"Клиент {@return.GetKey()} не подписан по ключу {key}.");

                        @return.To(false);
                    }
                },
                Header.Events.RECEIVE_NEW_CONNECT);

            listen_impuls(BUS.Impuls.RESTART)
                .output_to((info) =>
                {
                    if (StateInformation.IsDestroy == false)
                    {
                        if ((_currentAttempts++) <= NUMBER_OF_RESTART_ATTEMPTS)
                        {
                            SystemLoggerInfo("TcpSocket запросил перезапуск" +
                                $"[{_currentAttempts}/{NUMBER_OF_RESTART_ATTEMPTS}].");

                            if (try_obj<Tcp>(Tcp.NAME, out Tcp obj))
                            {
                                obj.destroy();

                                obj<Tcp>(Tcp.NAME, Field);
                            }
                            else throw new Exception("");
                        }
                        else
                        {
                            SystemLoggerInfo("TcpSocket запросил перезапуск, но " +
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

                        SystemLoggerInfo("TcpSocket отчитался о начале своей работы.");
                    }
                },
                Header.Events.SYSTEM);
        }

        void Start()
        {
            SystemLoggerInfo("Start");

            obj<Tcp>(Tcp.NAME, Field);
        }

        void Stop()
        {
            SystemLoggerInfo("Stop");
        }

        void SystemLoggerInfo(string info)
        {
            if (StateInformation.IsCallConstruction)
                i_systemLogger.To(Logger.INFO, $"{NAME}:{info}");
        }

        void SystemLoggerError(string info)
        {
            if (StateInformation.IsCallConstruction)
                i_systemLogger.To(Logger.ERROR, $"{NAME}:{info}");
        }

        void Destroyed() 
        {
            if (_clients.Count > 0)
            {
                string clientKeyList = ""; int index = 0;
                foreach(var client in _clients.Values)
                    clientKeyList += $"\n{index}){client.GetKey()}.";

                SystemLoggerError($"Отписались клиенты:{clientKeyList}");
            }
        }

        private sealed class Tcp : Controller.Board.LocalField<Setting.Tcp>
        {
            public const string NAME = "Tcp";

            IInput<int, string> i_systemLogger;

            IInput i_start, i_restart;

            TcpSocket _tcpListen;

            void Construction()
            {
                _tcpListen = new TcpSocket(Field.Address, Field.Port, Destroy);

                send_message<int, string>(ref i_systemLogger,
                    Logger.Type.SYSTEM);

                send_message(ref _tcpListen.I_sendNewClient,
                    ClientsManager.BUS.Message.LISTEN_CONNECT_CLIENTS);

                send_impuls(ref i_start, TcpShell.BUS.Impuls.START);
                send_impuls(ref i_restart, TcpShell.BUS.Impuls.RESTART);

                add_event(Header.Events.RECEIVE_NEW_CONNECT, 100, _tcpListen.Update);
            }

            void Destroy(string info)
            {
                SystemLoggerInfo(info);

                i_restart.To();
            }

            void Start()
            {
                SystemLoggerInfo("Start");

                i_start.To();

                if (_tcpListen.Start())
                {
                    SystemLoggerInfo("Listen access.");
                }
                else SystemLoggerInfo("Listen error");
            }

            void Stop()
            {
                if (StateInformation.IsCallConfigurate)
                {
                    if (_tcpListen.Stop())
                    {
                        SystemLoggerInfo("Stop access.");
                    }
                    else SystemLoggerInfo("Stop error.");
                }
            }

            void Destroying()
            {
                _tcpListen.IsRunning = false;
            }

            void SystemLoggerInfo(string info)
            {
                if (StateInformation.IsCallConstruction)
                    i_systemLogger.To(Logger.INFO, $"{NAME}:{info}");
            }

            private sealed class TcpSocket : TcpListener
            {
                private readonly Action<string> _destroy;

                public bool IsRunning = false;

                public IInput<Socket> I_sendNewClient;

                public TcpSocket(string localaddr, int port, Action<string> destroy)
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
                    catch (Exception ex)
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
                    catch (Exception ex)
                    {
                        _destroy(ex.ToString());

                        return false;
                    }
                }
            }
        }
    }
}
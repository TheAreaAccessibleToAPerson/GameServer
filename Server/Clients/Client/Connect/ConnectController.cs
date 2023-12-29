using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace server.client
{
    public abstract class ConnectController : Controller.Board.LocalField<System.Net.Sockets.Socket>,
        ConnectController.ITcpConnectionReceive
    {
        private const string NAME = "ConnectController";

        protected IInput<int, string> I_clientLogger;

        protected IInput<connect.Data> I_verification;

        protected IInput<string, ConnectedInformation> I_creatingConnectedClient;

        protected IInput<string, ConnectController.ITcpConnectionReceive>
            I_subscribeToReceiveTcpConnection;

        protected IInput<string> I_unsubscribeToReceiveTcpConnection;

        protected readonly connect.State State = new();

        protected connect.Data Data = new(4, 16, 4, 16);

        protected void Process()
        {
            lock (State.Locker)
            {
                if (State.IsDestroy()) return;

                if (State.HasReceiveLoginAndPassword())
                {
                    if (State.SetVerification(out string info))
                    {
                        LoggerInfo(info);

                        I_verification.To(Data);
                    }
                    else LoggerWarning(info);
                }
                else if (State.HasVerification())
                {
                    if (Data.IsSuccessVerification())
                    {
                        if (TryIncrementEvent())
                        {
                            if (State.SetBeginSubscribeToReceiveTcpConnection(out string info))
                            {
                                LoggerInfo(info);

                                I_subscribeToReceiveTcpConnection.To
                                    (Data.Ssl.Address, this);
                            }
                            else LoggerWarning(info);
                        }
                        else LoggerWarning
                            ("Неудалось начать подписку на прослушку Tcp соединения," +
                                " обьект приступил к уничтожению.");
                    }
                }
                else if (State.HasBeginSubscribeToReceiveTcpConnection())
                {
                    if (State.SetSendingAccessVerificationAndRequestTcpConnection(out string info))
                    {
                        LoggerInfo(info);

                        invoke_event(() =>
                        {
                            Data.Ssl.Send(new byte[NetWork.Server.AccessVerification.LENGTH]
                            {
                            NetWork.Server.AccessVerification.LENGTH >> 8,
                            NetWork.Server.AccessVerification.LENGTH,

                            NetWork.Server.AccessVerification.TYPE >> 8,
                            NetWork.Server.AccessVerification.TYPE
                            });
                        },
                        Header.Events.SSL_SEND);

                        invoke_event(() =>
                        {
                            if (State.HasSendingAccessVerificationAndRequestTcpConnect())
                                Destroy("Истекло время ожидания Tcp соединения.");
                        },
                        2000, Header.Events.SYSTEM);
                    }
                    else LoggerWarning(info);
                }
                else if (State.HasSendingAccessVerificationAndRequestTcpConnect())
                {
                    if (TryIncrementEvent())
                    {
                        if (State.SetUnsubscribeRequestTcpConnect(out string info))
                        {
                            LoggerInfo(info);

                            I_unsubscribeToReceiveTcpConnection.To(Data.Ssl.Address);
                        }
                        else LoggerWarning(info);
                    }
                    else LoggerWarning
                        ("Неудалось начать отподписку прослушки Tcp соединения," +
                            " обьект приступил к уничтожению.");
                }
                else if (State.HasUnsubscribeReqestTcpConnect())
                {
                    if (State.SetEndConnectionClinet(out string info))
                    {
                        LoggerInfo(info);

                        destroy();
                    }
                    else Destroy(info);
                }
                else LoggerWarning(State.StepError());
            }
        }

        protected void Receive(byte[] message, int length)
        {
            int type = message[NetWork.TYPE_1BYTE_INDEX] << 8 ^
                message[NetWork.TYPE_2BYTE_INDEX];

            if (type == NetWork.Client.SendingLoginAndPassword.TYPE)
            {
                if (length == NetWork.Client.SendingLoginAndPassword.LENGTH)
                {
                    string login = "";
                    {
                        for (int i = NetWork.Client.SendingLoginAndPassword.LOGIN_START_INDEX;
                            i < NetWork.Client.SendingLoginAndPassword.LOGIN_END_INDEX; i++)
                        {
                            char c = Convert.ToChar(message[i]);

                            if (c == '\0') break;

                            login += c;
                        }
                    }

                    if (Data.SetLogin(login, out string loginError))
                    {
                        string password = "";
                        {
                            for (int i = NetWork.Client.SendingLoginAndPassword.PASSWORD_START_INDEX;
                                i < NetWork.Client.SendingLoginAndPassword.PASSWORD_END_INDEX; i++)
                            {
                                char c = Convert.ToChar(message[i]);

                                if (c == '\0') break;

                                password += c;
                            }
                        }

                        if (Data.SetPassword(password, out string passwordError))
                        {
                            Data.I_continueProcess.To();
                        }
                        else Destroy(passwordError);
                    }
                    else Destroy(loginError);
                }
                else Destroy("Неверная длина для сообщения в котором должен придти логин и пароль.");
            }
            else Destroy("Неизвестный тип сообщения.");
        }

        protected void Destroy(string info)
        {
            LoggerInfo(info);

            destroy();
        }

        protected void LoggerInfo(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_clientLogger.To(Logger.INFO, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerError(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_clientLogger.To(Logger.ERROR, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerWarning(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_clientLogger.To(Logger.WARNING, $"{NAME}:{GetKey()}[{info}]");
        }

        void ITcpConnectionReceive.Send(System.Net.Sockets.Socket sock)
        {
            if (Data.Tcp == null)
            {
                Data.Tcp = new Socket(2048, Destroy, sock);

                invoke_event(Process, Header.Events.SYSTEM);
            }
            else LoggerWarning("Повторно пришол tcp socket.");
        }

        public interface ITcpConnectionReceive
        {
            void Send(System.Net.Sockets.Socket sock);
            string GetKey();
        }
    }

}
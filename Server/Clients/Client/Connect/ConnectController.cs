using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace server.client
{
    public abstract class ConnectController : Controller.Board.LocalField<System.Net.Sockets.Socket>,
        ConnectController.ITcpConnectionReceive
    {
        private const string NAME = "ConnectController";

        private const int _connectID = 155;

        protected IInput<int, string> I_clientLogger;

        protected IInput<connect.Data> I_verification;

        protected IInput<string, ConnectedInformation> I_creatingConnectedClient;

        protected IInput<string, ConnectController.ITcpConnectionReceive>
            I_subscribeToReceiveTcpConnection;

        protected IInput<string> I_unsubscribeToReceiveTcpConnection;

        protected readonly connect.State State = new();
        protected connect.Data Data;

        protected void Process()
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
                    if (State.SetBeginSubscribeToReceiveTcpConnection(out string info))
                    {
                        LoggerInfo(info);

                        if (TryIncrementEvent())
                        {
                            I_subscribeToReceiveTcpConnection.To
                                (Data.Ssl.Address, this);
                        }
                        else LoggerWarning
                            ("Неудалось начать подписку на прослушку Tcp соединения," +
                                " обьект приступил к уничтожению.");
                    }
                    else LoggerWarning(info);
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

                            NetWork.Server.AccessVerification.TYPE
                        });
                    },
                    Header.Events.SSL_SEND);

                    invoke_event(() => 
                    {
                        if (State.HasSendingAccessVerificationAndRequestTcpConnect())
                            Destroy("Истекло время ожидания Tcp соединения.");
                    }, 
                    2000, Header.Events.SSL_SEND);
                }
                else LoggerWarning(info);
            }
            else if (State.HasSendingAccessVerificationAndRequestTcpConnect())
            {
                if (State.SetEndConnectionClient(out string info))
                {
                    LoggerInfo(info);

                    I_creatingConnectedClient.To(Data.Login, new ConnectedInformation()
                    {
                        ConnectedID = _connectID,
                        BDIndex = Data.Index,

                        Tcp = Data.Tcp,
                        Ssl = Data.Ssl,
                    });
                }
                else LoggerWarning(info);
            }
            else LoggerWarning(State.StepError());
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
                            i < NetWork.Client.SendingLoginAndPassword.LOGIN_LENGTH; i++)
                        { login += Convert.ToChar(message[i]); }
                    }

                    if (Data.SetLogin(login, out string loginError))
                    {
                        string password = "";
                        {
                            for (int i = NetWork.Client.SendingLoginAndPassword.PASSWORD_START_INDEX;
                                i < NetWork.Client.SendingLoginAndPassword.PASSWORD_LENGTH; i++)
                            { password += Convert.ToChar(message[i]); }
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

        void ITcpConnectionReceive.Send(Socket sock)
        {
            if (Data.Tcp == null)
            {
                Data.Tcp = sock;
            }
            else LoggerWarning("Повторно пришол tcp socket.");
        }

        public interface ITcpConnectionReceive
        {
            void Send(Socket sock);
            string GetKey();
        }
    }

}
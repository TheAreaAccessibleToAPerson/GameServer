using Butterfly;

namespace server.client
{
    public sealed class Connect : ConnectController
    {
        void Construction()
        {
            send_message(ref I_clientLogger, Logger.Type.CLIENT);

            Data.Ssl = new Socket(2048, Destroy, Field);
            {
                input_to(ref Data.I_continueProcess, Header.Events.SYSTEM, Process);
                input_to(ref Data.Ssl.I_output, Header.Events.SYSTEM, Receive);

                add_event(Header.Events.SSL_RECEIVE, Data.Ssl.Receive);
            }

            send_echo_2_1<string, ConnectController.ITcpConnectionReceive, bool>
                (ref I_subscribeToReceiveTcpConnection, receive.TcpShell.BUS.Echo.SUBSCRIBE)
                    .output_to((result) =>
                    {
                        if (result)
                        {
                            LoggerInfo("Успешно подписались и теперь ожидаете Tcp подключения.");

                            Process();
                        }
                        else
                        {
                            LoggerError("Неудалось подписаться на ожидание Tcp подключения.");

                            destroy();
                        }

                        DecrementEvent();
                    },
                    Header.Events.SYSTEM);

            send_echo_1_1<string, bool>
                (ref I_unsubscribeToReceiveTcpConnection, receive.TcpShell.BUS.Echo.UNSUBSCRIBE)
                    .output_to((result) =>
                    {
                        if (result)
                        {
                            LoggerInfo("Вы успеншно отписались от прослушки Tcp подключения.");

                            Process();
                        }
                        else
                        {
                            LoggerError("Неудалось отписаться от прослушки входящего Tcp соединения.");

                            destroy();
                        }

                        DecrementEvent();
                    },
                    Header.Events.SYSTEM);

            send_echo_2_0(ref I_creatingConnectedClient, ClientsManager.BUS.Echo.CREATING_CLIENT)
                .output_to(() =>
                {
                    LoggerInfo("Клиент подключился.");

                    destroy();
                });

            safe_send_message(ref I_verification,
                BD.AuthorizationShell.BUS.Message.VERIFICATION);
        }

        void Start()
        {
            Data.Ssl.Start();

            invoke_event(() =>
            {
                lock (State.Locker)
                {
                    if (State.HasReceiveLoginAndPassword())
                    {
                        SystemInformation($"CURRENT_STATE:" + State.CurrentState);
                        Destroy("Логин и пароль так и не пришел от клиента.");
                    }
                }
            },
            5000, Header.Events.SYSTEM);

            LoggerInfo("Start");
        }

        void Destruction()
        {
            LoggerInfo("Destruction");

            lock (State.Locker)
            {
                State.Destroy();

                if (State.HasBeginSubscribeToReceiveTcpConnection() ||
                    State.HasSendingAccessVerificationAndRequestTcpConnect())
                {
                    LoggerInfo("Отписка от ожидаения Tcp подключения была запущена в Destrction().");

                    I_unsubscribeToReceiveTcpConnection.To(Data.Ssl.Address);
                }
            }
        }

        void Stop()
        {
            LoggerInfo("Stop");

            Data.Ssl.Stop();
        }

        void Destroyed()
        {
            LoggerInfo($"Destroyed. CurrentState:{State.CurrentState}.");

            lock (State.Locker)
            {
                if (State.HasEndClientConnection())
                {
                    I_creatingConnectedClient.To(Data.Login, new ConnectedInformation()
                    {
                        BDIndex = Data.Index,

                        Tcp = Data.Tcp,
                    });
                }
                else
                {
                    if (State.HasUnsubscribeReqestTcpConnect())
                        Data.Tcp.Stop();
                }
            }
        }
    }
}
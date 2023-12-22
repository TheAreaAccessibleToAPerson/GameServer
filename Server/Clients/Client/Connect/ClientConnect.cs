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
            LoggerInfo("Start");

            invoke_event(() => 
            {
                if (State.HasReceiveLoginAndPassword())
                {
                    Destroy("Логин и пароль так и не пришел от клиента.");
                }
            }, 
            2000, Header.Events.SYSTEM);
        }

        void Destruction()
        {

        }

        void Stop()
        {
            LoggerInfo("Stop");
        }
    }
}
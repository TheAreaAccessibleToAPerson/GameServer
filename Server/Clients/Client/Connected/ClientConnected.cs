namespace server.client
{
    public class Connected : gameSession.Controller
    {
        void Construction()
        {
            send_message(ref I_clientLogger, Logger.Type.CLIENT);

            Field.Ssl.Destroy = Destroy;
            Field.Tcp.Destroy = Destroy;

            input_to(ref Field.Ssl.I_output, Header.Events.WORK, ReceiveSsl);
            input_to(ref Field.Tcp.I_output, Header.Events.WORK, ReceiveTcp);

            add_event(Header.Events.SSL_RECEIVE, Field.Ssl.Receive);
            add_event(Header.Events.TCP_RECEIVE, Field.Tcp.Receive);

            input_to(ref Data.Process, Header.Events.SYSTEM, Process);

            send_echo_1_2<Connected.IWorldReceive, bool, World.IClientReceive>
                (ref I_addToWorld, World.BUS.Echo.ADD)
                    .output_to((result, worldReseive) =>
                    {
                        if (result)
                        {
                            LoggerInfo($"Клиент был успешно добавлен в мир.");

                            Process();
                        }
                        else 
                        {
                            LoggerError($"Неудалось добавить клинта в мир.");

                            destroy();
                        }
                    },
                    Header.Events.SYSTEM);

            send_echo_1_1<string, bool>(ref I_removeFromWorld, World.BUS.Echo.REMOVE)
                .output_to((result) => 
                {
                    if (result)
                    {
                    }
                    else 
                    {
                    }
                },
                Header.Events.SYSTEM);
        }

        void Start() { }

        public interface IWorldReceive
        {
            public string GetNickname();
            public string GetKey();
        }
    }
}

namespace server.client
{
    public class Connected : gameSession.Controller
    {
        void Construction()
        {
            send_message(ref I_clientLogger, Logger.Type.CLIENT);

            input_to(ref I_process, Header.Events.SYSTEM, Process);


            input_to(ref Data.I_BDReceievRoom, Header.Events.SYSTEM, BDReceiveRoom);

            safe_send_message(ref I_DBLoadData, BD.Room.BUS.Message.GetRoom);

            Field.Tcp.Destroy = Destroy;
            Data.Process = I_process;

            input_to(ref Field.Tcp.I_output, Header.Events.WORK, ReceiveTcp);

            add_event(Header.Events.TCP_RECEIVE, Field.Tcp.Receive);

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

        void Start() 
        {
            LoggerInfo("Start");

            Field.Tcp.Start();

            I_process.To();
        }

        public interface IWorldReceive
        {
            public string GetNickname();
            public string GetKey();
        }
    }
}

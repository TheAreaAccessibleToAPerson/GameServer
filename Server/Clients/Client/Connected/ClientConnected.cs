namespace server.client
{
    public class Connected : gameSession.Controller
    {
        void Construction()
        {
            BDData.Index = Field.BDIndex;

            input_to(ref I_sendToClient, Field.Tcp.Send);

            send_message(ref I_clientLogger, Logger.Type.CLIENT);

            input_to(ref I_process, Header.Events.SYSTEM, Process);

            input_to(ref BDData.I_BDReceiveData, Header.Events.SYSTEM, BDReceiveData);

            safe_send_message(ref I_DBLoadData, BD.ClientData.BUS.Message.GetData);

            Field.Tcp.Destroy = Destroy;

            input_to(ref Field.Tcp.I_output, Header.Events.WORK, ReceiveTcp);

            add_event(Header.Events.TCP_RECEIVE, Field.Tcp.Receive);

            send_echo_2_1<world.room.Setting, Connected.IWorldReceive, world.room.Controller.IReceive>
                (ref I_addToWorld, World.BUS.Echo.CREATING)
                    .output_to((worldReceive) =>
                    {
                        LoggerInfo($"Комната успешно создана.");
                    });

            send_echo_1_1<string, bool>(ref I_removeFromWorld, World.BUS.Echo.REMOVE)
                .output_to((result) => 
                {
                    if (result)
                    {
                    }
                    else 
                    {
                    }
                });
        }

        void Start() 
        {
            LoggerInfo("Start");

            Field.Tcp.Start();

            I_process.To();
        }

        public interface IRoomReceive 
        {
            /// <summary>
            /// Комната создана.
            /// </summary>
            public void Creating(string roomName);
        }

        public interface IWorldReceive
        {
            /// <summary>
            /// Ник нейм клинта.
            /// </summary>
            /// <returns></returns>
            public string GetNickname();

            /// <summary>
            /// Получаем имя комнаты в которой находится клиент.
            /// </summary>
            /// <returns></returns>
            public string GetRoomName();

            /// <summary>
            /// Уникальный ключ клинта.
            /// </summary>
            /// <returns></returns>
            public string GetKey();
        }
    }
}

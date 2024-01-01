using Butterfly;

namespace server.client.gameSession
{
    public abstract class Main : Butterfly.Controller.Board.LocalField<client.ConnectedInformation>
    {
        protected IInput<byte[]> I_sendToClient;

        protected void SendCreatingRoom(string roomName)
        {
            I_sendToClient.To(new byte[]
            {
            });
        }
    }

    public abstract class Controller : Main, Connected.IWorldReceive, Connected.IRoomReceive
    {
        private const string NAME = "GameSession";

        protected BDData BDData = new();
        protected Data Data = new();

        protected IInput I_process;

        protected readonly State State = new();

        protected IInput<int, string> I_clientLogger;

        /// <summary>
        /// Загружаем данные клиента.
        /// </summary>
        protected IInput<BDData> I_DBLoadData;

        protected IInput<world.room.Setting, Connected.IWorldReceive> I_addToWorld;
        protected IInput<string> I_removeFromWorld;


        protected void ReceiveTcp(byte[] message, int length)
        {
        }

        /// <summary>
        /// Ожидаем данные клинта.
        /// </summary>
        /// <param name="result"></param>
        protected void BDReceiveData(string result)
        {
            // Имя комнаты.
            // ROOM_DEFAULT_NAME, LVL, EXP, HP, MP, SHIELD, SKILLS[LVL, KD]

            lock (State.Locker)
            {
                if (State.IsDestroy()) return;

                if (State.HasLoadingData())
                {
                    if (TryIncrementEvent())
                    {
                        if (State.SetInputToWorld(out string info))
                        {
                            LoggerInfo(info);

                            I_addToWorld.To(new world.room.Setting()
                            {
                                ClientReceive = this,
                            },
                            this);
                        }
                        else LoggerError(info);
                    }
                    else LoggerWarning("");
                }
                else throw new Exception();
            }
        }


        protected void Process()
        {
            lock (State.Locker)
            {
                if (State.HasNone())
                {
                    if (State.SetLoadingData(out string info))
                    {
                        LoggerInfo(info);

                        I_DBLoadData.To(BDData);
                    }
                    else Destroy(info);
                }
            }
        }

        void Connected.IRoomReceive.Creating(string roomName)
        {
            LoggerInfo($"Комната {roomName} создана.");

            lock (State.Locker)
            {
                if (State.HasInputToWorld())
                {
                }
                else LoggerWarning("В момент получения сообщения о создании" +
                    "комнаты обьект начал процесс уничтожения.");
            }
        }

        protected void Destroy(string info)
        {
            LoggerInfo(info);

            destroy();
        }


        string Connected.IWorldReceive.GetNickname() => Field.Nickname;

        public string GetRoomName() => "";

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
    }
}
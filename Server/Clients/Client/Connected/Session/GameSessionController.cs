using Butterfly;

namespace server.client.gameSession
{
    public abstract class Controller : Main
    {
        private const string NAME = "GameSession";

        protected BDData BDData = new();
        protected IInput I_process;
        protected readonly State State = new();
        protected IInput<int, string> I_clientLogger;

        //---------------------BD---------------------------
        /// <summary>
        /// Загружаем данные клиента.
        /// </summary>
        protected IInput<BDData> I_DBLoadData;
        //--------------------------------------------------


        //-----------------[Clinet->World]------------------
        protected IInput<world.room.Setting> I_addToWorld;
        protected IInput<string> I_removeFromWorld;
        //--------------------------------------------------

        //-----------------[ROOM->CLIENT]-------------------
        /// <summary>
        /// Комната сообщает клиенту о том что она создана.
        /// </summary>
        protected IInput<string, int, int> IRoom_creating;

        /// <summary>
        /// Команда сообщает клиенту о движении в указаную позицию.
        /// 1)Направление(поворот) 2)Позиция x 3)Позиция y
        /// 4)Время начала движения.
        /// 5)Время в миллисекундах за которое необходимо преодолеть дистанцию.
        /// </summary>
        protected IInput<int, int, int, DateTime, int> IRoom_characterMove;
        //--------------------------------------------------

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
                                StartPositionX = 167,
                                StartPositionY = 298,

                                SizeX = 1,
                                SizeY = 1,

                                Mobs = new unit.Mob[]
                                {
                                    new unit.Mob("name", 0, 1, 100, 0, 5, 5)
                                },

                                IRoom_creating = IRoom_creating,
                                IRoom_characterMove = IRoom_characterMove,
                            });
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

        protected void Destroy(string info)
        {
            LoggerInfo(info);

            destroy();
        }

        public string GetRoomName() => "";

        public string GetNickname() => "";

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
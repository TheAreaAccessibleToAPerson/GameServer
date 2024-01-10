using Butterfly;

namespace server.client.gameSession
{
    public abstract class Controller : Main
    {
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
                                    new unit.Mob("name", 
                                    0, 
                                    1, 
                                    467, 
                                    298, 
                                    5, 
                                    5)
                                },

                                ClientData = Data
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
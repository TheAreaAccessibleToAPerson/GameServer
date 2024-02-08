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
                                StartPositionX = 195,
                                StartPositionY = 195,

                                SizeX = 10,
                                SizeY = 10,

                                Mobs = new unit.Mob[]
                                {
                                    new unit.Mob(
                                    1, 
                                    0, 
                                    1, 
                                    600, 
                                    170, 
                                    210, 
                                    140,
                                    1000,
                                    100,
                                    100)
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


    }
}
using System.Net.NetworkInformation;
using Butterfly;

namespace server.client.world.room
{
    public abstract class Controller : Butterfly.Controller.Board.LocalField<Setting>,
        Controller.IReceive
    {
        public const string NAME = "Room";

        protected IInput<int, string> I_worldLogger;

        // 0 - rigth, 1 - left
        public int Direction;
        public int CurrentPositionX, CurrentPositionY;
        public int SizeX, SizeY;

        private bool IsRun = true;
        private bool IsSendRun = false;

        private bool IsAttack = false;

        System.DateTime d_localDateTime = System.DateTime.Now;

        protected unit.Mob[] Mobs;
        private int MobIndex = 0;

        protected void Update()
        {
            int deltaTime = (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

            d_localDateTime = System.DateTime.Now;

            if (IsRun)
            {
                if (IsSendRun == false)
                {
                    // Начинаем двигать персонажа до ближайшего моба.
                    // Узнаем растояние до него, вычисляем время движения в милисекундах.
                    int distance = Math.Abs
                        (CurrentPositionX + (SizeX / 2) - Mobs[MobIndex].PointLeft);

                    Field.ClientData.IRoom_characterMove.To 
                        (Direction, // Направление.
                        Mobs[MobIndex].PointLeft - (SizeX / 2), // Левый край моба - пол персонажа.
                        Field.StartPositionY); // Позиция Y у персонажа меняться не будет.

                    IsSendRun = true;
                }
            }
        }

        public string GetName() => GetKey();

        public interface IReceive
        {
            public string GetName();
        }

        protected void LoggerInfo(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_worldLogger.To(Logger.INFO, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerError(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_worldLogger.To(Logger.ERROR, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerWarning(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_worldLogger.To(Logger.WARNING, $"{NAME}:{GetKey()}[{info}]");
        }
    }
}
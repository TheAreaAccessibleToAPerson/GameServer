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

        private bool _isAttack = false;
        private int lastAttack = 0;

        protected void Update()
        {
            int delta = (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

            lastAttack += delta;

            if (_isAttack == true || lastAttack > 2000)
            {
                if (lastAttack > 100)
                {
                    //Console("ATTACK");
                    Field.ClientData.IRoom_characterDefaultAttack.To(10, 25);

                    lastAttack = 0;

                    _isAttack = true;
                }
            }


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

                    SystemInformation((Mobs[MobIndex].PointLeft - (SizeX / 2)).ToString());

                    Field.ClientData.IRoom_creatingMob.To(Mobs[MobIndex]);

                    IsSendRun = true;
                }
            }

            d_localDateTime = System.DateTime.Now;
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
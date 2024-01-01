using Butterfly;

namespace server.client.world.room
{
    public abstract class Controller : Butterfly.Controller.Board.LocalField<Setting>,
        Controller.IReceive
    {
        public const string NAME = "Room";

        protected IInput<int, string> I_worldLogger;

        protected Mob[] Mobs = new Mob[1];
        private int _currentMob = 0;

        private int _currentPosition = 0;
        private int _nextPosition = 0;

        private bool IsRun = false;
        private bool IsAttack = false;
        private bool IsBuff = false;

        protected void Update()
        {
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
using Butterfly;

namespace server.client.world 
{
    public abstract class Controller : Butterfly.Controller,
        World.IClientReceive
    {
        public const string NAME = "World";

        protected IInput<int, string> I_worldLogger;

        private readonly Dictionary<string, Connected.IWorldReceive> _clients = new();

        public bool Add(Connected.IWorldReceive client)
        {
            string name = client.GetNickname();

            if (_clients.ContainsKey(name))
            {
                LoggerError($"Клиент с именем {name} уже сущесвует и не может быть добавлен.");

                return false;
            }
            else
            {
                LoggerInfo($"Добавлен новый клиент {name}.");

                _clients.Add(name, client);

                return true;
            }
        }

        public bool Remove(string name)
        {
            if (_clients.Remove(name))
            {
                LoggerInfo($"Клиент с именем {name} был удален.");

                return true;
            }
            else 
            {
                LoggerError($"Клиента с именем {name} не сущесвует и он не может быть удален.");

                return false;
            }
        }

        void Check()
        {
            if (_clients.Count > 0)
            {
            }
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
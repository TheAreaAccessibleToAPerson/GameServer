using System.Net.Http.Headers;
using Butterfly;

namespace server.client.world 
{
    public abstract class Controller : Butterfly.Controller
    {
        public const string NAME = "World";

        protected IInput<int, string> I_worldLogger;

        private readonly Dictionary<string, room.Controller.IReceive> _rooms = new();

        public room.Controller.IReceive Creating(string key, room.Setting settings)
        {
            if (_rooms.ContainsKey(key))
            {
                LoggerError($"Конмната с ключом {key} уже сущесвует.");

                return null;
            }
            else
            {
                room.Controller.IReceive room = obj<Room>(key, settings);

                _rooms.Add(key, room);

                return room;
            }
        }

        public bool Remove(string key)
        {
            if (_rooms.Remove(key))
            {
                return true;
            }
            else 
            {
                LoggerError($"Неудалось удалить комнату {key}, так как ее не сущесувет.");

                return false;
            }
        }

        void Check()
        {
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
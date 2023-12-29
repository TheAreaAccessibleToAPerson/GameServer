using Butterfly;

namespace server.client.gameSession
{
    public abstract class Controller : Butterfly.Controller.Board.LocalField<client.ConnectedInformation>,
        Connected.IWorldReceive
    {
        private const string NAME = "GameSession";

        protected Data Data = new();
        protected IInput I_process;

        private readonly State _state = new();

        protected IInput<int, string> I_clientLogger;

        /// <summary>
        /// Загружаем данные клиента.
        /// </summary>
        protected IInput<Data> I_DBLoadData;

        protected IInput<Connected.IWorldReceive> I_addToWorld;
        protected IInput<string> I_removeFromWorld;

        protected void ReceiveTcp(byte[] message, int length)
        {
        }

        /// <summary>
        /// Прослушивает ответы из таблицы хранящей данные о
        /// текущей комнате.
        /// </summary>
        /// <param name="result"></param>
        protected void BDReceiveRoom(string result)
        {
        }

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

        protected void Process()
        {
            lock (_state.Locker)
            {
            }
        }

        protected void Destroy(string info)
        {
            LoggerInfo(info);

            destroy();
        }


        string Connected.IWorldReceive.GetNickname() => Data.Name;
    }
}
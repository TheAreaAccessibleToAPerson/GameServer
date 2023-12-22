using Butterfly;

namespace server.client.gameSession
{
    public abstract class Controller : Butterfly.Controller.Board.LocalField<client.ConnectedInformation>,
        IReceive
    {
        private const string NAME = "GameSession:";

        protected Data Data = new();

        private readonly State _state = new();

        protected IInput<int, string> I_clientLogger;

        protected void Process()
        {
        }

        protected void ReceiveTcp(byte[] message, int length)
        {
        }

        protected void ReceiveSsl(byte[] message, int length)
        {
        }

        void IReceive.Send(string message)
        {
        }

        void IReceive.Send(byte[] message)
        {
        }

        void IReceive.Send(string client, string message)
        {
        }

        void IReceive.Send(string client, byte[] message)
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

        protected void Destroy(string info)
        {
            LoggerInfo(info);

            destroy();
        }
    }
}
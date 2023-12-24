using Butterfly;

namespace server.client.gameSession
{
    public abstract class Controller : Butterfly.Controller.Board.LocalField<client.ConnectedInformation>,
        Connected.IWorldReceive
    {
        private const string NAME = "GameSession";

        protected Data Data = new();

        private readonly State _state = new();

        protected IInput<int, string> I_clientLogger;

        protected IInput<Connected.IWorldReceive> I_addToWorld;
        protected IInput<string> I_removeFromWorld;

        protected void ReceiveTcp(byte[] message, int length)
        {
        }

        protected void ReceiveSsl(byte[] message, int length)
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
                if (_state.IsDestroy)
                {
                    LoggerWarning("Невозможно продолжить процесс смены состояния." +
                        $"CurrentState:{_state.CurrentState}.");

                    return;
                }

                if (_state.HasNone())
                {
                    // Получаем данные из базы данных.
                    if (_state.SetLoadDbData(out string info))
                    {
                        LoggerInfo(info);

                        //ClientsManager.DBLoadClientData(Data);

                        invoke_event(() => 
                        {
                            if (_state.HasNone()) 
                                Destroy($"CurrentState:{_state.CurrentState}.Не были получены данные из BD.");
                        },
                        2000, Header.Events.SYSTEM);
                    }
                    else LoggerError(info);
                }
                else if (_state.HasLoadDBData()) 
                {
                    if (_state.SetAddToWorld(out string info))
                    {
                        LoggerInfo(info);

                        I_addToWorld.To(this);
                    }
                    else LoggerError(info);
                }
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
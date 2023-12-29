using Butterfly;

namespace server
{
    public sealed class Server : Controller.LocalField<Setting>
    {
        public const string NAME = "Server";

        IInput<int, string> i_systemLogger;

        void Construction()
        {
            send_message<int, string>
                (ref i_systemLogger, Logger.Type.SYSTEM);

            obj<ClientsManager>(ClientsManager.NAME);

            obj<receive.SslShell>(receive.SslShell.NAME, Field.SslSetting);
            obj<receive.TcpShell>(receive.TcpShell.NAME, Field.TcpSetting);

            obj<BD.AuthorizationShell>(BD.AuthorizationShell.NAME, Field.BDSetting);
            obj<BD.Room>(BD.Room.NAME, Field.BDSetting);
        }

        void Start() => SystemLogger("Start");

        void Stop() => SystemLogger("Stop");


        void SystemLogger(string info)
        {
            if (StateInformation.IsCallConstruction)
                i_systemLogger.To(Logger.INFO, $"{NAME}:{info}.");
        }
    }
}
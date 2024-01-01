using Butterfly;

namespace server.BD
{
    public sealed class ClientData : Controller.LocalField<Setting.BD>
    {
        public const string NAME = "Room";

        public struct BUS
        {
            public struct Message
            {
                public const string GetData = NAME + ":GetData"; 
                public const string SetData = NAME + ":SetData"; 
            }
        }

        void Construction()
        {
            extract_values<client.gameSession.BDData>
            (BUS.Message.SetData, (clientData) =>
            {
                // ... 
            });

            extract_values<client.gameSession.BDData>
            (BUS.Message.GetData, (clientData) =>
            {
                // ... 
            });
        }

        void Start() => obj<_object>(NAME);

        private sealed class _object : Controller.Board
        {
            void Construction()
            {
                safe_listen_message<client.gameSession.BDData>(BUS.Message.SetData,
                    Header.Events.DB_DATA, ClientData.NAME)
                        .output_to((clientData, confirm) =>
                        {
                            SystemInformation("set data.");
                            clientData.I_BDReceiveData.To("");
                            confirm.Invoke();
                            /*
                            if (clientData.DBRoom.TryDequeue(out string str))
                            {
                                string result = "RESULT";
                                clientData.I_BDReceievRoom.To(result);

                                confirm.Invoke();
                            }
                            */
                        });

                safe_listen_message<client.gameSession.BDData>(BUS.Message.GetData,
                    Header.Events.DB_DATA, ClientData.NAME)
                        .output_to((clientData, confirm) =>
                        {
                            SystemInformation("set data.");
                            clientData.I_BDReceiveData.To("");
                            confirm.Invoke();
                            /*
                            if (clientData.DBRoom.TryDequeue(out string str))
                            {
                                string result = "RESULT";
                                clientData.I_BDReceievRoom.To(result);

                                confirm.Invoke();
                            }
                            */
                        });
            }
        }
    }
}
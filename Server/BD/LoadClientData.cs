using Butterfly;

namespace server.BD
{
    public sealed class LoadClientData : Controller.LocalField<Setting.BD>
    {
        public const string NAME = "LoadClientData";

        public struct BUS
        {
            public struct Message
            {
                public const string LoadClientData = NAME + ":LoadClientData"; 
            }
        }

        void Construction()
        {
            extract_values<client.gameSession.Data>
            (BUS.Message.LoadClientData, (clientData) =>
            {
                // ... 
            });
        }

        void Start() => obj<_object>(NAME);

        private sealed class _object : Controller.Board
        {
            void Construction()
            {
                safe_listen_message<client.gameSession.Data>(BUS.Message.LoadClientData,
                    Header.Events.BD_LOAD_CLIENT_DATA, LoadClientData.NAME)
                        .output_to((clientData, confirm) =>
                        {
                            clientData.SetDBResult("");

                            confirm();
                        });
            }
        }
    }
}
using Butterfly;

namespace server.BD
{
    public sealed class Room : Controller.LocalField<Setting.BD>
    {
        public const string NAME = "Room";

        public struct BUS
        {
            public struct Message
            {
                public const string GetRoom = NAME + ":GetRoom"; 
                public const string SetRoom = NAME + ":SetRoom"; 
            }
        }

        void Construction()
        {
            extract_values<client.gameSession.Data>
            (BUS.Message.SetRoom, (clientData) =>
            {
                // ... 
            });

            extract_values<client.gameSession.Data>
            (BUS.Message.GetRoom, (clientData) =>
            {
                // ... 
            });
        }

        void Start() => obj<_object>(NAME);

        private sealed class _object : Controller.Board
        {
            void Construction()
            {
                safe_listen_message<client.gameSession.Data>(BUS.Message.SetRoom,
                    Header.Events.BD_ROOM, Room.NAME)
                        .output_to((clientData, confirm) =>
                        {
                            if (clientData.DBRoom.TryDequeue(out string str))
                            {
                                string result = "RESULT";
                                clientData.I_BDReceievRoom.To(result);

                                confirm.Invoke();
                            }
                        });

                safe_listen_message<client.gameSession.Data>(BUS.Message.GetRoom,
                    Header.Events.BD_ROOM, Room.NAME)
                        .output_to((clientData, confirm) =>
                        {
                            if (clientData.DBRoom.TryDequeue(out string str))
                            {
                                string result = "RESULT";
                                clientData.I_BDReceievRoom.To(result);

                                confirm.Invoke();
                            }
                        });
            }
        }
    }
}
using System.Data;
using Butterfly;
using server.client.world;

namespace server.client
{
    public sealed class World : world.Controller
    {

        public struct BUS
        {
            public struct Echo
            {
                public const string CREATING = NAME + ":Creating";

                public const string CONNECT = NAME + ":Connect";
                public const string DISCONNECT = NAME + ":Disconnect";

                public const string REMOVE = NAME + ":Remove";
            }
        }

        void Construction()
        {
            send_message(ref I_worldLogger, Logger.Type.WORLD);

            listen_echo_1_1<world.room.Setting, world.room.Controller.IReceive>
                (BUS.Echo.CREATING)
                    .output_to((settings, @return) =>
                    {
                        @return.To(Creating(GetUniqueID(), settings));
                    },
                    Header.Events.SYSTEM);

            listen_echo_1_1<string, bool>(BUS.Echo.REMOVE)
                .output_to((key, @return) =>
                {
                    @return.To(Remove(key));
                },
                Header.Events.SYSTEM);
        }

        /// <summary>
        /// Уникальный ID для комнаты.
        /// </summary>
        private ulong _uniqueID = 0;

        public string GetUniqueID()
        {
            if (_uniqueID == ulong.MaxValue) _uniqueID = 0;

            return _uniqueID++.ToString();
        }
    }
}


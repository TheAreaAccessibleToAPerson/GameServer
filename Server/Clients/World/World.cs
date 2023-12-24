using Butterfly;

namespace server.client
{
    public sealed class World : world.Controller 
    {

        public struct BUS 
        {
            public struct Echo 
            {
                public const string ADD = NAME + ":Add";
                public const string REMOVE = NAME + ":Remove";
            }
        }

        private readonly Dictionary<int, Connected.IWorldReceive> m = new();

        private static IInput<Connected.IWorldReceive> y;
        private short index = 0;

        void Contruction()
        {
            send_message(ref I_worldLogger, Logger.Type.WORLD);

            listen_echo_1_2<Connected.IWorldReceive, bool, IClientReceive>(BUS.Echo.ADD)
                .output_to((client, @return) => 
                {
                    if (Add(client))
                    {
                        @return.To(true, this);
                    }
                    else @return.To(false, null);
                },
                Header.Events.WORLD);

            listen_echo_1_1<string, bool>(BUS.Echo.REMOVE)
                .output_to((name, @return) => 
                { 
                    @return.To(Remove(name)); 
                },
                Header.Events.WORLD);
        }

        public static void Add1(Connected.IWorldReceive client)
        {
            y.To(client);
        }

        public interface IClientReceive
        {
        }
    }
}


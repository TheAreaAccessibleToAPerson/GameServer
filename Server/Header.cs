using System.ComponentModel.DataAnnotations;
using System.Data;
using Butterfly;

namespace server
{
    public sealed class Header : Controller, ReadLine.IInformation
    {
        public struct Events 
        {
            public const string SYSTEM = "System";

            public const string RECEIVE_NEW_CONNECT = "ReceiveNewConnect";

            public const string BD_AUTHORIZATION = "BDAuthorization";
            public const string BD_LOAD_CLIENT_DATA = "BDLoadClientData";

            public const string TCP_RECEIVE = "TcpReceive";
            public const string TCP_SEND = "TcpSend";

            public const string WORLD = "World";

            public const string SSL_RECEIVE = "SslReceive";
            public const string SSL_SEND = "SslSend";

            public const string WORK = "Work";
        }

        private readonly Logger _logger = new Logger();

        void Construction()
        {
            listen_events(Events.SYSTEM, Events.SYSTEM);
            listen_events(Events.TCP_RECEIVE, Events.TCP_RECEIVE);
            listen_events(Events.RECEIVE_NEW_CONNECT, Events.RECEIVE_NEW_CONNECT);
            listen_events(Events.TCP_SEND, Events.TCP_SEND);
            listen_events(Events.WORK, Events.WORK);

            listen_message<int, string>(Logger.Type.SYSTEM)
                .output_to(_logger.WriteSystem, Events.SYSTEM);

            listen_message<int, string>(Logger.Type.CLIENT)
                .output_to(_logger.WriteClient, Events.SYSTEM);

            listen_message<int, string>(Logger.Type.WORLD)
                .output_to(_logger.WriteClient, Events.SYSTEM);
        }

        public const string ADDRESS = "127.0.0.1";
        public const int SSL_PORT = 21111;

        public const int TCP_PORT = 21112;
        public const int UDP_PORT = 21113;

        void Start()
        {
            obj<Server>(Server.NAME, new Setting() 
            {
                SslSetting = new() 
                {
                    Address = "127.0.0.1",
                    Port = 21111
                },

                TcpSetting = new() 
                {
                    Address = "127.0.0.1",
                    Port = 21112
                }
            });
        }

        void ReadLine.IInformation.Command(string command)
        {
            //new Client().Start();
        }
    }
}

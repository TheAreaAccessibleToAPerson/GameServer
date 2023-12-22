using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace server
{
    public sealed class ClientsManager : Controller
    {
        public const string NAME = "ClientsManager";

        public struct BUS
        {
            public struct Message
            {
                public const string LISTEN_CONNECT_CLIENTS
                    = NAME + ":ListenConnectClients";

            }

            public struct Echo 
            {
                public const string CREATING_CLIENT
                    = NAME + ":CreatingClient";
            }
        }

        void Construction()
        {
            listen_message<Socket>(BUS.Message.LISTEN_CONNECT_CLIENTS)
                .output_to((socket) =>
                {
                    string name = $"{((IPEndPoint)socket.RemoteEndPoint).Address}" +
                        $"/{((IPEndPoint)socket.RemoteEndPoint).Port}";

                    if (try_obj(name, out client.Connect client))
                        client.destroy();

                    obj<client.Connect>(name, socket);
                },
                Header.Events.SYSTEM);

            listen_echo_2_0<string, client.ConnectedInformation>(BUS.Echo.CREATING_CLIENT)
                .output_to((login, info, @return) =>
                {
                    if (try_obj<client.Connected>
                        (login, out client.Connected client))
                            client.destroy();

                    obj<client.Connected>(login, info);

                    @return.To();
                },
                Header.Events.SYSTEM);
        }
    }

}
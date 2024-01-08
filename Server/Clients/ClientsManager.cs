using System.Net;
using System.Net.Sockets;
using Butterfly;
using server.client;

namespace server
{
    public sealed class ClientsManager : Controller
    {
        public const string NAME = "ClientsManager";

        protected IInput<int, string> I_systemLogger;

        public struct BUS
        {
            public struct Message
            {
                public const string LISTEN_CONNECT_CLIENTS = NAME + ":ListenConnectClients";

            }

            public struct Echo 
            {
                public const string CREATING_CLIENT = NAME + ":CreatingClient";

                public const string REMOVE = NAME + ":Remove";
                public const string ADD = NAME + ":Add";
                public const string GET_ALL = NAME + ":GetAll";
                public const string GET = NAME + ":Get";
            }
        }

        private readonly Dictionary<string, client.Connected.IClient> _clients = new();

        void Construction()
        {
            send_message(ref I_systemLogger, Logger.Type.SYSTEM);

            obj<World>(World.NAME);

            listen_message<System.Net.Sockets.Socket>(BUS.Message.LISTEN_CONNECT_CLIENTS)
                .output_to((socket) =>
                {
                    string name = $"{((IPEndPoint)socket.RemoteEndPoint).Address}" +
                        $"/{((IPEndPoint)socket.RemoteEndPoint).Port}";

                    if (try_obj(name, out client.Connect client))
                    {
                        LoggerWarning($"Клиент {name} уже подключается. Прервем подключение!");

                        client.destroy();
                    }

                    obj<client.Connect>(name, socket);
                },
                Header.Events.SYSTEM);

            listen_echo_2_0<string, client.ConnectedInformation>(BUS.Echo.CREATING_CLIENT)
                .output_to((login, info, @return) =>
                {
                    if (try_obj<client.Connected>(login, out client.Connected client))
                    {
                        LoggerWarning($"Клиент с логином {login} уже подключон. Отключаем его!");

                        client.destroy();
                    }

                    obj<client.Connected>(login, info);

                    @return.To();
                },
                Header.Events.SYSTEM);

            listen_echo_2_1<string, client.Connected.IClient, bool>(BUS.Echo.ADD)
                .output_to((nickname, client, @return) => 
                {
                    if (_clients.ContainsKey(nickname))
                    {
                        LoggerError($"Клиент {nickname} уже добавлен!");

                        @return.To(false);
                    }
                    else 
                    {
                        LoggerInfo($"Клиент {nickname} добавлен.");

                        _clients.Add(nickname, client);

                        @return.To(true);
                    }
                },
                Header.Events.SYSTEM);

            listen_echo_1_1<string, bool>(BUS.Echo.REMOVE)
                .output_to((nickname, @return) => 
                {
                    if (_clients.Remove(nickname))
                    {
                        LoggerInfo($"Клиент {nickname} удален.");

                        @return.To(true);
                    }
                    else 
                    {
                        LoggerError($"Клиента {nickname} не сущесвует.");

                        @return.To(false);
                    }
                },
                Header.Events.SYSTEM);

            listen_echo_1_1<string, client.Connected.IClient>(BUS.Echo.GET)
                .output_to((nickname, @return) => 
                {
                },
                Header.Events.SYSTEM);

            listen_echo<client.Connected.IClient[]>(BUS.Echo.GET_ALL)
                .output_to((@return) => 
                {
                },
                Header.Events.SYSTEM);
        }

        protected void LoggerInfo(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_systemLogger.To(Logger.INFO, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerError(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_systemLogger.To(Logger.ERROR, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerWarning(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_systemLogger.To(Logger.WARNING, $"{NAME}:{GetKey()}[{info}]");
        }
    }

}
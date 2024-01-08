namespace Butterfly
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            Butterfly.fly<server.Header>(new Butterfly.Settings()
            {
                Name = "Program",

                SystemEvent = new EventSetting(server.Header.Events.SYSTEM, 50),

                EventsSetting = new EventSetting[]
                {
                    new EventSetting(server.Header.Events.RECEIVE_NEW_CONNECT, 30),
                    new EventSetting(server.Header.Events.TCP_RECEIVE, 30),
                    new EventSetting(server.Header.Events.TCP_SEND, 30),
                    new EventSetting(server.Header.Events.SSL_RECEIVE, 30),
                    new EventSetting(server.Header.Events.SSL_SEND, 30),
                    new EventSetting(server.Header.Events.ROOM, 30),
                    new EventSetting(server.Header.Events.CLIENT, 30),
                    new EventSetting(server.Header.Events.WORK, 30),

                    // ________________________BD______________________________
                    new EventSetting(server.Header.Events.BD_AUTHORIZATION, 30),
                    // ________________________________________________________
                }
            });
        }
    }
}
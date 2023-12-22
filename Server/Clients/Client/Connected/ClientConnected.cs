namespace server.client 
{
    public class Connected : gameSession.Controller
    {
        void Construction()
        {
            send_message(ref I_clientLogger, Logger.Type.CLIENT);

            Field.Ssl.Destroy = Destroy;
            Field.Tcp.Destroy = Destroy;

            input_to(ref Field.Ssl.I_output, Header.Events.WORK, ReceiveSsl);
            input_to(ref Field.Tcp.I_output, Header.Events.WORK, ReceiveTcp);

            add_event(Header.Events.SSL_RECEIVE, Field.Ssl.Receive);
            add_event(Header.Events.TCP_RECEIVE, Field.Tcp.Receive);

            input_to(ref Data.Process, Header.Events.SYSTEM, Process);
        }

        void Start() { }
    }
}

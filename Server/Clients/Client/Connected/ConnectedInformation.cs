namespace server.client
{
    public class ConnectedInformation
    {
        public int BDIndex { init; get; }
        public string Nickname { init; get; }

        public client.Socket Tcp { init; get; }
    }
}
namespace server.client
{
    public class ConnectedInformation
    {
        public int ConnectedID { init; get; }

        public int BDIndex { init; get; }
        public int Nickname { init; get; }

        public client.Socket Ssl { init; get; }
        public client.Socket Tcp { init; get; }
    }
}
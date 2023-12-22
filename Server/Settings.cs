namespace server
{
    public sealed class Setting
    {
        public Ssl SslSetting { init; get; }
        public Tcp TcpSetting { init; get; }

        public BD BDSetting { init; get; }

        public sealed class Ssl
        {
            public string Address { init; get; }
            public int Port { init; get; }
        }

        public sealed class Tcp
        {
            public string Address { init; get; }
            public int Port { init; get; }
        }

        public sealed class BD
        {
        }
    }
}
public struct NetWork 
{
    public const int HEADER_LENGTH = 2;

    public const int LENGTH_1BYTE_INDEX = 0;
    public const int LENGTH_2BYTE_INDEX = 1;

    public const int TYPE_1BYTE_INDEX = 2;
    public const int TYPE_2BYTE_INDEX = 3;

    public struct Server 
    {
        private const int ACCESS_VERIFICATION = 1;

        public struct AccessVerification
        {
            public const int TYPE = NetWork.Server.ACCESS_VERIFICATION;
            public const int LENGTH = NetWork.HEADER_LENGTH + 1;
        }
    }

    public struct Client
    {
        private const int SENDING_LOGIN_AND_PASSWORD = 1;

        public struct SendingLoginAndPassword 
        {
            public const int TYPE = NetWork.Client.SENDING_LOGIN_AND_PASSWORD;
            public const int LENGTH = NetWork.HEADER_LENGTH + LOGIN_LENGTH + PASSWORD_LENGTH;


            public const int LOGIN_START_INDEX = NetWork.HEADER_LENGTH;
            public const int LOGIN_LENGTH = 16;

            public const int PASSWORD_START_INDEX = LOGIN_START_INDEX + LOGIN_LENGTH;
            public const int PASSWORD_LENGTH = 16;
        }
    }
}
#define INFO

using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace gameClient.manager
{
    public sealed class SSL : handler.Message, Thread.IUpdate
    {
        private readonly Socket _TCPSocket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        private readonly ConcurrentQueue<byte[]> _messages = new();
        private int _messagesCount = 0;

        private readonly Connection.ISSL _connectionResult;
        private readonly TCP.IConnection _tcpConnection;

        private int IDConnection = -1;

        /// <summary>
        /// Соединение установлено.
        /// </summary>
        private bool _isConnect = false;

        public SSL(Connection.ISSL connectionResult, TCP.IConnection tcpConnection) 
            : base("SSL//:")
        {
            _connectionResult = connectionResult;
            _tcpConnection = tcpConnection;
        }

        #region Update

        void Thread.IUpdate.Update()
        {
            if (_messages.Count > 0)
            {
                for (int i = 0; i < _messagesCount; i++)
                {
                    if (_messages.TryDequeue(out byte[] buffer))
                    {
#if INFO
                        SystemInformation($"SEND:\n" + string.Join(" ", buffer));
#endif

                        _TCPSocket.Send(buffer);

                        Interlocked.Decrement(ref _messagesCount);
                    }
                }
            }

            int available = _TCPSocket.Available;
            if (available > 0)
            {
                do
                {
                    byte[] buffer = new byte[8192];

                    int count = _TCPSocket.Receive(buffer);

                    available -= count;
                }
                while (available > 0);
            }
        }

        #endregion

        #region Output

        public void Authorization(string login, string password)
        {
            SystemInformation($"Отправляем Login:{login}, Password:{password}.", ConsoleColor.Yellow);

            if (login.Length > NetWork.Client.SendingLoginAndPassword.LOGIN_LENGTH)
                SystemInformation("Длина логина привышена.", ConsoleColor.Yellow);

            if (password.Length > NetWork.Client.SendingLoginAndPassword.PASSWORD_LENGTH)
                SystemInformation("Длина пароля привышена.", ConsoleColor.Yellow);

            byte[] message = new byte[NetWork.Client.SendingLoginAndPassword.LENGTH];
            {
                message[NetWork.LENGTH_1BYTE_INDEX]
                    = NetWork.Client.SendingLoginAndPassword.LENGTH >> 8;

                message[NetWork.LENGTH_2BYTE_INDEX]
                    = NetWork.Client.SendingLoginAndPassword.LENGTH;

                message[NetWork.TYPE_1BYTE_INDEX] 
                    = NetWork.Client.SendingLoginAndPassword.TYPE >> 8;

                message[NetWork.TYPE_2BYTE_INDEX] 
                    = NetWork.Client.SendingLoginAndPassword.TYPE;
            }

            byte[] l = Encoding.ASCII.GetBytes(login); int loginIndex = 0;
            for (int i = NetWork.Client.SendingLoginAndPassword.LOGIN_START_INDEX; 
                i < (NetWork.Client.SendingLoginAndPassword.LOGIN_START_INDEX + l.Length); i++)
                    message[i] = l[loginIndex++];

            byte[] p = Encoding.ASCII.GetBytes(password); int passwordIndex = 0;
            for (int i = NetWork.Client.SendingLoginAndPassword.PASSWORD_START_INDEX; 
                i < (NetWork.Client.SendingLoginAndPassword.PASSWORD_START_INDEX + p.Length); i++)
                    message[i] = l[passwordIndex++];

            _messages.Enqueue(message); Interlocked.Increment(ref _messagesCount);
        }

        #endregion

    }
}
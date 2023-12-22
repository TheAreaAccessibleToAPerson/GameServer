using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace server.client
{

    public sealed class Socket : System.Net.Sockets.Socket
    {
        private bool _isRunning = false;

        public Action<string> Destroy;
        public IInput<byte[], int> I_output;
        
        private readonly int _bufferSize;
        public readonly string Address;

        public Socket(int bufferSize, Action<string> destroy,
            System.Net.Sockets.Socket socket) : base(socket.SafeHandle)
        {
            Address = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();

            _bufferSize = bufferSize;

            Destroy = destroy;
        }

        public bool Start()
        {
            _isRunning = true;

            return true;
        }

        public void ReceiveUpdate()
        {
            if (_isRunning == false) return;

            try
            {
                if (Available > 0)
                {
                    byte[] buffer = new byte[_bufferSize];
                    int size = Receive(buffer);

                    if (size >= NetWork.HEADER_LENGTH)
                    {
                        int step = 0;
                        do
                        {
                            int length = buffer[step + NetWork.LENGTH_1BYTE_INDEX] << 8 ^    
                                buffer[step + NetWork.LENGTH_2BYTE_INDEX];

                            if (length < (size - step))
                            {
                                byte[] message = new byte[length];

                                int index = 0;
                                for (int i = step; i < step + length; i++)
                                    message[index++] = buffer[i];

                                I_output.To(message, length);

                                step = length;
                            }
                            else break;
                        }
                        while (step < size);
                    }
                }
            }
            catch (Exception ex)
            {
                _isRunning = false;

                Destroy(ex.ToString());
            }
        }

        public void Receive()
        {
            if (_isRunning == false) return;
        }

        public bool Stop()
        {
            _isRunning = false;

            try
            {
                Dispose();
                Close();

                return true;
            }
            catch (Exception ex)
            {
                Destroy(ex.ToString());

                return true;
            }
        }
    }
}
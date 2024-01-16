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
        public new void Send(byte[][] buffers)
        {
            if (_isRunning == false) return;

            try 
            {
                byte[] newBuffer = buffers[0];

                for (int i = 1; i < buffers.Length; i++)
                   newBuffer = newBuffer.Concat(buffers[i]).ToArray();

                base.Send(newBuffer);
            }
            catch (Exception ex)
            {
                _isRunning = false;

                Destroy(ex.ToString());
            }
        }

        public new void Send(byte[] buffer1, byte[] buffer2, byte[] buffer3, 
            byte[] buffer4, byte[] buffer5)
        {
            if (_isRunning == false) return;

            byte[] buffer = new byte[buffer1.Length + buffer2.Length + 
                buffer3.Length + buffer4.Length + buffer5.Length];

            int index = 0;

            for (int i = 0; i < buffer1.Length; i++) buffer[index++] = buffer1[i];
            for (int i = 0; i < buffer2.Length; i++) buffer[index++] = buffer2[i];
            for (int i = 0; i < buffer3.Length; i++) buffer[index++] = buffer3[i];
            for (int i = 0; i < buffer4.Length; i++) buffer[index++] = buffer4[i];
            for (int i = 0; i < buffer5.Length; i++) buffer[index++] = buffer4[i];

            try 
            {
                base.Send(buffer);
            }
            catch (Exception ex)
            {
                _isRunning = false;

                Destroy(ex.ToString());
            }
        }

        public new void Send(byte[] buffer1, byte[] buffer2, byte[] buffer3, 
            byte[] buffer4)
        {
            if (_isRunning == false) return;

            byte[] buffer = new byte[buffer1.Length + buffer2.Length + 
                buffer3.Length + buffer4.Length];

            int index = 0;

            for (int i = 0; i < buffer1.Length; i++) buffer[index++] = buffer1[i];
            for (int i = 0; i < buffer2.Length; i++) buffer[index++] = buffer2[i];
            for (int i = 0; i < buffer3.Length; i++) buffer[index++] = buffer3[i];
            for (int i = 0; i < buffer4.Length; i++) buffer[index++] = buffer4[i];

            try 
            {
                base.Send(buffer);
            }
            catch (Exception ex)
            {
                _isRunning = false;

                Destroy(ex.ToString());
            }
        }

        public new void Send(byte[] buffer1, byte[] buffer2, byte[] buffer3)
        {
            if (_isRunning == false) return;

            byte[] buffer = new byte[buffer1.Length + buffer2.Length + 
                buffer3.Length];

            int index = 0;

            for (int i = 0; i < buffer1.Length; i++) buffer[index++] = buffer1[i];
            for (int i = 0; i < buffer2.Length; i++) buffer[index++] = buffer2[i];
            for (int i = 0; i < buffer3.Length; i++) buffer[index++] = buffer3[i];

            try 
            {
                base.Send(buffer);
            }
            catch (Exception ex)
            {
                _isRunning = false;

                Destroy(ex.ToString());
            }
        }

        public new void Send(byte[] buffer1, byte[] buffer2)
        {
            if (_isRunning == false) return;

            byte[] buffer = new byte[buffer1.Length + buffer2.Length];
            int index = 0;

            for (int i = 0; i < buffer1.Length; i++) buffer[index++] = buffer1[i];
            for (int i = 0; i < buffer2.Length; i++) buffer[index++] = buffer2[i];

            try 
            {
                base.Send(buffer);
            }
            catch (Exception ex)
            {
                _isRunning = false;

                Destroy(ex.ToString());
            }
        }

        public new void Send(byte[] buffer)
        {
            if (_isRunning == false) return;

            try 
            {
                base.Send(buffer);
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

            try
            {
                if (Available > 0)
                {
                    byte[] buffer = new byte[_bufferSize];
                    int size = Receive(buffer);

                    string str = $"[SIZE:{size}]\n";
                    for (int i = 0; i < size; i++) str += buffer[i] + " ";
                    System.Console.WriteLine(str);

                    if (size >= NetWork.HEADER_LENGTH)
                    {
                        int step = 0;
                        do
                        {
                            int length = buffer[step + NetWork.LENGTH_1BYTE_INDEX] << 8 ^    
                                buffer[step + NetWork.LENGTH_2BYTE_INDEX];


                            if (length <= (size - step))
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

        public void Pause()
        {
            _isRunning = false;
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
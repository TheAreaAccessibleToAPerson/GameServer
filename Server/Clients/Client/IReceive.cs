namespace server.client
{
    public interface IReceive 
    {
        public void Send(string message);
        public void Send(byte[] message);
        public void Send(string client, string message);
        public void Send(string client, byte[] message);

        public string GetKey();
    }
}
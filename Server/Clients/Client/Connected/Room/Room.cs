using Butterfly;

namespace server.client.room
{
    public sealed class Default : Controller.Board.LocalField<Setting>,
        IReceive
    {
        void Construction()
        {
        }

        void Start()
        {
        }

        void IReceive.Send(string message){}
        void IReceive.Send(byte[] message){}
        void IReceive.Send(string client, string message){}
        void IReceive.Send(string client, byte[] message){}
    }
}
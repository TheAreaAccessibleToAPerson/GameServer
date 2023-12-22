using System.Net.Sockets;
using Butterfly;

namespace server
{
    public sealed class ConnectClient : Socket
    {
        public ConnectClient(SafeSocketHandle handle)
            : base(handle) {}
    }
}
using System.Collections.Concurrent;
using Butterfly;

namespace server.client.gameSession
{
    public sealed class BDData
    {
        public int Index;

        public IInput<string> I_BDReceiveData;
    }
}
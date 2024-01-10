using Butterfly;

namespace server.client.world.room
{
    public sealed class Setting
    {
        public string Name { init; get; }
        public int ID { init; get; }

        public int StartPositionX { init; get; } = 0;
        public int StartPositionY { init; get; } = 0;

        public int SizeX { init; get; } = 0;
        public int SizeY { init; get; } = 0;

        public unit.Mob[] Mobs { init; get; }

        public gameSession.Data ClientData { init; get; }
    }
}
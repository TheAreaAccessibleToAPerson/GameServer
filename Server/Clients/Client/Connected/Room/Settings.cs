namespace server.client.room
{
    public sealed class Setting
    {
        public string Name { init; get; }

        public int ID { init; get; }

        public unit.player.Main Player { init; get; }
        public unit.mobe.Default[] Mobs { init; get; }
    }
}
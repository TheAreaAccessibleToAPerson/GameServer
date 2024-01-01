namespace server.client.world.room
{
    public sealed class Setting
    {
        public string Name { init; get; }
        public int ID { init; get; }

        public Connected.IRoomReceive ClientReceive { init; get; }
    }
}
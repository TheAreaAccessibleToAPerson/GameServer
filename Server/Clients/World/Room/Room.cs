using Butterfly;

namespace server.client.world
{
    public sealed class Room : room.Controller
    {
        void Construction()
        {
            send_message(ref I_worldLogger, Logger.Type.WORLD);
        }

        void Start()
        {
            LoggerInfo("Start");

            // Сообщаем что комната создана.
            Field.ClientReceive.Creating(GetKey());
        }

        void Configurate()
        {
            LoggerInfo("Configurate");
        }
    }

    public struct Mob
    {
        public int PositionX;
        public int SizeX;
    }
}

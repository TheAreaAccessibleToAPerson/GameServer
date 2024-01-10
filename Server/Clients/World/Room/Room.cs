using System.Reflection.Emit;
using Butterfly;

namespace server.client.world
{
    public sealed class Room : room.Controller
    {
        void Construction()
        {
            send_message(ref I_worldLogger, Logger.Type.WORLD);

            add_event(Header.Events.ROOM, Update);
        }

        void Start()
        {
            LoggerInfo("Start");
        }

        void Configurate()
        {
            LoggerInfo("Configurate");

            if (Field.Mobs == null)
            {
                LoggerError($"В настройки комнаты не были переданы мобы.");

                destroy();
            }
            else if (Field.Mobs.Length == 0)
            {
                LoggerError($"В настройки комнаты было передано 0 мобов.");

                destroy();
            }
            else
            {
                LoggerInfo($"Было получено {Field.Mobs} мобов.");

                Mobs = Field.Mobs;
            }

            CurrentPositionX = Field.StartPositionX;
            CurrentPositionY = Field.StartPositionY;

            SizeX = Field.SizeX;
            SizeY = Field.SizeY;

            // Сообщаем что комната создана.
            Field.ClientData.IRoom_creating.To(GetKey(), CurrentPositionX, CurrentPositionY);
        }
    }
}

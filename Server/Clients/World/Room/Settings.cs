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

        /// <summary>
        /// Сообщает о том что комната создана.
        /// </summary>
        /// <value></value>
        public IInput<string, int, int> IRoom_creating { init; get; }

        /// <summary>
        /// Команда сообщает клиенту о движении в указаную позицию.
        /// 1)Направление(поворот) 2)Позиция x 3)Позиция y
        /// 4)Время начала движения.
        /// 5)Время в миллисекундах за которое необходимо преодолеть дистанцию.
        /// </summary>
        public IInput<int, int, int, DateTime, int> IRoom_characterMove { init; get; }
    }
}
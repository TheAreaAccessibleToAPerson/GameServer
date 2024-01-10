using Butterfly;

namespace server.client.gameSession
{
    public sealed class Data
    {
        /// <summary>
        /// Имя набора спрайтов для вашего героя.
        /// </summary>
        public int CharacterName = 1;

        public int SpeedMove = 100;

        /// <summary>
        /// Сообщает о том что комната создана.
        /// </summary>
        /// <value></value>
        public IInput<string, int, int> IRoom_creating;

        /// <summary>
        /// Команда сообщает клиенту о движении в указаную позицию.
        /// 1)Направление(поворот) 2)Позиция x 3)Позиция y
        /// </summary>
        public IInput<int, int, int> IRoom_characterMove;

        /// <summary>
        /// Создать моба.
        /// </summary>
        public IInput<unit.Mob> IRoom_creatingMob;

        /// <summary>
        /// Отправляет клинту новое значение скорости движения.
        /// </summary>
        public IInput<int> I_characterMoveSpeed;
    }
}
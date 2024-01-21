using System.Xml.Schema;
using Butterfly;

namespace server.client.gameSession
{
    public sealed class Data
    {
        public int BarName = 1;

        /// <summary>
        /// Имя набора спрайтов для вашего героя.
        /// </summary>
        public int CharacterName = 1;

        public int SpeedMove = 200;

        public int HP = 100, MP = 200, Shield = 100;
        public int CurrentHP = 100, CurrentMP = 200, CurrentShield = 100;

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
        public IInput<int> IRoom_characterMoveSpeed;

        /// <summary>
        /// Полученый урон.(Вычетает значение из HP и Shield)
        /// </summary>
        public IInput<int> IRoom_characterSubstractHS;

        /// <summary>
        /// Вычесть ману.(враг может сжеч ману)
        /// </summary>
        public IInput<int> IRoom_characterSubstractMP;

        /// <summary>
        /// Полученый урон.(Вычетает значение из HP, Shield и MP)
        /// </summary>
        public IInput<int, int> IRoom_characterSubstractHSM;

        /// <summary>
        /// Начать атаку.
        /// </summary>
        public IInput<int, int> IRoom_characterDefaultAttack;
    }
}
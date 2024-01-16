using Butterfly;

namespace server.client.gameSession
{
    namespace data
    {
        public interface IGet
        {
            int HP();
            int MP();
            int Shield();

            int CurrentHP();
            int CurrentMP();
            int CurrentShield();
        }

    }

    public sealed class Data : data.IGet
    {
        public int BarName = 1;

        /// <summary>
        /// Имя набора спрайтов для вашего героя.
        /// </summary>
        public int CharacterName = 1;

        public int SpeedMove = 200;

        public int HP = 100, MP = 200, Shield = 100;
        public int CurrentHP = 100, CurrentMP = 200, CurrentShield = 100;

        int data.IGet.HP() => HP;
        int data.IGet.MP() => MP;
        int data.IGet.Shield() => Shield;

        int data.IGet.CurrentHP() => CurrentHP;
        int data.IGet.CurrentMP() => CurrentMP;
        int data.IGet.CurrentShield() => CurrentShield;

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
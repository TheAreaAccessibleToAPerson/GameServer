namespace server.client.unit
{
    public abstract class Controller
    {
        /// <summary>
        /// Имя моба.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public readonly int ID;

        /// <summary>
        /// Находится ли Unit в движении.
        /// </summary>
        /// <value></value>
        public bool IsMove { private set; get; } = false;
        public void MoveStart() => IsMove = true;
        public void MoveStop() => IsMove = false;

        public bool IsAttack { private set; get; } = false;
        public void AttackStart() => IsAttack = true;
        public void AttackStop()=> IsAttack = false;


        /// <summary>
        /// Стартавая позиция.
        /// </summary>
        public readonly int StartPositionX;

        /// <summary>
        /// Текущая позиция.
        /// </summary>
        /// <value></value>
        public int CurrentPosition { private set; get; }

        public int Move(int timeDelay)
        {
            return CurrentPosition = CurrentPosition +
                (MoveSpeedCurrent * timeDelay);
        }

        /// <summary>
        /// Дефолтное количесво здоровья.
        /// </summary>
        /// <value></value>
        public readonly int HPDefault;
        /// <summary>
        /// Максимальный запас здоровья.(после бафов/дебафов)
        /// </summary>
        /// <value></value>
        public int HPMax { private set; get; }
        /// <summary>
        /// Текущее количесво здоровья.
        /// </summary>
        /// <value></value>
        public int HPCurrent { private set; get; }

        /// <summary>
        /// Увеличевает количесво HP в процентах от дефолтного значения.
        /// </summary>
        public int IncreaseHPFromDefault(int value)
        {
            return HPMax = HPDefault + (HPDefault / 100 * value);
        }

        public int AugmentHP(int value)
        {
            int i = HPCurrent + value;

            if (i < HPMax)
            {
                HPCurrent = i;
            }
            else HPCurrent = HPMax;

            return HPCurrent;
        }

        public readonly int MPDefault;
        public int MPMax { private set; get; }
        public int MPCurrent { private set; get; }

        public int ShieldDefault { private set; get; }
        public int ShieldMax { private set; get; }
        public int ShieldCurrent { private set; get; }

        public readonly int MoveSpeedDefault;
        public int MoveSpeedMax { private set; get; }
        public int MoveSpeedCurrent { private set; get; }

        public int PhysicsAttack { private set; get; }

        public int M { private set; get; }
        public int M { private set; get; }
        public int M { private set; get; }
        public int M { private set; get; }
        public int M { private set; get; }
        public int M { private set; get; }
    }
}
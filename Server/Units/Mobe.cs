namespace server.client.unit
{
    public struct Mob
    {
        public readonly int Name;

        /// <summary>
        /// ID ассета.
        /// </summary>
        public readonly int ID;

        public int BarName = 1;

        /// <summary>
        /// 0 - right
        /// 1 - left
        /// </summary>
        /// <value></value>
        public int Direction { private set; get; }

        public int PositionX { private set; get; }
        public int PositionY { private set; get; }

        public readonly int SizeX, SizeY;

        public readonly int PointLeft, PointRigth;
        public readonly int PointTop, PointBottom;

        public bool IsIntersection { private set; get; } = false;

        public readonly int DefaultHP, DefaultMP, DefaultShield;
        public int HP, MP, Shield;
        public int CurrentHP, CurrentMP, CurrentShield;

        public Mob(int name, int id, int direction,
            int positionX, int positionY, int sizeX, int sizeY,
            int defaultHP, int defaultMP, int defaultShield)
        {
            Name = name;

            ID = id;

            Direction = direction;

            PositionX = positionX; PositionY = positionY;

            SizeX = sizeX; SizeY = sizeY;

            PointLeft = positionX - (sizeX / 2);
            PointRigth = positionX + (sizeX / 2);

            PointTop = positionY + (sizeX / 2);
            PointBottom = positionY - (sizeX / 2);

            CurrentHP = HP = DefaultHP = defaultHP;
            CurrentMP = MP = DefaultMP = defaultMP;
            CurrentShield = Shield = DefaultShield = defaultShield;
        }

        public bool IsArrivedX(int pointX, int directionMove, out int point)
        {
            point = 0;

            // isRigth
            if (directionMove == 0)
            {
                if (pointX >= PointLeft)
                {
                    IsIntersection = true;

                    point = PointLeft;

                    return true;
                }
            }
            else if (directionMove == 1)
            {
                if (pointX <= PointLeft)
                {
                    IsIntersection = true;

                    point = PointLeft;

                    return true;
                }
            }
            else throw new Exception();

            return false;
        }
    }
}
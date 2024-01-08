namespace server.client.unit
{
    public struct Mob
    {
        public readonly string Name;

        /// <summary>
        /// ID ассета.
        /// </summary>
        public readonly int ID;

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

        public Mob(string name, int id, int direction,
            int positionX, int positionY, int sizeX, int sizeY)
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
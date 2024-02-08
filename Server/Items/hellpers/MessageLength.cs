namespace Items 
{
    /// <summary>
    /// Длина сообщения.
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// Добавляет предмет в инвентарь.
        /// </summary>
        public const int ADD = 1;

        /// <summary>
        /// Изменяет текущий предмет.
        /// </summary>
        public const int UPDATE = 2;

        /// <summary>
        /// Удаляет предмет из инвенторя.
        /// </summary>
        public const int DELETE = 3;

        public struct Length 
        {
            public struct Resource 
            {
                public const int ADD = 14;
                public const int DELETE = 8;
                public const int UPDATE = 12;
            }
        }
    }

    public struct Type 
    {
        public const int RESOURCE = 1;
        public const int WEAPON = 2;
    }

    public struct Name 
    {
        public const int GOLD = 1;
    }
}
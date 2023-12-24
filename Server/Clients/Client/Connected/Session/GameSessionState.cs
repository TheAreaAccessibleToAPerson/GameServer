namespace server.client.gameSession
{
    public sealed class State
    {
        private const string NONE = "None";

        /// <summary>
        /// Загружаем данные из базы данных.
        /// </summary>
        private const string LOAD_BD_DATA = "Load db data";

        private const string ADD_TO_WORLD = "Add to world";

        private const string CHANGE_ERROR =
            @"Неудалось сменить состояние c CurrentState:{0} на NextState:{1}." +
            @"Ожидалось что текущее состояние должно быть {2}.";

        private const string DESTROY_INFO =
            @"Failed to change state {0}->{1}";

        private const string CHANGE_INFO =
            @"Сменяем CurrentState:{0} на NextState:{1}.";

        /// <summary>
        /// Текущее состяние игровой сессии.
        /// </summary>
        public string CurrentState = NONE;

        public readonly object Locker = new object();

        public bool IsDestroy { set; get; } = false;

        public string Destroy()
        {
            lock (Locker)
            {
                IsDestroy = true;

                return $"Destroying. CurrentState:{CurrentState}";
            }
        }

        public bool HasNone()
        {
            lock (Locker)
            {
                return CurrentState == NONE;
            }
        }

        /// <summary>
        /// Загружаем данные из базы данных.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool SetLoadDbData(out string info)
        {
            lock (Locker)
            {
                if (CurrentState == NONE)
                {
                    info = String.Format(CHANGE_INFO,
                        CurrentState, LOAD_BD_DATA);

                    return true;
                }
                else
                {
                    info = String.Format(CHANGE_ERROR,
                        CurrentState, LOAD_BD_DATA);

                    return false;
                }
            }
        }

        public bool HasLoadDBData()
        {
            lock(Locker)
            {
                return CurrentState == LOAD_BD_DATA;
            }
        }

        public bool SetAddToWorld(out string info)
        {
            lock (Locker)
            {
                if (CurrentState == LOAD_BD_DATA)
                {
                    info = String.Format(CHANGE_INFO,
                        CurrentState, ADD_TO_WORLD);

                    return true;
                }
                else
                {
                    info = String.Format(CHANGE_ERROR,
                        CurrentState, ADD_TO_WORLD);

                    return false;
                }
            }
        }

        public bool HasAddToWorld()
        {
            lock(Locker)
            {
                return CurrentState == ADD_TO_WORLD;
            }
        }
    }
}
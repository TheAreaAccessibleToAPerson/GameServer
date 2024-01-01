using System.Diagnostics;
using System.Net.Http.Headers;

namespace server.client.gameSession
{
    public sealed class State
    {
        public const string NONE = "None";

        /// <summary>
        /// Загружаем данные.
        /// </summary>
        public const string LOADING_DATA = "Loading data";

        /// <summary>
        /// Данные загружены.
        /// </summary>
        public const string UPLOADING_DATA = "Loading data";

        /// <summary>
        /// Входим в мир.
        /// </summary>
        public const string INPUT_TO_WORLD = "Input to world";

        /// <summary>
        /// Комната создана.
        /// </summary>
        public const string CREATING_ROOM = "Creating room";

        /// <summary>
        /// Необходимое количесво данных.
        /// </summary>
        private int DATA_COUNT = 1;

        public string CurrentState { private set; get; } = NONE;

        /// <summary>
        /// Количесво загруженых данных.
        /// </summary>
        private int _uploadedData = 0;

        public readonly object Locker = new();

        private bool _isDestroy = false;

        public bool IsDestroy()
        {
            return _isDestroy;
        }

        public bool Destroy(out string info)
        {
            if (_isDestroy == false)
            {
                info = $"Вызов State.Destroy. CurrentState:{CurrentState}";

                _isDestroy = true;

                return true;
            }
            else
            {
                info = "Уже был вызван метод State.Destroy";

                return false;
            }
        }


        /// <summary>
        /// Проверяем загрузились ли все данные.
        /// </summary>
        /// <returns></returns>
        public bool IsUploadedData()
        {
            _uploadedData++;

            if (_uploadedData == DATA_COUNT) return true;

            return false;
        }

        /// <summary>
        /// Сбрасываем счетчик загруженых данных.
        /// </summary>
        public void Reset() => _uploadedData = 0;

        public bool HasNone() => CurrentState == NONE;

        public bool HasLoadingData() => CurrentState == LOADING_DATA;

        public bool SetLoadingData(out string info)
        {
            lock (Locker)
            {
                if (_isDestroy)
                {
                    info = $"Not call[Destroy]. CurrentState:{CurrentState} -> {LOADING_DATA}.";

                    return false;
                }

                if (CurrentState == NONE)
                {
                    info = $"Change state:access - CurrentState:{CurrentState} -> {LOADING_DATA}.";

                    CurrentState = LOADING_DATA;

                    return true;
                }
                else 
                {
                    info = $"Change state:error - CurrentState:{CurrentState} -> {LOADING_DATA}.";

                    throw new Exception();
                }

            }
        }

        public bool HasInputToWorld() => CurrentState == INPUT_TO_WORLD;

        public bool SetInputToWorld(out string info)
        {
            lock (Locker)
            {
                if (_isDestroy)
                {
                    info = $"Not call[Destroy]. CurrentState:{CurrentState} -> {INPUT_TO_WORLD}.";

                    return false;
                }

                if (CurrentState == LOADING_DATA)
                {
                    info = $"Change state:access - CurrentState:{CurrentState} -> {INPUT_TO_WORLD}.";

                    CurrentState = INPUT_TO_WORLD;

                    return true;
                }
                else 
                {
                    info = $"Change state:error - CurrentState:{CurrentState} -> {INPUT_TO_WORLD}.";

                    return false;
                }

            }
        }

        public bool HasCreatingRoom() => CurrentState == CREATING_ROOM;

        public bool SetCreatingRoom(out string info)
        {
            lock (Locker)
            {
                if (_isDestroy)
                {
                    info = $"Not call[Destroy]. CurrentState:{CurrentState} -> {CREATING_ROOM}.";

                    return false;
                }

                if (CurrentState == INPUT_TO_WORLD)
                {
                    info = $"Change state:access - CurrentState:{CurrentState} -> {CREATING_ROOM}.";

                    CurrentState = CREATING_ROOM;

                    return true;
                }
                else 
                {
                    info = $"Change state:error - CurrentState:{CurrentState} -> {CREATING_ROOM}.";

                    return false;
                }

            }
        }
    }
}
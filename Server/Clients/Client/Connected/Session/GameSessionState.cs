using System.Net.Http.Headers;

namespace server.client.gameSession
{
    public sealed class State
    {
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
        /// Необходимое количесво данных.
        /// </summary>
        private int DATA_COUNT = 1;

        public string CurrentState { private set; get; } = LOADING_DATA;

        /// <summary>
        /// Количесво загруженых данных.
        /// </summary>
        private int _uploadedData = 0;

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

        public bool HasLoadingData() => CurrentState == LOADING_DATA;

        //public bool SetLoadingData()
    }
}
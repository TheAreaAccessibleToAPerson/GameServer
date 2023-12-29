using System.Collections.Concurrent;
using Butterfly;

namespace server.client.gameSession
{
    public sealed class Data
    {
        public IInput<string> I_BDReceievRoom;
        public ConcurrentQueue<string> DBRoom = new();

        /// <summary>
        /// Результат полученый из базы данных.
        /// </summary>
        /// <value></value>
        private string DBResult;

        public Butterfly.IInput Process;


        /// <summary>
        /// Максимальное количесво персонажей на аккаунте.
        /// </summary>
        public const int MAX_CHARACTER_COUNT = 8;

        /// <summary>
        /// Игровое имя.
        /// </summary>
        /// <value></value>
        public string Name { private set; get; }

        /// <summary>
        /// Персонажи.
        /// </summary>
        /// <value></value>
        public Character[] Characters { private set; get; }

        /// <summary>
        /// Клан.
        /// </summary>
        /// <value></value>
        public Clan Clan { private set; get; }

        /// <summary>
        /// Пати.
        /// </summary>
        /// <value></value>
        public Party Party { private set; get; }

        public bool Define(out string info)
        {
            info = "";
            return false;
        }
    }
}
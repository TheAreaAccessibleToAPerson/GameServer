using Butterfly;

namespace server.client 
{
    public sealed class ClanManager : Controller
    {
        public const string NAME = "ClanManager";

        public struct BUS 
        {
            /// <summary>
            /// Запрос на вступление в клан.
            /// </summary>
            public const string REQUEST_TO_JOIN = NAME + ":Request to join.";

            /// <summary>
            /// Выйти из клана.
            /// </summary>
            public const string REQUEST_TO_LEAVE = NAME + ":Request to leave";

            /// <summary>
            /// Удалить учасника.
            /// </summary>
            public const string DELETE_MEMBER = NAME + ":DeleteMember";

            /// <summary>
            /// Принять приглашение.
            /// </summary>
            public const string ACCEPT_INVATE = ":Accept invate";

            /// <summary>
            /// Откланить приглашение.
            /// </summary>
            public const string DECLINE_INVATE = ":Decline invate";

            /// <summary>
            /// Участник клана авторизовался.
            /// </summary>
            public const string AUTHORIZATION = NAME + ":Authorization";

            /// <summary>
            /// Учасник клана вышел.
            /// </summary>
            public const string EXIT = NAME + ":Exit";
        }

        void Construction()
        {
        }
    }
}
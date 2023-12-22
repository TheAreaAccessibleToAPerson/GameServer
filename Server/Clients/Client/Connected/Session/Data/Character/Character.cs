namespace server.client.gameSession
{
    public sealed class Character
    {
        /// <summary>
        /// Уровень персонажа.
        /// </summary>
        /// <value></value>
        public int Level { set; get; }

        /// <summary>
        /// Текущий запас опыта.
        /// </summary>
        /// <value></value>
        public int CurrentExp { set; get; }

        /// <summary>
        /// Необходимое количесво экспы для нового уровня.
        /// </summary>
        /// <value></value>
        public int NecessaryExp { set; get; }

        /// <summary>
        /// Текущее количесво HP.
        /// </summary>
        /// <value></value>
        public int CurrentHP { set; get; }

        /// <summary>
        /// Максимальное количесво HP.
        /// </summary>
        /// <value></value>
        public int MaxHP { set; get; }

        /// <summary>
        /// Текущее количесво маны.
        /// </summary>
        /// <value></value>
        public int CurrentMP { set; get; }

        /// <summary>
        /// Максимальное количесво маны.
        /// </summary>
        /// <value></value>
        public int MaxMP { set; get; }

        /// <summary>
        /// Текущий запас щита.
        /// </summary>
        /// <value></value>
        public int CurrentSH { set; get; }

        /// <summary>
        /// Максимальный запас щита.
        /// </summary>
        /// <value></value>
        public int MaxSH { set; get; }

    }
}
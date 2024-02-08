namespace Items 
{
    public interface IItem
    {
        /// <summary>
        /// С помощью данного значения клиент и сервер будут общатся.
        /// </summary>
        /// <returns></returns>
        public int GetID();

        /// <summary>
        /// По данному значению клиент хранит картинку или ассет предмета.
        /// </summary>
        /// <returns></returns>
        public int GetName();

        /// <summary>
        /// Тип предмета.
        /// </summary>
        /// <returns></returns>
        public int GetType();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISendItem : IItem
    {
        public byte[] GetAddMessage();
    }
}
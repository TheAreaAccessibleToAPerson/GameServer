using Butterfly;

namespace server.client.items
{
    public class Main 
    {
        public IInput<int, string> I_clientLogger;
        public string ClientKey = null;

        protected readonly InventoryManager _inventoryManager = new(210);
        protected readonly EquipmentManager _equipmentManager = new();

        /// <summary>
        /// Создает сообщение инициализирующее весь инвентарь клиeнта.
        /// </summary>
        /// <returns></returns>
        public byte[] GetInitializeInventoryMessage()
        {
            return new byte[1];
        }

        protected void LoggerInfo(string info)
            => I_clientLogger.To(Logger.INFO, $"ItemsManager:{ClientKey}[{info}]");

        protected void LoggerError(string info)
            => I_clientLogger.To(Logger.ERROR, $"ItemsManager:{ClientKey}[{info}]");

        protected void LoggerWarning(string info)
            => I_clientLogger.To(Logger.WARNING, $"ItemsManager:{ClientKey}[{info}]");
    }
}
namespace server 
{
    public sealed class Logger 
    {
        public const int INFO = 0;
        public const int ERROR = 1;
        public const int WARNING = 2;

        public struct Type 
        {
            public const string SYSTEM = "SystemInformation";
            public const string CLIENT = "ClientInformation";
        }

        public void WriteSystem(int type, string info)
        {
            if (type == INFO)
            {
                System.Console.ForegroundColor = System.ConsoleColor.White;

                System.Console.WriteLine(info);
            }
            else if (type == WARNING)
            {
                System.Console.ForegroundColor = System.ConsoleColor.Yellow;

                System.Console.WriteLine(info);
            }
            else if (type == ERROR)
            {
                System.Console.ForegroundColor = System.ConsoleColor.Red;

                System.Console.WriteLine(info);
            }
            else throw new Exception("");
        }

        public void WriteClient(int type, string info)
        {
            if (type == INFO)
            {
                System.Console.ForegroundColor = System.ConsoleColor.White;

                System.Console.WriteLine(info);
            }
            else if (type == WARNING)
            {
                System.Console.ForegroundColor = System.ConsoleColor.Yellow;

                System.Console.WriteLine(info);
            }
            else if (type == ERROR)
            {
                System.Console.ForegroundColor = System.ConsoleColor.Red;

                System.Console.WriteLine(info);
            }
            else throw new Exception("");
        }
    }
}
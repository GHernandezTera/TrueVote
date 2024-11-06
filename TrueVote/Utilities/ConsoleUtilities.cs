namespace TrueVote.Utilities
{
    public class ConsoleUtilities
    {
        public static void Error(string message, Exception ex = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            if (ex != null)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.ResetColor();
        }

        public static void Sucess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}

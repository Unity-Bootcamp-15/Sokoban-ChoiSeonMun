namespace Sokoban
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 초기화
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Title = "My Sokoban";
            Console.CursorVisible = false;
            Console.Clear();
        }
    }
}

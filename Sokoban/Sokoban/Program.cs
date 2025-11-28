

namespace Sokoban
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ----------- 초기화 -------------
            // 콘솔 창 초기화
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Title = "My Sokoban";
            Console.CursorVisible = false;
            Console.Clear();

            // 게임 데이터 초기화
            int mapSizeMinX = 0;
            int mapSizeMinY = 0;
            int mapSizeMaxX = 10;
            int mapSizeMaxY = 10;

            int playerX = 5;
            int playerY = 10;

            int wallX = 3;
            int wallY = 3;
            
            // ------------ 게임 루프 -----------
            while (true)
            {
                // ------------ Render -----------
                // 이전 화면 지움
                Console.Clear();

                // 플레이어 출력
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("P");

                // 벽 출력
                Console.SetCursorPosition(wallX, wallY); ;
                Console.Write("#");

                // ------------ ProcessInput -----------
                // 키보드 입력
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // ------------ Update -----------
                // 플레이어 이동 처리
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    playerY = Math.Min(mapSizeMaxY, playerY + 1);
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    playerY = Math.Max(mapSizeMinY, playerY - 1);
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    playerX = Math.Max(mapSizeMinX, playerX - 1);
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    playerX = Math.Min(mapSizeMaxX, playerX + 1);
                }
            }
        }
    }
}

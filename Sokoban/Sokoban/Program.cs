

using System.Reflection.Metadata.Ecma335;

namespace Sokoban
{
    internal class Program
    {
        enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

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
            bool isGameOver = false;

            // 플레이어 데이터
            int playerX = 5;
            int playerY = 10;
            Direction playerDirection = Direction.None;

            // 벽 데이터
            int[] wallX = { 3, 4, 5, 6, 7 };
            int[] wallY = { 3, 3, 3, 3, 3 };
            int wallCount = wallX.Length;

            // 박스 데이터
            int[] boxX = { 6, 8 };
            int[] boxY = { 6, 7 };
            int boxCount = boxX.Length;

            // 골 데이터
            int[] goalX = { 4, 7 };
            int[] goalY = { 5, 10 };
            bool[] isBoxOnGoal = { false, false };
            int goalCount = goalX.Length;

            // ------------ 게임 루프 -----------
            while (isGameOver == false)
            {
                // ------------ Render -----------
                // 이전 화면 지움
                Console.Clear();

                // 박스 출력
                for (int i = 0; i < boxCount; ++i)
                {
                    Console.SetCursorPosition(boxX[i], boxY[i]); ;
                    Console.Write("@");
                }
                
                // 골 출력
                for (int i = 0; i < goalCount; ++i)
                {
                    Console.SetCursorPosition(goalX[i], goalY[i]);
                    Console.Write(isBoxOnGoal[i] ? "*" : "O");
                }

                // 플레이어 출력
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("P");

                // 벽 출력
                for (int i = 0; i < wallCount; ++i)
                {
                    Console.SetCursorPosition(wallX[i], wallY[i]); ;
                    Console.Write("#");
                }

                // ------------ ProcessInput -----------
                // 키보드 입력
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // ------------ Update -----------
                // 키 입력에 따라 방향 처리
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    playerDirection = Direction.Down;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    playerDirection = Direction.Up;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    playerDirection = Direction.Left;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    playerDirection = Direction.Right;
                }

                // 플레이어 이동 처리
                int newPlayerX = playerX;
                int newPlayerY = playerY;
                switch (playerDirection)
                {
                    case Direction.Left:
                        newPlayerX = Math.Max(mapSizeMinX, playerX - 1);
                        break;
                    case Direction.Right:
                        newPlayerX = Math.Min(mapSizeMaxX, playerX + 1);
                        break;
                    case Direction.Up:
                        newPlayerY = Math.Max(mapSizeMinY, playerY - 1);
                        break;
                    case Direction.Down:
                        newPlayerY = Math.Min(mapSizeMaxY, playerY + 1);
                        break;
                    default:
                        // Log를 이용한 오류 처리
                        // ㄴ 오류를 처리하기 위해서 필요한 문맥: 실제 값. 발생한 위치. 오류 메시지
                        Console.Clear();
                        Console.WriteLine($"[Error] at 플레이어 이동 처리: 잘못된 방향입니다. {playerDirection}");
                        return;
                }

                // 플레이어와 벽의 충돌 처리
                // 1. 플레이어와 벽들 중 하나라도 충돌했는지 감지한다.
                bool isCollidedPlayerWithWall = false;
                for (int i = 0; i < wallCount; ++i)
                {
                    bool isSamePlayerXAndWallX = newPlayerX == wallX[i];
                    bool isSamePlayerYAndWallY = newPlayerY == wallY[i];
                    bool isCollided = isSamePlayerXAndWallX && isSamePlayerYAndWallY;

                    if (isCollided)
                    {
                        isCollidedPlayerWithWall = true;
                        break;
                    }
                }

                // 2. 충돌했다면 위치를 다시 되돌린다.
                if (isCollidedPlayerWithWall)
                {
                    // 움직임을 반영하지 않는다.
                    continue;
                }

                // 플레이어와 박스의 충돌 처리
                // 1. 플레이어가 어떤 박스와 충돌했는지 찾는다.
                const int NoCollidedBox = -1;
                int collidedBoxIndex = NoCollidedBox;
                for (int i = 0; i < boxCount; ++i)
                {
                    bool isSamePlayerXAndBoxX = newPlayerX == boxX[i];
                    bool isSamePlayerYAndBoxY = newPlayerY == boxY[i];
                    bool isCollidedPlayerWithBox = isSamePlayerXAndBoxX && isSamePlayerYAndBoxY;

                    if (isCollidedPlayerWithBox)
                    {
                        collidedBoxIndex = i;
                        break;
                    }
                }
                

                // 2. 충돌했다면
                if (collidedBoxIndex != NoCollidedBox)
                {
                    // 2-1. 박스의 새로운 좌표를 구한다.
                    int currentBoxX = boxX[collidedBoxIndex];
                    int currentBoxY = boxY[collidedBoxIndex];
                    int newBoxX = currentBoxX;
                    int newBoxY = currentBoxY;
                    switch (playerDirection)
                    {
                        case Direction.Left:
                            newBoxX = Math.Max(mapSizeMinX, currentBoxX - 1);
                            newPlayerX = newBoxX + 1;
                            break;
                        case Direction.Right:
                            newBoxX = Math.Min(mapSizeMaxX, currentBoxX + 1);
                            newPlayerX = newBoxX - 1;
                            break;
                        case Direction.Up:
                            newBoxY = Math.Max(mapSizeMinY, currentBoxY - 1);
                            newPlayerY = newBoxY + 1;
                            break;
                        case Direction.Down:
                            newBoxY = Math.Min(mapSizeMaxY, currentBoxY + 1);
                            newPlayerY = newBoxY - 1;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"[Error] at 박스 이동 처리: 잘못된 방향입니다. {playerDirection}");
                            return;
                    }

                    // 2-2. 벽과의 충돌 처리
                    // 1. 박스와 벽의 충돌을 감지한다.
                    bool isCollidedBoxWithWall = false;
                    for (int i = 0; i < wallCount; ++i)
                    {
                        bool isSameBoxXAndWallX = newBoxX == wallX[i];
                        bool isSameBoxYAndWallY = newBoxY == wallY[i];
                        bool isCollided = isSameBoxXAndWallX && isSameBoxYAndWallY;

                        if (isCollided)
                        {
                            isCollidedBoxWithWall = true;
                            break;
                        }
                    }

                    // 2. 충돌했다면 좌표 갱신을 하지 않는다.
                    if (isCollidedBoxWithWall)
                    {
                        continue;
                    }

                    // 2-3. 박스끼리의 충돌 처리
                    bool isCollidedBetweenBoxes = false;
                    for (int i = 0; i < boxCount; ++i)
                    {
                        if (i == collidedBoxIndex)
                        {
                            continue;
                        }

                        bool isSameBoxXAndOtherBoxX = newBoxX == boxX[i];
                        bool isSameBoxYAndOtherBoxY = newBoxY == boxY[i];
                        bool isCollided = isSameBoxXAndOtherBoxX && isSameBoxYAndOtherBoxY;

                        if (isCollided)
                        {
                            isCollidedBetweenBoxes = true;
                            break;
                        }
                    }

                    if (isCollidedBetweenBoxes)
                    {
                        continue;
                    }

                    // 2-4. 박스의 좌표를 갱신한다.
                    boxX[collidedBoxIndex] = newBoxX;
                    boxY[collidedBoxIndex] = newBoxY;
                }

                // 박스와 골의 충돌 처리
                // ㄴ 골 위에 박스가 있는지 확인한다.
                for (int goalIdx = 0; goalIdx < goalCount; ++goalIdx)
                {
                    isBoxOnGoal[goalIdx] = false;

                    for (int boxIdx = 0; boxIdx < boxCount; ++boxIdx)
                    {
                        bool isSameBoxXAndGoalX = boxX[boxIdx] == goalX[goalIdx];
                        bool isSameBoxYAndGoalY = boxY[boxIdx] == goalY[goalIdx];
                        bool isCollidedBoxWithGoal = isSameBoxXAndGoalX && isSameBoxYAndGoalY;

                        if (isCollidedBoxWithGoal)
                        {
                            isBoxOnGoal[goalIdx] = true;
                            break;
                        }
                        
                    }
                }

                // 게임의 종료 여부
                // ㄴ 모든 골 위에 박스가 올려져 있는 것
                bool isAllBoxOnGoal = true;
                for (int i = 0; i < goalCount; ++i)
                {
                    isAllBoxOnGoal &= isBoxOnGoal[i];
                }

                if (isAllBoxOnGoal)
                {
                    isGameOver = true;
                }

                // 플레이어의 좌표를 갱신한다.
                playerX = newPlayerX;
                playerY = newPlayerY;
            }

            // 게임 종료 처리
            Console.Clear();
            Console.WriteLine("축하합니다! 게임을 클리어하셨습니다!");
        }
    }
}



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
                Render();
                ConsoleKeyInfo keyInfo = ProcessInput();
                Update(keyInfo);
            }

            ShowClearMessage();

            // ---------------------------------------------------
            void Render()
            {
                Console.Clear();

                RenderObjects(boxX, boxY, boxCount, _ => "@");
                RenderObjects(goalX, goalY, goalCount, idx => isBoxOnGoal[idx] ? "*" : "O");
                RenderObject(playerX, playerY, "P");
                RenderObjects(wallX, wallY, wallCount, _ => "#");

                // ---------------------------------------------------

                void RenderObject(int x, int y, string symbol)
                {
                    Console.SetCursorPosition(x, y); ;
                    Console.Write(symbol);
                }

                // NOTE: 조건부 심볼이 있어 Func를 받는다.
                void RenderObjects(int[] x, int[] y, int count, Func<int, string> symbolSelector)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        RenderObject(x[i], y[i], symbolSelector(i));
                    }
                }
            }

            ConsoleKeyInfo ProcessInput() => Console.ReadKey();

            void Update(ConsoleKeyInfo keyInfo)
            {
                playerDirection = GetDirectionFrom(keyInfo);
                if (TryMovePlayer(playerDirection))
                {
                    UpdateGoalState();
                    CheckGameClear();
                }
 
                // ---------------------------------------------------

                Direction GetDirectionFrom(ConsoleKeyInfo keyInfo) => keyInfo.Key switch
                {
                    ConsoleKey.DownArrow => Direction.Down,
                    ConsoleKey.UpArrow => Direction.Up,
                    ConsoleKey.LeftArrow => Direction.Left,
                    ConsoleKey.RightArrow => Direction.Right,
                    _ => Direction.None
                };

                (int newX, int newY) GetNewPositionByDirection(Direction direction, int oldX, int oldY) => direction switch
                {
                    Direction.Left => (Math.Max(mapSizeMinX, oldX - 1), oldY),
                    Direction.Right => (Math.Min(mapSizeMaxX, oldX + 1), oldY),
                    Direction.Up => (oldX, Math.Max(mapSizeMinY, oldY - 1)),
                    Direction.Down => (oldX, Math.Min(mapSizeMaxY, oldY + 1))
                };

                bool IsSamePosition(int x1, int y1, int x2, int y2) => (x1 == x2 && y1 == y2);

                int GetCollidedIndex(int x, int y, int[] targetX, int[] targetY, int targetCount, int excludedIndex = -1)
                {
                    int found = -1;
                    for (int idx = 0; idx < targetCount; ++idx)
                    {
                        if (idx == excludedIndex)
                        {
                            continue;
                        }

                        if (IsSamePosition(x, y, targetX[idx], targetY[idx]))
                        {
                            found = idx;
                            break;
                        }
                    }

                    return found;
                }

                bool IsCollided(int x, int y, int[] targetX, int[] targetY, int targetCount, int excludedIndex = -1)
                {
                    return -1 != GetCollidedIndex(x, y, targetX, targetY, targetCount, excludedIndex);
                }

                void UpdateGoalState()
                {
                    for (int goalIdx = 0; goalIdx < goalCount; ++goalIdx)
                    {
                        int currentGoalX = goalX[goalIdx];
                        int currentGoalY = goalY[goalIdx];
                        isBoxOnGoal[goalIdx] = IsCollided(currentGoalX, currentGoalY, boxX, boxY, boxCount);
                    }
                }

                void CheckGameClear()
                {
                    isGameOver = Array.TrueForAll(isBoxOnGoal, elem => elem);
                }

                bool TryMovePlayer(Direction playerDirection)
                {
                    if (playerDirection == Direction.None)
                    {
                        return false;
                    }

                    var (newPlayerX, newPlayerY) = GetNewPositionByDirection(playerDirection, playerX, playerY);

                    // 플레이어와 벽의 충돌 처리
                    if (IsCollided(newPlayerX, newPlayerY, wallX, wallY, wallCount))
                    {
                        return false;
                    }

                    // 플레이어와 박스의 충돌 처리
                    // 1. 플레이어가 어떤 박스와 충돌했는지 찾는다.
                    const int NoCollidedBox = -1;
                    int collidedBoxIndex = GetCollidedIndex(newPlayerX, newPlayerY, boxX, boxY, boxCount);
                    if (collidedBoxIndex != NoCollidedBox)
                    {
                        // 2-1. 박스의 새로운 좌표를 구한다.
                        int currentBoxX = boxX[collidedBoxIndex];
                        int currentBoxY = boxY[collidedBoxIndex];
                        var (newBoxX, newBoxY) = GetNewPositionByDirection(playerDirection, currentBoxX, currentBoxY);

                        // 2-2. 벽과의 충돌 처리
                        if (IsCollided(newBoxX, newBoxY, wallX, wallY, wallCount))
                        {
                            return false;
                        }

                        // 2-3. 박스끼리의 충돌 처리
                        if (IsCollided(newBoxX, newBoxY, boxX, boxY, boxCount, collidedBoxIndex))
                        {
                            return false;
                        }

                        // 2-4. 박스의 좌표를 갱신한다.
                        boxX[collidedBoxIndex] = newBoxX;
                        boxY[collidedBoxIndex] = newBoxY;
                    }

                    // 플레이어의 좌표를 갱신한다.
                    playerX = newPlayerX;
                    playerY = newPlayerY;

                    return true;
                }
            }

            void ShowClearMessage()
            {
                Console.Clear();
                Console.WriteLine("축하합니다! 게임을 클리어하셨습니다!");
            }
        }
    }
}

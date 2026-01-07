

using System.Reflection.Metadata.Ecma335;

namespace Sokoban
{

    // 개체 지향 프로그래밍: 개체
    // ㄴ 상태와 행위를 가진 것

    // 어떤 데이터끼리 연관이 있는가?
    // 그 데이터를 조작하는 함수는 무엇인가?

    // 게임오브젝트
    // ㄴ플레이어
    // ㄴ벽
    // ㄴ박스
    // ㄴ골

    // 게임

    // X, Y를 묶은 개체 => Position
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

            Position mapMinSize = Position.At(0, 0);
            Position mapMaxSize = Position.At(10, 10);
            bool isGameOver = false;

            // 플레이어 데이터
            Position playerPosition = Position.At(5, 10);
            Direction playerDirection = Direction.None;

            // 벽 데이터
            Position[] wallPositions = new Position[5]
            {
                Position.At(3, 3),
                Position.At(4, 3),
                Position.At(5, 3),
                Position.At(6, 3),
                Position.At(7, 3),
            };
            int wallCount = wallPositions.Length;

            // 박스 데이터
            Position[] boxPositions = new Position[2]
            {
                Position.At(6, 6),
                Position.At(8, 7)
            };
            int boxCount = boxPositions.Length;

            // 골 데이터
            Position[] goalPositions = new Position[2]
            {
                Position.At(4, 5),
                Position.At(7, 10)
            };
            bool[] isBoxOnGoal = { false, false };
            int goalCount = goalPositions.Length;

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

                RenderObjects(boxPositions, boxCount, _ => "@");
                RenderObjects(goalPositions, goalCount, idx => isBoxOnGoal[idx] ? "*" : "O");
                RenderObject(playerPosition, "P");
                RenderObjects(wallPositions, wallCount, _ => "#");

                // ---------------------------------------------------

                void RenderObject(Position pos, string symbol)
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write(symbol);
                }

                // NOTE: 조건부 심볼이 있어 Func를 받는다.
                void RenderObjects(Position[] positions, int count, Func<int, string> symbolSelector)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        RenderObject(positions[i], symbolSelector(i));
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

                // 필드를 조작하는 함수는 Position을 조작하는 것도 상위 개체에서 만드는 게 옳더.
                Position GetNewPositionByDirection(Direction direction, Position old) => direction switch
                {
                    Direction.Left => Position.At(Math.Max(mapMinSize.X, old.X - 1), old.Y),
                    Direction.Right => Position.At(Math.Min(mapMaxSize.X, old.X + 1), old.Y),
                    Direction.Up => Position.At(old.X, Math.Max(mapMinSize.Y, old.Y - 1)),
                    Direction.Down => Position.At(old.X, Math.Min(mapMaxSize.Y, old.Y + 1))
                };

                int GetCollidedIndex(Position pos, Position[] targetPositions, int targetCount, int excludedIndex = -1)
                {
                    int found = -1;
                    for (int idx = 0; idx < targetCount; ++idx)
                    {
                        if (idx == excludedIndex)
                        {
                            continue;
                        }

                        if (pos == targetPositions[idx])
                        {
                            found = idx;
                            break;
                        }
                    }

                    return found;
                }

                bool IsCollided(Position pos, Position[] targetPositions, int targetCount, int excludedIndex = -1)
                {
                    return -1 != GetCollidedIndex(pos, targetPositions, targetCount, excludedIndex);
                }

                void UpdateGoalState()
                {
                    for (int goalIdx = 0; goalIdx < goalCount; ++goalIdx)
                    {
                        Position currentGoalPosition = goalPositions[goalIdx];
                        isBoxOnGoal[goalIdx] = IsCollided(currentGoalPosition, boxPositions, boxCount);
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

                    Position newPlayerPosition = GetNewPositionByDirection(playerDirection, playerPosition);

                    // 플레이어와 벽의 충돌 처리
                    if (IsCollided(newPlayerPosition, wallPositions, wallCount))
                    {
                        return false;
                    }

                    // 플레이어와 박스의 충돌 처리
                    // 1. 플레이어가 어떤 박스와 충돌했는지 찾는다.
                    const int NoCollidedBox = -1;
                    int collidedBoxIndex = GetCollidedIndex(newPlayerPosition, boxPositions, boxCount);
                    if (collidedBoxIndex != NoCollidedBox)
                    {
                        // 2-1. 박스의 새로운 좌표를 구한다.
                        Position currentBoxPosition = boxPositions[collidedBoxIndex];
                        Position newBoxPosition = GetNewPositionByDirection(playerDirection, currentBoxPosition);

                        // 2-2. 벽과의 충돌 처리
                        if (IsCollided(newBoxPosition, wallPositions, wallCount))
                        {
                            return false;
                        }

                        // 2-3. 박스끼리의 충돌 처리
                        if (IsCollided(newBoxPosition, boxPositions, boxCount, collidedBoxIndex))
                        {
                            return false;
                        }

                        // 2-4. 박스의 좌표를 갱신한다.
                        boxPositions[collidedBoxIndex] = newBoxPosition;
                    }

                    // 플레이어의 좌표를 갱신한다.
                    playerPosition = newPlayerPosition;

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

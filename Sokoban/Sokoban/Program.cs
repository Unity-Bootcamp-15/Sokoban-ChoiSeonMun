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
            Map map = new(minSize: Position.At(0, 0), maxSize: Position.At(10, 10));
            bool isGameOver = false;

            // 플레이어 데이터
            // ㄴ 1. 모든 데이터가 캡슐화 되지 않음.
            GameObject player = GameObjectFactory.Create(GameObjectType.Player, Position.At(5, 10));
            Direction playerDirection = Direction.None;

            // 벽 데이터
            List<GameObject> walls = new()
            {
                GameObjectFactory.Create(GameObjectType.Wall, Position.At(3, 3)),
                GameObjectFactory.Create(GameObjectType.Wall, Position.At(4, 3)),
                GameObjectFactory.Create(GameObjectType.Wall, Position.At(5, 3)),
                GameObjectFactory.Create(GameObjectType.Wall, Position.At(6, 3)),
                GameObjectFactory.Create(GameObjectType.Wall, Position.At(7, 3)),
            };
            
            // 박스 데이터
            List<GameObject> boxes = new()
            {
                GameObjectFactory.Create(GameObjectType.Box, Position.At(6, 6)),
                GameObjectFactory.Create(GameObjectType.Box, Position.At(8, 7)),
            };

            // 골 데이터
            List<GameObject> goals = new()
            {
                GameObjectFactory.Create(GameObjectType.Goal, Position.At(4, 5)),
                GameObjectFactory.Create(GameObjectType.Goal, Position.At(7, 10)),
            };
            bool[] isBoxOnGoal = { false, false };

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

                RenderObjects(boxes, _ => "@");
                RenderObjects(goals, idx => isBoxOnGoal[idx] ? "*" : "O");
                RenderObject(player.Position, "P");
                RenderObjects(walls, _ => "#");

                // ---------------------------------------------------

                void RenderObject(Position pos, string symbol)
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write(symbol);
                }

                // NOTE: 조건부 심볼이 있어 Func를 받는다.
                void RenderObjects(List<GameObject> objs, Func<int, string> symbolSelector)
                {
                    for (int i = 0; i < objs.Count; ++i)
                    {
                        RenderObject(objs[i].Position, symbolSelector(i));
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

                void UpdateGoalState()
                {
                    for (int goalIdx = 0; goalIdx < goals.Count; ++goalIdx)
                    {
                        GameObject currentGoal = goals[goalIdx];
                        isBoxOnGoal[goalIdx] = boxes.ExistsAt(currentGoal.Position);
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

                    Position deltaPosition = playerDirection.ToOffset();
                    Position newPlayerPosition = player.Position + deltaPosition;
                    if (map.IsOutOfRange(newPlayerPosition))
                    {
                        return false;
                    }

                    // 플레이어와 벽의 충돌 처리
                    if (walls.ExistsAt(newPlayerPosition))
                    {
                        return false;
                    }

                    // 플레이어와 박스의 충돌 처리
                    // 1. 플레이어가 어떤 박스와 충돌했는지 찾는다.
                    const int NoCollidedBox = -1;
                    // 플레이어의 위치랑 박스랑 충돌했는지?
                    int collidedBoxIndex = boxes.IndexAt(newPlayerPosition);
                    if (collidedBoxIndex != NoCollidedBox)
                    {
                        // 2-1. 박스의 새로운 좌표를 구한다.
                        GameObject currentBox = boxes[collidedBoxIndex];
                        Position currentBoxPosition = currentBox.Position;
                        Position boxDeltaPosition = playerDirection.ToOffset();
                        Position newBoxPosition = currentBoxPosition + boxDeltaPosition;
                        if (map.IsOutOfRange(newBoxPosition))
                        {
                            return false;
                        }

                        // 2-2. 벽과의 충돌 처리
                        if (walls.ExistsAt(newBoxPosition))
                        {
                            return false;
                        }

                        // 2-3. 박스끼리의 충돌 처리
                        if (boxes.ExistsAt(newBoxPosition, collidedBoxIndex))
                        {
                            return false;
                        }

                        // 2-4. 박스의 좌표를 갱신한다.
                        currentBox.Position = newBoxPosition;
                    }

                    // 플레이어의 좌표를 갱신한다.
                    player.Position = newPlayerPosition;

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

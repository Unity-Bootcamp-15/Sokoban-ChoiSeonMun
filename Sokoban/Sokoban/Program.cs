using System.Diagnostics;

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


            // 플레이어 데이터
            GameObject player = GameObjectFactory.Create(GameObjectType.Player, Position.At(5, 10));

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

            // 게임 데이터 초기화
            IEnumerable<GameObject> obstacles = boxes.Concat(walls).Append(player);
            Map map = new(minSize: Position.At(0, 0), maxSize: Position.At(10, 10), obstacles);
            bool isGameOver = false;

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

                boxes.ForEach(x => RenderObject(x));
                goals.ForEach(x => RenderObject(x));
                RenderObject(player);
                walls.ForEach(x => RenderObject(x));

                // ---------------------------------------------------

                // Render하는 코드
                void RenderObject(IRenderingObject obj)
                {
                    Console.SetCursorPosition(obj.X, obj.Y);
                    Console.Write(obj.Symbol);
                }
            }

            ConsoleKeyInfo ProcessInput() => Console.ReadKey();

            void Update(ConsoleKeyInfo keyInfo)
            {
                Direction direction = GetDirectionFrom(keyInfo);
                if (direction == Direction.None)
                {
                    return;
                }

                if (map.TryMove(player, direction))
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
                        Goal? currentGoal = goals[goalIdx] as Goal;
                        Debug.Assert(currentGoal != null);

                        currentGoal.HasBox = boxes.ExistsAt(currentGoal.Position);
                    }
                }

                void CheckGameClear()
                {
                    isGameOver = goals.TrueForAll(x =>
                    {
                        Goal? goal = x as Goal;
                        Debug.Assert(goal != null);
                        return goal.HasBox;
                    });
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

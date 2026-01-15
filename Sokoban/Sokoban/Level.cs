using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Level
    {
        public GameObject Player { get; set; }
        public List<GameObject> Walls { get; set; }
        public List<GameObject> Boxes { get; set; }
        public List<GameObject> Goals { get; set; }

        public Map Map { get; set; }

        public IEnumerable<GameObject> Obstacles => Boxes.Concat(Walls);
        public IEnumerable<GameObject> AllObject => Obstacles.Concat(Goals).Append(Player);

        public bool TryMovePlayer(Direction direction) => Map.TryMove(Player, direction);

        public void UpdateGoalState()
        {
            foreach (Goal goal in Goals.Cast<Goal>())
            {
                goal.HasBox = Boxes.ExistsAt(goal.Position);
            }
        }

        public bool CheckClear()
        {
            return Goals.Cast<Goal>().All(goal => goal.HasBox);
        }

        public static Level CreateFirst()
        {
            Level level = new Level()
            {
                Player = GameObjectFactory.Create(GameObjectType.Player, Position.At(5, 10)),
                Boxes = new List<GameObject>()
                {
                    GameObjectFactory.Create(GameObjectType.Box, Position.At(6, 6)),
                    GameObjectFactory.Create(GameObjectType.Box, Position.At(8, 7)),
                },
                Walls = new List<GameObject>()
                {
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(3, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(4, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(5, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(6, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(7, 3)),
                },
                Goals = new List<GameObject>()
                {
                    GameObjectFactory.Create(GameObjectType.Goal, Position.At(4, 5)),
                    GameObjectFactory.Create(GameObjectType.Goal, Position.At(7, 10)),
                }
            };
            level.Map = new(minSize: Position.At(0, 0), maxSize: Position.At(10, 10), level.Obstacles);

            return level;
        }
    }
}

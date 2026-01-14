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
    }
}

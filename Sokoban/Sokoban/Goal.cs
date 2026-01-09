using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public sealed class Goal : GameObject
    {
        public bool HasBox { get; set; }
        public override string Symbol => HasBox ? Config.GoalHasBox : Config.GoalHasNoBox;

        public Goal(Position pos)
            : base(pos, Config.GoalHasNoBox)
        {
            HasBox = false;
        }
    }
}

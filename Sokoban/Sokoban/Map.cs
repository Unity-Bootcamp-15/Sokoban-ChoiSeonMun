using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Map
    {
        private Position _minSize;
        private Position _maxSize;

        public Map(Position minSize, Position maxSize)
        {
            _minSize = minSize;
            _maxSize = maxSize;
        }

        public bool IsOutOfRange(Position pos)
        {
            bool isOutOfRangeX = pos.X < _minSize.X || pos.X > _maxSize.X;
            bool isOutOfRangeY = pos.Y < _minSize.Y || pos.Y > _maxSize.Y;

            return isOutOfRangeX || isOutOfRangeY;
        }
    }
}

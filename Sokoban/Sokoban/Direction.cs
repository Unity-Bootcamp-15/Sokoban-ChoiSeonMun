using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public static class DirectionExtensions
    {
        public static Position ToOffset(this Direction direction) => direction switch
        {
            Direction.Left => Position.At(-1, 0),
            Direction.Right => Position.At(1, 0),
            Direction.Up => Position.At(0, -1),
            Direction.Down => Position.At(0, 1),
            Direction.None => Position.At(0, 0)
        };
    }
}

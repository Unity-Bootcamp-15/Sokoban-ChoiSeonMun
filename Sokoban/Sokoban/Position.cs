using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sokoban
{
    // Position 타입끼리의 비교
    internal struct Position : IEquatable<Position>
    {
        private int _x;
        private int _y;

        public int X => _x;
        public int Y => _y;

        private Position(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static Position At(int x, int y)
        {
            return new Position(x, y);
        }

        public bool Equals(Position other)
        {
            return (X == other.X && Y == other.Y);
        }


        // 같음 연산자 ==
        public static bool operator ==(Position left, Position right) => left.Equals(right);
        public static bool operator !=(Position left, Position right) => !(left == right);
    }
}

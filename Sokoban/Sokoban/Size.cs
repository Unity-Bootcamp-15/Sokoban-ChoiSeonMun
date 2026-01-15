using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public struct Size : IEquatable<Size>
    {
        public static readonly Size Zero = new Size(0, 0);

        public int Width { get; init; }
        public int Height { get; init; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public static bool operator==(Size lhs, Size rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(Size lhs, Size rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}

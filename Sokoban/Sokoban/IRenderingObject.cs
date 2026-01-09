using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public interface IRenderingObject
    {
        int X { get; }
        int Y { get; }
        string Symbol { get; }
    }
}

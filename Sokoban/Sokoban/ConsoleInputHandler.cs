using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class ConsoleInputHandler : IInputHandler
    {
        private ConsoleKeyInfo _keyInfo;

        public Direction GetDirection() => _keyInfo.Key switch
        {
            ConsoleKey.DownArrow => Direction.Down,
            ConsoleKey.UpArrow => Direction.Up,
            ConsoleKey.LeftArrow => Direction.Left,
            ConsoleKey.RightArrow => Direction.Right,
            _ => Direction.None
        };


        public void ProcessInput()
        {
            _keyInfo = Console.ReadKey();
        }
    }
}

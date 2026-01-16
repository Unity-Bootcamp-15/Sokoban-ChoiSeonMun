using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class ConsoleRenderer : IRenderer
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void Prepare()
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Title = "My Sokoban";
            Console.CursorVisible = false;
            Console.Clear();
        }

        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Render(IEnumerable<GameObject> allObjects)
        {
            foreach (GameObject obj in allObjects)
            {
                Console.SetCursorPosition(obj.X, obj.Y);
                Console.Write(obj.Symbol);
            }
        }
    }
}

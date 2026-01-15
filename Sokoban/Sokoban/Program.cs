using System.Diagnostics;

namespace Sokoban
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Level firstLevel = Level.CreateFirst();
            IInputHandler inputHandler = new ConsoleInputHandler();
            IRenderer renderer = new ConsoleRenderer(firstLevel.AllObject);
            renderer.Prepare();

            Game game = new Game(inputHandler, renderer, firstLevel);
            game.Run();
        }
    }
}

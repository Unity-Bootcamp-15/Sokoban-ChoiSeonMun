using System.Diagnostics;

namespace Sokoban
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Level firstLevel = new Level()
            {
                Player = GameObjectFactory.Create(GameObjectType.Player, Position.At(5, 10)),
                Boxes = new List<GameObject>()
                {
                    GameObjectFactory.Create(GameObjectType.Box, Position.At(6, 6)),
                    GameObjectFactory.Create(GameObjectType.Box, Position.At(8, 7)),
                },
                Walls = new List<GameObject>()
                {
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(3, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(4, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(5, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(6, 3)),
                    GameObjectFactory.Create(GameObjectType.Wall, Position.At(7, 3)),
                },
                Goals = new List<GameObject>()
                {
                    GameObjectFactory.Create(GameObjectType.Goal, Position.At(4, 5)),
                    GameObjectFactory.Create(GameObjectType.Goal, Position.At(7, 10)),
                }
            };
            firstLevel.Map = new(minSize: Position.At(0, 0), maxSize: Position.At(10, 10), firstLevel.Obstacles);

            IInputHandler inputHandler = new ConsoleInputHandler();
            IRenderer renderer = new ConsoleRenderer(firstLevel.AllObject);
            renderer.Prepare();

            Game game = new Game(inputHandler, renderer, firstLevel);
            game.Run();
        }
    }
}

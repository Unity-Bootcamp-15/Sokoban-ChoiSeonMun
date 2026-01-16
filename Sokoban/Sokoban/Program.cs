using System.Diagnostics;

namespace Sokoban
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<LevelData> levelDatas = LevelLoader.ReadAllLevel();
            IEnumerable<Level> levels = levelDatas.Select(data => new Level(data));

            IInputHandler inputHandler = new ConsoleInputHandler();
            IRenderer renderer = new ConsoleRenderer();
            renderer.Prepare();

            // Game이 직접 Level을 그때그때마다 로드할 것이냐
            // 초반에 다 로드 후에 Game에게 넘겨줄 것이냐

            Game game = new Game(inputHandler, renderer, levels);
            game.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public static class LevelLoader
    {
        public static IEnumerable<LevelData> ReadAllLevel()
        {
            List<LevelData> levelDatas = new();
            for (int level = Config.MinLevel; level <= Config.MaxLevel; level++)
            {
                levelDatas.Add(ReadLevel(level));
            }

            return levelDatas;
        }

        private static LevelData ReadLevel(int level)
        {
            const string kLevelPath = $"Level";

            Debug.Assert(Config.MinLevel <= level && level <= Config.MaxLevel);
            
            LevelData levelData = new();
            string[] content = File.ReadAllLines($"{kLevelPath}{level}.txt");
            
            int width = 0;
            int height = content.Length;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < content[y].Length; ++x)
                {
                    switch (content[y][x])
                    {
                        case '#':
                            levelData.Wall.Add(Position.At(x, y));
                            break;
                        case 'P':
                            levelData.Player = Position.At(x, y);
                            break;
                        case '@':
                            levelData.Box.Add(Position.At(x, y));
                            break;
                        case 'O':
                            levelData.Goal.Add(Position.At(x, y));
                            break;
                        default:
                            break;
                    }
                }

                width = Math.Max(width, content[y].Length);
            }
            levelData.MapSize = new Size(width, height);

            return levelData;
        }
    }
}

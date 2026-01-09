using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class GameObject : IRenderingObject
    {
        private string _symbol;

        public Position Position { get; set; }
        public virtual string Symbol => _symbol;

        public int X => Position.X;

        public int Y => Position.Y;

        public GameObject(Position position, string symbol)
        {
            Position = position;
            _symbol = symbol;
        }
    }
    
    public static class GameObjectExtensions
    {
        public static bool ExistsAt(this List<GameObject> gameObjects, Position targetPosition, int excludedIndex = -1)
        {
            return -1 != gameObjects.IndexAt(targetPosition, excludedIndex);
        }

        public static int IndexAt(this List<GameObject> gameObjects, Position targetPosition, int excludedIndex = -1)
        {
            for (int idx = 0; idx < gameObjects.Count; ++idx)
            {
                if (excludedIndex != idx && gameObjects[idx].Position == targetPosition)
                {
                    return idx;
                }
            }
            
            return -1;
        }
    }

    public enum GameObjectType
    {
        None,
        Wall,
        Player,
        Box,
        Goal
    }

    public static class GameObjectFactory
    {
        public static GameObject Create(GameObjectType type, Position pos) => type switch
        {
            GameObjectType.Wall => new GameObject(pos, symbol: Config.Wall),
            GameObjectType.Player => new GameObject(pos, symbol: Config.Player),
            GameObjectType.Box => new GameObject(pos, symbol: Config.Box),
            GameObjectType.Goal => new Goal(pos),
            _ => throw new ArgumentException()
        };
    }
}

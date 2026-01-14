using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class GameObject
    {
        private readonly string _symbol;

        public Position Position { get; set; }
        public virtual string Symbol => _symbol;

        public int X => Position.X;

        public int Y => Position.Y;

        public bool IsMovable { get; init; }
        public bool CanPushOut { get; init; }

        public GameObject(Position position, string symbol, bool isMovable, bool canPushOut = false)
        {
            Position = position;
            _symbol = symbol;
            IsMovable = isMovable;
            CanPushOut = canPushOut;
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
            GameObjectType.Wall => new GameObject(pos, symbol: Config.Wall, isMovable: false, canPushOut: false),
            GameObjectType.Player => new GameObject(pos, symbol: Config.Player, isMovable: true, canPushOut: true),
            GameObjectType.Box => new GameObject(pos, symbol: Config.Box, isMovable: true),
            GameObjectType.Goal => new Goal(pos),
            _ => throw new ArgumentException()
        };
    }
}

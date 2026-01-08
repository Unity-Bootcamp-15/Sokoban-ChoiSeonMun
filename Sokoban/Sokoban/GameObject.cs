using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class GameObject
    {
        private string _symbol;

        public Position Position { get; set; }
        public string Symbol => _symbol;

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
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    // GameObject GameObject?

    // Map -> GameObject
    // 
    public class Map
    {
        private Position _minSize;
        private Position _maxSize;
        private IEnumerable<GameObject> _obstacles;

        public Map(Position minSize, Position maxSize, IEnumerable<GameObject> obstacles)
        {
            _minSize = minSize;
            _maxSize = maxSize;
            _obstacles = obstacles;
        }

        public bool TryMove(GameObject objectToMove, Direction direction)
        {
            // 1. 새로운 위치를 계산한다.
            Position newPos = objectToMove.Position + direction.ToOffset();

            // 2. 새 위치가 맵을 벗어났는지 확인한다.
            if (IsOutOfRange(newPos))
            {
                return false;
            }

            // 3. objectToMove가 다른 게임오브젝트와 충돌했는지 확인한다.
            GameObject? collidedObject = _obstacles.FirstOrDefault(
                // (1) 나는 제외한다.
                x => x != objectToMove &&
                // (2) 새로운 위치에 있는가?
                x.Position == newPos);

            if (collidedObject != null)
            {
                if (collidedObject.IsMovable && objectToMove.CanPushOut)
                {
                    if (false == TryMove(collidedObject, direction))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // 4. 새로운 위치로 objectToMove의 Position을 설정한다.
            objectToMove.Position = newPos;
            return true;
        }

        private bool IsOutOfRange(Position pos)
        {
            bool isOutOfRangeX = pos.X < _minSize.X || pos.X > _maxSize.X;
            bool isOutOfRangeY = pos.Y < _minSize.Y || pos.Y > _maxSize.Y;

            return isOutOfRangeX || isOutOfRangeY;
        }
    }
}

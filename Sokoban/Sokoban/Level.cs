using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    // DTO(Data Transfer Object)
    public class LevelData
    {
        public Position Player { get; set; }
        public List<Position> Wall { get; set; } = new();
        public List<Position> Box { get; set; } = new();
        public List<Position> Goal { get; set; } = new();
        public Size MapSize { get; set; }
    }

    public class Level
    {
        private readonly GameObject _player;
        private readonly List<GameObject> _walls;
        private readonly List<GameObject> _boxes;
        private readonly List<GameObject> _goals;
        private readonly Map _map;

        public IEnumerable<GameObject> AllObject => _walls.Concat(_boxes).Concat(_goals).Append(_player);

        public Level(LevelData data)
        {
            if (data.Box.Count < data.Goal.Count)
            {
                throw new ArgumentException("Box는 Goal의 개수보다 같거나 많아야 합니다.");
            }
            if (data.MapSize == Size.Zero)
            {
                throw new ArgumentException("MapSize는 (0, 0)일 수 없습니다.");
            }

            _player = GameObjectFactory.Create(GameObjectType.Player, data.Player);
            _walls = data.Wall.ConvertAll(wallPos => GameObjectFactory.Create(GameObjectType.Wall, wallPos));
            _boxes = data.Box.ConvertAll(boxPos => GameObjectFactory.Create(GameObjectType.Box, boxPos));
            _goals = data.Goal.ConvertAll(goalPos => GameObjectFactory.Create(GameObjectType.Goal, goalPos));

            IEnumerable<GameObject> obstacles = _boxes.Concat(_walls);
            _map = new(data.MapSize, obstacles);
        }

        public bool TryMovePlayer(Direction direction) => _map.TryMove(_player, direction);

        public void UpdateGoalState()
        {
            foreach (Goal goal in _goals.Cast<Goal>())
            {
                goal.HasBox = _boxes.ExistsAt(goal.Position);
            }
        }

        public bool CheckClear()
        {
            return _goals.Cast<Goal>().All(goal => goal.HasBox);
        }
    }
}

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
        public List<Position> Wall { get; set; }
        public List<Position> Box { get; set; }
        public List<Position> Goal { get; set; }
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

        private Level(LevelData data)
        {
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

        public static Level Create(Action<LevelData> configure)
        {
            LevelData data = new();

            configure(data);

            if (data.Box == null || data.Goal == null)
            {
                throw new ArgumentNullException("Box나 Goal은 필수적으로 설정되어야 합니다.");
            }
            if (data.Box.Count < data.Goal.Count)
            {
                throw new ArgumentException("Box는 Goal의 개수보다 같거나 많아야 합니다.");
            }
            if (data.MapSize == Size.Zero)
            {
                throw new ArgumentException("MapSize는 (0, 0)일 수 없습니다.");   
            }

            data.Wall = data.Wall ?? new();

            return new Level(data);
        }

        public static Level CreateFirst()
        {
            return Create(data =>
            {
                data.Player = Position.At(5, 10);
                data.Box = new(){ Position.At(6, 6), Position.At(8, 7) };
                data.Wall = new() { Position.At(3, 3), Position.At(4, 3), Position.At(5, 3), Position.At(6, 3), Position.At(7, 3) };
                data.Goal = new() { Position.At(4, 5), Position.At(7, 10) };
                data.MapSize = new(10, 10);
            });
        }
    }
}

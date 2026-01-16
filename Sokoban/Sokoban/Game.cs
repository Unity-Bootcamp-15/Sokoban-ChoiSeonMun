using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Game
    {
        private IInputHandler _inputHandler;
        private IRenderer _renderer;
        private IEnumerable<Level> _levels;
        private bool _isGameOver = false;

        public Game(IInputHandler inputHandler, IRenderer renderer, IEnumerable<Level> levels)
        {
            _inputHandler = inputHandler;
            _renderer = renderer;
            _levels = levels;
        }

        public void Run()
        {
            foreach (Level currentLevel in _levels)
            {
                while (_isGameOver == false)
                {
                    Render(currentLevel);
                    ProcessInput();
                    Update(currentLevel);
                }

                ShowNoticeToGoToNextLevel();
                ChangeNextLevel();
            }
            
            ShowClearMessage();

            // ---------------------------------------------------
            void Render(Level level)
            {
                _renderer.Clear();

                _renderer.Render(level.AllObject);
            }

            void ProcessInput() => _inputHandler.ProcessInput();

            void ShowClearMessage()
            {
                _renderer.Clear();
                _renderer.PrintMessage(Config.ClearMessage);
            }

            void ShowNoticeToGoToNextLevel()
            {
                _renderer.Clear();
                _renderer.PrintMessage(Config.NextLevelNoticeMessage);
            }

            void ChangeNextLevel()
            {
                _isGameOver = false;

                // NOTE: 사용자로부터 입력을 받은 후에 다음 레벨로 넘어간다.
                _inputHandler.ProcessInput();
            }

            void Update(Level level)
            {
                Direction direction = _inputHandler.GetDirection();
                if (direction == Direction.None)
                {
                    return;
                }

                if (level.TryMovePlayer(direction))
                {
                    level.UpdateGoalState();
                    _isGameOver = level.CheckClear();
                }
            }
        }
    }
}

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
        private Level _level;
        private bool _isGameOver = false;

        public Game(IInputHandler inputHandler, IRenderer renderer, Level level)
        {
            _inputHandler = inputHandler;
            _renderer = renderer;
            _level = level;
        }

        public void Run()
        {
            while (_isGameOver == false)
            {
                Render();
                ProcessInput();
                Update();
            }

            ShowClearMessage();

            // ---------------------------------------------------
            void Render()
            {
                _renderer.Clear();

                _renderer.Render();
            }

            void ProcessInput() => _inputHandler.ProcessInput();

            void ShowClearMessage()
            {
                _renderer.Clear();
                _renderer.PrintMessage(Config.ClearMessage);
            }

            void Update()
            {
                Direction direction = _inputHandler.GetDirection();
                if (direction == Direction.None)
                {
                    return;
                }

                if (_level.TryMovePlayer(direction))
                {
                    _level.UpdateGoalState();
                    _isGameOver = _level.CheckClear();
                }
            }
        }
    }
}

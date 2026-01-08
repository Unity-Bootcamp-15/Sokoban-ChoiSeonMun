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
}

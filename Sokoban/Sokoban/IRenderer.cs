using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public interface IRenderer
    {
        void Clear();
        void Render(IEnumerable<GameObject> allObjects);
        void PrintMessage(string message);

        void Prepare();
    }
}

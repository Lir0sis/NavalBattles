using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class Mesh
    {
        public string[,] mesh;
        public Mesh(int startX, int startY, int endX, int endY)
        {
            mesh = new string[endX - startX, endY - startX];
        }
    }
}

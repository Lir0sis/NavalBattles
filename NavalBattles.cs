using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class NavalBattles
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "NavalBattles";

            Mesh mesh = new Mesh();
            mesh.DrawGameBoard(1, 1);

        }

        


    }
}

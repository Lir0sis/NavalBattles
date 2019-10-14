using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class Mesh
    {
        public string[,] Vmesh;
        public Ship[,] Gmesh;
        public Mesh(int startX, int startY, int endX, int endY)
        {
            Vmesh = new string[endX + 1 - startX, endY + 1 - startY];
            //Console.WriteLine($"{Vmesh.GetLength(0)} {Vmesh.GetLength(1)}");
        }
        public Mesh(int sizeX, int sizeY)
        {
            Gmesh = new Ship[sizeX, sizeY];
        }

        private string SetCursor(int x, int y)
        { 
            return "\u001b[" + x + ";" + y + "H";
        }

        public void SetMesh()
        {
            int lastX = Vmesh.GetLength(0) - 1, lastY = Vmesh.GetLength(1) - 1;

            for (int i = 0; i < lastX + 1; i++)
            {
                for (int j = 0; j < lastY + 1; j++)
                {
                    int I = i + 1, J = j + 1;
                    if(i == 0 && j != 0 && j != lastY)
                        Vmesh[i, j] = $"{SetCursor(I, J)}=";
                    else if (i != 0 && j == 0 && i != lastX)
                        Vmesh[i, j] = $"{SetCursor(I, J)}\u2503";
                    else if (i == lastX && j != 0 && j != lastY)
                        Vmesh[i, j] = $"{SetCursor(I, J)}=";
                    else if (i != 0 && j == lastY && i != lastX)
                        Vmesh[i, j] = $"{SetCursor(I, J)}\u2503";
                    else if (i == 0 && j == 0)
                        Vmesh[i, j] = $"{SetCursor(I, J)}\u250f";
                    else if (i == 0 && j == lastY)
                        Vmesh[i, j] = $"{SetCursor(I, J)}\u2513";
                    else if (i == lastX && j == 0)
                        Vmesh[i, j] = $"{SetCursor(I, J)}\u2517";
                    else if (i == lastX && j == lastY)
                        Vmesh[i, j] = $"{SetCursor(I, J)}\u251b";
                   
                }

            }
        }

        public void DrawMesh()
        {
            for (int i = 0; i < Vmesh.GetLength(0); i++)
            {
                for (int j = 0; j < Vmesh.GetLength(1); j++)
                {
                    //Console.WriteLine($"{i} {j}");
                    Console.Write(Vmesh[i, j]);
                }
            }
        }
    }
}

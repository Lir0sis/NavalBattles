using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class Mesh
    {
        public string[,] gameBoard;

        public Mesh()
        {
            gameBoard = new string[10, 10];

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    gameBoard[i, j] = "E";

        }

        public void SetCursor(int x, int y)
        { 
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }
        public void SetCursor() 
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
        }

        public void DrawGameBoard(int x, int y) 
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    SetCursor((i + x) * 2, (j + y));
                    Console.Write(gameBoard[i, j]);
                }
                    
        }

       /* public void SetMesh22()
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
        }*/

    }
}

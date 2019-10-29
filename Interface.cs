using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    public class Interface
    {
        public Mesh UsrMesh, EnemyMesh;

        public Interface()
        {
            
            UsrMesh = new Mesh();
            EnemyMesh = new Mesh(true);
        }

        public void DrawInterface(int usrX = 4, int usrY = 4, int enemyX = 32, int enemyY = 4)
        {
            EnemyMesh.x = enemyX + 2;
            EnemyMesh.y = enemyY + 2;
            UsrMesh.x = usrX + 2;
            UsrMesh.y = usrY + 2;
            
            usrX += (usrX % 2 != 0 ? 1 : 0);
            usrY += (usrY % 2 != 0 ? 1 : 0);

            enemyX += (enemyX % 2 != 0 ? 1 : 0);
            enemyY += (enemyY % 2 != 0 ? 1 : 0);

            DrawWindowBase(usrX, usrY/2, usrX + 22, usrY/2 + 11);
            DrawWindowBase(enemyX, enemyY/2 , enemyX + 22, enemyY/2 + 11);
            DrawWindowBase(4, 15, 54, 20);

            //Console.Clear();

            UsrMesh.DrawGameBoard(usrX + 2, usrY + 2, false);
            EnemyMesh.DrawGameBoard(enemyX + 2, enemyY + 2, true);

            Mesh.SetCursor();
        }

        public static void DrawWindowBase(int X1, int Y1, int X2, int Y2)
        {
            int length = X2 - X1, height = Y2 - Y1;
            for (int i = 0; i < length + 1; i++)
                for (int j = 0; j < height + 1; j++)
                {
                    string toWrite = "";
                    Mesh.SetCursor(i + X1, j + Y1);

                    if (i == 0 && j == 0 || i == length && j == height || i == 0 && j == height || i == length && j == 0)
                        toWrite = "\u25A0";
                    else if (i == length || i == 0 && j > 0 && j < height)
                        toWrite = "\u2016";
                    else if (i > 0 && i < length && j == 0 || j == height)
                        toWrite = "=";

                    Console.Write(toWrite);
                }
            Mesh.SetCursor();
        }

        public void DrawSetupStatus(User player)
        {
            int[,] Borders = { { 5, 14 }, { 53, 19 } };


        }
    }

    public class Mesh
    {
        public string[,] gameBoard;
        public int x, y;
        public bool isEnemy;

        public Mesh(bool isEnemy = false)
        {
            this.isEnemy = isEnemy;
            gameBoard = new string[10, 10];

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    gameBoard[i, j] = "E";
        }

        public static void SetCursor(int x = 0, int y = 21, int spacingForX = 1, ConsoleColor color = ConsoleColor.White, ConsoleColor Bcolor = ConsoleColor.Black)
        {
            Console.BackgroundColor = Bcolor;
            Console.ForegroundColor = color;
            Console.CursorLeft = x * spacingForX;
            Console.CursorTop = y;
        }

        public void DrawGameBoard(int offsetX, int offsetY, bool isEnemy = false, bool drawSymbols = true) 
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    string toWrite = "";
                    SetCursor(i + offsetX/2, j + offsetY/2, 2);
                    if (drawSymbols)
                        switch (gameBoard[i, j].ToCharArray()[0].ToString())
                        {
                            case "E":
                                toWrite = "~";
                                break;
                            case "D":
                                toWrite = "X";
                                break;
                            case "S":
                                //if (isEnemy) Console.BackgroundColor = ConsoleColor.Red;
                                toWrite = (isEnemy ? "~" : "\u25A0");
                                break;
                            case "M":
                                toWrite = "\u2022";
                                break;
                        }
                    else
                        toWrite = gameBoard[i, j];
                    Console.Write(toWrite);
                    //Console.BackgroundColor = ConsoleColor.Black;
                }
        }

        public void DrawBoardCell(int cellX, int cellY, bool isEnemy = false, ConsoleColor color = ConsoleColor.White, ConsoleColor Bcolor = ConsoleColor.Black) 
        {
            SetCursor(x + cellX * 2, y - 3 + cellY, 1, color, Bcolor);
            string toWrite = "";
            switch (gameBoard[cellX, cellY].ToCharArray()[0].ToString())
            {
                case "E":
                    toWrite = "~";
                    break;
                case "D":
                    toWrite = "X";
                    break;
                case "S":
                    //if (isEnemy) Console.BackgroundColor = ConsoleColor.Red;
                    toWrite = (isEnemy ? "~" : "\u25A0");
                    break;
                case "M":
                    toWrite = "\u2022";
                    break;
            }
            Console.Write(toWrite);
            //Console.BackgroundColor = ConsoleColor.Black;
            SetCursor(x + cellX * 2, y - 3 + cellY, 1, color, Bcolor);
        }
    }
}

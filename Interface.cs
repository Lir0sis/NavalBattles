﻿using System;
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
            EnemyMesh = new Mesh();
        }

        public void DrawInterface(int usrX = 4, int usrY = 4, int enemyX = 32, int enemyY = 4)
        {
            EnemyMesh.x = enemyX + 2;
            EnemyMesh.y = enemyY + 2;
            UsrMesh.x = usrX + 2;
            UsrMesh.y = usrY + 2;
            
            usrX += (usrX % 2 != 0 ? 1 : 0);
            usrY += (usrY % 2 != 0 ? 1 : 0);

            DrawWindowBase(usrX, usrY/2, usrX + 22, usrY/2 + 11);
            DrawWindowBase(enemyX, enemyY/2 , enemyX + 22, enemyY/2 + 11);
            DrawWindowBase(4, 15, 54, 20);

            UsrMesh.DrawGameBoard(usrX + 2, usrY + 2, true);
            EnemyMesh.DrawGameBoard(enemyX + 2, enemyY + 2);

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


    }

    public class Mesh
    {
        public string[,] gameBoard;
        public int x, y;

        public Mesh()
        {
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

        public void DrawGameBoard(int offsetX, int offsetY, bool isEnemy = false) 
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    string toWrite = "";
                    SetCursor(i + offsetX/2, j + offsetY/2, 2);

                    switch (gameBoard[i, j]) 
                    {
                        case "E":
                            toWrite = "-";
                            break;
                        case "D":
                            toWrite = "X";
                            break;
                        case "S":
                            toWrite = (isEnemy ? "~" : "\u25A0");
                            break;
                        case "M":
                            toWrite = ".";
                            break;
                    }

                    Console.Write(toWrite);
                }
        }

        public void DrawBoardCell(int cellX, int cellY, ConsoleColor color = ConsoleColor.White, ConsoleColor Bcolor = ConsoleColor.Black) 
        {
            SetCursor(x + cellX * 2, y - 3 + cellY, 1, color, Bcolor);
            string toWrite = "";
            switch (gameBoard[cellX, cellY])
            {
                case "E":
                    toWrite = "-";
                    break;
                case "D":
                    toWrite = "X";
                    break;
                case "S":
                    toWrite = "\u25A0";
                    break;
                case "M":
                    toWrite = ".";
                    break;
            }
            Console.Write(toWrite);
            SetCursor(x + cellX * 2, y - 3 + cellY, 1, color, Bcolor);
        }
    }
}

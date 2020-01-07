using System;
using System.Threading;

namespace NavalBattles
{
    public class Interface
    {
        public Mesh UsrMesh, EnemyMesh;
        bool isSecond;

        public Interface(bool isSecond = false)
        {
            this.isSecond = isSecond;
            UsrMesh = new Mesh();
            EnemyMesh = new Mesh(true);
        }

        public void DrawInterface(int usrX = 4, int usrY = 7, int enemyX = 32, int enemyY = 7)
        {
            EnemyMesh.x = enemyX + 2;
            EnemyMesh.y = enemyY + 2 - 1;
            UsrMesh.x = usrX + 2;
            UsrMesh.y = usrY + 2  - 1;
            
            usrX += (usrX % 2 != 0 ? 1 : 0);
            usrY += (usrY % 2 != 0 ? 1 : 0);

            enemyX += (enemyX % 2 != 0 ? 1 : 0);
            enemyY += (enemyY % 2 != 0 ? 1 : 0);

            DrawWindowBase(usrX, usrY/2, usrX + 22, usrY/2 + 11);
            DrawWindowBase(enemyX, enemyY/2 , enemyX + 22, enemyY/2 + 11);
            DrawWindowBase(usrX/2 + 17, 1, enemyX + 22 - usrX/2 - 13 , 3);
            DrawWindowBase(4, 17, 54, 22);
            DrawWindowBase(4, 19, 54, 22);
            DrawWindowBase(4, 19, 29, 22);
            DrawWindowBase(usrX , usrY - 6, usrX + 4, usrY - 4);
            DrawWindowBase(enemyX + 22 - 4, enemyY - 6, enemyX + 22, enemyY -4);
            

            //Console.Clear();

            UsrMesh.DrawGameBoard(UsrMesh.x, UsrMesh.y);
            EnemyMesh.DrawGameBoard(EnemyMesh.x, EnemyMesh.y, true, false);

            Mesh.SetCursor(usrX + 2, usrY - 5, 1, ConsoleColor.Yellow);
            Console.Write(Convert.ToInt32(isSecond) + 1);

            Mesh.SetCursor(enemyX + 22 - 2, enemyY - 5, 1, ConsoleColor.Yellow);
            Console.Write(Convert.ToInt32(!isSecond) + 1);

            Mesh.SetCursor();
        }

        public static void Pause(User player, string message = "")
        {
            Console.Clear();
            DrawWindowBase(Console.BufferWidth / 2 - 11, Console.BufferHeight / 2 - 1, Console.BufferWidth / 2 + 11, Console.BufferHeight / 2 + 1) ;
            if( message == "")
            {
                Mesh.SetCursor(Console.BufferWidth / 2 - 8, Console.BufferHeight / 2);
                Console.Write($"Очередь Игрока №{Convert.ToInt32(player.isSecond) + 1}");
            }
            else
            {
                Mesh.SetCursor(Console.BufferWidth / 2 - 8, Console.BufferHeight / 2);
                Console.Write(message);
            }
            Console.ReadKey();
            Console.Clear();
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

        public void CurrentPlayer(User player)
        {
            Mesh.SetCursor(23, 2);
            Console.WriteLine($"Ход Игрока №{Convert.ToInt32(player.isSecond) + 1}");
            Mesh.SetCursor();
        }
        
        static public void WriteAction(string message = "", ConsoleColor color = ConsoleColor.White)
        {
            Mesh.SetCursor(6, 18);
            Console.Write("                                                ");
            Mesh.SetCursor(6, 18, 1, color);
            Console.Write(message);
        }

        public static void UpdatePlayersStatus(User player1 = null, User player2 = null)
        {
            int count4 = 0, count3 = 0, count2 = 0, count1 = 0;

            if (player1 != null)
            {
                Mesh.SetCursor(9, 20);
                
                foreach (var value in player1.Size4Ship)
                    if (value == null) ++count4;
                foreach (var value in player1.Size3Ship)
                    if (value == null) ++count3;
                foreach (var value in player1.Size2Ship)
                    if (value == null) ++count2;
                foreach (var value in player1.Size1Ship)
                    if (value == null) ++count1;

                Console.Write($" 4\u25A0: {player1.Size4Ship.Length - count4} |");
                Console.Write($" 3\u25A0: {player1.Size3Ship.Length - count3} ");

                Mesh.SetCursor(9, 20 + 1);

                Console.Write($" 2\u25A0: {player1.Size2Ship.Length - count2} |");
                Console.Write($" 1\u25A0: {player1.Size1Ship.Length - count1} ");

            }

            if (player2 == null)
                return;

            Mesh.SetCursor(34, 20);

            count4 = 0; count3 = 0; count2 = 0; count1 = 0;
            foreach (var value in player2.Size4Ship)
                if (value == null) ++count4;
            foreach (var value in player2.Size3Ship)
                if (value == null) ++count3;
            foreach (var value in player2.Size2Ship)
                if (value == null) ++count2;
            foreach (var value in player2.Size1Ship)
                if (value == null) ++count1;

            Console.Write($" 4\u25A0: {player2.Size4Ship.Length - count4} |");
            Console.Write($" 3\u25A0: {player2.Size3Ship.Length - count3} ");

            Mesh.SetCursor(34, 20 + 1);

            Console.Write($" 2\u25A0: {player2.Size2Ship.Length - count2} |");
            Console.Write($" 1\u25A0: {player2.Size1Ship.Length - count1} ");
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

        public static void SetCursor(int x = 0, int y = 24, int spacingForX = 1, ConsoleColor color = ConsoleColor.White, ConsoleColor Bcolor = ConsoleColor.Black)
        {
            Console.BackgroundColor = Bcolor;
            Console.ForegroundColor = color;
            Console.CursorLeft = x * spacingForX;
            Console.CursorTop = y;
        }

        public void DrawGameBoard(int offsetX, int offsetY, bool isEnemy = false, bool drawSymbols = true) 
        {
            int CursorX = Console.CursorLeft;
            int CursorY = Console.CursorTop;
            offsetY += 2;
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
            SetCursor(CursorX, CursorY);
        }

        public void DrawBoardCell(int cellX, int cellY, bool isEnemy = false, bool drawSymbols = true, ConsoleColor color = ConsoleColor.White, ConsoleColor Bcolor = ConsoleColor.Black) 
        {
            SetCursor(x + cellX * 2, y - 3 + cellY, 1, color, Bcolor);
            string toWrite = "";
            if (drawSymbols)
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
            else toWrite = gameBoard[cellX, cellY];

            Console.Write(toWrite);
            //Console.BackgroundColor = ConsoleColor.Black;
            SetCursor(x + cellX * 2, y - 3 + cellY, 1, color, Bcolor);
        }

        public void DrawLooseAnimation()
        {
            for (int i = 10 - 1; i > -10; i--)
            {
                int x = i > 0 ? i : 0;
                int y = i < 0 ? -i : 0;

                for (int j = 0; ; j++)
                {
                    if (x + j >= 10 || y + j >= 10)
                        break;
                    DrawBoardCell(y + j, x + j, false, true, ConsoleColor.Red, ConsoleColor.Red);
                    Thread.Sleep(100 / (10 - Math.Abs(i)));
                }
            }

        }
    }
}

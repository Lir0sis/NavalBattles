using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace NavalBattles
{
    class Game
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "NavalBattles";
            Console.SetWindowSize(60, 22);
            Console.SetBufferSize(60, 22);
            Console.WindowLeft = 0;
            Console.WindowTop = 0;

            User player1 = new User(true);
            User player2 = new User(false);

            Setup(player1);
            //Setup(player2);
            
            Mesh.SetCursor();
            //player1.Interface.DrawInterface();
            //PlaceShips(player1.Interface.EnemyMesh);
        }

        public static void Setup(User player)
        {
            player.Interface.DrawInterface();

            while (true)
            {
                player.Interface.UsrMesh.DrawGameBoard(6, 6);
                try
                {
                    PlaceShips(player);
                    player.CheckForRules();
                }
                catch (ShipSetupFail)
                {
                    NullifyShips(player);
                    continue;
                }
                catch (BrokenRules)
                {
                    NullifyShips(player);
                    continue;
                }

                break;
            }
        }

        public static void PlaceShips(User usr) 
        {
            var mesh = usr.Interface.UsrMesh;
            int posX = 0, posY = 0;
            while (true)
            {
                int preX = 0, preY = 0;

                mesh.DrawBoardCell(posX, posY, ConsoleColor.Black, ConsoleColor.Cyan);
                ConsoleKey Key = Console.ReadKey().Key;

                PickCell(Key, ref posX, ref posY, ref preX, ref preY);
                mesh.DrawBoardCell(preX, preY);

                if (!Action(Key, posX, posY, mesh)) continue;
                usr.CheckForBoardShips();

                mesh.DrawBoardCell(posX, posY);
                Mesh.SetCursor();
                break;
            }
        }

        public static void PickCell(ConsoleKey Key, ref int posX, ref int posY, ref int preX, ref int preY)
        {
            switch (Key)
            {
                case ConsoleKey.UpArrow:
                    posY = (posY > 0 ? posY - 1 : 9);
                    preX = posX;
                    preY = (posY + 1 > 9 ? 0 : posY + 1);
                    break;
                case ConsoleKey.DownArrow:
                    posY = (posY < 9 ? posY + 1 : 0);
                    preX = posX;
                    preY = (posY - 1 < 0 ? 9 : posY - 1);
                    break;
                case ConsoleKey.LeftArrow:
                    posX = (posX > 0 ? posX - 1 : 9);
                    preY = posY;
                    preX = (posX + 1 > 9 ? 0 : posX + 1);
                    break;
                case ConsoleKey.RightArrow:
                    posX = (posX < 9 ? posX + 1 : 0);
                    preY = posY;
                    preX = (posX - 1 < 0 ? 9 : posX - 1);
                    break;
            }
        }

        public static bool Action(ConsoleKey Key, int posX, int posY, Mesh mesh) 
        {
            switch (Key)
            {
                case ConsoleKey.F1:
                    return true;
                case ConsoleKey.Enter:
                    mesh.gameBoard[posX, posY] = (mesh.gameBoard[posX, posY] != "S" ? "S" : "E");
                    break;

            }
            return false;
        }
        
        public static void NullifyShips(User usr)
        {
            var mesh = usr.Interface.UsrMesh;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    mesh.gameBoard[y, x] = (mesh.gameBoard[y, x].ToCharArray()[0].ToString() == "S" ? "S" : "E");
                }
            }

            usr.Size1Ship = new Ship[usr.Size1Ship.Length];
            usr.Size2Ship = new Ship[usr.Size2Ship.Length];
            usr.Size3Ship = new Ship[usr.Size3Ship.Length];
            usr.Size4Ship = new Ship[usr.Size4Ship.Length];
        }
    }
    public class ShipSetupFail : Exception
    {
        private int size;
        public ShipSetupFail()
        {
        }
        public ShipSetupFail(string message, int size) : base(message)
        {
            this.size = size;
        }
        public ShipSetupFail(string message, Exception inner)
            : base(message, inner)
        {

        }
        public int GetSize()
        {
            return size;
        }
    }
    public class BrokenRules : Exception
    {
        public BrokenRules()
        {
        }
        public BrokenRules(string message) : base(message)
        {
        }
        public BrokenRules(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}

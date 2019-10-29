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

            User player1 = new User();
            User player2 = new User();

            Setup(player1);
            Setup(player2);

            PrepareBoard(player2, player1);
            PrepareBoard(player1, player2);

            while(true)
            {
                DoTurn(player1, player2);
                if (isLooser(player2))
                    break;
                DoTurn(player2, player1);
                if (isLooser(player1))
                    break;
            }

            Console.Clear();
            Console.WriteLine(isLooser(player1) ? "player2 has won" : "player1 has won");

            Mesh.SetCursor();
        }

        public static bool isLooser(User playerToCheck)
        {
            int Dships = 0;
            foreach(var key in playerToCheck.Size1Ship)
                if (key == null) Dships++;

            foreach (var key in playerToCheck.Size2Ship)
                if (key == null) Dships++;

            foreach (var key in playerToCheck.Size3Ship)
                if (key == null) Dships++;

            foreach (var key in playerToCheck.Size4Ship)
                if (key == null) Dships++;

            if (Dships == 10)
                return true;

            return false;
        }

        public static void DoTurn(User player, User enemy)
        {
            bool hit = true;
            while (hit)
            {
                player.CheckForDestroyedShips(enemy);
                player.SurroundDestrShips();
                if (isLooser(enemy))
                    break;
                hit = false;
                player.Interface.DrawInterface();
                hit = Shoot(player, enemy);
                UpdateBoard(enemy, player);
            }
        }
        public static void PrepareBoard(User player1, User player2)
        {
            player1.Interface.EnemyMesh = player2.Interface.UsrMesh;
        }

        public static void UpdateBoard(User player1, User player2)
        {
            player1.Interface.UsrMesh = player2.Interface.EnemyMesh;
        }

        public static bool Shoot(User shootingPlayer, User playeBeingShot)
        {
            var mesh = shootingPlayer.Interface.EnemyMesh;
            int posX = 0, posY = 0;
            while (true)
            {
                int preX = 0, preY = 0;

                mesh.DrawBoardCell(posX, posY, true, ConsoleColor.Black, ConsoleColor.Cyan);
                var Key = Console.ReadKey().Key;

                PickCell(Key, ref posX, ref posY, ref preX, ref preY);
                mesh.DrawBoardCell(preX, preY, true);

                if (!Action(Key, posX, posY, mesh)) continue;

                mesh.DrawBoardCell(posX, posY, true);
                Mesh.SetCursor();

                if (mesh.gameBoard[posX, posY].ToCharArray()[0].ToString() == "S")
                {
                    mesh.gameBoard[posX, posY] = "D";
                    return true;
                }
                else if (mesh.gameBoard[posX, posY] == "E")
                {
                    mesh.gameBoard[posX, posY] = "M";
                    break;
                }
            }
   
            return false;
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

                mesh.DrawBoardCell(posX, posY, false, ConsoleColor.Black, ConsoleColor.Cyan);
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

        public static bool Action(ConsoleKey Key, int posX = 0, int posY = 0, Mesh mesh = null)
        {
            if (Key == ConsoleKey.Enter)
                return true;

            if (mesh != null && !mesh.isEnemy && Key == ConsoleKey.E)
                mesh.gameBoard[posX, posY] = (mesh.gameBoard[posX, posY] != "S" ? "S" : "E");
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

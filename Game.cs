using System;
using System.Text;

namespace NavalBattles
{

    class Game
    {

        static public int PercentHardness = 0;
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "NavalBattles";
            Console.SetWindowSize(60, 25);
            Console.SetBufferSize(60, 25);
            Console.WindowLeft = 0;
            Console.WindowTop = 0;


            User player1 = new User();
            User player2 = new User(true);
            Bot bot = null;


            string[] Options =
            {
                "PlayerVsPlayer",
                "PlayerVsBot",
            };

            string[] BotOptions =
            {
                "Easy",
                "Medium",
                "Hard",
                "God"
            };


            int option = PickOption(Options);

            if (option == 1)
            {
                Interface.Pause(null, "   Морской Бой");

                Interface.WriteAction("Игрок №1 подготовиться! (Клавиши Е/Enter)", ConsoleColor.Yellow);
                Interface.UpdatePlayersStatus(player1, player2);
                Setup(player1);

                Interface.Pause(player2);

                Interface.WriteAction("Игрок №2 подготовиться! (Клавиши Е/Enter)", ConsoleColor.Yellow);
                Interface.UpdatePlayersStatus(player2, player1);
                Setup(player2);

                PrepareBoard(player2, player1);
                PrepareBoard(player1, player2);


                while (true)
                {
                    Interface.Pause(player1);

                    DoTurn(player1, player2);

                    if (isLooser(player2))
                        break;

                    Interface.Pause(player2);

                    DoTurn(player2, player1);
                    if (isLooser(player1))
                        break;
                }
            }
            else if (option == 2)
            {

                /*
                weight = 40;
                if(rand(0 100) > weight) shoottokill
                else 
                
                */
                option = PickOption(BotOptions);

                Interface.Pause(null, "   Морской Бой");
                bot = new Bot(player2, player1);

                if (option == 1)
                    bot.BotLevel = 0;

                else if (option == 2)
                    bot.BotLevel = 30;

                else if (option == 3)
                    bot.BotLevel = 65;

                else if (option == 4)
                    bot.BotLevel = 100;

                Interface.WriteAction("Игрок №1 подготовиться! (Клавиши Е/Enter)", ConsoleColor.Yellow);
                Interface.UpdatePlayersStatus(player1, player2);
                Setup(player1);
                
                bot.PrepareBoard();
                Interface.UpdatePlayersStatus(player2, player1);


                PrepareBoard(player2, player1);
                PrepareBoard(player1, player2);

                

                while (true)
                {
                    DoTurn(player1, player2);
                    if (isLooser(player2))
                        break;

                    bot.DoTurn();
                    if (isLooser(player1))
                        break;
                }

            }
            if (isLooser(player1))
            {
                var color = ConsoleColor.Green;

                if (bot == null)
                    player2.Interface.EnemyMesh.DrawLooseAnimation();
                else
                {
                    color = ConsoleColor.Yellow;
                    player1.Interface.UsrMesh.DrawLooseAnimation();
                }
                Interface.WriteAction("Игрок №2 - победил!", ConsoleColor.Green);
            }
            else if (isLooser(player2))
            {
                
                player1.Interface.EnemyMesh.DrawLooseAnimation();

                Interface.WriteAction("Игрок №1 - победил!", ConsoleColor.Green);
            }

            Mesh.SetCursor();
        }

        static int PickOption(string[] Options)
        {
            Console.Clear();
            Interface.DrawWindowBase(18, 10 - Options.Length / 2, 42, 14 + Options.Length / 2 - 1);

            int count = 0;
            while (count < Options.Length)
            {
                Mesh.SetCursor(21, 11 - (Options.Length / 2) + 1 + count);
                Console.Write(Options[count]);
                count++;
            }

            int option = 1;
            int preOption = 0;
            int x1 = 21, y1 = 11 - Options.Length / 2 + 1;
            while (true) 
            {

                Mesh.SetCursor(x1, y1 + option - 1, 1, ConsoleColor.Black, ConsoleColor.White);
                Console.Write(Options[option - 1]);

                var Key = Console.ReadKey().Key;
                if (Key == ConsoleKey.DownArrow || Key == ConsoleKey.S)
                {
                    preOption = option;
                    option = option + 1 > Options.Length ? option + 1 - Options.Length : option + 1;
                }
                else if (Key == ConsoleKey.UpArrow || Key == ConsoleKey.W)
                {
                    preOption = option;
                    option = option - 1 <= 0 ? option - 1 + Options.Length : option - 1;
                }
                else if (Key == ConsoleKey.Enter)
                    break;
                else
                    continue;

                Mesh.SetCursor(x1, y1 + preOption - 1, 1, ConsoleColor.White, ConsoleColor.Black);
                Console.Write(Options[preOption - 1] + " ");

            }

            Mesh.SetCursor(0,0);

            return option;
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
            player.Interface.DrawInterface();
            Interface.UpdatePlayersStatus(player, enemy);
            player.Interface.CurrentPlayer(player);
            bool hit = true;
            bool @bool;
            while (hit)
            {
                player.CheckForDestroyedShips(enemy, out @bool);
                player.SurroundDestrShips();
                if (isLooser(enemy))
                    break;
                hit = false;
                player.Interface.EnemyMesh.DrawGameBoard(player.Interface.EnemyMesh.x, player.Interface.EnemyMesh.y, true);
                hit = Shoot(player, enemy);
                UpdateBoard(enemy, player);
            }
            player.CheckForDestroyedShips(enemy, out @bool);

        }
        public static void PrepareBoard(User player1, User player2)
        {
            player1.Interface.EnemyMesh.gameBoard = player2.Interface.UsrMesh.gameBoard;
        }

        public static void UpdateBoard(User player1, User player2)
        {
            player1.Interface.UsrMesh.gameBoard = player2.Interface.EnemyMesh.gameBoard;
        }

        public static bool Shoot(User shootingPlayer, User playeBeingShot)
        {
            var mesh = shootingPlayer.Interface.EnemyMesh;
            int posX = 0, posY = 0;
            while (true)
            {
                int preX = 0, preY = 0;

                mesh.DrawBoardCell(posX, posY, true, true, ConsoleColor.Black, ConsoleColor.Cyan);
                var Key = Console.ReadKey().Key;

                PickCell(Key, ref posX, ref posY, ref preX, ref preY);
                mesh.DrawBoardCell(preX, preY, true, true);

                if (!Action(Key, posX, posY, mesh)) continue;
                mesh.DrawBoardCell(posX, posY, true, true);
                Mesh.SetCursor();

                if (mesh.gameBoard[posX, posY].ToCharArray()[0].ToString() == "S")
                {
                    mesh.gameBoard[posX, posY] = "D";
                    Interface.WriteAction($"Игрок №{Convert.ToInt32(!shootingPlayer.isSecond) + 1}: есть пробитие!", ConsoleColor.Green);
                    return true;
                }
                else if (mesh.gameBoard[posX, posY] == "E")
                {
                    Interface.WriteAction($"Игрок №{Convert.ToInt32(!shootingPlayer.isSecond) + 1}: мимо!", ConsoleColor.Yellow);
                    mesh.gameBoard[posX, posY] = "M";
                    break;
                }
            }
   
            return false;
        }

        public static void Setup(User player)
        {
            player.Interface.CurrentPlayer(player);
            player.Interface.UsrMesh.gameBoard[0, 0] = "E";
            player.Interface.DrawInterface();

            while (true)
            {
                player.Interface.UsrMesh.DrawGameBoard(6, 9);
                try
                {
                    PlaceShips(player);
                    player.CheckForRules();
                }
                catch (ShipSetupFail exception)
                {
                    Interface.WriteAction("Расположение: " + exception.reason, ConsoleColor.Red);
                    NullifyShips(player);
                    continue;
                }
                catch (BrokenRules exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Interface.WriteAction("Правила: " + exception.reason, ConsoleColor.Red);
                    Console.ForegroundColor = ConsoleColor.White;
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

                mesh.DrawBoardCell(posX, posY, false, true, ConsoleColor.Black, ConsoleColor.Cyan);
                ConsoleKey Key = Console.ReadKey().Key;

                PickCell(Key, ref posX, ref posY, ref preX, ref preY);
                mesh.DrawBoardCell(preX, preY, false, true);

                if (!Action(Key, posX, posY, mesh)) continue;
                usr.CheckForBoardShips();

                mesh.DrawBoardCell(posX, posY, false, true);
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
        public string reason;
        public ShipSetupFail()
        {
        }
        public ShipSetupFail(string message, int size) : base(message)
        {
            this.size = size;
            reason = message;
        }
        public ShipSetupFail(string message, Exception inner)
            : base(message, inner)
        {
            reason = message;
        }
        public int GetSize()
        {
            return size;
        }
    }
    public class BrokenRules : Exception
    {
        public string reason = "";
        public BrokenRules()
        {
        }
        public BrokenRules(string message) : base(message)
        {
            reason = message;
        }
        public BrokenRules(string message, Exception inner)
            : base(message, inner)
        {
            reason = message;
        }
    }
}

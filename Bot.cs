using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NavalBattles
{
    class Bot
    {
        User BotUser;
        User Oponent;
        Mesh oponentMesh;
        static Random rand = new Random();

        public int BotLevel;

        //Turn values
        bool LastTurnSuccess = true, KnowsDir = false, KnowsPos = false;
        int lastX, lastY, rotation;
       

        public Bot (User player, User enemy)
        {
            BotUser = player;
            Oponent = enemy;
            oponentMesh = player.Interface.EnemyMesh;

        }

        public void PrepareBoard()
        {
            //BotUser.Interface.CurrentPlayer(BotUser);
            BotUser.Interface.UsrMesh.gameBoard[0, 0] = "E";
            //BotUser.Interface.DrawInterface();
            string[,] Backup = new string[10, 10];

            int size = 4, i = 1 - size + 4;
            while (true)
            {
                try
                {
                    PlaceShips(ref size, ref i, ref Backup);
                    Game.NullifyShips(BotUser);
                    BotUser.CheckForBoardShips();
                    BotUser.CheckForRules();
                }
                catch (ShipSetupFail)
                {
                    Game.NullifyShips(BotUser);
                    Array.Copy(Backup, 0, BotUser.Interface.UsrMesh.gameBoard, 0, Backup.Length);
                    continue;
                }
                catch (BrokenRules)
                {
                    Game.NullifyShips(BotUser);
                    Array.Copy(Backup, 0, BotUser.Interface.UsrMesh.gameBoard, 0, Backup.Length);
                    continue;
                }
                break;
            }
        }

        void PlaceShips(ref int size, ref int i, ref string[,] Backup)
        {
            int x = 0, y = 0, rotation = 0;
            ChooseCell(ref x, ref y);

            while (size > 0)
            {
                while (i > 0)
                {
                    Game.NullifyShips(BotUser);
                    Array.Copy(BotUser.Interface.UsrMesh.gameBoard, 0, Backup, 0, BotUser.Interface.UsrMesh.gameBoard.Length);
                    while (true)
                    {
                        int random = rand.Next(399);
                        rotation = (random - random % 100) / 100;
                        switch (rotation)
                        {
                            case 0:
                                if (y - size >= 0)
                                {
                                    int count = size;

                                    while (count > 0)
                                    {
                                        if (BotUser.Interface.UsrMesh.gameBoard[y - count, x] == "E")
                                            BotUser.Interface.UsrMesh.gameBoard[y - count, x] = "S";
                                        else throw new ShipSetupFail("Совпали координаты!?", size);
                                        count--;
                                    }
                                    BotUser.CheckForBoardShips();
                                    BotUser.CheckForRules(true);
                                }
                                else continue;
                                break;
                            case 1:
                                if (x + size < 10)
                                {
                                    int count = 0;

                                    while (count < size)
                                    {
                                        if (BotUser.Interface.UsrMesh.gameBoard[y, x + count] == "E")
                                            BotUser.Interface.UsrMesh.gameBoard[y, x + count] = "S";
                                        else throw new ShipSetupFail("Совпали координаты!?", size);
                                        count++;

                                    }
                                    BotUser.CheckForBoardShips();
                                    BotUser.CheckForRules(true);
                                }
                                else continue;
                                break;
                            case 2:
                                if (y + size < 10)
                                {
                                    int count = 0;

                                    while (count < size)
                                    {
                                        if (BotUser.Interface.UsrMesh.gameBoard[y + count, x] == "E")
                                            BotUser.Interface.UsrMesh.gameBoard[y + count, x] = "S";
                                        else throw new ShipSetupFail("Совпали координаты!?", size);
                                        count++;
                                    }
                                    BotUser.CheckForBoardShips();
                                    BotUser.CheckForRules(true);
                                }
                                else continue;
                                break;
                            case 3:
                                if (x - size >= 0)
                                {
                                    int count = size;

                                    while (count > 0)
                                    {
                                        if (BotUser.Interface.UsrMesh.gameBoard[y, x - count] == "E")
                                            BotUser.Interface.UsrMesh.gameBoard[y, x - count] = "S";
                                        else throw new ShipSetupFail("Совпали координаты!?",size);
                                        count--;
                                    }
                                    BotUser.CheckForBoardShips();
                                    BotUser.CheckForRules(true);
                                }
                                else continue;
                                break;
                                
                        }
                        
                        break;
                    }
                    i--;
                }
                size--;
                i = 1 - size + 4;
            }
        }
        public void DoTurn()
        {
            int zaloopCount = 0;
            bool TurnEnd = true;
            int Anothercount = 0;
            while (true)
            { 
                if (LastTurnSuccess)
                {

                    if (!KnowsPos && rand.Next(0, 1000) / 10 < BotLevel)
                        KnowsPos = true;
                    if (Anothercount >= BotLevel / 2)
                        KnowsPos = false;
                    if (Anothercount >= BotLevel * 2)
                        break;

                    KnowsDir = false; TurnEnd = true; lastX = 0; lastY = 0;
                    var board = oponentMesh.gameBoard;
                    int x = 0, y = 0;
                    ChooseCell(ref x, ref y);

                    if (board[y, x][0] == 'S')
                    {
                        board[y, x] = "D";

                        BotUser.CheckForDestroyedShips(Oponent, out bool DestroyedSingle);
                        BotUser.SurroundDestrShips();

                        if (DestroyedSingle)
                        {
                            KnowsPos = false;
                            LastTurnSuccess = true;
                            TurnEnd = false;
                            continue;
                        }

                        int random = rand.Next(0, 199);
                        rotation = (random - random % 50) / 50 + 1;
                        bool DestroyedShip = false;

                        int count = 0;

                        if (BotLevel < 40) KnowsPos = false;

                        while (true)
                        {
                            //Thread.Sleep(200);
                            if (count > 4) count = 0;
                            count++;
                            

                            switch (rotation)
                            {
                                case 1:
                                    if (y - count >= 0)
                                    {
                                        if (board[y - count, x][0] == 'S')
                                        {
                                            board[y - count, x] = "D";
                                            BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                            if (DestroyedShip)
                                            {
                                                KnowsPos = false;
                                                TurnEnd = false;
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (board[y - count, x][0] == 'E')
                                        {
                                            if (KnowsPos && ((count == 1 && BotLevel > 40) || BotLevel > 75))
                                                continue;

                                            board[y - count, x] = "M";

                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = true;
                                            KnowsDir = false;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                        else if (board[y - count, x][0] == 'M')
                                        {
                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = false;
                                            KnowsDir = false;

                                            if (DestroyedShip)
                                                LastTurnSuccess = true;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                    }
                                    else if (y - count < 0)
                                    {
                                        count = 0;
                                        rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                        continue;
                                    }
                                    break;
                                case 2:
                                    if (x + count <= 9)
                                    {
                                        if (board[y, x + count][0] == 'S')
                                        {
                                            board[y, x + count] = "D";
                                            BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                            if (DestroyedShip)
                                            {
                                                KnowsPos = false;
                                                TurnEnd = false;
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (board[y, x + count][0] == 'E')
                                        {
                                            if (KnowsPos && ((count == 1 && BotLevel > 40) || BotLevel > 75))
                                                continue;

                                            board[y, x + count] = "M";

                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = true;
                                            KnowsDir = false;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                        else if (board[y, x + count][0] == 'M')
                                        {
                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = false;
                                            KnowsDir = false;

                                            if (DestroyedShip)
                                                LastTurnSuccess = true;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                    }
                                    else if (x + count > 9)
                                    {
                                        count = 0;
                                        rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                        continue;
                                    }
                                    break;
                                case 3:
                                    if (y + count <= 9)
                                    {
                                        if (board[y + count, x][0] == 'S')
                                        {
                                            board[y + count, x] = "D";
                                            BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                            if (DestroyedShip)
                                            {
                                                KnowsPos = false;
                                                TurnEnd = false;
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (board[y + count, x][0] == 'E')
                                        {
                                            if (KnowsPos && ((count == 1 && BotLevel > 40) || BotLevel > 75))
                                                continue;

                                            board[y + count, x] = "M";

                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = true;
                                            KnowsDir = false;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                        else if (board[y + count, x][0] == 'M')
                                        {
                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = false;
                                            KnowsDir = false;

                                            if (DestroyedShip)
                                                LastTurnSuccess = true;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                    }
                                    else if (y + count > 9)
                                    {
                                        count = 0;
                                        rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                        continue;
                                    }
                                    break;
                                case 4:
                                    if (x - count >= 0)
                                    {
                                        if (board[y, x - count][0] == 'S')
                                        {
                                            board[y, x - count] = "D";
                                            BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                            if (DestroyedShip)
                                            {
                                                KnowsPos = false;
                                                TurnEnd = false;
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (board[y, x - count][0] == 'E')
                                        {
                                            if (KnowsPos && ((count == 1 && BotLevel > 40) || BotLevel > 75))
                                                continue;

                                            board[y, x - count] = "M";

                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = true;
                                            KnowsDir = false;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }
                                        }
                                        else if (board[y, x - count][0] == 'M')
                                        {
                                            lastX = x;
                                            lastY = y;

                                            LastTurnSuccess = false;
                                            TurnEnd = false;
                                            KnowsDir = false;

                                            if (DestroyedShip)
                                                LastTurnSuccess = true;

                                            if (count > 1)
                                            {
                                                KnowsDir = true;
                                                rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                            }

                                        }
                                    }
                                    else if (x - count < 0)
                                    {
                                        count = 0;
                                        rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                        continue;
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                    else if (board[y, x][0] == 'E')
                    {
                        //Thread.Sleep(200);

                        LastTurnSuccess = true;

                        if (KnowsPos)
                            continue;

                        board[y, x] = "M";
                    }
                    else if (board[y, x][0] == 'D' || board[y, x][0] == 'M')
                    {
                        continue;
                    }

                }

                else if (!LastTurnSuccess)
                {
                    BotUser.SurroundDestrShips();
                    BotUser.CheckForDestroyedShips(Oponent, out bool ok);

                    var board = oponentMesh.gameBoard;
                    int x = lastX, y = lastY;
                    
                    if (!KnowsDir)
                    {
                        int random = 0;

                        do
                        {
                            random = rand.Next(0, 199);
                        }
                        while ((random - random % 50) / 50 + 1 == rotation);

                        rotation = (random - random % 50) / 50 + 1;
                    }
                    
                    bool DestroyedShip = false; TurnEnd = true; KnowsDir = false; lastX = 0; lastY = 0;

                    int count = 0;
                    

                    while (true)
                    {
                        if (zaloopCount > 10)
                        {
                            zaloopCount = 0;
                            LastTurnSuccess = true;
                            break;
                        }
                        //Thread.Sleep(200);
                        count++;
                        

                        switch (rotation)
                        {
                            case 1:
                                if (y - count >= 0)
                                {
                                    if (board[y - count, x][0] == 'S')
                                    {
                                        board[y - count, x] = "D";
                                        BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                        if (DestroyedShip)
                                        {
                                            BotUser.SurroundDestrShips();
                                            LastTurnSuccess = true;
                                            TurnEnd = true;
                                            break;
                                        }
                                        continue;
                                    }
                                    else if (board[y - count, x][0] == 'E')
                                    {
                                        board[y - count, x] = "M";

                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = true;
                                        KnowsDir = false;

                                        if (count > 1) KnowsDir = true;
                                    }
                                    else if (board[y - count, x][0] == 'M')
                                    {
                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = false;
                                        KnowsDir = false;

                                        if (DestroyedShip) LastTurnSuccess = true;

                                        if (count > 1) KnowsDir = true;

                                        zaloopCount++;
                                    }
                                }
                                else if (y - count < 0)
                                {
                                    count = 0;
                                    rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                    zaloopCount++;
                                    continue;
                                }
                                break;
                            case 2:
                                if (x + count <= 9)
                                {
                                    if (board[y, x + count][0] == 'S')
                                    {
                                        board[y, x + count] = "D";
                                        BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                        if (DestroyedShip)
                                        {
                                            BotUser.SurroundDestrShips();
                                            LastTurnSuccess = true;
                                            TurnEnd = true;
                                            break;
                                        }
                                        continue;
                                    }
                                    else if (board[y, x + count][0] == 'E')
                                    {
                                        board[y, x + count] = "M";

                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = true;
                                        KnowsDir = false;

                                        if (count > 1) KnowsDir = true;
                                    }
                                    else if (board[y, x + count][0] == 'M')
                                    {
                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = false;
                                        KnowsDir = false;

                                        if (DestroyedShip) LastTurnSuccess = true;

                                        if (count > 1) KnowsDir = true;
                                        zaloopCount++;
                                    }
                                }
                                else if (x + count > 9)
                                {
                                    count = 0;
                                    rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                    zaloopCount++;
                                    continue;
                                }
                                break;
                            case 3:
                                if (y + count <= 9)
                                {
                                    if (board[y + count, x][0] == 'S')
                                    {
                                        board[y + count, x] = "D";
                                        BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                        if (DestroyedShip)
                                        {
                                            BotUser.SurroundDestrShips();
                                            LastTurnSuccess = true;
                                            TurnEnd = true;
                                            break;
                                        }
                                        continue;
                                    }
                                    else if (board[y + count, x][0] == 'E')
                                    {
                                        board[y + count, x] = "M";

                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = true;
                                        KnowsDir = false;

                                        if (count > 1) KnowsDir = true;
                                    }
                                    else if (board[y + count, x][0] == 'M')
                                    {
                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = false;
                                        KnowsDir = false;

                                        if (DestroyedShip) LastTurnSuccess = true;

                                        if (count > 1) KnowsDir = true;
                                        zaloopCount++;
                                    }
                                }
                                else if (y + count > 9)
                                {
                                    count = 0;
                                    rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                    zaloopCount++;
                                    continue;
                                }
                                break;
                            case 4:
                                if (x - count >= 0)
                                {
                                    if (board[y, x - count][0] == 'S')
                                    {
                                        board[y, x - count] = "D";
                                        BotUser.CheckForDestroyedShips(Oponent, out DestroyedShip);
                                        if (DestroyedShip)
                                        {
                                            BotUser.SurroundDestrShips();
                                            LastTurnSuccess = true;
                                            TurnEnd = true;
                                            break;
                                        }
                                        continue;
                                    }
                                    else if (board[y, x - count][0] == 'E')
                                    {
                                        board[y, x - count] = "M";

                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = true;
                                        KnowsDir = false;

                                        if (count > 1) KnowsDir = true;
                                    }
                                    else if (board[y, x - count][0] == 'M')
                                    {
                                        lastX = x;
                                        lastY = y;

                                        LastTurnSuccess = false;
                                        TurnEnd = false;
                                        KnowsDir = false;

                                        if (DestroyedShip) LastTurnSuccess = true;

                                        if (count > 1) KnowsDir = true;
                                        zaloopCount++;
                                    }
                                }
                                else if (x - count < 0)
                                {
                                    count = 0;
                                    rotation = (rotation - 2 <= 0 ? rotation + 2 : rotation - 2);
                                    zaloopCount++;
                                    continue;
                                }
                                break;
                        }
                        break;
                    }
                }
                BotUser.SurroundDestrShips();
                Anothercount++;
                if (TurnEnd && !KnowsPos) break;
            }
        }

        static void ChooseCell(ref int x, ref int y)
        {
            int radnom = rand.Next(999);
            y = (radnom - radnom % 100)/ 100;

            radnom = rand.Next(999);
            x = (radnom - radnom % 100) / 100;
        }


    }

}

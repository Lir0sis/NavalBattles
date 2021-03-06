﻿using System;

namespace NavalBattles
{
    public class User
    {

        public Interface Interface;
        public Ship[] Size1Ship, Size2Ship, Size3Ship, Size4Ship;
        public bool isSecond;

        public User(bool isSecond = false)
        {
            Interface = new Interface(isSecond);
            Size1Ship = new Ship[4];
            Size2Ship = new Ship[3];
            Size3Ship = new Ship[2];
            Size4Ship = new Ship[1];
            this.isSecond = isSecond;
        }

        public void CheckForBoardShips()
        {
            var mesh = Interface.UsrMesh;
            var board = mesh.gameBoard;

            
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (board[y, x] == "S")
                    {
                        int[,] Coords = new int[2, 2];
                        bool isVertical = true;
                        bool isHorizontal = true;
                        int i = 0;
                        while (true)
                        {
                            i++;
                            if (x + i < 10 && isHorizontal)
                            {
                                if (board[y, x + i] == "S")
                                {
                                    isVertical = false;
                                    continue;
                                }
                            }

                            if (y + i < 10 && isVertical)
                            {
                                if (board[y + i, x] == "S")
                                {
                                    isHorizontal = false;
                                    continue;
                                }
                            }

                            Coords[0, 0] = y;
                            Coords[0, 1] = x;
                            Coords[1, 0] = isVertical ? y + i - 1 : y;
                            Coords[1, 1] = isHorizontal ? x + i - 1 : x;

                            SetupSingleShip(Coords, i);
                            break;
                        }

                    }
                }
            }

        }

        public void CheckForDestroyedShips(User playerToCheck, out bool hasDestroyed)
        {
            hasDestroyed = false;

            var board = Interface.EnemyMesh.gameBoard;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (board[y, x] == "D")
                    {
                        bool isVertical = true;
                        bool isHorizontal = true;
                        int count = 0;

                        while (true)
                        {
                            count++;

                            if (x + count < 10 && isHorizontal)
                            {
                                if (board[y, x + count] == "D")
                                {
                                    isVertical = false;
                                    continue;
                                }
                            }

                            if (y + count < 10 && isVertical)
                            {
                                if (board[y + count, x] == "D")
                                {
                                    isHorizontal = false;
                                    continue;
                                }
                            }

                            #region cool
                            /*
                            int wrongCount = 0;
                            while (true)
                            {
                                wrongCount++;

                                if (isHorizontal && x - wrongCount >= 0)
                                {
                                    if (board[y, x - wrongCount].ToCharArray()[0].ToString() == "S")
                                        notD++;
                                    continue;
                                    //else if (board[y, x - wrongCount].ToCharArray()[0].ToString() == "E") break;
                                }

                                if (isVertical && y - wrongCount >= 0)
                                {
                                    if (board[y - wrongCount, x].ToCharArray()[0].ToString() == "S")
                                        notD++;
                                    continue;
                                }
                                break;
                            }
                            
                            if (notD > 0) 
                                break;
                            */
                            #endregion

                            if (isHorizontal && isVertical)
                            {
                                int i = 0;
                                foreach (var key in playerToCheck.Size1Ship)
                                {
                                    if (key != null && y == key.Coords[0, 0] && x == key.Coords[0, 1])
                                    {
                                        board[y, x] = "D1";
                                        playerToCheck.Size1Ship[i] = null;
                                        break;

                                    }
                                    i++;
                                }
                                break;
                            }


                            bool isDestroyed = false;
                            switch (count)
                            {
                                case 2:
                                    int i = 0;
                                    foreach (var key in playerToCheck.Size2Ship)
                                    {
                                        if (key != null)
                                            if (y >= key.Coords[0, 0] && y <= key.Coords[1, 0] && x >= key.Coords[0, 1] && x <= key.Coords[1, 1])
                                            {
                                                playerToCheck.Size2Ship[i] = null;
                                                isDestroyed = true;
                                                break;
                                            }
                                        i++;
                                    }
                                    break;
                                case 3:
                                    i = 0;
                                    foreach (var key in playerToCheck.Size3Ship)
                                    {
                                        if (key != null)
                                            if (y >= key.Coords[0, 0] && y <= key.Coords[1, 0] && x >= key.Coords[0, 1] && x <= key.Coords[1, 1])
                                            {
                                                playerToCheck.Size3Ship[i] = null;
                                                isDestroyed = true;
                                                break;
                                            }
                                        i++;
                                    }
                                    break;
                                case 4:
                                    i = 0;
                                    foreach (var key in playerToCheck.Size4Ship)
                                    {
                                        if (key != null)
                                            if (y >= key.Coords[0, 0] && y <= key.Coords[1, 0] && x >= key.Coords[0, 1] && x <= key.Coords[1, 1])
                                            {
                                                playerToCheck.Size4Ship[i] = null;
                                                isDestroyed = true;
                                                break;
                                            }
                                        i++;
                                    }
                                    break;
                            }

                            if (isDestroyed && isHorizontal)
                                for (int j = 0; j < count; j++)
                                {
                                    board[y, x + j] = $"D{count}";
                                }

                            if (isDestroyed && isVertical)
                                for (int j = 0; j < count; j++)
                                {
                                    board[y + j, x] = $"D{count}";
                                }

                            if (isDestroyed)
                                {
                                Interface.WriteAction($"Игрок №{Convert.ToInt32(playerToCheck.isSecond) + 1}: {count}\u25A0 корабль потоплен!", ConsoleColor.Green);
                                Interface.UpdatePlayersStatus(null, playerToCheck);
                                }

                            hasDestroyed = isDestroyed;
                            break;
                        }
                    }
                }
            }
        }

        public void SurroundDestrShips()
        {
            var board = Interface.EnemyMesh.gameBoard;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (board[y, x].ToCharArray()[0].ToString() == "D" && board[y, x].Length > 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (x - 1 + j > -1 && y - 1 + i > -1 && x - 1 + j < 10 && y - 1 + i < 10)
                                    if (board[y - 1 + i, x - 1 + j] == "E") board[y - 1 + i, x - 1 + j] = "M";
                            }
                        }
                    }

                }
            }

        }

        public void CheckForRules(bool BotCheck = false)
        {
            var mesh = Interface.UsrMesh;
            var board = mesh.gameBoard;

            for (int y = 0; y < 10; y++) 
            {
                for (int x = 0; x < 10; x++)
                {
                    if (BotCheck && board[y, x].ToCharArray()[0].ToString() == "S" && board[y, x].ToCharArray().Length == 2)
                    {
                        board[y, x] += "C";
                        int bcount = 1;
                        bool isVertical = true;
                        bool isHorizontal = true;
                        int x1 = x, y1 = y;

                        while (bcount <= int.Parse(board[y, x].ToCharArray()[1].ToString()) + 1)
                        {
                            x1 = isHorizontal ? x + bcount - 1 : x1;
                            y1 = isVertical ? y + bcount - 1 : y1;
                            
                            #region diagonal check
                            if (y1 + 1 < 10 && x1 + 1 < 10 && board[y1 + 1, x1 + 1] != "E")
                                throw new BrokenRules("корабль неправильно расположен");
                            else if (y1 + 1 < 10 && x1 - 1 > 0 && board[y1 + 1, x1 - 1] != "E")
                                throw new BrokenRules("корабль неправильно расположен");
                            else if (y1 - 1 > 0 && x1 + 1 < 10 && board[y1 - 1, x1 + 1] != "E")
                                throw new BrokenRules("корабль неправильно расположен");
                            else if (y1 - 1 > 0 && x1 - 1 > 0 && board[y1 - 1, x1 - 1] != "E")
                                throw new BrokenRules("корабль неправильно расположен");
                            #endregion

                            if (x1 + 1 < 10 && isHorizontal)
                            {
                                if (board[y1, x1].Length > 1 && board[y1,x1 + 1] == board[y1, x1].Substring(0, 2))
                                {
                                    board[y1, x1 + 1] += "C";
                                    isVertical = false;
                                    bcount++;
                                    continue;
                                }
                            }

                            if (y1 + 1 < 10 && isVertical)
                            {
                                if (board[y1, x1].Length > 1 && board[y1 + 1, x1] == board[y1, x1].Substring(0, 2))
                                {
                                    board[y1 + 1, x1] += "C";
                                    isHorizontal = false;
                                    bcount++;
                                    continue;
                                }
                            }

                            if (bcount > int.Parse(board[y, x].ToCharArray()[1].ToString()))
                                throw new BrokenRules("корабль неправильно расположен");

                            break;
                        }


                    }
                    else if (!BotCheck && board[y,x].ToCharArray()[0].ToString() == "S")
                    {
                        int count = 1;

                        if (x + 1 < 10)
                            if (board[y, x + 1] == board[y, x])
                                count++;
                            else if (board[y, x +1] != "E") count = 3;

                        if (y + 1 < 10)
                            if (board[y + 1, x] == board[y, x])
                                count++;
                            else if (board[y + 1, x] != "E") count = 3;

                        if (y + 1 < 10 && x + 1 < 10 && board[y + 1, x + 1] != "E")
                            count = 3;
                        if (y + 1 < 10 && x - 1 > 0 && board[y + 1, x - 1] != "E")
                            count = 3;

                        if (count > 2)
                            throw new BrokenRules("корабль неправильно расположен");
                    }
                    
                }
            }

            if (!BotCheck && (Array.IndexOf(Size1Ship, null) != -1 || 
                Array.IndexOf(Size2Ship, null) != -1 || 
                Array.IndexOf(Size3Ship, null) != -1 || 
                Array.IndexOf(Size4Ship, null) != -1))
                throw new BrokenRules("неверное количество кораблей");
        }

        public void SetupSingleShip(int[,] Coords, int size = 1)
        {
            int index = 0;
            switch (size)
            {
                case 1:
                    index = Array.IndexOf(Size1Ship, null);
                    if (index != -1)
                        Size1Ship[index] = new Ship(1, Coords);
                    else throw new ShipSetupFail("1\u25A0 корабль", 1);
                    break;
                case 2:
                    index = Array.IndexOf(Size2Ship, null);
                    if (index != -1)
                        Size2Ship[index] = new Ship(2, Coords);
                    else throw new ShipSetupFail("2\u25A0 корабль", 2);
                    break;
                case 3:
                    index = Array.IndexOf(Size3Ship, null);
                    if (index != -1)
                        Size3Ship[index] = new Ship(3, Coords);
                    else throw new ShipSetupFail("3\u25A0 корабль", 3);
                    break;
                case 4:
                    index = Array.IndexOf(Size4Ship, null);
                    if (index != -1)
                        Size4Ship[index] = new Ship(4, Coords);
                    else throw new ShipSetupFail("4\u25A0 корабль", 4);
                    break;
                default:
                    throw new ShipSetupFail("неверный размер корабля", size);
            }

            if (Coords[0, 1] - Coords[1, 1] != 0)
            { 
                for (int i = Coords[0, 1]; i <= Coords[1, 1]; i++)
                    Interface.UsrMesh.gameBoard[Coords[0, 0], i] = $"S{size}";
            }
            if (Coords[0, 0] - Coords[1, 0] != 0)
            {
                for (int i = Coords[0, 0]; i <= Coords[1, 0]; i++)
                    Interface.UsrMesh.gameBoard[i, Coords[0, 1]] = $"S{size}";
            }
            if (Coords[0, 0] == Coords[1, 0] && Coords[0, 1] == Coords[1, 1])
                Interface.UsrMesh.gameBoard[Coords[0, 0], Coords[0, 1]] = "S1";
        }

    }
}
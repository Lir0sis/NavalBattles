using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    public class User
    {
        public bool IsFirst = false;
        public Interface Interface;
        public Ship[] Size1Ship, Size2Ship, Size3Ship, Size4Ship;

        public User(bool IsFirst)
        {
            this.IsFirst = IsFirst;
            Interface = new Interface();
            Size1Ship = new Ship[4];
            Size2Ship = new Ship[3];
            Size3Ship = new Ship[2];
            Size4Ship = new Ship[1];
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

        public void CheckForRules()
        {
            var mesh = Interface.UsrMesh;
            var board = mesh.gameBoard;

            for (int y = 0; y < 10; y++) 
            {
                for (int x = 0; x < 10; x++)
                {
                    if ( board[y,x].ToCharArray()[0].ToString() == "S")
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
                            throw new BrokenRules();
                    }
                }
            }

            if (Array.IndexOf(Size1Ship, null) != -1 || 
                Array.IndexOf(Size2Ship, null) != -1 || 
                Array.IndexOf(Size3Ship, null) != -1 || 
                Array.IndexOf(Size4Ship, null) != -1)
                throw new BrokenRules();
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
                    else throw new ShipSetupFail("Size1 ship", 1);
                    break;
                case 2:
                    index = Array.IndexOf(Size2Ship, null);
                    if (index != -1)
                        Size2Ship[index] = new Ship(2, Coords);
                    else throw new ShipSetupFail("Size2 ship", 2);
                    break;
                case 3:
                    index = Array.IndexOf(Size3Ship, null);
                    if (index != -1)
                        Size3Ship[index] = new Ship(3, Coords);
                    else throw new ShipSetupFail("Size3 ship", 3);
                    break;
                case 4:
                    index = Array.IndexOf(Size4Ship, null);
                    if (index != -1)
                        Size4Ship[index] = new Ship(4, Coords);
                    else throw new ShipSetupFail("Size4 ship", 4);
                    break;
                default:
                    throw new ShipSetupFail("Wrong sized ship", size);
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
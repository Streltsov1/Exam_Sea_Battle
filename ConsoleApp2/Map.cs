using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    enum horisontalShip { Horisontal, Vertical, Unknown };
    class Map
    {
        private int X { get; set; }
        private int Y { get; set; }
        public int MapPosition { get; set; }
        private char[,] maps = new char[10, 20];
        private bool IsHorisontal = true;
        private bool hit = false;
        private int shipNum;
        private Cords firstHit = new Cords(0, 0);
        private horisontalShip horisontal = horisontalShip.Unknown;
        private Player player;
        public Player Player => player;
        public Map()
        {

        }
        public Map(Player player, int mapPosition = 0)
        {
            this.player = player;
            MapPosition = mapPosition;
            for (int y = 0; y < maps.GetLength(0); y++)
            {
                for (int x = 0; x < maps.GetLength(1); x++)
                {
                    maps[y, x] = ' ';
                }
            }
            X = 0;
            Y = 0;
        }
        public void ShowMap(bool IsFight = false)
        {
            char p = 'A';
            Console.SetCursorPosition(20 + MapPosition, 5);
            for (int y = 0; y < maps.GetLength(1) + 2; y++)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                if ((y % 2) == 1 && y != maps.GetLength(1) + 1)
                {
                    Console.Write(p);
                    p++;
                }
                else
                    Console.Write(" ");
            }
            Console.WriteLine();
            for (int y = 0; y < maps.GetLength(0); y++)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(20 + MapPosition, 5 + y + 1);
                Console.Write(y);
                for (int x = 0; x < maps.GetLength(1); x++)
                {
                    if (maps[y, x] == ' ')
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" ");
                    }
                    else if (maps[y, x] == 'k')
                    {
                        if(IsFight)
                            Console.BackgroundColor = ConsoleColor.Blue;
                        else
                            Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    else if (maps[y, x] == 'o')
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" ");
                    }
                    else if (maps[y, x] == 'x')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write(" ");
                    }
                    else if (maps[y, x] == '-')
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" ");
                    }
                }
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(" ");
            }
            Console.SetCursorPosition(20 + MapPosition, 5 + maps.GetLength(0) + 1);
            Console.WriteLine(new String(' ', maps.GetLength(1) + 2));
            Console.ResetColor();
        }
        private void RandomCord()
        {
            Random rnd = new Random();
            X = rnd.Next(0, 10)*2;
            Y = rnd.Next(0, 10);
        }
        public void AddRandomShip()
        {
            Random rnd = new Random();
            int i = 0;
            while(i != 10)
            {
                RandomCord();
                if(rnd.Next(0,2) == 1)
                {
                    IsHorisontal = true;
                    if (X + WhatShip_int(i) <= 19 && IsFreeSpace(i))
                    {
                        AddOneShip(i);
                        player.SetShip(i, X, Y, IsHorisontal, WhatShip_int(i)/2);
                        AddGrayZone(i);
                    }
                    else
                        continue;
                }
                else
                {
                    IsHorisontal = false;
                    if (Y + WhatShip_int(i) / 2 <= 9 && IsFreeSpace(i))
                    {
                        AddOneShip(i);
                        player.SetShip(i, X, Y, IsHorisontal, WhatShip_int(i)/2);
                        AddGrayZone(i);
                    }
                    else
                        continue;
                }
                i++;
            }
            X = 0;
            Y = 0;
            ShowMap();
        }
        public bool CanShoot(int x = 0, int y = 0)
        {
            if (maps[Y + y, X + x] == '-' || maps[Y + y, X + x] == ' ' || maps[Y+y,X+x] == 'k')
                return true;
            return false;
        }
        public void RandomFight()
        {
            int i = 0;
            Cords cords = new Cords(0, 0);
            
            while (i != 1)
            {
                Console.ResetColor();
                Console.SetCursorPosition(20 + MapPosition  - 4, 5 - 2 + 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Гравець {player.Name}, життя");
                if(player.AllHealth() <= 9)
                    Console.WriteLine($" {player.AllHealth()}, ходи {player.Steps}");
                else
                    Console.WriteLine($"{player.AllHealth()}, ходи {player.Steps}");
                Console.ResetColor();
                ShowMap();
                if (!player.AnyHealth())
                    break;
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
                Console.WriteLine(WhatShip(10));
                if (!hit)
                {
                    RandomCord();
                    horisontal = horisontalShip.Unknown;
                }
                if (CanShoot())
                {
                    player.Steps++;
                    for (int p = 0; p < player.Ship.Length; p++)
                        if (player.Ship[p].Check(X, Y))
                        {
                            shipNum = p;
                            break;
                        }
                    if (player.Ship[shipNum].Check(X, Y))
                    {
                        if(!hit)
                        {
                            firstHit.X = X;
                            firstHit.Y = Y;
                        }
                        hit = true;
                        player.Ship[shipNum].Health--;
                        maps[Y, X] = 'x';
                        maps[Y, X + 1] = 'x';
                        if (!player.Ship[shipNum].Alive())
                        {
                            if (!player.Ship[shipNum].IsDestroyed)
                            {
                                PreviousColor(10);
                                cords.X = X;
                                cords.Y = Y;
                                X = player.Ship[shipNum].StartCords.X;
                                Y = player.Ship[shipNum].StartCords.Y;
                                IsHorisontal = player.Ship[shipNum].IsHorisontal;
                                AddGrayZone(shipNum, 'o');
                                player.Ship[shipNum].IsDestroyed = true;
                                X = cords.X;
                                Y = cords.Y;
                                hit = false;
                                continue;
                            }
                        }
                        if (player.Ship[shipNum].Health == player.Ship[shipNum].Size - 2)
                        {
                            if (player.Ship[shipNum].IsHorisontal)
                                horisontal = horisontalShip.Horisontal;
                            else
                                horisontal = horisontalShip.Vertical;
                        }
                        if (horisontal == horisontalShip.Unknown)
                            RandomShoot();
                        else if (horisontal == horisontalShip.Horisontal)
                        {
                            if (X < 17 && CanShoot(2, 0))
                                X += 2;
                            else if (X > 0 && CanShoot(-2, 0))
                                X -= 2;
                            else
                            {
                                Y = firstHit.Y;
                                X = firstHit.X;
                                if (X < 17 && CanShoot(2, 0))
                                    X += 2;
                                else if (X > 0 && CanShoot(-2, 0))
                                    X -= 2;
                            }
                        }
                        else
                        {
                            if (Y < 9 && CanShoot(0, 1))
                                Y++;
                            else if (Y > 0 && CanShoot(0, -1))
                                Y--;
                            else
                            {
                                Y = firstHit.Y;
                                X = firstHit.X;
                                if (Y < 9 && CanShoot(0, 1))
                                    Y++;
                                else if (Y > 0 && CanShoot(0, -1))
                                    Y--;
                            }
                        }
                        continue;
                    }
                    else
                    {
                        maps[Y, X] = 'o';
                        maps[Y, X + 1] = 'o';
                        if (hit)
                        {
                            Y = firstHit.Y;
                            X = firstHit.X;
                            if (horisontal == horisontalShip.Unknown)
                            {
                                RandomShoot();
                            }
                            else if (horisontal == horisontalShip.Horisontal)
                            {
                                if (X < 17 && CanShoot(2, 0))
                                    X += 2;
                                else if (X > 0 && CanShoot(-2, 0))
                                    X -= 2;
                            }
                            else
                            {
                                if (Y < 9 && CanShoot(0, 1))
                                    Y++;
                                else if (Y > 0 && CanShoot(0, -1))
                                    Y--;
                            }
                        }
                    }
                    i++;
                }
            }
        }
        private void RandomShoot()
        {
            //Random rnd = new Random();
            //int k = 0;
            //while (!CanShoot())
            //{
            //    k = rnd.Next(0, 4);
            //    if (k == 0 && Y > 0)
            //        Y--;
            //    else if (k == 1 && X < 16)
            //        X += 2;
            //    else if (k == 2 && Y < 9)
            //        Y++;
            //    else if (k == 3 && X > 0)
            //        X -= 2;
            //}
            if (Y > 0 && CanShoot(0, -1))
                Y--;
            else if (X < 17 && CanShoot(2, 0))
                X += 2;
            else if (X > 0 && CanShoot(-2, 0))
                X -= 2;
            else if (Y < 9 && CanShoot(0, 1))
                Y++;
        }
        public void FightMap(bool cheat = false)
        {
            int i = 0;
            Cords cords = new Cords(0, 0);
            while (i != 1)
            {
                Console.ResetColor();
                Console.SetCursorPosition(20 + MapPosition - 4, 5 - 2 + 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Гравець {player.Name}, життя");
                if (player.AllHealth() <= 9)
                    Console.WriteLine($" {player.AllHealth()}, ходи {player.Steps}");
                else
                    Console.WriteLine($"{player.AllHealth()}, ходи {player.Steps}");
                Console.ResetColor();
                ShowMap(cheat);
                if (!player.AnyHealth())
                    break;
                ConsoleKey key;
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
                Console.WriteLine(WhatShip(10));
                while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
                {
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (Y > 0)
                                {
                                    PreviousColor(10,cheat);
                                    Y--;
                                    MovingShip(10);
                                }
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                if (Y < 9)
                                {
                                    PreviousColor(10, cheat);
                                    Y++;
                                    MovingShip(10);
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            {
                                if (X < 17)
                                {
                                    PreviousColor(10, cheat);
                                    X += 2;
                                    MovingShip(10);
                                }
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            {
                                if (X > 0)
                                {
                                    PreviousColor(10, cheat);
                                    X -= 2;
                                    MovingShip(10);
                                }
                            }
                            break;
                    }
                }
                if(CanShoot())
                {
                    player.Steps++;
                    for (int p = 0; p < player.Ship.Length; p++)
                        if (player.Ship[p].Check(X, Y))
                        {
                            shipNum = p;
                        }
                    if (player.Ship[shipNum].Check(X, Y))
                    {
                        player.Ship[shipNum].Health--;
                        maps[Y, X] = 'x';
                        maps[Y, X + 1] = 'x';
                        for (int j = 0; j < 10; j++)
                        {
                            if(!player.Ship[j].Alive())
                            {
                                if(!player.Ship[j].IsDestroyed)
                                {
                                    PreviousColor(10, true);

                                    cords.X = X;
                                    cords.Y = Y;
                                    X = player.Ship[j].StartCords.X;
                                    Y = player.Ship[j].StartCords.Y;
                                    IsHorisontal = player.Ship[j].IsHorisontal;
                                    AddGrayZone(j, 'o');
                                    player.Ship[j].IsDestroyed = true;
                                    X = cords.X;
                                    Y = cords.Y;
                                }
                            }    
                        }
                        
                        continue;
                    }
                    else
                    {
                        maps[Y, X] = 'o';
                        maps[Y, X+1] = 'o';
                    }
                    i++;
                }
            }
        }
        public void AddShips()
        {
            int i = 0;
            while(i != 10)
            {
                ShowMap();
                ConsoleKey key;
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(20 + X + 1, 5 + Y + 1);
                if (IsHorisontal)
                    Console.WriteLine(WhatShip(i));
                else
                    MoveVertical(i);
                while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
                {
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (Y > 0)
                                {
                                    PreviousColor(i);
                                    Y--;
                                    if (IsHorisontal) MovingShip(i);
                                    else MoveVertical(i);
                                }
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                if (Y < 9 && IsHorisontal)
                                {
                                    PreviousColor(i);
                                    Y++;
                                    MovingShip(i);
                                }
                                else if(Y < 10 - WhatShip_int(i) / 2 && !IsHorisontal)
                                {
                                    PreviousColor(i);
                                    Y++;
                                    MoveVertical(i);
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            {
                                if (X < 20 - WhatShip_int(i)-1 && IsHorisontal)
                                {
                                    PreviousColor(i);
                                    X += 2;
                                    MovingShip(i);
                                }
                                else if (X < 17 && !IsHorisontal)
                                {
                                    PreviousColor(i);
                                    X += 2;
                                    MoveVertical(i);
                                }
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            {
                                if (X > 0)
                                {
                                    PreviousColor(i);
                                    X -= 2;
                                    if (IsHorisontal) MovingShip(i);
                                    else MoveVertical(i);
                                }
                            }
                            break;
                        case ConsoleKey.R:
                            {
                                if (IsHorisontal && Y < 10 - WhatShip_int(i)/2 + 1)
                                {
                                    PreviousColor(i);
                                    Console.SetCursorPosition(20 + X + 1, 5 + Y + 1);
                                    MoveVertical(i);
                                    IsHorisontal = false;
                                }
                                else if(!IsHorisontal && X < 20 - WhatShip_int(i) + 1)
                                {
                                    PreviousColor(i);
                                    Console.SetCursorPosition(20 + X + 1, 5 + Y + 1);
                                    Console.BackgroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine(WhatShip(i));
                                    IsHorisontal = true;
                                }
                            }
                            break;
                    }
                }
                if (IsFreeSpace(i))
                {
                    AddOneShip(i);
                    player.SetShip(i,X,Y,IsHorisontal,WhatShip_int(i)/2);
                    AddGrayZone(i);
                    i++;
                }
            }
            X = 0;
            Y = 0;
            ShowMap();
        }
        private void ShowArr()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(maps[i, j]);
                }
                Console.WriteLine();
            }
        }
        private void AddGrayZone(int i, char symbol = '-')
        {
            if (IsHorisontal)
            {
                for (int j = 0; j < WhatShip_int(i); j++)
                {
                    if (Y - 1 >= 0)
                        maps[Y - 1, X + j] = symbol;
                    if(Y + 1 <= 9)
                        maps[Y + 1, X + j] = symbol;
                }
                if (X - 2 >= 0)
                {
                    maps[Y, X - 1] = symbol;
                    maps[Y, X - 2] = symbol;
                    if (Y - 1 >= 0)
                    {
                        maps[Y - 1, X - 1] = symbol;
                        maps[Y - 1, X - 2] = symbol;
                    }
                    if (Y + 1 <= 9)
                    {
                        maps[Y + 1, X - 1] = symbol;
                        maps[Y + 1, X - 2] = symbol;
                    }
                }
                if (X + 2 <= 20 - WhatShip_int(i))
                {
                    maps[Y, X + WhatShip_int(i) + 1] = symbol;
                    maps[Y, X + WhatShip_int(i)] = symbol;
                    if (Y - 1 >= 0)
                    {
                        maps[Y - 1, X + WhatShip_int(i)+ 1] = symbol;
                        maps[Y - 1, X + WhatShip_int(i)] = symbol;
                    }
                    if (Y + 1 <= 9)
                    {
                        maps[Y + 1, X + WhatShip_int(i) + 1] = symbol;
                        maps[Y + 1, X + WhatShip_int(i)] = symbol;
                    }
                }

            }
            else
            {
                for (int j = 0; j < WhatShip_int(i)/2; j++)
                {
                    if (X - 2 >= 0)
                    {
                        maps[Y + j, X - 2] = symbol;
                        maps[Y + j, X - 1] = symbol;
                    }
                    if (X + 3 <= 19)
                    {
                        maps[Y + j, X + 3] = symbol;
                        maps[Y + j, X + 2] = symbol;
                    }
                }
                if (Y - 1 >= 0)
                {
                    maps[Y -1, X] = symbol;
                    maps[Y -1, X+1] = symbol;
                    if (X - 2 >= 0)
                    {
                        maps[Y - 1, X - 1] = symbol;
                        maps[Y - 1, X - 2] = symbol;
                    }
                    if (X + 3 <= 20)
                    {
                        maps[Y - 1, X + 2] = symbol;
                        maps[Y - 1, X + 3] = symbol;
                    }
                }
                if(Y <= 9 - WhatShip_int(i)/2)
                {
                    maps[Y + WhatShip_int(i)/2, X] = symbol;
                    maps[Y + WhatShip_int(i)/2, X + 1] = symbol;
                    if (X - 2 >= 0)
                    {
                        maps[Y + WhatShip_int(i) / 2, X - 1] = symbol;
                        maps[Y + WhatShip_int(i) / 2, X - 2] = symbol;
                    }
                    if (X + 3 <= 19)
                    {
                        maps[Y + WhatShip_int(i) / 2, X + 2] = symbol;
                        maps[Y + WhatShip_int(i) / 2, X + 3] = symbol;
                    }
                }
            }

        }
        private bool IsFreeSpace(int i)
        {
            for (int j = 0; j < WhatShip_int(i)/2; j++)
            {
                if(IsHorisontal)
                {
                    if (maps[Y, X + j * 2] != ' ')
                        return false;
                }
                else
                {
                    if (maps[Y + j, X] != ' ')
                        return false;
                }
            }
            return true;
        }
        private string WhatShip(int i)
        {
            if (i == 0)
                return"        ";
            else if (i > 0 && i < 3)
                return "      ";
            else if (i >= 3 && i < 6)
                return "    ";
            else
                return "  ";
        }
        private int WhatShip_int(int i)
        {
            if (i == 0)
                return 8;
            else if (i > 0 && i < 3)
                return 6;
            else if (i >= 3 && i < 6)
                return 4;
            else
                return 2;
        }
        private void MovingShip(int i)
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
            Console.WriteLine(WhatShip(i));
        }
        public void AddOneShip(int i)
        {
            if (IsHorisontal)
            {
                for (int j = 0; j < WhatShip_int(i); j++)
                {
                    maps[Y, X + j] = 'k';
                }
            }
            else
            {
                for (int j = 0; j < WhatShip_int(i)/2; j++)
                {
                    maps[Y + j, X] = 'k';
                    maps[Y + j, X + 1] = 'k';
                }
            }
        }
        private void MoveVertical(int i)
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            for (int j = 0; j < WhatShip_int(i) / 2; j++)
            {
                Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1 + j);
                Console.WriteLine("  ");
            }
        }
        public void PreviousColor(int i, bool IsFight = false)
        {
            if (IsHorisontal)
            {
                if (maps[Y, X] == ' ' || maps[Y, X] == '-')
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
                    Console.Write(WhatShip(i));
                }
                else if (maps[Y, X] == 'k')
                {
                    if(IsFight)
                        Console.BackgroundColor = ConsoleColor.Blue;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
                    Console.Write(WhatShip(i));
                }
                else if (maps[Y, X] == 'x')
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
                    Console.Write(WhatShip(i));
                }
                else if (maps[Y, X] == 'o')
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1);
                    Console.Write(WhatShip(i));
                }

            }
            else
            {
                if (maps[Y, X] == ' ' || maps[Y, X] == '-')
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    for (int j = 0; j < WhatShip_int(i) / 2; j++)
                    {
                        Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1 + j);
                        Console.WriteLine("  ");
                    }
                }
                else if (maps[Y, X] == 'k')
                {
                    if (IsFight)
                        Console.BackgroundColor = ConsoleColor.Blue;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;
                    for (int j = 0; j < WhatShip_int(i) / 2; j++)
                    {
                        Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1 + j);
                        Console.WriteLine("  ");
                    }
                }
                else if (maps[Y, X] == 'x')
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    for (int j = 0; j < WhatShip_int(i) / 2; j++)
                    {
                        Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1 + j);
                        Console.WriteLine("  ");
                    }
                }
                else if (maps[Y, X] == 'o')
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int j = 0; j < WhatShip_int(i) / 2; j++)
                    {
                        Console.SetCursorPosition(20 + MapPosition + X + 1, 5 + Y + 1 + j);
                        Console.WriteLine("  ");
                    }
                }
            }
        }
    }
}

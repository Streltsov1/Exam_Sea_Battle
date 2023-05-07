using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    interface ICheckble
    {
        Cords StartCords { get; set; }
        bool IsHorisontal { get; set; }
        int Size { get; set; }
        int Health { get; set; }
        public bool Check(int x, int y);
    }
    struct Cords
    {
        public int X;
        public int Y;
        public Cords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Ship : ICheckble
    {
        public Cords StartCords { get; set; }
        public bool IsHorisontal { get; set; }
        public bool IsDestroyed { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        public Ship(int x, int y, bool horisontal, int size)
        {
            Cords cords = new Cords(x, y);
            StartCords = cords;
            IsHorisontal = horisontal;
            IsDestroyed = false;
            Size = size;
            Health = size;
        }
        public bool Alive()
        {
            if (Health != 0)
                return true;
            return false;
        }
        public bool Check(int x, int y)
        {
            if (Health == 0)
                return false;
            if (IsHorisontal)
            {
                if (StartCords.Y != y)
                    return false;
                else
                {
                    for (int i = 0; i < Size; i++)
                    {
                        if (StartCords.X + i*2 == x)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (StartCords.X != x)
                    return false;
                else
                {
                    for (int i = 0; i < Size; i++)
                    {
                        if (StartCords.Y + i == y)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

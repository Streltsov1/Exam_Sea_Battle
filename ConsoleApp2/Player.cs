using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Player
    {
        public int Steps { get; set; }
        private Ship[] ships = new Ship[10];
        public Ship[] Ship => ships;

        public string Name { get; set; }
        public Player()
        {

        }
        public Player(string name)
        {
            Name = name;
            Steps = 0;
        }
        public void SetShip(int i, int x, int y, bool horisontal, int size)
        {
            ships[i] = new Ship(x, y, horisontal, size);
        }
        public bool Check(int x, int y)
        {
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i].Check(x, y))
                    return true;
            }
            return false;
        }
        public bool AnyHealth()
        {
            if (AllHealth() > 0)
                return true;
            return false;
        }
        public int AllHealth()
        {
            int health = 0;
            for (int i = 0; i < ships.Length; i++)
            {
                health += ships[i].Health;
            }
            return health;
        }
        public override string ToString()
        {
            return $"кількість ходів: {Steps}";
        }
        //public void Show()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Console.WriteLine($"{ships[i].StartCords.X} {ships[i].StartCords.Y} {ships[i].IsHorisontal}");
        //    }
        //}
    }
}

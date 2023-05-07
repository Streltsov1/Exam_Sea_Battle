using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Menu
    {
        private int count = 0;
        private List<string> item;
        private Player player1 = new Player();
        private Player player2 = new Player();
        private Map our_map = new Map();
        private Map enemy_map = new Map();
        private List<Stats> AllStats = new List<Stats>();
        public Menu()
        {
            item = new List<string>() { "Розставити кораблі самому, та змагатися з ботом", "Розставити кораблі випадково, та змагатися з ботом", "Грати проти іншого гравця(на одному ПК)","Завантажити рейтинг" };

        }
        public int ShowMenu()
        {
            Console.Clear();
            for (int i = 0; i < item.Count; i++)
            {
                Console.SetCursorPosition(40, 5+i);

                if (i == count)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine(item[i]);
            }
            ConsoleKey key;
            while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
            {
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            if (count > 0)
                            {
                                count--;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        {
                            if (count < item.Count - 1)
                            {
                                count++;
                            }
                        }
                        break;
                }
                Console.Clear();
                for (int i = 0; i < item.Count; i++)
                {
                    Console.SetCursorPosition(40, 5 + i);
                    if (i == count)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;

                    Console.WriteLine(item[i]);
                }
            }
            Console.Clear();
            return count;
        }
        public void StartGame()
        {
            Stats stats = new Stats();
            string name;
            int variant = ShowMenu();
            if (variant == 3)
            {
                string jsonResult = File.ReadAllText("data.json");
                var peopleResult = JsonSerializer.Deserialize<List<Stats>>(jsonResult);
                foreach (var p in peopleResult)
                {

                    Console.WriteLine(p);
                }
                Console.ReadKey();
            }
            else if (variant == 0 || variant == 1 || variant == 2)
            {
                Console.ResetColor();
                Console.Write("ВВедіть імя гравця:");
                name = Console.ReadLine();
                player1 = new Player(name);
                our_map = new Map(player1);
                Fight fight;
                Console.Clear();
                if (variant == 0)
                {
                    player2 = new Player("BOT");
                    enemy_map = new Map(player2, 40);
                    our_map.AddShips();
                    enemy_map.AddRandomShip();
                }
                else if(variant == 1)
                {
                    player2 = new Player("BOT");
                    enemy_map = new Map(player2, 40);
                    our_map.AddRandomShip();
                    enemy_map.AddRandomShip();
                }
                else
                {
                    Console.Write("ВВедіть імя другого гравця:");
                    name = Console.ReadLine();
                    player2 = new Player(name);
                    enemy_map = new Map(player2, 40);
                    our_map.AddRandomShip();
                    enemy_map.AddRandomShip();
                }
                fight = new Fight(our_map, enemy_map);
                if(variant != 2)
                    name = fight.Battle_Bot(true).Name;
                else
                    name = fight.Battle_Player(true).Name;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\t\t\t\t\t{name} виграв!!!");
                if (player1.Name == name)
                {
                    Console.WriteLine($"{ name } { player2.ToString()}");
                    stats.Add(name, player2.Steps, fight.StartTime, fight.FinishTime);
                }
                else
                {
                    Console.WriteLine($"{ name } { player1.ToString()}");
                    stats.Add(name, player1.Steps, fight.StartTime, fight.FinishTime);
                }
                Console.WriteLine("Зберегти гру до статистики\n1-Так\n2-Ні");
                variant = 2;
                do
                {
                    variant = int.Parse(Console.ReadLine());
                } while (variant < 1 || variant > 2);
                if(variant == 1)
                {
                    AllStats.Add(stats);
                    string json = JsonSerializer.Serialize(AllStats);
                    File.WriteAllText("data.json", json);
                }
            }
            StartGame();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Fight
    {
        private Map player1;
        public Map Player1 => player1;
        private Map player2;
        public Map Player2 => player2;
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public Fight(Map p1, Map p2)
        {
            player1 = p1;
            player2 = p2;
        }
        public Player Battle_Bot(bool cheat = false)
        {
            StartTime = DateTime.Now;
            while (player1.Player.AnyHealth() && player2.Player.AnyHealth())
            {
                player2.RandomFight();//FightMap(cheat);
                if(player2.Player.AnyHealth())
                    player1.RandomFight();
            }
            FinishTime = DateTime.Now;
            if(!player1.Player.AnyHealth())
            {
                return player2.Player;
            }
            else
            {
                return player1.Player;
            }

        }
        public Player Battle_Player(bool cheat = false)
        {
            StartTime = DateTime.Now;
            while (player1.Player.AnyHealth() && player2.Player.AnyHealth())
            {
                player1.ShowMap(true);
                player2.FightMap(cheat);
                if (player2.Player.AnyHealth())
                    player1.FightMap(cheat);
            }
            FinishTime = DateTime.Now;
            if (!player1.Player.AnyHealth())
            {
                return player2.Player;
            }
            else
            {
                return player1.Player;
            }

        }
    }
}

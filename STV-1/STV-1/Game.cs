using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    public class Game
    {
        public Dungeon d;
        Player player;
        int level;

        public Game()
        {
            NextDungeon();
        }

        // geen save en load meer

        public Dungeon NextDungeon()
        {
            if (d == null)
            {
                d = new Dungeon(1);
                level = 1;
                player = new Player(50, 5, d.nodes[0], d);
            }
            else
            {
                level++;
                d = new Dungeon(level);
                player.Location = d.nodes[0];
            }
            return d;
        }
    }
}
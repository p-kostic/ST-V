using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    public class Game
    {
        public Dungeon Dungeon;

        public Game()
        {
            Dungeon d = new Dungeon(2);
        }

        // geen save en load meer

        public void NextDungeon(Dungeon curDungeon)
        {
            // Change 1 for level increase
            Dungeon d = new Dungeon(curDungeon.level += 1);
        }
    }
}
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

        public void NextDungeon()
        {
            // First we check if a dungeon exists. If not, we make a new one with difficulty 1.
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STV1;

namespace STV_1
{ 
    public class Specification
    {
        public bool TestSpecifications(Dungeon d)
        {
            if (TestDungeonMonsters(d))
                return true;
            return false;

        }

        // In any state s along a play, the number of monsters in every node u in s does not
        // exceed its capacity.
        public bool TestDungeonMonsters(Dungeon d)
        {
            foreach (Node n in d.nodes)
            {
                int count = 0;
                foreach (Pack pack in n.nodePacks)
                {
                    count += pack.monsters.Count;
                }
                if (n.MaxMonsters < count)
                    return false;
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STV1;

namespace STV_1
{
    public class Specification
    {
        public static bool TestSpecifications(Dungeon d)
        {
            bool AllTest = true;

            AllTest &= SpecificationTestDungeonMonsters(d);

            return AllTest;

        }

        // #############################   1    ############################################
        // In any state s along a play, the number of monsters in every node u in s does not
        // exceed its capacity.
        public static bool SpecificationTestDungeonMonsters(Dungeon d)
        {
            bool characteristics = true;



            foreach (Node n in d.nodes)
            {
                int count = 0;
                foreach (Pack pack in n.nodePacks)
                {
                    count += pack.monsters.Count;
                }
                if (n.MaxMonsters < count)
                {
                    return false;
                }
            }
            return true;
        }

        // #############################   2    ############################################
        // Every monster pack never leaves its zone
        public static bool SpecificationTestLeaveZones(Dungeon d)
        {
            throw new NotImplementedException();
        }

        // #############################   3    ############################################
        // Suppose Z is the player’s current zone. At every turn, and while the player is still in Z, the
        // distance between every monster pack in Z to either the player or the bridge at the zone’s
        // end should not increase
        public static bool SpecificationTestDistanceZone(Dungeon d)
        {
            throw new NotImplementedException();
        }


        // #############################   4    ############################################
        // Suppose that k is the last turn the player uses a time crystal on a bridge(thus destroying
        // it). If there is no such moment, then k = 0.For any state s after sk, and before the next
        // use of a time crystal on a bridge, The sum of:
        // player.KP + total number of monsters in s remains constant.
        // This time, define your own black box coverage criterion.
        public static bool SpecificationTestTimeCrystalDistance(Dungeon d)
        {
            throw new NotImplementedException();
        }

        // #############################   5    ############################################
        // Tweak your monster-packs distribution scheme, so that it is guaranteed that to survive
        // a dungeon of level L, the player must be confronted with at least L+1 unique combats
        // (combats against different packs) before he can reach the exit.
        // Come up with your own black box coverage criterion.
        public static bool SpecificationTestSurvival(Dungeon d)
        {
            throw new NotImplementedException();
        }

    }
}

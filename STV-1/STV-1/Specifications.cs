using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using STV1;

namespace STV_1
{
    public class Specification
    {
        public bool TestSpecifications(Dungeon d, Player p)
        {
            bool AllTest = true;

            AllTest &= SpecificationTestDungeonMonsters(d);
            // AllTest &= SpecificationTestLeaveZones(d, p);

            return AllTest;
        }

        // #############################[   1   ]############################################
        // In any state s along a play, the number of monsters in every node u in s does not
        // exceed its capacity.
        bool cap = true;
        bool zero = false;
        bool low = false;
        bool high = false;
        bool zeroN = false;
        bool partial = false;
        bool full = false;

        bool entered = false;
        bool left = false;

        Dictionary<Pack, Node> prevLocations = new Dictionary<Pack, Node>();
        int lastDungeonLevel = -1;
        public bool SpecificationTestDungeonMonsters(Dungeon d)
        {


            //----------- [General] Test Node capacity -------------//
            foreach (Node n in d.nodes)
            {
                int count = 0;
                foreach (Pack pack in n.nodePacks)
                    count += pack.monsters.Count;
                if (n.MaxMonsters < count)
                    cap = false;
            }

            //-----------[A + B] Test Node Level & Occupancy -------------// 

            foreach (Node n in d.nodes)
            {
                int count = 0;
                foreach (Pack pack in n.nodePacks)
                    count += pack.monsters.Count;

                if (count == 0)
                    zero = true;
                if (count > 0 && count < n.MaxMonsters)
                    partial = true;
                if (count == n.MaxMonsters)
                    full = true;

                if (n.level == 0)
                    zeroN = true;
                if (n.level == 1)
                    low = true;
                if (n.level > 1)
                    high = true;
            }

            //-----------[C] Test Node Movement -------------// 
            // pack movement, all four of disjoint combinations of: there is (or not) pack entering u
            // and there is (or not) a pack leaving u.
            if (d.level != this.lastDungeonLevel)
            {
                prevLocations.Clear();
                lastDungeonLevel = d.level;
            }
            else
            {
                if (prevLocations.Count == 0)
                    foreach (Pack t in d.packs)
                        prevLocations[t] = t.PackLocation;


                foreach (Node n in d.nodes)
                {
                    foreach (Pack p in n.nodePacks)
                        if (prevLocations[p] != n)
                            entered = true;


                    foreach (Pack p in n.nodePacks)
                        if (prevLocations[p] == n && p.PackLocation != n)
                            left = true;
                }
            }
            return cap & zeroN & low & high & zero & partial & full & entered & left;
        }


        // #############################[   2   ]############################################
        // Every monster pack never leaves its zone
        Dictionary<Pack, int> startingLevels = new Dictionary<Pack, int>();
        private bool general = true;
        private bool first, middle, last = false;
        public static bool fleeing, moving = false;
        private bool onBridge, notOnBridge = false;

        int lastDungeonLevel2 = -1;

        public bool SpecificationTestLeaveZones(Dungeon d)
        {
            //-----------[General] Never leaves its own zone -------------// 
            if (d.level != this.lastDungeonLevel2)
            {
                foreach (Pack p in d.packs)
                    startingLevels[p] = p.PackLocation.level;

                lastDungeonLevel = d.level;
            }
            else
            {
                foreach (Pack p in d.packs)
                    if (p.PackLocation.level != startingLevels[p])
                        general = false;
            }

            //-----------[A] The Zone's position n (first, middle, last). -------------// 
            for (int i = 0; i < d.packs.Count; i++)
            {
                if (d.packs[i].PackLocation.level == 1)
                    first = true;

                if (d.packs[i].PackLocation.level == 2)
                    middle = true;

                if (d.packs[i].PackLocation.level > 2)
                    last = true;

                //-----------[C] the monster's location (on a bridge, not on a bridge). -----// 
                if (d.packs[i].PackLocation.type == "bridge")
                    onBridge = true;
                else
                    notOnBridge = true;
            }

            //-----------[B] The monster's action n (fleeing a combat, just moving). -------------//
            // We didn't know how to do this properly, so we made the booleans static and set them to true inside
            // the fleeing code (Node.cs doCombatRound method) and the move code (Pack.cs MovePack method)

            return general & first & middle & last & fleeing & moving & onBridge & notOnBridge;
        }

        // #############################[   3   ]############################################
        // Suppose Z is the player’s current zone. At every turn, and while the player is still in Z, the
        // distance between every monster pack in Z to either the player or the bridge at the zone’s
        // end should not increase
        private bool first3, middle3, last3 = false;
        private bool entering3, remaining3, notEntered3 = false;
        private bool towards3, awayFrom3, stay3 = false;
        private bool general3 = true;
        Dictionary<Pack, int> distanceToPlayer = new Dictionary<Pack, int>();
        private int previousZone = -1;
        public bool SpecificationTestDistanceZone(Dungeon d, Player player)
        {

            //-----------[General] Distance between monster and player never increases -------------// 
            if (player.Location.level != previousZone)
            {
                distanceToPlayer.Clear();
                previousZone = player.Location.level;
                foreach (Pack p in d.packs)
                {
                    if (p.PackLocation.level == player.Location.level)
                    {
                        // If we enter a zone, we add all the distances of the pack to the player to the dictionary
                        int curDistanceToPlayer = d.FindShortestPath(p.PackLocation, player.Location).Count;
                        distanceToPlayer[p] = curDistanceToPlayer;
                    }
                }
            }
            else
            {
                foreach (Pack p in d.packs)
                {
                    int curDistanceToPlayer = d.FindShortestPath(p.PackLocation, player.Location).Count;
                    if (curDistanceToPlayer > distanceToPlayer[p])
                    {
                        general3 = false;
                    }
                }
            }

            //------------[A] The zone’s location (first, middle, last). ----------------//



            //------------[B] the player’s location (just entering the bridge at the end of the zone, ----------------//
            //-------------                       remaining on that bridge, not yet on that bridge).  ----------------//

            //------------[C] the player’s movement (moving towards the                                      ----------------//
            //------------  bridge at the end of the zone, moving away from that bridge, stay on its place). ----------------//


            return first3 & middle3 & last3 & entering3 & remaining3 & notEntered3 & towards3 & awayFrom3 & stay3;
        }


        // #############################[   4   ]############################################
        // Suppose that k is the last turn the player uses a time crystal on a bridge(thus destroying
        // it). If there is no such moment, then k = 0.For any state s after sk, and before the next
        // use of a time crystal on a bridge, The sum of:
        // player.KP + total number of monsters in s remains constant.
        // This time, define your own black box coverage criterion.
        int prevTcCount = 0;
        int k = 0;
        int prevK = 0;
        int sum = 0;
        int prevSum = 0;
        public bool SpecificationTestTimeCrystalDistance(Dungeon d, Player player)
        {
            /*int curCount = player.inventory.Count(s => s.ItemType() == "TimeCrystal");
            if(prevTcCount > curCount){ // Check if a TC was used this turn
                k++;
            }
            prevTcCount = curCount;

            int monsterCount = 0;
            foreach(Pack p in d.packs){
                monsterCount += p.monsters.Count;
            }

            sum = player.kp + monsterCount;
            if (k > prevK)
            {
                prevK = k;
                prevSum = sum;
                return true;
            }
            else {
                if (prevSum != sum)
                {
                    return false;
                }
            }
            return true;*/
            throw new NotImplementedException();
        }

        // #############################[   5   ]############################################
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV_1
{
    class Node
    {
        public List<Node> connections;
        private const int maxconnections = 4;

        private int maxmonsters;

        private int level;
        public string type;
        public int id;

        List<Pack> nodePacks;
        Player nodePlayer;

        /// <summary>
        /// Creates a new node that can be occupied by a player, monster packs or both.
        /// There's a limit of how many monsters each node can accomodate.
        /// </summary>
        /// <param name="level">Node u's level</param>
        /// <param name="M">constant which is the same over the whole dungeon</param>
        public Node(int level, int M, string type = "normal")
        {
            this.type = type;
            this.level = level;
            this.maxmonsters = M * (level + 1);
            connections = new List<Node>();
            nodePacks = new List<Pack>();
        }

        public void AddPack(Pack pack)
        {
            nodePacks.Add(pack);
        }

        public void RemovePack(Pack pack)
        {
            nodePacks.Remove(pack);
        }

        public void AddPlayer(Player player)
        {
            nodePlayer = player;
        }

        public void RemovePlayer()
        {
            nodePlayer = null;
        }

        public void UseItem(Item i)
        {
            i.UseItem(nodePlayer);
        }

        public void DoCombatRound(Pack pack)
        {
            /*
             * 1: Speler kan item gebruiken
             * 2: Speler valt aan
             * 3: Pack valt aan
             * 4: 1 vd twee dood, dan einde combat
             * 5: anders kans om te vluchten
             * 6: repeat
             */
        }

        public void DoCombat(Pack pack)
        {
        }

        public bool PackFitsInNode(Pack pack)
        {
            int amountOfMonsters = 0;
            foreach (Pack p in nodePacks)
                amountOfMonsters += p.PackSize;
            return maxmonsters - (amountOfMonsters + pack.PackSize) >= 0;
        }

        public bool PlayerInNode()
        {
            return nodePlayer != null;
        }
    }
}
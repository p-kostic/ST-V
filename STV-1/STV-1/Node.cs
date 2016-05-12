using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    public class Node
    {
        public List<Node> connections;
        private const int maxconnections = 4;

        private int maxmonsters;

        private int level;
        public string type;
        public int id;
        static int idCounter = 0; // To give an id to a certain node.

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
            id = idCounter;
            idCounter++;
        }

        // Adds a pack to the node.
        public void AddPack(Pack pack)
        {
            if (PackFitsInNode(pack))
                nodePacks.Add(pack);
        }

        // Removes a certain pack from the node.
        public void RemovePack(Pack pack)
        {
            nodePacks.Remove(pack);
        }

        // Add the player to the node.
        public void AddPlayer(Player player)
        {
            nodePlayer = player;
        }

        // Remove the player from the node.
        public void RemovePlayer()
        {
            nodePlayer = null;
        }

        // Lets the player use an item.
        public void UseItem(Item i)
        {
            i.UseItem(nodePlayer);
        }

        // Check for a combat situation in the node.
        public void CheckInCombat()
        {
            if (nodePacks.Count > 1 || (nodePacks.Count > 0 && nodePlayer != null))
                DoCombat(nodePacks[0]);
        }

        public void DoCombatRound(Pack pack)
        {
            /* TODO: 
             * Speler kan item gebruiken
             * Speler kan vluchten
             */

            // If there is a player in the node and there's at least one pack in the node,
            // We will walk through the loop. Also, if the combat has been ended we will stop
            // as well.
            if (true) // CHANGE TO WHEN TIMECRYSTAL ACTIVE
            {
                for (int i = 0; i < pack.PackSize; i++)
                    nodePlayer.Attack(pack.Monsters[i]);
            }
            else
                nodePlayer.Attack(pack.Monsters[0]);

            pack.PackAttack(nodePlayer);

            /*IMPLEMENT RETREAT PLAYER HERE*/
            if (pack.PackHP < nodePlayer.HP && pack.PackHP > 0)
            {
                pack.MovePack(this.connections[0]);
            }
        }

        public void DoCombat(Pack pack)
        {
            bool endCombat = false;
            if ((nodePlayer != null && nodePacks.Count > 0) || !endCombat)
                DoCombatRound(pack);

            if (nodePlayer.IsDead || pack.PackDied)
                endCombat = true;
        }

        // This method checks if a given pack can fit in the node, based on the formula
        // and the amount of monsters already in the node.
        public bool PackFitsInNode(Pack pack)
        {
            int amountOfMonsters = 0;
            foreach (Pack p in nodePacks)
                amountOfMonsters += p.PackSize;
            return maxmonsters - (amountOfMonsters + pack.PackSize) >= 0;
        }

        // Check if the player is in a certain node.
        public bool PlayerInNode()
        {
            return nodePlayer != null;
        }

        // Check if there is at least one pack in a certain node
        public bool PackInNode()
        {
            return (nodePacks.Count > 0);
        }

        public int MaxMonsters { get { return maxmonsters; } }
    }
}
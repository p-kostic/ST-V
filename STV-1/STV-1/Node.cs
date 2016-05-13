﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    public class Node
    {
        public List<Node> connections;

        private int maxmonsters;

        private int level;
        public string type;
        public int id;
        static int idCounter = 0; // To give an id to a certain node.

        public List<Pack> nodePacks;
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

        /// <summary>
        /// Adds a pack to the current node and updates the location of all the monsters accordingly
        /// It also also adds the pack to the nodePacks list of the current node.
        /// </summary>
        /// <param name="pack">The pack that is being added</param>
        public void AddPack(Pack pack)
        {
            if (this.type == "normal" || this.type == "bridge")
                if (PackFitsInNode(pack))
                {
                    nodePacks.Add(pack);
                    for (int i = 0; i < pack.Monsters.Count; i++)
                    {
                        pack.Monsters[i].Location = this;
                    }
                }
        }

        /// <summary>
        /// Removes the pack from this node.
        /// </summary>
        /// <param name="pack">The pack that is to be removed</param>
        public void RemovePack(Pack pack)
        {
            if (this.type == "normal" || this.type == "bridge")
                nodePacks.Remove(pack);
        }

        /// <summary>
        /// Adds a player to this node
        /// </summary>
        /// <param name="player">The player that you want to add to this node</param>
        public void AddPlayer(Player player)
        {
            nodePlayer = player;
        }

        /// <summary>
        /// Removes the player from this node
        /// </summary>
        public void RemovePlayer()
        {
            nodePlayer = null;
        }

        /// <summary>
        /// Uses the item at the player node
        /// </summary>
        /// <param name="i">The item you want to use</param>
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
                // @Bor, hoe werkt die node identifier? de pack moet naar een willekeurige
                // aangrenzende node worden gestuurd.

                // Antwoord: Gebruik van de huidige node, node.connections, dit is een lijst met alle nodes die geconnect zijn aan de huidige node (aka de neighbours)
                //pack.MovePack();
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

        /// <summary>
        /// Checks if the given pack can fit in the node, based on the provided formula
        /// and the amount of monsters already in the node
        /// </summary>
        /// <param name="pack">The pack you want to check this for</param>
        /// <returns></returns>
        public bool PackFitsInNode(Pack pack)
        {
            int amountOfMonsters = 0;
            foreach (Pack p in nodePacks)
                amountOfMonsters += p.PackSize;
            return maxmonsters - (amountOfMonsters + pack.PackSize) >= 0;
        }

        /// <summary>
        /// Return true if the player is in this Node
        /// Returns false if the player is not in this node
        /// </summary>
        /// <returns></returns>
        public bool PlayerInNode()
        {
            return nodePlayer != null;
        }

        /// <summary>
        /// Return true if at least one pack is in this node
        /// Returns false if there are no packs in this node
        /// </summary>
        /// <returns></returns>
        public bool PackInNode()
        {
            return (nodePacks.Count > 0);
        }

        /// <summary>
        /// Checks whether this node is connected to the other node you provided
        /// </summary>
        /// <param name="b">The node you want to check connectivity with</param>
        /// <returns></returns>
        public bool ConnectedTo(Node b)
        {
            if (connections.Contains(b) && b.connections.Contains(this))
                return true;
            return false;
        }

        public int MaxMonsters { get { return maxmonsters; } }
    }
}
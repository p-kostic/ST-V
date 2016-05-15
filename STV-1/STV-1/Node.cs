using System;
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
        private bool repeatCombat;

        public int level;
        public string type;
        public int id;
        static int idCounter = 0; // To give an id to a certain node.

        public List<Pack> nodePacks;
        public List<Item> items;
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
            items = new List<Item>();
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
                        pack.Monsters[i].Location = this;
                }
        }

        /// <summary>
        /// Removes the pack from this node.
        /// </summary>
        /// <param name="pack">The pack that is to be removed</param>
        public void RemovePack(Pack pack)
        {
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

        // This method checks if the player should be in combat, by checking health, position and pack count.
        public void CheckInCombat()
        {
            while (nodePacks.Count > 0 && nodePlayer != null && !nodePlayer.IsDead)
                DoCombat();
        }

        // This method will play out a combat situation as long as the player is in combat.
        public void DoCombat()
        {
            repeatCombat = true;
            while (repeatCombat)
                DoCombatRound(nodePacks[0], nodePlayer);
        }

        // This method will do a round of combat. We will explain how it works as we go through it.
        public void DoCombatRound(Pack pack, Player player)
        {
            nodePlayer = player;

            // We get the next command from the botplayer, and set the timecrystal use to false.
            BotPlayer command = nodePlayer.GetCommand();
            bool activeTimeCrystal = false;

            // As long as there is a command, we pass.
            if (command != null)
            {
                // If the command is to use an item, we will check which item has been used and act
                // accordingly.
                if (command.itemUsed)
                {
                    if (command.usedHP)
                        nodePlayer.UsePotion();
                    else if (command.usedTC)
                        activeTimeCrystal = nodePlayer.UseCrystal();
                }

                // If a timecrystal has been used, we let the player attack each monster in the group.
                // Else the player will only attack the first monster in the group.
                if (activeTimeCrystal)
                    foreach (Monster monster in pack.Monsters)
                        nodePlayer.Attack(monster);
                else
                    nodePlayer.Attack(pack.Monsters[0]);
            }
            // If there is no command, we will just do the basic combat, in which the player attacks, and
            // then the pack will attack.
            else
                nodePlayer.Attack(pack.Monsters[0]);

            // We update the pack to see if it died and whatnot.
            pack.UpdatePack();

            // Let the pack attack the player.
            pack.PackAttack(nodePlayer);

            // If the pack or the player died, we will end combat the next turn.
            if (pack.PackHP <= 0 || nodePlayer.IsDead)
                repeatCombat = false;

            // Else we will check for retreats. if the command is the retreat command, move to the
            // adjoining node, else if the accumulative HP of the pack is lower then the HP of the player,
            // move to the pack to the adjoining node and end the combat in both cases.
            if (command != null && command.retreated && connections.Count > 0)
            {
                nodePlayer.Move(connections[0]);
                repeatCombat = false;
            }
            else if (pack.PackHP < nodePlayer.HP && connections.Count > 0)
            {
                pack.MovePack(connections[0]);
                repeatCombat = false;
            }
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
            {
                amountOfMonsters += p.PackSize;
            }
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

        public bool RepeatCombat { get { return repeatCombat; } }
    }
}
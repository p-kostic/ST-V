using System;
using System.Collections.Generic;

namespace STV1
{
    public class TimeCrystal : Item
    {
        string type = "TimeCrystal";

        public override void UseItem(Player player)
        {
            // Get the gate and de specific dungeon.
            Node gate = player.Location;
            Dungeon dungeon = player.Dungeon;
            
            // Check if the crystal is used on a gate.
            bool UsedOnGate = dungeon.CheckGate(gate);

            // If used on a gate, We first find all the possible nodes the player can flee to,
            // and then we destroy the gate. After that we move the player to one of the fleeing nodes.
            if (UsedOnGate)
            {
                List<Node> fleeingNodes = new List<Node>();
                foreach (Node node in gate.connections)
                    fleeingNodes.Add(node);

                dungeon.Destroy(gate);

                foreach (Node node in fleeingNodes)
                {
                    if (dungeon.GetSpecificPath(node, dungeon.GetExit))
                    {
                        player.Location = node;
                        break;
                    }
                }

                return;
            }
        }

        public override string ItemType()
        {
            return type;
        }
    }
}
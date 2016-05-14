using System.Collections.Generic;

namespace STV1
{
    public class TimeCrystal : Item
    {
        public override void UseItem(Player player)
        {
            // Get the gate and de specific dungeon.
            Node gate = player.Location;
            Dungeon dungeon = player.Dungeon;
            
            // Check if the crystal is used on a gate.
            bool UsedOnGate = player.Location.type == "bridge";

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
                    if (dungeon.FindShortestPath(node, dungeon.GetExit) != null)
                    {
                        player.Location = node;
                        break;
                    }
                }

                return;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    public class Dungeon
    {
        const int M = 3;
        private Node startNode;
        private List<Node> nodes;
        private Node exitNode;
        private int nodeNr;
        Random rand = new Random();

        public Dungeon(int level)
        {
            nodeNr = 1;
            nodes = new List<Node>();
            GenerateDungeon(level);
        }

        void GenerateDungeon(int level)
        {
            startNode = new Node(0, M, "start");
            nodes.Add(startNode);

            for (int i = 0; i < level; i++)
            { // Generates the chosen amount of zones with bridges inbetween them
                nodes.AddRange(GenerateZone(nodes[nodes.Count() - 1], i));
            }
            nodes[nodes.Count() - 1].type = "exit"; // Make the last bridge the exit node

            for (int i = 0; i < nodes.Count(); i++)
            {
                Console.WriteLine(nodes[i].type + " " + nodes[i].id);
                for (int j = 0; j < nodes[i].connections.Count(); j++)
                {
                    Console.WriteLine("  " + nodes[i].connections[j].type + " " + nodes[i].connections[j].id);
                }
            }

            while (true) { }
        }

        List<Node> GenerateZone(Node firstNode, int zoneLvl)
        {
            int zoneNodeNr = rand.Next(1, 6);
            List<Node> curZone = new List<Node>();
            curZone.Add(firstNode);

            for (int i = 0; i < zoneNodeNr; i++) // Loop that fills the zone with a random amount of nodes
            {
                Node curNode = new Node(zoneLvl, M);
                curNode.id = nodeNr;
                nodeNr++;
                int cNr = rand.Next(1, 4); // Determines how many connections the current node will attempt to have
                for (int k = 0; k < cNr; k++) // Loop that fills these connections if possible
                {
                    if (curZone.Count() > 1)
                    {
                        int randNodeIndex = rand.Next(0, curZone.Count() - 1); // Picks a random node in the zone
                        if (!curNode.connections.Contains(curZone[randNodeIndex]) && curZone[randNodeIndex].connections.Count() < 5) // Checks if current node isn't already connected to that node and if the node has 4 or less connections
                        {
                            curNode.connections.Add(curZone[randNodeIndex]); // Adds the chosen node to the connection list of the current node
                            curZone[randNodeIndex].connections.Add(curNode); // Adds the current node to the connection list of the chosen node
                        }
                    }
                    else
                    { // Does the same as above but in case there is only one node. Apparently a random between 1 and 1 can't be chosen.
                        if (!curNode.connections.Contains(curZone[0]) && curZone[0].connections.Count() < 5)
                        {
                            curNode.connections.Add(curZone[0]);
                            curZone[0].connections.Add(curNode);
                        }

                    }

                }
                curZone.Add(curNode); // Adds the created node to the zone list
            }

            Node curbridge = new Node(zoneLvl, M, "bridge");
            curbridge.id = nodeNr;
            nodeNr++;
            int connectionNr = rand.Next(1, 4);
            for (int k = 0; k < connectionNr; k++)
            {
                if (curZone.Count() > 1)
                {
                    int randNodeIndex = rand.Next(0, curZone.Count() - 1);
                    if (!curbridge.connections.Contains(curZone[randNodeIndex]) && curZone[randNodeIndex].connections.Count() < 5)
                    {
                        curbridge.connections.Add(curZone[randNodeIndex]);
                        curZone[randNodeIndex].connections.Add(curbridge);
                    }
                }
                else
                {
                    if (!curbridge.connections.Contains(curZone[0]) && curZone[0].connections.Count() < 5)
                    {
                        curbridge.connections.Add(curZone[0]);
                        curZone[0].connections.Add(curbridge);
                    }

                }

            }

            curZone.Add(curbridge);
            curZone.RemoveAt(0);
            return curZone;
        }

        //List<Node> FindShortestPath(Node start, Node end)
        //{
        //    bool found = false;
        //    List<Node> shortestPath = new List<Node>();
        //    Queue<Node> queue = new Queue<Node>();
        //    List<Node> closed = new List<Node>();
        //    Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        //    queue.Enqueue(start);
        //    closed.Add(start);
        //    if (end == start)
        //    {
        //        shortestPath.Add(start);
        //        found = true;
        //    }

        //    while (!found && queue.Count > 0)
        //    {
        //        Node curNode = queue.Dequeue();

        //        for (int i = 0; i < curNode.connections.Count(); i++)
        //        {
        //            if (!closed.Contains(curNode))
        //            {
        //                queue.Enqueue(curNode);
        //                previous.Add(curNode.connections[i], curNode);
        //                closed.Add(curNode);
        //            }
        //            if (end == curNode)
        //            {
        //                Node temp = curNode;
        //                while (temp != start)
        //                {
        //                    shortestPath.Add(temp);
        //                    previous.TryGetValue(temp, out temp);
        //                }
        //                found = true;
        //                shortestPath.Add(temp);
        //                break;
        //            }
        //        }

        //        return shortestPath;
        //    }
        //}
    }
}

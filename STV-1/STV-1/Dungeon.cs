﻿using System;
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
        public List<Node> nodes;
        private Node exitNode;
        private int nodeNr;
        public int level;
        Random rand = new Random();

        int monsterNr;
        int packNr;

        public Dungeon(int level)
        {
            this.level = level;
            nodeNr = 1;
            monsterNr = 15 * level;
            packNr = 5 * level;
            nodes = new List<Node>();
            GenerateDungeon(level);

            List<Node> kut = FindShortestPath(nodes[0], nodes[nodes.Count() - 1]);

            foreach (Node n in kut)
            {
                Console.WriteLine(n.id);
            }


            //Console.WriteLine("-----------------------");
            //foreach (Node curNode in nodes)
            //{
            //    if (curNode.type == "bridge")
            //    {
            //        Destroy(curNode);
            //        for (int i = 0; i < nodes.Count(); i++)
            //        {
            //            Console.WriteLine(nodes[i].type + " " + nodes[i].id);
            //            for (int j = 0; j < nodes[i].connections.Count(); j++)
            //            {
            //                Console.WriteLine("  " + nodes[i].connections[j].type + " " + nodes[i].connections[j].id);
            //            }
            //        }
            //        break;
            //    }
            //}

            Console.ReadLine();
        }

        void GenerateDungeon(int level)
        {
            startNode = new Node(0, M, "start");
            nodes.Add(startNode);

            for (int i = 0; i <= level; i++) // Generates the chosen amount of zones with bridges inbetween them
            {
                nodes.AddRange(GenerateZone(nodes[nodes.Count() - 1], i));
            }
            nodes[nodes.Count() - 1].type = "exit"; // Make the last bridge the exit node

            for (int i = 0; i < nodes.Count(); i++)
            {
                Console.WriteLine(nodes[i].type + " " + nodes[i].id + " lvl: " + nodes[i].level);
                for (int j = 0; j < nodes[i].connections.Count(); j++)
                {
                    Console.WriteLine("  " + nodes[i].connections[j].type + " " + nodes[i].connections[j].id);
                }
            }


        }

        List<Node> GenerateZone(Node firstNode, int zoneLvl)
        {
            int zoneNodeNr = rand.Next(1, 6);
            List<Node> curZone = new List<Node>();
            curZone.Add(firstNode);

            // Determines how many monsters and packs should be in the current zone
            int zoneMonsterNr = (2 * (zoneLvl + 1) * monsterNr) / ((level + 2) * (level + 1));
            Console.WriteLine("zonemonsternr: " + zoneMonsterNr);
            int zonePackNr = (int)Math.Floor((double)packNr / (double)(level + 1));

            for (int i = 0; i < zoneNodeNr; i++) // Loop that fills the zone with a random amount of nodes
            {
                Node curNode = new Node(zoneLvl + 1, M);
                curNode.id = nodeNr;
                nodeNr++;
                bool hasConnection = false;
                int cNr = rand.Next(1, 4); // Determines how many connections the current node will attempt to have
                while (!hasConnection)
                {
                    for (int k = 0; k < cNr; k++) // Loop that fills these connections if possible
                    {
                        if (curZone.Count() > 1)
                        {
                            int randNodeIndex = rand.Next(0, curZone.Count() - 1); // Picks a random node in the zone
                            if (!curNode.connections.Contains(curZone[randNodeIndex]) &&
                                curZone[randNodeIndex].connections.Count() < 4)
                            // Checks if current node isn't already connected to that node and if the node has 4 or less connections
                            {
                                curNode.connections.Add(curZone[randNodeIndex]);
                                // Adds the chosen node to the connection list of the current node
                                curZone[randNodeIndex].connections.Add(curNode);
                                // Adds the current node to the connection list of the chosen node
                                hasConnection = true;
                            }
                        }
                        else
                        {
                            // Does the same as above but in case there is only one node. Apparently a random between 1 and 1 can't be chosen.
                            if (!curNode.connections.Contains(curZone[0]) && curZone[0].connections.Count() < 4)
                            {
                                curNode.connections.Add(curZone[0]);
                                curZone[0].connections.Add(curNode);
                                hasConnection = true;
                            }

                        }

                    }
                }

                int drop = rand.Next(1, 100); // Determines the drop chance for items in a node
                if (drop < 5)
                {
                    curNode.items.Add(new TimeCrystal());
                }
                if (drop < 15)
                {
                    curNode.items.Add(new HealingPotion());
                }

                curZone.Add(curNode); // Adds the created node to the zone list
            }
            bool placed = false;
            Node curbridge = new Node(zoneLvl + 1, M, "bridge");
            curbridge.id = nodeNr;
            nodeNr++;
            int connectionNr = rand.Next(1, 4);
            while (!placed)
            {

                for (int k = 0; k < connectionNr; k++)
                {
                    if (curZone.Count() > 1)
                    {
                        int randNodeIndex = rand.Next(0, curZone.Count() - 1);
                        if (!curbridge.connections.Contains(curZone[randNodeIndex]) &&
                            curZone[randNodeIndex].connections.Count() < 4)
                        {
                            curbridge.connections.Add(curZone[randNodeIndex]);
                            curZone[randNodeIndex].connections.Add(curbridge);
                            placed = true;
                        }
                    }
                    else
                    {
                        if (!curbridge.connections.Contains(curZone[0]) && curZone[0].connections.Count() < 5)
                        {
                            curbridge.connections.Add(curZone[0]);
                            curZone[0].connections.Add(curbridge);
                            placed = true;
                        }

                    }

                }
            }
            curZone.Add(curbridge);
            curZone.RemoveAt(0);

            // Add packs of monsters to the generated dungeon
            int remainingMonsters = zoneMonsterNr;
            for (int i = 0; i < zonePackNr && remainingMonsters > 0; i++)
            {
                int packLocation = rand.Next(0, curZone.Count() - 1);
                Pack curPack = new Pack(zoneMonsterNr / zonePackNr, curZone[packLocation]);
                remainingMonsters -= zoneMonsterNr / zonePackNr;
                Console.WriteLine("Added pack to node " + curZone[packLocation].id + ", remaining monsters: " + remainingMonsters);
            }

            return curZone;
        }

        public List<Node> FindShortestPath(Node start, Node end)
        {
            bool found = false;
            List<Node> shortestPath = new List<Node>(); // The final list that gets returned
            Queue<Node> queue = new Queue<Node>(); // Keeps track of which nodes to look at next
            List<Node> closed = new List<Node>(); // Keeps track of which nodes have already been visited
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>(); // Used for the final backtrace
            queue.Enqueue(start);
            closed.Add(start);
            if (end == start) // Checks for a trivial path
            {
                shortestPath.Add(start);
                found = true;
            }

            while (!found && queue.Count > 0)
            {
                Node curNode = queue.Dequeue();

                for (int i = 0; i < curNode.connections.Count(); i++)
                {
                    if (!closed.Contains(curNode.connections[i]))
                    {
                        queue.Enqueue(curNode.connections[i]);
                        previous.Add(curNode.connections[i], curNode);
                        closed.Add(curNode.connections[i]);
                    }
                    if (end == curNode)
                    {
                        Node temp = curNode;
                        while (temp != start)
                        {
                            shortestPath.Add(temp);
                            previous.TryGetValue(temp, out temp);
                        }
                        found = true;
                        shortestPath.Add(temp);
                        break;
                    }
                }
            }

            if (!found)
                return null;

            shortestPath.Reverse();
            return shortestPath;
        }

        public bool GetSpecificPath(Node node1, Node node2)
        {
            return FindShortestPath(node1, node2) != null;
        }

        public void Destroy(Node b)
        {
            if (b.type != "bridge")
                return;
            List<Node> toRemove = new List<Node>();
            foreach (Node curNode in nodes)
            {
                if (curNode.id <= b.id)
                {
                    Console.WriteLine("removing... " + curNode.id);
                    toRemove.Add(curNode);
                }
            }

            foreach (Node curNode in toRemove)
            {
                if (nodes.Contains(curNode))
                {
                    nodes.Remove(curNode);
                }
            }

            foreach (Node curNode in nodes)
            {
                for (int i = 0; i < curNode.connections.Count; i++)
                {
                    if (curNode.connections[i].id <= b.id)
                    {
                        curNode.connections.RemoveAt(i);
                    }
                }
            }
        }

        public int GetLevel(Node node)
        {
            int level = node.MaxMonsters / M - 1;
            if (level >= 0)
                return level;
            else
                return 0;
        }

        public bool CheckGate(Node node)
        {
            return GetLevel(node) >= 1;
        }

        public Node GetStart { get { return startNode; } }
        public Node GetExit { get { return exitNode; } }

        public int getLevel(Node u)
        {
            return u.level;
        }
    }

}
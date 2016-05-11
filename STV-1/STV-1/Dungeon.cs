using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        for (int i = 0; i < level; i++) { // Generates the chosen amount of zones with gates inbetween them
            nodes.AddRange(GenerateZone(nodes[nodes.Count()-1], i)); 
        }

        for (int i = 0; i < nodes.Count() - 1; i++ )
        {
            Console.WriteLine(nodes[i].type + " " + nodes[i].id);
            for (int j = 0; j < nodes[i].connections.Count() - 1; j++ )
            {
                Console.WriteLine("  " + nodes[i].connections[j].type + " " + nodes[i].connections[j].id);
            }
        }

        while (true) { }
    }

    List<Node> GenerateZone(Node firstNode, int zoneLvl)
    {
        int zoneNodeNr = rand.Next(0, 6);
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
                        Console.WriteLine("- connecting " + curNode.id + "and " + curZone[randNodeIndex].id); // Debug print
                    }
                }
                else { // Does the same as above but in case there is only one node. Apparently a random between 1 and 1 can't be chosen.
                    if (!curNode.connections.Contains(curZone[0]) && curZone[0].connections.Count() < 5)
                    {
                        curNode.connections.Add(curZone[0]);
                        curZone[0].connections.Add(curNode);
                        Console.WriteLine("-- connecting " + curNode.id + "and " + curZone[0].id); // Debug print
                    }
                    
                }
                
            }
            curZone.Add(curNode); // Adds the created node to the zone list
        }

        Node curGate = new Node(zoneLvl, M, "gate");
        curGate.id = nodeNr;
        nodeNr++;
        int connectionNr = rand.Next(1, 4);
        for (int k = 0; k < connectionNr; k++)
        {
            if (curZone.Count() > 1)
            {
                int randNodeIndex = rand.Next(0, curZone.Count() - 1);
                if (!curGate.connections.Contains(curZone[randNodeIndex]))
                {
                    curGate.connections.Add(curZone[randNodeIndex]);
                    curZone[randNodeIndex].connections.Add(curGate);
                    Console.WriteLine("--- connecting " + curGate.id + "and " + curZone[0].id); // Debug print
                }
            }
            else {
                curGate.connections.Add(curZone[0]);
                curZone[0].connections.Add(curGate);
                Console.WriteLine("---- connecting " + curGate.id + "and " + curZone[0].id); // Debug print
            }
            
        }

        curZone.Add(curGate);
        curZone.RemoveAt(0);
        return curZone;
    } 
}

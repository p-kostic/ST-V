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
    Random rand = new Random();
    
    public Dungeon(int level)
    {
        nodes = new List<Node>();
        GenerateDungeon(level);
    }

    void GenerateDungeon(int level)
    {
        startNode = new Node(0, M, "start");
        nodes.Add(startNode);

        for (int i = 0; i < level; i++) {
            nodes.AddRange(GenerateZone(nodes[nodes.Count()-1]));
        }

        Console.WriteLine("test");
    }

    List<Node> GenerateZone(Node firstNode)
    {
        int zoneNodeNr = rand.Next(0, 6);
        int j = 0;
        List<Node> curZone = new List<Node>();
        curZone.Add(firstNode);

        for (int i = 0; i < zoneNodeNr; i++)
        {
            Node curNode = new Node(j, M);
            int cNr = rand.Next(1, 4);
            for (int k = 0; k < cNr; k++)
            {
                if (curZone.Count() > 1)
                {
                    int randNodeIndex = rand.Next(0, curZone.Count() - 1);
                    if (!curNode.connections.Contains(curZone[randNodeIndex]))
                    {
                        curNode.connections.Add(curZone[randNodeIndex]);
                    }
                }
                else {
                    curNode.connections.Add(curZone[0]);
                }
                
            }
        }

        Node curGate = new Node(j, M, "gate");
        int connectionNr = rand.Next(1, 4);
        for (int k = 0; k < connectionNr; k++)
        {
            if (curZone.Count() > 1)
            {
                int randNodeIndex = rand.Next(0, curZone.Count() - 1);
                if (!curGate.connections.Contains(curZone[randNodeIndex]))
                {
                    curGate.connections.Add(curZone[randNodeIndex]);
                }
            }
            else {
                curGate.connections.Add(curZone[0]);
            }
            
        }

        curZone.Add(curGate);
        curZone.RemoveAt(0);
        return curZone;
    } 
}

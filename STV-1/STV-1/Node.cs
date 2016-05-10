using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Node
{
    private const int maxconnections = 4;
    private List<Node> Connections;
    private int level;
    private int maxmonsters;
    public string type;
    public List<Item> loot;

    /// <summary>
    /// Creates a new node that can be occupied by a player, monster packs or both.
    /// There's a limit of how many monsters each node u can accomodate
    /// </summary>
    /// <param name="level">Node u's level</param>
    /// <param name="M">constant which is the same over the whole dungeon</param>
    public Node(int level, int M, string type = "normal")
    {
        this.type = type;
        this.level = level;
        this.maxmonsters = M*(level + 1);
        Connections = new List<Node>();

        // if occupied by both monsters and player, the node is contested
        // bool contested = true;
        // bool combat = true;
        // InitiateRound();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Node
{
    public List<Node> connections;
    private const int maxconnections = 4;

    private int maxmonsters;

    private int level;
    public string type;
    public int id;

    List<Pack> nodePack;
    Player player;

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
        this.maxmonsters = M*(level + 1);
        connections = new List<Node>();
        nodePack = new List<Pack>();
    }

    public void UseItem(Item i)
    {
        i.UseItem(player);
    }

    public void DoCombatRound(Pack pack, Player player)
    {
    }

    public void DoCombat(Pack pack, Player player)
    {
    }
}
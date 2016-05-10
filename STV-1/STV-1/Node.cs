using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Node
{
    public string type;
    public List<Node> connections;
    public List<Item> loot;

    public Node(string type = "normal") {
        this.type = type;
    }
}
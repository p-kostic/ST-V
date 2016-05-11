using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * In this class we will define the packs. Monsters will be grouped
 * In packs, and in this class we will handle a few methods like
 * movement, attacking and creating the actual pack.
 */
class Pack
{
    List<Monster> monsters;

    public Pack(int packSize, Node packStartLocation)
    {
        monsters = new List<Monster>();
        for (int i = 0; i < packSize; i++)
        {
            Monster monster = new Monster(10, 2, packStartLocation);
            monsters.Add(monster);
        }
    }

    public void MovePack(Node destination)
    {
        foreach(Monster monster in monsters)
        {
            monster.Location = destination;
        }
    }

    public void PackAttack(Creature creature)
    {
        foreach(Monster monster in monsters)
        {
            monster.Attack(creature);
        }
    }

    public int PackSize { get { return monsters.Count; } }
    public Node PackLocation { get { return monsters[0].Location; } }
    public bool PackDied
    {
        get
        {
            if (monsters.Count == 0)
                return true;
            else
                return false;
        }
    }
}

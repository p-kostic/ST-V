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
    List<Monster> monsters; // A list to hold all the monsters.

    // The constructor will handle the creation of the pack. We will give the pack a 
    // size and a location to start in the dungeon. The monsters will have 10 hp and
    // 2 atk (although this could be easily changed).
    public Pack(int packSize, Node packStartLocation)
    {
        monsters = new List<Monster>();
        for (int i = 0; i < packSize; i++)
        {
            Monster monster = new Monster(10, 2, packStartLocation);
            monsters.Add(monster);
        }
    }

    // We will move the pack by moving each monster in the pack to the destination location.
    public void MovePack(Node destination)
    {
        foreach(Monster monster in monsters)
        {
            monster.Location = destination;
        }
    }

    // We will attack the target creature with each monster in the pack.
    public void PackAttack(Creature creature)
    {
        foreach(Monster monster in monsters)
        {
            monster.Attack(creature);
        }
    }

    // A few getters/setters to get the size and location of the pack,
    // and a more complex getter (a boolean) to find out if a pack has died or not.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    /*
     * In this class we will define the packs. Monsters will be grouped
     * In packs, and in this class we will handle a few methods like
     * movement, attacking and creating the actual pack.
     */
    public class Pack
    {
        List<Monster> monsters; // A list to hold all the monsters.

        // The constructor will handle the creation of the pack. We will give the pack a 
        // size and a location to start in the dungeon. The monsters will have 10 hp and
        // 2 atk (although this could be easily changed).

        public Node packStartLocation;

        public Pack(int packSize, Node packStartLocation)
        {
            monsters = new List<Monster>();

            if (packStartLocation.type == "normal" || packStartLocation.type == "bridge")
            {
                if (packSize >= packStartLocation.MaxMonsters)
                    packSize = packStartLocation.MaxMonsters;

                for (int i = 0; i < packSize; i++)
                {
                    Monster monster = new Monster(10, 2, packStartLocation, null);
                    monsters.Add(monster);
                }

                // Every node contains a list with all packs in it, we make sure to also add the pack to it here
                this.packStartLocation = packStartLocation;
                this.packStartLocation.nodePacks.Add(this);

            }
            else throw new NotSupportedException("You cannot create a pack on a start or exit node");
        }

        // We will move the pack by moving each monster in the pack to the destination location.
        public void MovePack(Node destination)
        {
            if (this.PackLocation.ConnectedTo(destination) && destination.type != "exit" && destination.type != "start")
            {
                foreach (Monster monster in monsters)
                {
                    monster.Location = destination;
                }
                // Update the nodePack list accordingly to the move
                this.packStartLocation.RemovePack(this);
                destination.AddPack(this);
            }
        }

        // We will attack the target creature with each monster in the pack.
        public void PackAttack(Creature creature)
        {
            if (creature.Location == this.PackLocation)
            {
                foreach (Monster monster in monsters)
                {
                    monster.Attack(creature);
                }
            }
        }

        // A few getters/setters to get the size and location of the pack,
        // and a more complex getter (a boolean) to find out if a pack has died or not.
        public int PackSize { get { return monsters.Count; } }

        // Het is lelijk, maar het werkt...
        public Node PackLocation { get { return monsters[0].Location; } }
        public List<Monster> Monsters { get { return monsters; } }
        public int PackHP
        {
            get
            {
                int totalHP = 0;
                foreach (Monster monster in monsters)
                    totalHP += monster.HP;
                return totalHP;
            }
        }
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
}

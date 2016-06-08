﻿using System;
using System.CodeDom;
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
        public List<Monster> monsters; // A list to hold all the monsters.
        public Node packStartLocation;
        public Node rememberedLocation;

        // The constructor will handle the creation of the pack. We will give the pack a 
        // size and a location to start in the dungeon. The monsters will have 10 hp and
        // 2 atk (although this could be easily changed).
        public Pack(int packSize, Node packStartLocation)
        {
            monsters = new List<Monster>();

            if (packStartLocation.type == "normal" || packStartLocation.type == "bridge")
            {
                if (packSize >= packStartLocation.MaxMonsters)
                    packSize = packStartLocation.MaxMonsters;

                for (int i = 0; i < packSize; i++)
                {
                    Monster monster = new Monster(10, 2, packStartLocation);
                    monsters.Add(monster);
                }

                // Every node contains a list with all packs in it, we make sure to also add the pack to it here
                this.packStartLocation = packStartLocation;
                this.packStartLocation.nodePacks.Add(this);

            }
        }

        // We will move the pack by moving each monster in the pack to the destination location.
        public void MovePack(Node destination)
        {
            if (PackLocation.ConnectedTo(destination) && destination.type != "exit" && destination.type != "start" && destination.PackFitsInNode(this))
            {
                foreach (Monster monster in monsters)
                    monster.Location = destination;

                // Update the nodePack list accordingly to the move
                this.packStartLocation.RemovePack(this);
                destination.AddPack(this);
            }
        }

        // We will attack the target creature with each monster in the pack.
        public void PackAttack(Creature creature)
        {
            if (creature.Location == this.PackLocation)
                foreach (Monster monster in monsters)
                    monster.Attack(creature);
        }

        public void UpdatePack()
        {
            for (int i = monsters.Count - 1; i > -1; i--)
            {
                if (monsters[i].IsDead)
                    monsters.Remove(monsters[i]);
                if (monsters.Count == 1)
                    rememberedLocation = monsters[0].Location;
            }

            if (monsters.Count == 0)
                PackLocation.RemovePack(this);
        }

        public void HandlePackAI(Player player, Dungeon dungeon)
        {
            List<List<Node>> paths = new List<List<Node>>();
            // Find path to closest bridge
            foreach (Node node in dungeon.nodes)
            {
                if (node.type == "bridge")
                    paths.Add(dungeon.FindShortestPath(PackLocation, node));
            }

            List<Node> closestBridge = paths.OrderByDescending(x => x.Count).ElementAt(paths.Count - 1);
            List<Node> pathToPlayer = dungeon.FindShortestPath(this.PackLocation, player.Location);

            // If the player is closer to the pack than the closest bridge node, we move to the player
            if (pathToPlayer.Count <= closestBridge.Count)
                this.MovePack(pathToPlayer[0]);
            // else, we move to the closest bridge
            else
                this.MovePack(closestBridge[0]);

        }

        // A few getters/setters to get the size and location of the pack,
        // and a more complex getter (a boolean) to find out if a pack has died or not.
        public int PackSize { get { return monsters.Count; } }
        public Node PackLocation
        {
            get
            {

                if (monsters.Count > 0)
                    return monsters[0].Location;
                else if (monsters.Count == 0)
                    return rememberedLocation;
                else
                {
                    throw new NullReferenceException("WATF");
                    return null;
                }

            }
        }
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
    }
}

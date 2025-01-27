﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    /* 
     * This class will handle all the player specific actions.
     * Since the player is a creature, it will inherit from the
     * abstract creature class.
     */
    public class Player : Creature
    {
        private int maxHP;
        private int atk;
        public int kp = 0;
        private Dungeon dungeon;
        public List<int> visitedList = new List<int>();
        public int levelChecker = 0;
        public List<Item> inventory = new List<Item>(); // To keep track of the items in the player's inventory.

        // We call the base method in the abstract creature class, and we will set the
        // maxHP to the hp value.
        public Player(int hp, int atk, Node loc, Dungeon dungeon)
            : base(hp, atk, loc)
        {
            this.maxHP = hp;
            this.atk = atk;
            this.dungeon = dungeon;
            loc.AddPlayer(this);
        }

        // We will move the player to the given destination.
        public override void Move(Node destination)
        {
            Location.RemovePlayer();
            Location = destination;
            Location.AddPlayer(this);
            GrabItems();
        }

        // The attacked creature is dealt damage equal to the player's attack power.
        public override void Attack(Creature creature)
        {
            creature.HP -= this.atk;
            if (creature.IsDead)
                kp++;
        }

        // This will add items to the player's inventory.
        public void GrabItems()
        {
            if (Location.items.Count > 0)
                foreach (Item i in Location.items)
                {
                    inventory.Add(i);
                    Location.items = new List<Item>();
                }
        }

        // For testing purposes we added a method to add an item to a inventory on the fly.
        public void AddItem(bool healingPotion)
        {
            if (healingPotion)
                inventory.Add(new HealingPotion());
            else
                inventory.Add(new TimeCrystal());
        }

        public bool UsePotion()
        {
            foreach (Item item in inventory)
                if (item.ItemType() == "HealingPotion")
                {
                    inventory.Remove(item);
                    item.UseItem(this);
                    return true;
                }

            return false;
        }

        public bool UseCrystal()
        {
            foreach (Item item in inventory)
                if (item.ItemType() == "TimeCrystal")
                {
                    inventory.Remove(item);
                    item.UseItem(this);
                    return true;
                }

            return false;
        }

        // We override the virtual method of the creature class here, and add a
        // testcase to make sure the hp value can't exceed the maximum value.
        public override int HP
        {
            get { return base.HP; }
            set
            {
                if (value > maxHP)
                    base.HP = maxHP;
                else
                    base.HP = value;
            }
        }

        public Dungeon Dungeon { get { return dungeon; } }

        // A method that keeps track of the visited nodes.
        public string VisitedList(int curLevel)
        {
            if (levelChecker != curLevel)
            {
                levelChecker = curLevel;
                visitedList.Clear();
            }

            string visitedString = "";
            if (!visitedList.Contains(this.Location.id))
                visitedList.Add(this.Location.id);

            foreach (int id in visitedList)
            {
                if (id == visitedList.First())
                    visitedString += id;
                else
                    visitedString += "," + id;
            }

            return visitedString;
        }
    }
}

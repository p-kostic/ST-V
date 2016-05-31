﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    public class Game
    {
        public Dungeon d;
        Player player;
        int level;
        bool quit = false;

        public Game()
        {
            NextDungeon();
            player = new Player(100, 10, d.nodes[0], d);

            while (!quit) { // Game loop
                DrawUI();
                string input = GetInput();
                if (input == "quit")
                    quit = true;
                if (input == "?")
                    DisplayHelp();
                switch (player.inCombat) { 
                    case true:
                        HandleCombat(input);
                        break;
                    case false:
                        HandleMovement(input);
                        break;
                }
            }         
        }

        // geen save en load meer

        public Dungeon NextDungeon()
        {
            if (d == null)
            {
                d = new Dungeon(1);
                d.GenerateDungeon(1);
                level = 1;
                player = new Player(50, 5, d.nodes[0], d);
            }
            else
            {
                level++;
                d = new Dungeon(level);
                d.GenerateDungeon(level);
                player.Location = d.nodes[0];
            }
            return d;
        }

        public void DrawUI() {
            Console.SetCursorPosition(0,0);
            Console.Write("HP: " + player.HP);
            Console.SetCursorPosition(0,1);
            Console.Write("ATK: " + player.ATK);
            Console.SetCursorPosition(0,2);
            Console.Write("KP: " + player.kp);

            Console.SetCursorPosition(0, 4);
            Console.Write("Inventory: ");
            Console.SetCursorPosition(0, 5);
            int potionCount = player.inventory.Count(s => s.ItemType() == "HealingPotion");
            int crystalCount = player.inventory.Count(s => s.ItemType() == "TimeCrystal");
            Console.Write("Healing Potions: " + potionCount);
            Console.SetCursorPosition(0, 6);
            Console.Write("Time Crystals: " + crystalCount);

            int curX = 1;
            int curY = 10;
            Console.SetCursorPosition(curX, curY);
            Console.Write("Paths from this node:");
            curY++;
            Console.SetCursorPosition(curX, curY);
            for (int i = 0; i < player.Location.connections.Count; i++ )
            {
                Console.Write(player.Location.connections[i].id + ", type: " + player.Location.connections[i].type);
                Console.SetCursorPosition(curX, curY + i);
            }
            
            Console.SetCursorPosition(25, 15);
            Console.Write("Current node: " + player.Location.id);
            Console.SetCursorPosition(33, 16);
            Console.Write("type: " + player.Location.type);

            curX = 60;
            curY = 0;
            Console.SetCursorPosition(curX, curY);
            Console.Write("Current packs: " + player.Location.nodePacks.Count);
            curY++;
            Console.SetCursorPosition(curX, curY);
            for (int i = 0; i < player.Location.nodePacks.Count; i++ )
            {
                
                Console.Write("Pack nr: " + i);
                curY++;
                Console.SetCursorPosition(curX, curY);
                for (int j = 0; j < player.Location.nodePacks[i].monsters.Count; j++ )
                {
                    curX++;
                    Console.SetCursorPosition(curX, curY);
                    Console.Write("Monster " + j);
                    curY++;
                    curX++;
                    Console.SetCursorPosition(curX, curY);
                    Console.Write("HP: " + player.Location.nodePacks[i].monsters[j].HP);
                    curY++;
                    Console.SetCursorPosition(curX, curY);
                    Console.Write("ATK: " + player.Location.nodePacks[i].monsters[j].ATK);
                    curY++;
                    curX--;
                    curX--;
                }
            }

            Console.SetCursorPosition(0, 23);
            Console.Write("Give the next command by typing below. Type '?' for a list of commands.");
            Console.SetCursorPosition(0, 24);
        }

        // This method retrieves the input from the player, 
        // and makes sure that it's a valid string.
        private string GetInput()
        {
            string i = Console.ReadLine();
            string[] iArray = i.Split(' ');
            if (iArray[0] == "goto" || i == "stay" ||
                i == "retreat" || i == "continue" || 
                i == "quit" || i == "?")
                return i;
            else return "input not valid";
        }

        // Displays all the usable commands in the game.
        private void DisplayHelp()
        {
            Console.WriteLine("List of commands:");
            Console.WriteLine("- quit    : quit the game");
            Console.WriteLine("- goto x  : move to node x (x has to be an adjecent node)");
            Console.WriteLine("- stay    : stay in the current node");
            Console.WriteLine();
            Console.WriteLine("When in combat:");
            Console.WriteLine("- continue: continue with another round of combat");
            Console.WriteLine("- retreat : retreat from combat to an adjecent node");      
        }

        private void HandleMovement(string input)
        { 
            //TODO: Player movement based on input
            //TODO: Monster movement and monsters attacking eachother
        }

        private void HandleCombat(string input)
        { 
            //TODO: Player attack/item usage based on input

            //TODO: Monsters attacking players
        }
    }
}
using System;
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

        public int cursorInfoPos = 19;

        public Game()
        {
            NextDungeon();
            player = new Player(100, 10, d.nodes[0], d);

            // The console window size.
            Console.SetWindowSize(80, 33);

            // Game loop
            while (!quit)
            {
                DrawUI(); // Draw the UI.

                // Get the input and clear the previous console window.
                // Then use the new input to advance the game.
                string input = GetInput();
                Console.Clear();
                if (input == "quit")
                    quit = true;
                if (input == "?")
                    DisplayHelp();
                if (input != "input not valid")
                {
                    switch (player.inCombat)
                    {
                        case true:
                            HandleCombat(input);
                            break;
                        case false:
                            HandleMovement(input);
                            break;
                    }
                }
                else
                {
                    Console.SetCursorPosition(0, cursorInfoPos);
                    Console.Write("The given input was not valid.");
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
            
            Console.SetCursorPosition(25, 14);
            Console.Write("Current node: " + player.Location.id);
            Console.SetCursorPosition(33, 15);
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

            Console.SetCursorPosition(0, 17);
            Console.Write("Give the next command. Type '?' for a list of commands.");
            Console.SetCursorPosition(0, 18);
        }

        // This method retrieves the input from the player, 
        // and makes sure that it's a valid string.
        private string GetInput()
        {
            string i = Console.ReadLine();
            string[] iArray = i.Split(' ');
            if (iArray[0] == "goto" || i == "stay" ||
                i == "retreat" || i == "continue" || 
                i == "quit" || i == "?" || i == "y" ||
                i == "n" || i == "hp" || i == "tc")
                return i;
            else return "input not valid";
        }

        // Displays all the usable commands in the game.
        private void DisplayHelp()
        {
            Console.SetCursorPosition(0, cursorInfoPos);
            Console.WriteLine("List of commands:");
            Console.WriteLine("- quit     : quit the game");
            Console.WriteLine("- goto x   : move to node x (x has to be a valid path)");
            Console.WriteLine("- stay     : stay in the current node");
            Console.WriteLine();
            Console.WriteLine("When in combat:");
            Console.WriteLine("- continue : continue with another round of combat");
            Console.WriteLine("- retreat  : retreat from combat to an adjecent node");
            Console.WriteLine();
            Console.WriteLine("Confirmations and item usage: ");
            Console.WriteLine("- y  : confirm question");
            Console.WriteLine("- n  : dismiss question");
            Console.WriteLine("- hp : use a healing potion (if you have any)");
            Console.WriteLine("- tc : use a timecrystal (if you have any)");
        }

        // This method handles the movement of the player.
        private void HandleMovement(string input)
        {
            // Check for the goto command. Makes sure that a node is given,
            // and that it's a valid node. If it's valid, move the player to
            // that node.
            string[] i = input.Split(' ');
            if (i[0] == "goto")
            {
                if(i.Length > 1)
                {
                    int goToNode = -1;
                    try
                    {
                        goToNode = int.Parse(i[1]);
                    }
                    catch
                    {
                        Console.SetCursorPosition(0, cursorInfoPos);
                        Console.Write("Command invalid: given node isn't an integer.");
                    }

                    if (player.Location.connections.Exists(item => item.id == goToNode))
                    {
                        Node destination = player.Location.connections.First(item => item.id == goToNode);
                        player.Move(destination);
                        Console.SetCursorPosition(0, cursorInfoPos);
                        Console.Write("You moved to path " + destination.id);
                    } 
                    else
                    {
                        Console.SetCursorPosition(0, cursorInfoPos);
                        Console.Write("Command invalid: non-existing path or already at that location.");
                    }
                }
                else
                {
                    Console.SetCursorPosition(0, cursorInfoPos);
                    Console.Write("Command invalid: no destination node given.");
                }
            }

            else if (input == "stay")
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.Write("You stay at your current location.");

                // TODO: monsters moeten zich verplaatsen naar de speler toe etc.
            }

            else
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.Write("Command invalid: not in a combat situation.");
            } 
        }

        private void HandleCombat(string input)
        { 
            if (input == "continue")
            {
                DrawUI();
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.WriteLine("Commencing combat!");
                Console.WriteLine("Do you want to use an item? (y/n)");

                bool validAction = false;
                string action;
                while (!validAction)
                {
                    action = GetInput();

                    if (action == "y")
                    {
                        validAction = true;
                        bool validItem = false;
                        Console.WriteLine("Which item do you want to use? (hp/tc)");

                        while (!validItem)
                        {
                            action = GetInput();

                            if (action == "hp")
                            {
                                validItem = true;
                                if (player.inventory.Exists(item => item.ItemType() == "HealingPotion"))
                                {
                                    player.UsePotion();
                                    Console.WriteLine("Used a healing potion!");
                                }
                                else
                                    Console.WriteLine("You don't have a healing potion!");          
                            }

                            else if (action == "tc")
                            {
                                validItem = true;
                                if (player.inventory.Exists(item => item.ItemType() == "TimeCrystal"))
                                {
                                    player.UseCrystal();
                                    Console.WriteLine("Used a time crystal!");
                                }
                                else  
                                    Console.WriteLine("You don't have a time crystal!");                
                            }

                            else
                                Console.WriteLine("Action not valid! Choose 'hp' or 'tc'."); 
                        }

                        player.Location.DoCombatRound(player.Location.nodePacks[0], player);
                    }

                    else if (action == "n")
                    {
                        validAction = true;
                        player.Location.DoCombatRound(player.Location.nodePacks[0], player);
                    }

                    else { Console.WriteLine("Action not valid! Choose 'y' or 'n'."); }
                }
            }

            else if (input == "retreat")
            {
                if (!player.IsDead)
                {
                    player.inCombat = false;
                    player.Move(player.Location.connections[0]);
                    Console.SetCursorPosition(0, cursorInfoPos);
                    Console.WriteLine("Got away safely to node " + player.Location.connections[0].id + "!");
                }
            }

            else
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                if (input != "?")
                    Console.Write("Command invalid: Only combat actions allowed!");
            }
        }
    }
}
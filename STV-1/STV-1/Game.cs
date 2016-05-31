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

        public int cursorInfoPos = 25;

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
                    Console.WriteLine("The given input was not valid.");
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
            Console.SetCursorPosition(0, cursorInfoPos);
            Console.WriteLine("List of commands:");
            Console.WriteLine("- quit     : quit the game");
            Console.WriteLine("- goto x   : move to node x (x has to be a valid path)");
            Console.WriteLine("- stay     : stay in the current node");
            Console.WriteLine();
            Console.WriteLine("When in combat:");
            Console.WriteLine("- continue : continue with another round of combat");
            Console.WriteLine("- retreat  : retreat from combat to an adjecent node");      
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
                        Console.WriteLine("Command invalid: given node not an integer.");
                    }

                    // TODO: methode afschrijven.
                }
                else
                {
                    Console.SetCursorPosition(0, cursorInfoPos);
                    Console.WriteLine("Command invalid: no destination node given.");
                }
            }

            else if (input == "stay")
            {
                // TODO: deze command afmaken.
            }

            else
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.WriteLine("Command invalid: not in a combat situation.");
            } 
        }

        private void HandleCombat(string input)
        { 
            //TODO: Player attack/item usage based on input

            //TODO: Monsters attacking players
        }
    }
}
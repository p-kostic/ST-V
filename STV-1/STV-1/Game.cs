using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STV_1;

namespace STV1
{
    public class Game
    {
        public Dungeon d;
        static Player player;
        static int level;
        bool quit = false;
        bool inCombat;
        int curSeed;
        Specification spec = new Specification();
        bool play;
        string[] inputList;
        List<string> saveInputList;
        int curCommand = 0;

        public int cursorInfoPos = 21;

        public Game()
        {
            Console.SetWindowSize(80, 37);
            Console.WriteLine("Type 'r' followed by a filename if you want to record a playthrough, and 'p' if you want to play an existing one");
            string firstInput = Console.ReadLine();
            if (firstInput[0] == 'r')
                play = true;
            else play = false;
            Console.Clear();

            NextDungeon();
            player = new Player(100, 10, d.nodes[0], d);
            // The console window size.
            string inputName = Console.ReadLine();
            if (!play)
            {
                string location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GitHub\\ST-V\\STV-1\\STV-1\\inputlists\\" + inputName + ".txt";
                inputList = File.ReadAllLines(location);
            }
            else
            {
                saveInputList = new List<string>();
            }
            // Game loop
            while (!quit)
            {
                DrawUI(); // Draw the UI.

                // Check if the player is in combat or not.
                if (player.Location.CheckInCombat())
                    inCombat = true;
                else
                    inCombat = false;

                // Get the input and clear the previous console window.
                // Then use the new input to advance the game.
                string input = GetInput();
                string[] iArray = input.Split(' ');
                Console.Clear();

                if (input == "quit")
                    quit = true;
                if (input == "?")
                    DisplayHelp();
                if (iArray[0] == "save")
                    SaveGame(iArray[1]);
                if (iArray[0] == "load")
                    LoadGame(iArray[1]);

                if (input != "input not valid")
                {
                    if (inCombat)
                        HandleCombat(input);
                    else
                        HandleMovement(input);
                }
                else
                {
                    Console.SetCursorPosition(0, cursorInfoPos);
                    Console.Write("The given input was not valid.");
                }

                // Test specifications each turn
                spec.TestSpecifications(d, player);
            }
            if(!spec.TestSpecifications(d, player))
                throw new Exception("KAPPASTORM");

            if (play)
            {
                string saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                      "\\GitHub\\ST-V\\STV-1\\STV-1\\inputlists\\" + inputName + ".txt";
                File.WriteAllLines(saveLocation, saveInputList);
            }
        }

        public Dungeon NextDungeon()
        {
            Random rand = new Random();
            curSeed = rand.Next(0, 1000);
            Node.idCounter = 0;
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

        public void DrawUI()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("HP : " + player.HP);
            Console.SetCursorPosition(0, 1);
            Console.Write("ATK: " + player.ATK);
            Console.SetCursorPosition(0, 2);
            Console.Write("KP : " + player.kp);

            Console.SetCursorPosition(0, 4);
            Console.Write("Inventory: ");
            Console.SetCursorPosition(0, 5);
            int potionCount = player.inventory.Count(s => s.ItemType() == "HealingPotion");
            int crystalCount = player.inventory.Count(s => s.ItemType() == "TimeCrystal");
            Console.Write("Healing Potions: " + potionCount);
            Console.SetCursorPosition(0, 6);
            Console.Write("Time Crystals  : " + crystalCount);

            int curX = 0;
            int curY = 10;
            Console.SetCursorPosition(curX, curY);
            Console.Write("Paths from this node: " + player.Location.connections.Count);
            curY++;
            Console.SetCursorPosition(curX, curY);
            for (int i = 0; i < player.Location.connections.Count; i++)
            {
                Console.WriteLine(player.Location.connections[i].id + ", type: " + player.Location.connections[i].type);
                // Console.SetCursorPosition(curX, curY + i);
            }

            Console.SetCursorPosition(25, 14);
            Console.Write("Current dungeon level: " + d.level);
            Console.SetCursorPosition(25, 15);
            Console.Write("Visited nodes: " + player.VisitedList(level));
            Console.SetCursorPosition(25, 16);
            Console.Write("Current node : " + player.Location.id + "  ");
            Console.SetCursorPosition(33, 17);
            Console.Write("type : " + player.Location.type);

            curX = 60;
            curY = 0;
            Console.SetCursorPosition(curX, curY);
            Console.Write("Current packs: " + player.Location.nodePacks.Count);
            curY++;
            Console.SetCursorPosition(curX, curY);
            for (int i = 0; i < player.Location.nodePacks.Count; i++)
            {
                Console.SetCursorPosition(curX, curY);
                Console.Write("Pack nr: " + i);
                curY++;
                Console.SetCursorPosition(curX, curY);
                for (int j = 0; j < player.Location.nodePacks[i].monsters.Count; j++)
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

            Console.SetCursorPosition(0, 19);
            Console.Write("Give the next command. Type '?' for a list of commands.");
            Console.SetCursorPosition(0, 20);
        }

        // This method retrieves the input from the player, 
        // and makes sure that it's a valid string.
        private string GetInput()
        {
            string i = "";
            if (play)
            {
                i = Console.ReadLine();
                saveInputList.Add(i);
            }
            else
            {
                if (curCommand < inputList.Count())
                {
                    i = inputList[curCommand];
                    curCommand++;
                }
                else
                {
                    quit = true;
                }

            }

            string[] iArray = i.Split(' ');
            if (iArray[0] == "goto" || i == "stay" ||
                i == "retreat" || i == "continue" ||
                i == "quit" || i == "?" || i == "y" ||
                i == "n" || i == "hp" || i == "tc" ||
                iArray[0] == "save" || iArray[0] == "load")
            {
                if (iArray[0] == "save" || iArray[0] == "load")
                {
                    if (!CheckName(iArray[1]))
                    {
                        return "input not valid";
                    }
                }
                return i;
            }
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
            Console.WriteLine("- save x   : save the current game under a chosen filename x.txt");
            Console.WriteLine("- load x   : load the chosen savefile x");
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

            #region goto command
            if (i[0] == "goto")
            {
                if (i.Length > 1)
                {
                    int goToNode = -1;
                    try { goToNode = int.Parse(i[1]); } catch { }
                    if (player.Location.connections.Exists(dest => dest.id == goToNode))
                    {
                        Node destination = player.Location.connections.First(n => n.id == goToNode);
                        if (destination.type == "exit")
                        {
                            d = NextDungeon();
                            player.Move(d.nodes[0]);

                            Console.SetCursorPosition(0, cursorInfoPos);
                            Console.Write("Welcome to level " + d.level + ". This is node " + d.nodes[0].id);
                        }
                        else
                        {
                            player.Move(destination);
                            Console.SetCursorPosition(0, cursorInfoPos);
                            Console.Write("You moved to path " + destination.id);
                        }
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
            #endregion

            #region save command
            else if (i[0] == "save")
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.Write("Successfully saved the game");
            }
            #endregion

            #region load command
            else if (i[0] == "load")
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.Write("Successfully loaded the game");
            }
            #endregion

            #region stay command
            else if (input == "stay")
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.Write("You stay at your current location.");
            }
            #endregion

            else
            {
                if (input != "?")
                {
                    Console.SetCursorPosition(0, cursorInfoPos);
                    Console.Write("Command invalid: not in a combat situation.");
                }
            }
            // Move the packs this turn accordingly after a player action 
            foreach (Node node in d.nodes)
                foreach (Pack pack in node.nodePacks)
                    pack.HandlePackAI(player, d);
        }

        private void HandleCombat(string input)
        {
            if (input == "continue")
            {
                Console.Clear();
                DrawUI();
                Console.SetCursorPosition(0, cursorInfoPos);
                Console.WriteLine("Commencing combat!");
                Console.WriteLine("Do you want to use an item? (y/n)");

                bool usedTC = false;
                bool validAction = false;
                string action;

                #region combat loop
                while (!validAction)
                {
                    action = GetInput();

                    #region item loop
                    if (action == "y")
                    {
                        validAction = true;
                        bool validItem = false;
                        Console.WriteLine("Which item do you want to use? (hp/tc)");

                        while (!validItem)
                        {
                            action = GetInput();

                            #region healthpotion
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
                            #endregion

                            #region timecrystal
                            else if (action == "tc")
                            {
                                validItem = true;
                                if (player.inventory.Exists(item => item.ItemType() == "TimeCrystal"))
                                {
                                    player.UseCrystal();
                                    usedTC = true;
                                    Console.WriteLine("Used a time crystal!");
                                }
                                else
                                    Console.WriteLine("You don't have a time crystal!");
                            }
                            #endregion

                            else
                                Console.WriteLine("Action not valid! Choose 'hp' or 'tc'.");
                        }
                        for (int i = 0; i < player.Location.nodePacks.Count; i++)
                        {
                            if (player.Location.nodePacks.Count > 0)
                            {
                                player.Location.DoCombatRound(player.Location.nodePacks[i], player, usedTC);
                                break;
                            }
                        }
                    }
                    #endregion

                    else if (action == "n")
                    {
                        validAction = true;
                        for (int i = 0; i < player.Location.nodePacks.Count; i++)
                        {
                            if (player.Location.nodePacks.Count > 0)
                            {
                                player.Location.DoCombatRound(player.Location.nodePacks[i], player, usedTC);
                                break;
                            }
                        }

                    }

                    else { Console.WriteLine("Action not valid! Choose 'y' or 'n'."); }
                }
                #endregion

                #region post combat
                bool validMove = false;
                while (!validMove && player.Location.CheckInCombat())
                {
                    Console.WriteLine("What's your next action? Choose 'continue' or 'retreat'.");
                    action = GetInput();
                    if (action == "continue")
                    {
                        validMove = true;
                        HandleCombat(action);
                    }
                    else if (action == "retreat")
                    {
                        validMove = true;
                        if (!player.IsDead)
                        {
                            Console.WriteLine("Got away safely to node " + player.Location.connections[0].id + "!");
                            player.Move(player.Location.connections[0]);
                            inCombat = false;
                        }
                    }
                }
                Console.WriteLine("Combat round finished! Press a key to continue.");
                Console.ReadKey();
                Console.Clear();
                #endregion
            }

            else
            {
                Console.SetCursorPosition(0, cursorInfoPos);
                if (input != "?")
                    Console.Write("Command invalid: Only 'continue' allowed!");
            }
        }

        void SaveGame(string fileName)
        {
            if (fileName == "")
                throw new ArgumentException("Not a valid filename");
            if (d == null)
                throw new NullReferenceException("No dungeon to save");

            string saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GitHub\\ST-V\\STV-1\\STV-1\\" + fileName + ".txt";
            List<string> saveList = new List<string>();
            saveList.Add("" + level);
            saveList.Add("" + player.HP);
            saveList.Add("" + player.ATK);
            saveList.Add("" + player.kp);
            saveList.Add("" + player.Location.id);
            int potionCount = player.inventory.Count(s => s.ItemType() == "HealingPotion");
            int crystalCount = player.inventory.Count(s => s.ItemType() == "TimeCrystal");
            saveList.Add("" + potionCount);
            saveList.Add("" + crystalCount);
            saveList.Add("" + d.seed);
            int temp = 0;
            foreach (Pack pack in d.packs)
            {
                saveList.Add("" + pack.monsters.Count);
                saveList.Add("" + pack.PackLocation.id);
                temp++;
            }

            File.WriteAllLines(saveLocation, saveList);
        }

        void LoadGame(string fileName)
        {
            string location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GitHub\\ST-V\\STV-1\\STV-1\\" + fileName + ".txt";
            string[] loaded = File.ReadAllLines(location);
            level = int.Parse(loaded[0]);
            Node.idCounter = 0;
            d = new Dungeon(level, int.Parse(loaded[7]));
            d.GenerateDungeon(level, false);
            Node playerLocation = d.GetNodeByID(int.Parse(loaded[4]));
            player = new Player(int.Parse(loaded[1]), int.Parse(loaded[2]), playerLocation, d);
            player.kp = int.Parse(loaded[3]);
            int temp = 0;

            for (int i = 8; i < loaded.Length - 1; i += 2)
            {
                int monsterNr = int.Parse(loaded[i]);
                Node destinationNode = d.GetNodeByID(int.Parse(loaded[i + 1]));
                d.packs.Add(new Pack(monsterNr, d.GetNodeByID(int.Parse(loaded[i + 1]))));
                temp++;
            }
        }

        bool CheckName(string name)
        {
            string[] illegalChars = { ",", ".", "\\", "/" };
            foreach (string chr in illegalChars)
            {
                if (name.Contains(chr))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STV1
{
    class Program
    {
        [ExcludeFromCodeCoverage]
        static void Main(string[] args)
        {
            bool gamemode = false;
            Console.WriteLine("Type 'r' followed by a filename if you want to record a playthrough.");
            Console.WriteLine("Type 'p' if you want to play an existing one");
            string input = Console.ReadLine();

            if (input == "r")
                gamemode = true;
            else if (input == "p") 
                gamemode = false;

            Console.WriteLine("Based on your first input, enter the filename you want to create or");
            Console.WriteLine("the filename you want to be be used for your recording. Note: Don't include the .txt");
            string input2 = Console.ReadLine();
            Console.Clear();
            Game game = new Game(gamemode, input2, false);
        }
    }
}
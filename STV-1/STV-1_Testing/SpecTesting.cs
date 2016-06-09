using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STV1;
using STV_1;

namespace STV_1_Testing
{
    [TestClass()]
    public class SpecTesting
    {
        [TestMethod]
        public void SpecTestingAll()
        {
            // Specify the name of the file you want to test using Visual Studio's unit testing without the txt here.
            Game game = new Game(false, "meme");
        }
    }
}

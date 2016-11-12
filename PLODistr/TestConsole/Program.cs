using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPT;
using PLOLogic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new Calculator();
            calc.Calculate("Ac5h2c",600000,100,4);
            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rangedisplaytest
{
    public class Range
    {
        public List<string> Hands;
        List<char> ranks = new List<char>() { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
        public Range(string rangetext)
        {
            //Hands = new List<string>() { "AKs", "55", "65o", "72o"};
            Hands = File.ReadAllLines("hands.txt").ToList();
        }

        public void Display()
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    //Console.Write(i + " " + j + " ");
                    var h = i > j ? ranks[j].ToString() + ranks[i].ToString() + "o" : ranks[i].ToString() + ranks[j].ToString() + "s";
                    if (i == j) h = h[0].ToString() + h[1];
                    if (Hands.IndexOf(h) > -1)
                    {
                        if (i == j) h += " ";
                        Console.Write(h + " ");
                    }
                    else
                        Console.Write("    ");

                }
                Console.WriteLine();
            }
        }
    }
}

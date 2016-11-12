using PPT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLOLogic
{
    public class Calculator
    {
        public Calculator()
        {
            Client.Init();
        }
        public List<HandData> Calculate(string board, int trials = 600000,int samples = 100, int cores = 1)
        {
            var list = new List<HandData>();
            var herorange = File.ReadAllText("herorange.txt").ToPLOHands();
            var villainrange = File.ReadAllText("villainrange.txt");


            //var board = "A52:xyz";
            //var P1 = "AsKd6c5c";
            //var P2 = "8d7d6h5h";
            var P2 = villainrange;
            var subsetcount = Math.Min(samples, herorange.Count);
            var rnd = new Random();
            var randomsubset = herorange.OrderBy(xz => rnd.Next()).Take(subsetcount).ToList();
            var oi = 0;
            foreach (var hand in randomsubset)
            {
                oi++;
                Console.WriteLine(oi);
                if (!(Helpers.CommonCards(hand, board)==(board.Length/2 + 4))) continue;
                var P1 = hand;
                var pql1 = "select avg(riverEquity(PLAYER_1)) as PLAYER_1_equity1" + " " +
                           "from game = 'omahahi', syntax = 'Generic', board = '" + board + "', PLAYER_1 = '" + P1 + "',  PLAYER_2 = '" + P2 + "'";

                System.Object obj = Client.proxy.executePQL(pql1, trials, 10, cores);
                string x = obj.ToString();
                double eq = Math.Round(Convert.ToDouble(x.ToEquity().Replace(".", ",")), 2);
                list.Add(new HandData() { Hand = hand, Equity = eq });                
            }
            list = list.OrderBy(i => i.Equity).ToList();
            var ct = list.Count();
            File.WriteAllLines(board + ".csv", list.Select((item, index) => Math.Round(((double)index/ct),3)*100 + "%:  " + item.Hand + "\t" + item.Equity));
            return list;
        }
    }
}

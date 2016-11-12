using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLOLogic
{
    public static class Helpers
    {
        public static List<string> ToPLOHands(this string handstxt)
        {            
            return handstxt.Split(new string[] { "\t", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public static string ToEquity(this string handstxt)
        {
            return handstxt.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[2];
        }
        public static int CommonCards(string h1, string h2)
        {
            var cards1 = new List<string> { h1[0].ToString() + h1[1], h1[2].ToString() + h1[3], h1[4].ToString() + h1[5], h1[6].ToString() + h1[7] };
            var cards2 = new List<string> { h2[0].ToString() + h2[1], h2[2].ToString() + h2[3], h2[4].ToString() + h2[5]};
            cards1.AddRange(cards2);
            return cards1.Distinct().Count() ;
        }
    }
}

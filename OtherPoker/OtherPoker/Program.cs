using Poker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherPoker
{
    class Program
    {
        static void Main(string[] args)
        {
            //var h = new BadugiHand("Ac", "Th", "3c", "Kc");
            var h = new BadugiHand("As", "Ah", "4s", "5h");
            var show = h.Show();
            var s = BadugiHand.DescriptionFromHandMask(h.HandMask);
            //var all = Range.GetAllHands();
            //var min = Range.MinHand;
            //var max = Range.MaxHand;
            //var r = new Range("AcTh3cKc","AcTh3d2d","custom");
            //var x = r.ToLine();
            //var r2 = new Range(x);
            //var newrange = new Range(3, "3A5");
            //var newrange2 = new Range(newrange.Bottom, newrange.Top);
        }
    }
}
using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var wnds = Logic.Wins.FindRectsWithText("NLHE Spin");
            foreach(var wnd in wnds)
            {
                Console.WriteLine("TITLE: " + wnd.T + " " + "TOP: " + wnd.R.Top + " LEFT: " + wnd.R.Left);
            }
            Console.WriteLine("IDX: " + Logic.Wins.GetWindowIndexByTID("1658406941"));
            Console.ReadLine();*/



            var files = Directory.GetFiles(@"\images").ToList();
            var paths = files.Select(x => Path.GetFileName(x));
            var list = paths.Select(y => GetCatalogItem(y, files)).Cast<CatalogItem>().ToList();
            File.WriteAllText("catalog.xml", list.Serialize());
            //var watcher = new Logic.Watcher();
            Console.WriteLine("DONE");



            //var preflop_regex_match = new Regex(RegexColl.PreflopRegex).Match(HH);
            //var flop_regex_match = new Regex(RegexColl.FlopRegex).Match(HH);
            //var turn_regex_match = new Regex(RegexColl.TurnRegex).Match(HH);
            //var river_regex_match = new Regex(RegexColl.RiverRegex).Match(HH);

            //var uncalled_regex_matches = new Regex(RegexColl.UncalledRegex).Matches(HH);
            //var collected_regex_matches = new Regex(RegexColl.CollectedRegex).Matches(HH);
            //var handdata_regex_matches = new Regex(RegexColl.HandDataRegex, RegexOptions.Singleline).Match(HH);

            //var tid = handdata_regex_matches.Groups["TID"].Value;
            /*

            var list = new List<CatalogItem>();
            list.Add(new CatalogItem() { Stack = 6, Position = Positions.SB, ImagePath = "path of image", Players = 3, Description = "Description123", Comment="" });
            list.Add(new CatalogItem() { Stack = 6, Position = Positions.SB, ImagePath = "path of image", Players = 3, Description = "Description123", Comment = "" });
            list.Add(new CatalogItem() { Stack = 6, Position = Positions.SB, ImagePath = "path of image", Players = 3, Description = "Description123", Comment = "" });
            list.Add(new CatalogItem() { Stack = 6, Position = Positions.SB, ImagePath = "path of image", Players = 3, Description = "Description123", Comment = "" });


            File.WriteAllText("catalog.xml", list.Serialize());
            */

            //var HH = File.ReadAllText("hh.txt");
            //var h = new Hand(HH);

            Console.ReadLine();
        }
        public static CatalogItem GetCatalogItem(string s, List<string> fullpathlists)
        {
            var s1 = s.Split(new char[] { '.' })[0];
            var els = s1.Split(new char[] { '_' });
            var players = Convert.ToInt32(els[0][0].ToString());
            var position = (Positions)Enum.Parse(typeof(Positions), (els[2]).ToUpper());
            var desc = els[3].ToUpper();
            var stack = Convert.ToInt32(els[1].Replace("bb",""));
            var i = fullpathlists.FirstOrDefault(j => j.Contains(s));
            var imagepath = i == null ? "" : i;
            return new CatalogItem() { Stack = stack, Position = position, ImagePath = imagepath, Players = players, Description = desc, Comment = "" };
        }
    }
}

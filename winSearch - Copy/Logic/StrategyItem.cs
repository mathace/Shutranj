using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class Init
    {
        public static List<StrategyItem> Items = new List<StrategyItem>();
        public static void Begin()
        {
            Items.Add(new StrategyItem() { Title = "Title1", Keywords = new List<string>() { "BB", "vsSB" }, FileName = @"\Strats\strat1.txt", Strat = new Strategy(), CleanStrat = new Strategy() });
            Items.Add(new StrategyItem() { Title = "Title2", Keywords = new List<string>() { "MP", "vsUTG" }, FileName = @"\Strats\strat2.txt", Strat = new Strategy(), CleanStrat = new Strategy() });
            File.WriteAllText("catalog.xml", Items.Serialize());
        }
    }

    public class StrategyItem
    {
        public string Title { get; set; }
        public List<string> Keywords { get; set; }
        public string FileName { get; set; }
        public Strategy Strat { get; set; }
        public Strategy CleanStrat { get; set; }
    }
}

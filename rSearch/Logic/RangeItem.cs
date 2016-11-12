using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class RangeItem
    {
        public ObservableCollection<StrategyOption> StrategyOptions { get; set; }
        public int Stack { get; set; }
        public Positions Position { get; set; }

        public int Players { get; set; }

        public RangeItem (int stack, Positions position, int players)
        {
            this.Stack = stack;
            this.Players = players;
            this.Position = position;
            StrategyOptions = new ObservableCollection<StrategyOption>();
            StrategyOptions.Add(new StrategyOption() { Description = "Opener", ImagePath = @"E:\rangeImages\3SB_25bb_OPENER_HALF.png" });
            StrategyOptions.Add(new StrategyOption() { Description = "Opener2", ImagePath = @"E:\rangeImages\3SB_25bb_OPENER_HALF.png" });
            StrategyOptions.Add(new StrategyOption() { Description = "Opener2", ImagePath = @"E:\rangeImages\3SB_25bb_OPENER_HALF.png" });
            StrategyOptions.Add(new StrategyOption() { Description = "Opener2", ImagePath = @"E:\rangeImages\3SB_25bb_OPENER_HALF.png" });
        }
    }
    public class StrategyOption
    {
        public string Description { get; set; }
        public string ImagePath { get; set; }

    }
}

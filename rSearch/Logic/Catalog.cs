using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Catalog
    {
        public ObservableCollection<BBDatas> BBDatasList { get; set; }

        public Catalog(List<CatalogItem> catalogitemlist)
        {
            
            var bbs = new ObservableCollection<BBDatas>(catalogitemlist.GroupBy(ci => ci.Stack).Select(g => g.First().Stack).Select(bb => new BBDatas() { BB=bb, PosDataList = new ObservableCollection<PosDatas>()}).ToList());
            foreach (var bbdata in bbs)
            {
                var poss = catalogitemlist.Where(citem => citem.Stack == bbdata.BB).GroupBy(cu => cu.Position).Select(h => h.First().Position).Select(p => new PosDatas() { Position = p, ActionDataList = new ObservableCollection<ActionData>() }).ToList();
                foreach(var pos in poss)
                {
                    pos.ActionDataList = new ObservableCollection<ActionData>( catalogitemlist.Where(citem => citem.Stack == bbdata.BB && citem.Position == pos.Position).Select(j => new ActionData() { ActionString = j.Description, ActionPath = j.ImagePath, Players = j.Players.ToString() }).ToList());
                }
                bbdata.PosDataList = new ObservableCollection<PosDatas>(poss);
            }
            this.BBDatasList = new ObservableCollection<BBDatas>(bbs.OrderByDescending(b=>b.BB            ));            
        }
    }
    public class BBDatas
    {
        public int BB { get; set; }
        public ObservableCollection<PosDatas> PosDataList { get; set; }
        List<CatalogItem> cilist;        
    }
    public class PosDatas
    {
        public Positions Position { get; set; }
        public ObservableCollection<ActionData> ActionDataList { get; set; }
        
    }
    public class ActionData
    {
        public string ActionString { get; set; }
        public string ActionPath { get; set; }
        public string Players { get; set; }
    }
}

using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSearch
{
    public static class Helper
    {        public static CatalogItem GetCatalogItem(string suj)
        {
            var s = System.IO.Path.GetFileName(suj);
            var s1 = s.Split(new char[] { '.' })[0];
            var els = s1.Split(new char[] { '_' });
            var players = Convert.ToInt32(els[0][0].ToString());
            var position = (Positions)Enum.Parse(typeof(Positions), (els[2]).ToUpper());
            var desc = els[3].ToUpper();
            if (desc.StartsWith("VS")) desc = desc.Insert(2, " ");
            var stack = Convert.ToInt32(els[1].Replace("bb", ""));            
            var imagepath = suj;
            return new CatalogItem() { Stack = stack, Position = position, ImagePath = imagepath, Players = players, Description = desc, Comment = "" };
        }
    }
}

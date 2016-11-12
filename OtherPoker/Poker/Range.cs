using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Poker
{
    public class Range
    {
        public static Range AllHands = new Range();
        public static List<BadugiCard> Deck = new List<BadugiCard>();
        public static BadugiHand MinHand;
        public static BadugiHand MaxHand;

        public bool Saveable = false;

        public BadugiHand Bottom { get; set; }
        public BadugiHand Top { get; set; }
       
        public Range(List<BadugiHand> hands, string name)
        {
            Name = name;
            Hands = hands;
        }
        
        public static Range GetAllHands()
        {
            var ranks = new List<char>() { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
            var suits = new List<char>() { 'c', 'd', 'h', 's' };
            Deck = (from r in ranks
                       from s in suits
                       select new BadugiCard(r.ToString() + s)).ToList();
            if (AllHands.Hands.Count != 0) return AllHands;
            
             var hands = from c1 in Deck
                        from c2 in Deck.Where(c => c != c1)
                        from c3 in Deck.Where(c => c != c1 && c != c2)
                        from c4 in Deck.Where(c => c != c1 && c != c2 && c != c3)                       
                        select new BadugiHand((string)c1, (string)c2, (string)c3, (string)c4);          
            

            AllHands = new Range(hands.OrderBy(h => h.HandValue).ToList(),"allhands");
            MinHand = AllHands.Hands[0];
            MaxHand = AllHands.Hands[AllHands.Hands.Count - 1];
            return AllHands;
        }
        public Range()
        {
            Hands = new List<BadugiHand>();
            Name = "none";
        }
        public Range(RangeParams rp) : this(new BadugiHand(rp.MinHand), new BadugiHand(rp.MaxHand), rp.Name)
        {

        }
        public Range(BadugiHand min, BadugiHand max, string name = "default")
        {
            Bottom = min;
            Top = max;
            this.Hands = AllHands.Hands.SkipWhile(h => h.HandValue < min.HandValue).TakeWhile(h => h.HandValue <= max.HandValue).ToList();            
            this.Name = name;
            this.Saveable = true;
        }
        public Range (int cardseval, string cards)
        {
           
            var cs = String.Concat(cards.OrderBy(c=>c));
            var temp = AllHands.Hands.Select(h => h.Category);
            this.Hands = AllHands.Hands.Where(h => h.Category.Cards == cs &&  h.Category.CardsEval==cardseval).OrderBy(j => j.HandValue).ToList();
            var t2 = temp.Skip(50000).Take(10000);
            this.Bottom = Hands[0];
            this.Top = Hands.Last();
            this.Saveable = true;
            

        }
        public Range(string minhand, string maxhand, string name) : this(new BadugiHand(minhand), new BadugiHand(maxhand), name)
        {        }
        public Range(string line) : this(line.Split(new char[] { ';'})[0], line.Split(new char[] { ';' })[1], line.Split(new char[] { ';' })[2])
        {

        }

        public Range (List<Range> ranges)
        {
            this.Saveable = false;
            this.Hands = new List<BadugiHand>();
            foreach(var r in ranges)
            {
                this.Hands.AddRange(r.Hands);
            }
            this.Hands = this.Hands.Distinct().ToList();
        }

        public string ToLine()
        {
            return Bottom.Display() + ";" + Top.Display() + ";" + Name;
        }
        [XmlIgnore]
        public List<BadugiHand> Hands;
        public string Name { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
        public static void SaveRanges(List<Range> ranges)
        {
            File.WriteAllLines("saves.badugi", ranges.Where(p => p.Saveable == true).Select(r=>r.ToLine()));
        }
        public static List<Range> LoadRanges()
        {
            return File.ReadAllLines("saves.badugi").Select(r => new Range(r)).ToList();
        }
        public string SaveString()
        {
            return Bottom.Show() + ";" + Top.Show() + ";" + Name;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic
{
    public enum Positions { BTN, SB, BB}
    public class Player
    {
        public string Name { get; set; }
        public int Stack { get; set; }
        public int Seat { get; set; }

        public Positions Position { get; set; }
        public List<int> Bets { get; set; }
       
    }
    public class Hand
    {
        public string HH { get; set; }
        public string TournamentID { get; set; }
        public List<Player> Players { get; set; }
        public int RelatedTableID { get; set; }
        public int BBSize { get; set; }

        public string HeroName { get; set; }
        public Hand(string hh)
        {
            Players = new List<Player>();
            this.HH = hh;
            var preflop_regex_match = new Regex(RegexColl.PreflopRegex, RegexOptions.Singleline).Match(HH);
            var preflop_regex_matches = new Regex(RegexColl.ActionRegex).Matches(preflop_regex_match.Value);
            var flop_regex_match = new Regex(RegexColl.FlopRegex, RegexOptions.Singleline).Match(HH);
            var flop_regex_matches = new Regex(RegexColl.ActionRegex).Matches(flop_regex_match.Value);
            var turn_regex_match = new Regex(RegexColl.TurnRegex, RegexOptions.Singleline).Match(HH);
            var turn_regex_matches = new Regex(RegexColl.ActionRegex).Matches(turn_regex_match.Value);
            var river_regex_match = new Regex(RegexColl.RiverRegex, RegexOptions.Singleline).Match(HH);
            var river_regex_matches = new Regex(RegexColl.ActionRegex).Matches(river_regex_match.Value);

            var uncalled_regex_matches = new Regex(RegexColl.UncalledRegex).Matches(HH);
            var collected_regex_matches = new Regex(RegexColl.CollectedRegex).Matches(HH);
            var handdata_regex_matches = new Regex(RegexColl.HandDataRegex, RegexOptions.Singleline).Match(HH);

            var eliiminatedplayer_regex_match = new Regex(RegexColl.EliminatedPlayerRegex).Match(HH);

            this.TournamentID = handdata_regex_matches.Groups["TID"].Value;
            this.RelatedTableID = Wins.GetWindowIndexByTID(TournamentID);

            var hero_regex_match = new Regex(RegexColl.HeroRegex).Match(HH);

            HeroName = hero_regex_match.Groups["HERONAME"].Value;

            if (eliiminatedplayer_regex_match.Success)
            {

            }

            var actions = new List<List<PAction>>();

            var SBname = handdata_regex_matches.Groups["SBPLAYER"].Value;
            var BBname = handdata_regex_matches.Groups["BBPLAYER"].Value;

            BBSize = Convert.ToInt32(handdata_regex_matches.Groups["BB"].Value);
            var SBSize = Convert.ToInt32(BBSize / 2);

            //Players.Single(p => p.Name == SBname).Bets.Add(-1 * Convert.ToInt32(BBSize / 2));
            //Players.Single(p => p.Name == BBname).Bets.Add(-1 * Convert.ToInt32(BBSize));

            // PREFLOP ACTIONS

            var preflop_actions = new List<PAction>();

            preflop_actions.Add(new PAction() { ActionType = "bets", Size = SBSize, PlayerName = SBname, SizeTo = 0 });
            preflop_actions.Add(new PAction() { ActionType = "bets", Size = BBSize, PlayerName = BBname, SizeTo = 0 });

            for (var i = 0; i < preflop_regex_matches.Count; i++)
            {
                var a = new PAction()
                {
                    PlayerName = preflop_regex_matches[i].Groups["PLAYER"].Value,
                    ActionType = preflop_regex_matches[i].Groups["ACTION"].Value,
                    Size = Convert.ToInt32(preflop_regex_matches[i].Groups["AMOUNT"].Value),
                    SizeTo = preflop_regex_matches[i].Groups["TOAMOUNT"].Value == "" ? 0 : Convert.ToInt32(preflop_regex_matches[i].Groups["TOAMOUNT"].Value)
                };
                preflop_actions.Add(a);
            }
            preflop_actions.Reverse();
            actions.Add(preflop_actions);

            //FLOP ACTIONS

            var flop_actions = new List<PAction>();
            for (var i = 0; i < flop_regex_matches.Count; i++)
            {
                var a = new PAction()
                {
                    PlayerName = flop_regex_matches[i].Groups["PLAYER"].Value,
                    ActionType = flop_regex_matches[i].Groups["ACTION"].Value,
                    Size = Convert.ToInt32(flop_regex_matches[i].Groups["AMOUNT"].Value),
                    SizeTo = flop_regex_matches[i].Groups["TOAMOUNT"].Value == "" ? 0 : Convert.ToInt32(flop_regex_matches[i].Groups["TOAMOUNT"].Value)
                };
                preflop_actions.Add(a);
            }
            flop_actions.Reverse();
            actions.Add(flop_actions);

            //TURN ACTIONS

            var turn_actions = new List<PAction>();
            for (var i = 0; i < turn_regex_matches.Count; i++)
            {
                var a = new PAction()
                {
                    PlayerName = turn_regex_matches[i].Groups["PLAYER"].Value,
                    ActionType = turn_regex_matches[i].Groups["ACTION"].Value,
                    Size = Convert.ToInt32(turn_regex_matches[i].Groups["AMOUNT"].Value),
                    SizeTo = turn_regex_matches[i].Groups["TOAMOUNT"].Value == "" ? 0 : Convert.ToInt32(turn_regex_matches[i].Groups["TOAMOUNT"].Value)
                };
                turn_actions.Add(a);
            }
            turn_actions.Reverse();
            actions.Add(turn_actions);

            //RIVER ACTIONS

            var river_actions = new List<PAction>();
            for (var i = 0; i < river_regex_matches.Count; i++)
            {
                var a = new PAction()
                {
                    PlayerName = river_regex_matches[i].Groups["PLAYER"].Value,
                    ActionType = river_regex_matches[i].Groups["ACTION"].Value,
                    Size = Convert.ToInt32(river_regex_matches[i].Groups["AMOUNT"].Value),
                    SizeTo = river_regex_matches[i].Groups["TOAMOUNT"].Value == "" ? 0 : Convert.ToInt32(river_regex_matches[i].Groups["TOAMOUNT"].Value)
                };
                river_actions.Add(a);
            }
            river_actions.Reverse();
            actions.Add(river_actions);

            //Players.Add(new Logic.Player() { Bets = new List<int>(), Name = handdata_regex_matches.Groups["SEAT1"].Value, Seat = 1, Stack = Convert.ToInt32(handdata_regex_matches.Groups["STACK1"].Value), Position = Positions.BTN });
            //Players.Add(new Logic.Player() { Bets = new List<int>(), Name = handdata_regex_matches.Groups["SEAT2"].Value, Seat = 2, Stack = Convert.ToInt32(handdata_regex_matches.Groups["STACK2"].Value), Position = Positions.BTN });

            var seat3regex_match = new Regex(RegexColl.Seat3Regex).Match(hh);
            if (seat3regex_match.Groups["SEAT3"].Value != "")
            {
                Players.Add(new Logic.Player() { Bets = new List<int>(), Name = seat3regex_match.Groups["SEAT3"].Value, Seat = 3, Stack = Convert.ToInt32(seat3regex_match.Groups["STACK3"].Value), Position = Positions.BTN });
            }

            var seat2regex_match = new Regex(RegexColl.Seat2Regex).Match(hh);
            if (seat2regex_match.Groups["SEAT2"].Value != "")
            {
                Players.Add(new Logic.Player() { Bets = new List<int>(), Name = seat2regex_match.Groups["SEAT2"].Value, Seat = 2, Stack = Convert.ToInt32(seat2regex_match.Groups["STACK2"].Value), Position = Positions.BTN });
            }

            var seat1regex_match = new Regex(RegexColl.Seat1Regex).Match(hh);
            if (seat1regex_match.Groups["SEAT1"].Value != "")
            {
                Players.Add(new Logic.Player() { Bets = new List<int>(), Name = seat1regex_match.Groups["SEAT1"].Value, Seat = 1, Stack = Convert.ToInt32(seat1regex_match.Groups["STACK1"].Value), Position = Positions.BTN });
            }



            Players.Single(p => p.Name == SBname).Position = Positions.SB;
            Players.Single(p => p.Name == BBname).Position = Positions.BB;

            //handdata_regex_matches.Groups[""]

            foreach (Match mt in uncalled_regex_matches)
            {
                Players.Single(h => h.Name == mt.Groups["PLAYERNAME"].Value).Bets.Add(Convert.ToInt32(mt.Groups["AMOUNT"].Value));
            }
            foreach (Match mt in collected_regex_matches)
            {
                Players.Single(h => h.Name == mt.Groups["PLAYERNAME"].Value).Bets.Add(Convert.ToInt32(mt.Groups["AMOUNT"].Value));
            }

            foreach (var street in actions)
                // foreach(var action in street)
                foreach (var p in Players)
                {
                    var raise = street.FirstOrDefault(sa => sa.PlayerName == p.Name && sa.ActionType == "raises");
                    if (raise != null)
                    {
                        p.Bets.Add(-1 * raise.SizeTo);
                        continue;
                    }
                    p.Bets.Add(-1 * street.Where(r => r.PlayerName == p.Name && (r.ActionType == "bets" || r.ActionType == "calls")).Sum(j => j.Size));
                }


            foreach (var player in Players)
            {
                player.Stack += player.Bets.Sum();
            }
            //Console.WriteLine("HH: " + Environment.NewLine + Environment.NewLine + hh + Environment.NewLine + Environment.NewLine + "IDX: " + index);          

            var elimination = eliiminatedplayer_regex_match.Groups["ELIMINATEDPLAYER"].Value != "";

            foreach(var player in Players)
            {
                player.Position = NewPosition(player.Position);
                if ((elimination || Players.Count==2)&& player.Position == Positions.BTN)
                {
                    player.Position = NewPosition(player.Position);
                }
            }

            foreach (var player in Players)
            {
                Console.WriteLine(player.Name + "(" + player.Position.ToString() +  "): " + player.Stack.ToString());
            }
        }
        public Positions GetHeroPosition()
        {
            return Players.Single(p => p.Name == HeroName).Position;
        }
        public int GetStackForPosition(Positions p)
        {
            return Players.Single(r => r.Position == p).Stack;
        }
        public int GetHeroBB()
        {            
            int effstack = Math.Min(GetStackForPosition(Positions.SB), GetStackForPosition(Positions.BB));
            return Convert.ToInt32(Math.Round(((double)effstack / BBSize),0));
        }
        public int GetPlayersCount()
        {
            return Players.Count;
        }
        private Positions NewPosition(Positions pos)
        {
            if (pos == Positions.BB) return Positions.SB;
            if (pos == Positions.SB) return Positions.BTN;
            return Positions.BB;
        }
    }
    public class PAction
    {
        public string PlayerName { get; set; }
        public string ActionType { get; set; }
        public int Size { get; set; }
        public int SizeTo { get; set; }
    }
   

}

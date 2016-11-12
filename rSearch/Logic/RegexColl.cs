using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class RegexColl
    {
        //public static string HandDataRegex = @".*Tournament\s#(?<TID>[0-9]*).*Seat\s#(?<BTN>[1-9]).*Seat\s1:\s(?<SEAT1>[^(\s)]*)\s.(?<STACK1>[0-9]*)\s.*Seat\s2:\s(?<SEAT2>[^(\s)]*)\s.(?<STACK2>[0-9]*)\s.*Seat\s3:\s(?<SEAT3>[^(\s)]*)\s.(?<STACK3>[0-9]*)\s.*\n(?<SBPLAYER>[^(\s)]*):\sposts\ssmall\sblind\s(?<SB>[0-9]*).+\n(?<BBPLAYER>[^(\s)]*):\sposts\sbig\sblind\s(?<BB>[0-9]*).*"; //SINGLELINE
        public static string HandDataRegex =  @".*Tournament\s#(?<TID>[0-9]*).*Seat\s#(?<BTN>[1-9]).*Seat\s[123]:\s(?<SEAT1>[^(\s)]*)\s.(?<STACK1>[0-9]*)\s.*Seat\s[123]:\s(?<SEAT2>[^(\s)]*)\s.(?<STACK2>[0-9]*)\s.*(Seat\s[123]:\s(?<SEAT3>[^(\s)]*)\s.(?<STACK3>[0-9]*))?\s.*\n(?<SBPLAYER>[^(\s)]*):\sposts\ssmall\sblind\s(?<SB>[0-9]*).+\n(?<BBPLAYER>[^(\s)]*):\sposts\sbig\sblind\s(?<BB>[0-9]*).*";

        public static string Seat1Regex = @"Seat\s[1]:\s(?<SEAT1>[^(\s)]*)\s.(?<STACK1>[0-9]*)\sin\schips";
        public static string Seat2Regex = @"Seat\s[2]:\s(?<SEAT2>[^(\s)]*)\s.(?<STACK2>[0-9]*)\sin\schips";
        public static string Seat3Regex = @"Seat\s[3]:\s(?<SEAT3>[^(\s)]*)\s.(?<STACK3>[0-9]*)\sin\schips";

        public static string HeroRegex = @"Dealt\sto\s(?<HERONAME>[^\s]*)\s";

        //public static string ActionRegex = @"(?<PLAYER>[^\s]*):\s(?<ACTION>(raises|bets|calls))\s(?<AMOUNT>[0-9]*)(\sto\s(?<TOAMOUNT>[0-9]*))?\n";

        public static string UncalledRegex = @"Uncalled\sbet\s\((?<AMOUNT>[0-9]*)\)\sreturned\sto\s(?<PLAYERNAME>[^\s]*)";

        public static string ActionRegex = @"(?<PLAYER>[^\s]*):\s(?<ACTION>(raises|bets|calls))\s(?<AMOUNT>[0-9]*)(\sto\s(?<TOAMOUNT>[0-9]*))?";

        //public static string CollectedRegex = @".*:\s(?<PLAYERNAME>[^\s]*)\s.*collected\s\((?<AMOUNT>[0-9]*)\)";
        public static string CollectedRegex = @"(?<PLAYERNAME>[^\s]*)\scollected\s(?<AMOUNT>[0-9]*)\sfrom\spot";

        public static string PreflopRegex = @"HOLE(?<PREFLOP>.*?)(SUMMARY|FLOP|SHOW\sDOWN)";

        public static string FlopRegex = @"FLOP(?<PREFLOP>.*?)(SUMMARY|TURN|SHOW\sDOWN)";

        public static string TurnRegex = @"TURN(?<PREFLOP>.*?)(SUMMARY|RIVER|SHOW\sDOWN)";

        public static string RiverRegex = @"RIVER(?<PREFLOP>.*)(SUMMARY|SHOW\sDOWN)";

        public static string TitleRegex = @".*Tournament\s(?<TID>[0-9]*)\s.*";

        public static string EliminatedPlayerRegex = @"^(?<ELIMINATEDPLAYER>[^\s]*)\sfinished\sthe\stournament";
    }
}

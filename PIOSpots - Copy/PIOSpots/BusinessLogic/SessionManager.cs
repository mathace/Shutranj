using PioConnector.API;
using PIOSpots.Model;
using PIOSpots.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIOSpots.Utility;
using GalaSoft.MvvmLight;

namespace PIOSpots.BusinessLogic
{
    public class SessionManager 
    {
        public PlayViewModel PlayVM;
        public List<Spot> SpotList;
        private Random rnd;
        //private Settings _settings = new Settings();
        public Settings UserSettings
        {
            get;set;
        }
        
        private Spot _actualspot;
        private string _actualnode;
        public string ActualNode
        {
            get { return _actualnode; }
            set { _actualnode = value;}
        }
        public SessionManager(List<Spot> SpotList, PlayViewModel PlayVM, Settings settings)
        {
            this.UserSettings = settings;
            this.PlayVM = PlayVM;
            //this.SpotList = SpotList;
            this.SpotList = new List<Spot>();
            _actualspot = new Spot();
            foreach(var spot in SpotList)
            {
                if (spot.Herotype == HeroType.OOP || spot.Herotype == HeroType.IP) this.SpotList.Add(spot);
                if (spot.Herotype == HeroType.Any)
                {
                    var spot1 = spot;
                    var spot2 = spot;
                    spot1.Herotype = HeroType.IP;
                    spot2.Herotype = HeroType.OOP;
                    this.SpotList.Add(spot1);
                    this.SpotList.Add(spot2);
                }
            }
            rnd = new Random();
            try
            {
                PIO.Connect(settings.PIOPath);
            }
            catch (Exception e)
            {
                Debug.WriteLine("PIO EXE NOF FOUND");
            }
        }
        public void GetNewSpotOld()
        {
            GetNewhand();
            //PlayVM.ActualSpot = SpotList[0];
            //PlayVM.HeroCards = "As5s";
            //PlayVM.VillainCards = "";
            //PlayVM.BoardCards = "2s3s7d7sQs";
            //PlayVM.PotSize = 100;
            //PlayVM.HeroActions = new List<HeroAction>() { new HeroAction() { ActionString = "CALL", Weight = 1.0 }, new HeroAction() { ActionString = "FOLD", Weight = 1.0 }, new HeroAction() { ActionString = "RAISE 660", Weight = 1.0 } };
            //PlayVM.VillainAction = "BET 88";
         }
        public void DisplayNode(string node)
        {
            try
            {
                var pionode = PIO.GetNode(node);
                pionode.FillCsildren();
                PlayVM.ActualSpot = _actualspot;
                PlayVM.BoardCards = pionode.Board.Replace(" ", "");
                var ind1 = pionode.Range.Hands.PickRandomWeightedElement(rnd);
                var ind2 = pionode.Range2.Hands.PickRandomWeightedElement(rnd);
                PlayVM.HeroStack = _actualspot.EffStack;
                PlayVM.VillainStack = _actualspot.EffStack;
                PlayVM.PotSize = pionode.Pot.InitialPot;
                if (pionode.NodeType == NODE_TYPE.END_NODE)
                {
                    PlayVM.VillainCards = PlayVM.siddenVillainCards;
                }
                //GET STRAT OPTIONS WITs WEIGsT
                var actualStrategy = new List<StrategyOption>();
                var Heromoves = new List<HeroAction>();
                foreach (var csild in pionode.Csildren)
                {
                    var freqs = (csild.ActivePlayer == PLAYER.IP && _actualspot.Herotype == HeroType.IP) ? csild.Range.Hands : csild.Range2.Hands;
                    var ind = pionode.Csildren.IndexOf(csild);
                    actualStrategy.Add(new StrategyOption() { Title = csild.ActionString, Frequencies = freqs, DisplayColor = StrategyOption.ColorList[ind] });
                    var _piostring = csild.Id.Split(new char[] { ':' }).Last() ;
                    Heromoves.Add(new HeroAction() { ActionString = csild.ActionString, Weight = csild.Range.Hands.Sum() / pionode.Range.Hands.Sum(), PioString = _piostring });
                }
                PlayVM.ActualStrategy = actualStrategy;
                if (_actualspot.Herotype == HeroType.OOP)
                {
                    PlayVM.HeroPosition = _actualspot.OOPPosition;
                    PlayVM.VillainPosition = _actualspot.IPPosition;
                    PlayVM.HeroName = _actualspot.OOPName;
                    PlayVM.VillainName = _actualspot.IPName;
                }
                else
                {
                    PlayVM.HeroPosition = _actualspot.IPPosition;
                    PlayVM.VillainPosition = _actualspot.OOPPosition;
                    PlayVM.HeroName = _actualspot.IPName;
                    PlayVM.VillainName = _actualspot.OOPName;
                }
                if (_actualspot.Herotype == HeroType.OOP && pionode.ActivePlayer == PLAYER.OOP)
                {                    
                    if (node == "r:0")
                    {
                        PlayVM.HeroCards = PIO.Hands1326[ind1];
                        PlayVM.siddenVillainCards = PIO.Hands1326[ind2];
                        PlayVM.siddenVillainCards = "";
                        PlayVM.VillainInitialRange = pionode.Range2.Hands;
                        PlayVM.HeroInitialRange = pionode.Range.Hands;
                        PlayVM.VillainAction = "";
                        PlayVM.HeroActions = null;
                    }
                    PlayVM.HeroActualRange = pionode.Range.Hands;
                    PlayVM.VillainActualRange = pionode.Range2.Hands;
                    PlayVM.HeroActions = Heromoves;
                }
                if (_actualspot.Herotype == HeroType.OOP && pionode.ActivePlayer == PLAYER.IP)
                {                    
                    if (node == "r:0")
                    {
                        PlayVM.HeroCards = PIO.Hands1326[ind2];
                        PlayVM.VillainCards = PIO.Hands1326[ind1];
                        PlayVM.VillainInitialRange = pionode.Range.Hands;
                        PlayVM.HeroInitialRange = pionode.Range2.Hands;
                    }
                    PlayVM.HeroActualRange = pionode.Range2.Hands;
                    PlayVM.VillainActualRange = pionode.Range.Hands;
                    var rind = Heromoves.Select(s => s.Weight).PickRandomWeightedElement(this.rnd);
                    PlayVM.VillainAction = Heromoves[rind].ActionString;
                    MakeMove(Heromoves[rind]);
                }               
            }
            catch (Exception ex)
            { Debug.WriteLine("ERROR IN DISPLAYSPOT" + "\r\n" + ex.Message); }
        }
        public void MakeMove(HeroAction action)
        {
            _actualnode += ":" + action.PioString;
            if (action.ActionString.Split(new char[] { ' '})[0]=="CALL")
            {
                if (PlayVM.BoardCards.Length==6)
                {

                }
                if (PlayVM.BoardCards.Length == 8)
                {

                }
            }
            DisplayNode(_actualnode);
        }
        public void GetNewTree()
        {
            try
            {
                _actualspot = SpotList[rnd.Next(SpotList.Count)];
                var trees = Directory.GetFiles(_actualspot.FolderPath, "*.cfr");
                var tree = trees[rnd.Next(trees.Count())];
                PIO.LoadTree(tree);
                _actualnode = "r:0:c:c:2c";
            }
            catch (Exception ex)
            { Debug.WriteLine("ERROR IN DISPLAYSPOT" + "\r\n" + ex.Message); }
        }
        public void GetNewhand()
        {
            GetNewTree();
            _actualnode = "r:0";
            DisplayNode(_actualnode);
            //_actualnode = "r:0:c:c:2c";
            //DisplayNode(_actualnode);

        }
        public void GetNewSpot()
        {
            GetNewhand();
        }
    }
}

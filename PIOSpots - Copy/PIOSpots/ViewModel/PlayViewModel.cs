using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PIOSpots.BusinessLogic;
using PIOSpots.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOSpots.ViewModel
{
    public class PlayViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string _HeroCards = String.Empty;
        public string HeroCards
        {
            get
            {
                return _HeroCards;
            }
            set
            {
                Set(ref _HeroCards, value);
                HeroCard1 = null;
                HeroCard2 = null;
                if (HeroCards.Length == 4)
                {
                    HeroCard1 = HeroCards[0].ToString() + HeroCards[1];
                    HeroCard2 = HeroCards[2].ToString() + HeroCards[3];
                }
            }
        }
        private string _villainCards = String.Empty;
        public string VillainCards
        {
            get
            {
                return _villainCards;
            }
            set
            {
                Set(ref _villainCards, value);                
                VillainCard1 = null;
                VillainCard2 = null;
                if (VillainCards.Length == 4)
                {
                    VillainCard1 = VillainCards[0].ToString() + VillainCards[1];
                    VillainCard2 = VillainCards[2].ToString() + VillainCards[3];
                }
                if (VillainCards=="")
                {
                    VillainCard1 = "_";
                    VillainCard2 = "_";
                }
            }
        }
        private string _boardCards;
        public string BoardCards
        {
            get { return _boardCards; }
            set {
                Set(ref _boardCards, value);
                BoardCard1 = null;
                BoardCard2 = null;
                BoardCard3 = null;
                BoardCard4 = null;
                BoardCard5 = null;
                if (_boardCards.Length > 0)
                {
                    BoardCard1 = _boardCards[0].ToString() + _boardCards[1];
                    BoardCard2 = _boardCards[2].ToString() + _boardCards[3];
                    BoardCard3 = _boardCards[4].ToString() + _boardCards[5];
                }
                if (_boardCards.Length > 6)
                {
                    BoardCard4 = _boardCards[6].ToString() + _boardCards[7];                    
                }
                if (_boardCards.Length > 8)
                {
                    BoardCard5 = _boardCards[8].ToString() + _boardCards[9];
                }
            }
        }
        private string _HeroCard1 = String.Empty;
        public string HeroCard1
        {
            get { return _HeroCard1; }
            set { Set(ref _HeroCard1, value); }
        }
        private string _HeroCard2 = String.Empty;
        public string HeroCard2
        {
            get { return _HeroCard2; }
            set { Set(ref _HeroCard2, value); }
        }
        private string _villainCard1 = String.Empty;
        public string VillainCard1
        {
            get { return _villainCard1; }
            set { Set(ref _villainCard1, value); }
        }
        private string _villainCard2 = String.Empty;
        public string VillainCard2
        {
            get { return _villainCard2; }
            set { Set(ref _villainCard2, value); }
        }
        private string _boardCard1 = String.Empty;
        public string BoardCard1
        {
            get { return _boardCard1; }
            set { Set(ref _boardCard1, value); }
        }
        private string _boardCard2 = String.Empty;
        public string BoardCard2
        {
            get { return _boardCard2; }
            set { Set(ref _boardCard2, value); }
        }
        private string _boardCard3 = String.Empty;
        public string BoardCard3
        {
            get { return _boardCard3; }
            set { Set(ref _boardCard3, value); }
        }
        private string _boardCard4 = String.Empty;
        public string BoardCard4
        {
            get { return _boardCard4; }
            set { Set(ref _boardCard4, value); }
        }
        private string _boardCard5 = String.Empty;
        public string BoardCard5
        {
            get { return _boardCard5; }
            set { Set(ref _boardCard5, value); }
        }
        private Spot _actualSpot = new Spot();
        public Spot ActualSpot
        {
            get
            {
                return _actualSpot;
            }
            set
            {
                Set(ref _actualSpot, value);
            }
        }
        private int _potSize = 0;
        public int PotSize
        {
            get
            {
                return _potSize;
            }
            set
            {
                Set(ref _potSize, value);
            }
        }
        private string _villainAction = String.Empty;
        public string VillainAction
        {
            get { return _villainAction; }
            set { Set(ref _villainAction, value); }
        }
        private List<HeroAction> _Heroactions = new List<HeroAction>();
        public List<HeroAction> HeroActions
        {
            get { return _Heroactions; }
            set { Set(ref _Heroactions, value); }
        }
        private string _HeroName = String.Empty;
        public string HeroName
        {
            get { return _HeroName; }
            set { Set(ref _HeroName, value); }
        }
        private string _villainName = String.Empty;
        public string VillainName
        {
            get { return _villainName; }
            set { Set(ref _villainName, value); }
        }
        private int _HeroStack = 0;
        public int HeroStack
        {
            get { return _HeroStack; }
            set { Set(ref _HeroStack, value); }
        }
        private int _villainStack = 0;
        public int VillainStack
        {
            get { return _villainStack; }
            set { Set(ref _villainStack, value); }
        }
        private string _HeroPosition = String.Empty;
        public string HeroPosition
        {
            get { return _HeroPosition; }
            set { Set(ref _HeroPosition, value); }
        }
        private string _villainPosition = String.Empty;
        public string VillainPosition
        {
            get { return _villainPosition; }
            set { Set(ref _villainPosition, value); }
        }
        private List<double> _HeroInitialRange = new List<double>();
        public List<double> HeroInitialRange
        {
            get { return _HeroInitialRange; }
            set { Set(ref _HeroInitialRange, value); }
        }
        private List<double> _villainInitialRange = new List<double>();
        public List<double> VillainInitialRange
        {
            get { return _villainInitialRange; }
            set { Set(ref _villainInitialRange, value); }
        }
        private List<double> _HeroActualRange = new List<double>();
        public List<double> HeroActualRange
        {
            get { return _HeroActualRange; }
            set { Set(ref _HeroActualRange, value); }
        }
        private List<double> _villainActualRange = new List<double>();
            public List<double> VillainActualRange
        {
            get { return _villainActualRange; }
            set { Set(ref _villainActualRange, value); }
        }
        private List<StrategyOption> _actualStrategy;
        public List<StrategyOption> ActualStrategy
        {
            get { return _actualStrategy; }
            set { Set(ref _actualStrategy, value); }
        }
        public string siddenVillainCards { get; set; }
        #endregion
        public SessionManager Sessionmanager { get; set; }
        public RelayCommand GetNewSpotCommand { get; set; }
        public RelayCommand<HeroAction> HeroMoveCommand { get; set; }
        
        public PlayViewModel()
        {            
            GetNewSpotCommand = new RelayCommand(
                () => Sessionmanager.GetNewSpot()
                );
            HeroMoveCommand = new RelayCommand<HeroAction>(m => Sessionmanager.MakeMove(m));
        }

    }
}

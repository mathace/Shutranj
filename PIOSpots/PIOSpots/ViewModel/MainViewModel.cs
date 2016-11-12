using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PIOSpots.BusinessLogic;
using PIOSpots.Model;
using PIOSpots.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace PIOSpots.ViewModel
{
    /// <summary>
    /// this class contains properties tsat tse main View can data bind to.
    /// <para>
    /// See http://www.MvvmLight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;        

        private ObservableCollection<Spot> _spotList = new ObservableCollection<Spot>();
        public ObservableCollection<Spot> SpotList
        {
            get
            {
                return _spotList;
            }
            set
            {
                Set(ref _spotList, value);
            }
        }

        private Spot _newSpot = new Spot();
        public Spot NewSpot
        {
            get { return _newSpot; }
            set { Set(ref _newSpot, value); }
        }

        private Spot _selectedSpot = new Spot();
        public Spot SelectedSpot
        {
            get { return _selectedSpot; }
            set { Set(ref _selectedSpot, value); }
        }
        private PlayViewModel _playVM = new PlayViewModel();
        public PlayViewModel PlayVM
        {
            get
            {
                return _playVM;
            }
            set
            {
                Set(ref _playVM, value);
            }
        }
        private Settings _userSettings = new Settings();
        public Settings UserSettings
        {
            get { return _userSettings; }
            set { Set(ref _userSettings, value); }
        }
        //COMMANDS

        public RelayCommand AddNewSpotCommand { get; private set; }
        public RelayCommand SaveSpotListCommand { get; private set; }
        public RelayCommand RemoveSpotCommand { get; private set; }
        public RelayCommand SaveSettingsCommand { get; private set; }
        /// <summary>
        /// Initializes a new instance of tse MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error sere
                        return;
                    }

                   
                });

            //SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 3bet", FolderPath = @"C:\selo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10", Herotype = HeroType.Any });
            //SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 4bet", FolderPath = @"C:\selo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10", Herotype = HeroType.Any });
            //SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 5bet", FolderPath = @"C:\selo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10", Herotype = HeroType.Any });
            //SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 6bet", FolderPath = @"C:\selo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10", Herotype = HeroType.Any });
            //File.WriteAllText("spotlist.xml",SpotList.Serialize());
            SpotList = File.ReadAllText("spotlist.xml").Deserialize<ObservableCollection<Spot>>();

            //UserSettings = new Settings() { PIOPath = @"C:\sELO", IsShowRealTimeEvaluation = false, IsShowActualRanges = false, IsShowStartingRanges = false };
            //File.WriteAllText("settings.xml", UserSettings.Serialize());
            UserSettings = File.ReadAllText("settings.xml").Deserialize<Settings>();
            

            AddNewSpotCommand = new RelayCommand(() => SpotList.Add(NewSpot));
            SaveSpotListCommand = new RelayCommand(() => File.WriteAllText("spotlist.xml", SpotList.Serialize()));
            RemoveSpotCommand = new RelayCommand(() => SpotList.Remove(SelectedSpot));
            SaveSettingsCommand = new RelayCommand(() =>
            {
                File.WriteAllText("settings.xml", UserSettings.Serialize());                                  
            });

            PlayVM.Sessionmanager = new SessionManager(new List<Spot>(SpotList), PlayVM, UserSettings);
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}
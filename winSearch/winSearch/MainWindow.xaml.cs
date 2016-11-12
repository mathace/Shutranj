using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace winSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<StrategyItem> StrategyList { get; set; }
        public List<StrategyItem> FilteredStrategyList { get; set; }
        public List<HandStrategy> CleanStrategy { get; set; }
        public List<string> KeyWords { get; set; }
        public List<List<StrategyItem>> SideStrategies { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            StrategyList = File.ReadAllText("catalog.xml").Deserialize<List<StrategyItem>>();
            var positions = new List<string> { "UTG", "MP", "CO", "BTN", "SB", "BB" };
            StrategyList = StrategyList.OrderBy(item => positions.IndexOf(item.Title.Split(' ')[0])).ToList();
            //KeyWords = new List<string>();
            //foreach (var s in StrategyList)
            //    foreach (var kw in s.Keywords)
            //        KeyWords.Add(kw);
            KeyWords = File.ReadAllLines("keywords.txt").ToList();
            //var txt = File.ReadAllText(StrategyList[0].FileName);
            foreach(var s in StrategyList)
            {
                s.Strat = new Strategy(s.FileName);
            }

            FilteredStrategyList = StrategyList.Where(i => true).ToList();
            rangeTitlesListBox.ItemsSource = FilteredStrategyList;
            keyWordsListBox.ItemsSource = KeyWords;

            SideStrategies = new List<List<StrategyItem>>();
            for (var i=0;i<5;i++)
            {
                var list = File.ReadAllText("sidecatalog" + (i + 1).ToString() + ".xml").Deserialize<List<StrategyItem>>();
                list = list.OrderBy(item => positions.IndexOf(item.Title.Split(' ')[0])).ToList();
                foreach(var s in list)
                {
                    s.Strat = new Strategy(s.FileName);
                }
                SideStrategies.Add(list);
            }
            sideStrategiesListBox.ItemsSource = SideStrategies;
        }

        private void rangeTitlesListBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void rangeTitlesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rangeTitlesListBox.SelectedItem == null) return;
            strategyListView.ItemsSource = (rangeTitlesListBox.SelectedItem as StrategyItem).Strat.HandStrategyList;
            optionsListView.ItemsSource = (rangeTitlesListBox.SelectedItem as StrategyItem).Strat.StrategyOptions;
            CleanStrategy = (rangeTitlesListBox.SelectedItem as StrategyItem).Strat.GetCleanStrategy();
            cleanStrategyListView.ItemsSource = CleanStrategy;
            percentagesListView.ItemsSource = (rangeTitlesListBox.SelectedItem as StrategyItem).Strat.GetSize().Select((size, index) => new { Freq = size, Action = (rangeTitlesListBox.SelectedItem as StrategyItem).Strat.StrategyOptions[index].Description }).ToList();
        }

        private void keyWordsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var addedItems = e.AddedItems;
            if (addedItems.Count < 1) return;
            var keyword = addedItems[0] as string;
            if (keyword == "") FilteredStrategyList = StrategyList;
            else FilteredStrategyList = StrategyList.Where(s => s.Keywords.Contains(keyword)).ToList();
            rangeTitlesListBox.ItemsSource = FilteredStrategyList;
            rangeTitlesListBox.InvalidateArrange();
            rangeTitlesListBox.UpdateLayout();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var addedItems = e.AddedItems;
            if (addedItems.Count < 1) return;
            var item = addedItems[0];
            strategyListView.ItemsSource = (item as StrategyItem).Strat.HandStrategyList;
            optionsListView.ItemsSource = (item as StrategyItem).Strat.StrategyOptions;
            CleanStrategy = (item as StrategyItem).Strat.GetCleanStrategy();
            cleanStrategyListView.ItemsSource = CleanStrategy;
            //var sizes = (item as StrategyItem).Strat.GetSize();
            //percentagesListView.ItemsSource = sizes.Select((size, index) => new { Freq = size, Action = (rangeTitlesListBox.SelectedItem as StrategyItem).Strat.StrategyOptions[index].Description }).ToList();
        }
    }
}

using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace iSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<RangeItem> items;
        public List<CatalogItem> catalog;
        public FileSystemWatcher fwatcher;
        public string Path { get; set; }
        public MainWindow()
        {            
            InitializeComponent();
            catalog = File.ReadAllText("catalog.xml").Deserialize<List<CatalogItem>>();
            //var watcher = new Logic.Watcher();
            fwatcher = new FileSystemWatcher();
            Path = @"C:\Users\Tom\Documents\PSHH\Kensix";
            items = new ObservableCollection<RangeItem>();
            
            items.Add(new RangeItem(25, Positions.SB, 3));
            items.Add(new RangeItem(25, Positions.SB, 3));
            items.Add(new RangeItem(25, Positions.SB, 3));
            items.Add(new RangeItem(25, Positions.SB, 3));
            items.Add(new RangeItem(25, Positions.SB, 3));
            items.Add(new RangeItem(25, Positions.SB, 3));


            displayListBox.ItemsSource = items;
            
        }
        public void StartWatcher(string path)
        {
            var dir = Directory.GetCurrentDirectory() + @"\images";
            var files = Directory.GetFiles(dir,"*.png").ToList();
            var paths = files.Select(x => System.IO.Path.GetFileName(x));
            var list = files.Select(y => Helper.GetCatalogItem(y)).Cast<CatalogItem>().ToList();
            File.WriteAllText("catalog.xml", list.Serialize());
            catalog = File.ReadAllText("catalog.xml").Deserialize<List<CatalogItem>>();
            this.UpdateTable(0, 25, 3, Positions.SB);
            this.UpdateTable(1, 25, 3, Positions.BB);
            this.UpdateTable(2, 25, 3, Positions.BTN);
            this.UpdateTable(3, 25, 3, Positions.BTN);
            fwatcher.Path = path;            
            fwatcher.Changed += watcher_Changed;
            fwatcher.EnableRaisingEvents = true;
        }
       

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (WatcherChangeTypes.Changed == e.ChangeType)
            {
                var filetext = File.ReadAllText(e.FullPath);
                var thisHH = filetext.Substring(filetext.LastIndexOf("PokerStars Hand"), filetext.Length - filetext.LastIndexOf("PokerStars Hand"));
                try
                {
                    var h = new Hand(thisHH);
                    //this.UpdateTable(h.RelatedTableID, Convert.ToInt32(h.TournamentID.Substring(0,4)), 5, Positions.BB);
                    this.UpdateTable(h.RelatedTableID, h.GetHeroBB(), h.GetPlayersCount(), h.GetHeroPosition());
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            sizexBox.Text = (Convert.ToInt32(sizexBox.Text) + e.Delta).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            items.RemoveAt(0);
            var i = items[0];
            i.Stack = 100;
            items.Insert(0, i);
        }
        private void UpdateTable(int index, int stack, int players, Positions position)
        {
            try
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    items.RemoveAt(index);
                    var ri = new RangeItem(stack, position, players);
                    ri.StrategyOptions.Clear();
                //ri.StrategyOptions = new ObservableCollection<StrategyOption>(catalog.Where(c => c.Players == players && c.Stack == stack && c.Position == position).Select(j => new StrategyOption() { Description = j.Description, ImagePath = j.ImagePath }));
                var mindifflist = catalog.Where(c => c.Players == players && c.Position == position).Select(o => Math.Abs(o.Stack - stack)).OrderBy(u => u);
                    var mindiff = mindifflist.Count() == 0 ? 0 : mindifflist.ElementAt(0);
                    ri.StrategyOptions = new ObservableCollection<StrategyOption>(catalog.Where(c => c.Players == players && c.Position == position && Math.Abs(c.Stack - stack) == mindiff).Select(j => new StrategyOption() { Description = j.Description, ImagePath = j.ImagePath }));
                    items.Insert(index, ri);
                });
            }
            catch(Exception ex)
            {

            }
        }

        private void pathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           Path = pathTextBox.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartWatcher(pathTextBox.Text);
        }
    }
}

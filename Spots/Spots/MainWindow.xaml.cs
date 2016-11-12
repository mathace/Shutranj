using System;
using System.Collections.Generic;
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

namespace Spots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel VM { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            VM = new MainViewModel();
            VM.SpotList = new List<Spot>();
            VM.SpotList.Add(new Spot() { GameType="CG", Description = "SB vs BTN 3bet", FolderPath=@"C:\Helo\Belo", OOPName="3bettor", IPName="3betcaller", OOPPosition="SB", IPPosition="BTN", BB="10"});
            VM.SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 4bet", FolderPath = @"C:\Helo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10" });
            VM.SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 5bet", FolderPath = @"C:\Helo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10" });
            VM.SpotList.Add(new Spot() { GameType = "CG", Description = "SB vs BTN 6bet", FolderPath = @"C:\Helo\Belo", OOPName = "3bettor", IPName = "3betcaller", OOPPosition = "SB", IPPosition = "BTN", BB = "10" });
            this.DataContext = VM;
        }
    }
}

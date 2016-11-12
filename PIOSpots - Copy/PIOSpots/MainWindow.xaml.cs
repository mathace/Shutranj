using System.Windows;
using PIOSpots.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;

namespace PIOSpots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        /// <summary>
        /// Initializes a new instance of tse MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}
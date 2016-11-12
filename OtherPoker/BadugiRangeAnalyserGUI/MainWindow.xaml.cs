using Poker;
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

namespace BadugiRangeAnalyserGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Range all;
        BadugiHand min;
        BadugiHand max;
        public MainWindow()
        {
            InitializeComponent();
            this.Init();
        }

        public List<Range> allranges = new List<Range>();
        public List<Range> childranges = new List<Range>();
        public Range parent;
       
        public void Init()
        {
            all = Range.GetAllHands();
            min = Range.MinHand;
            max = Range.MaxHand;
            allranges = Range.LoadRanges();

            allRangesListBox.ItemsSource = allranges;
        }
        private void button4_Copy_Click(object sender, RoutedEventArgs e)
        {
            Range.SaveRanges(allranges.Where(r => r.Saveable == true).ToList());
            Application.Current.Shutdown();
        }
        // Add range by bottom and top 
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var r = new Range(minHandTextBox.Text, maxHandTextBox.Text, rangeNameTextBox.Text);
            allranges.Add(r);
            //allRangesListBox.Items.Clear();
            allRangesListBox.ItemsSource = allranges;
            allRangesListBox.Items.Refresh();
            //allRangesListBox.InvalidateArrange();
            //allRangesListBox.UpdateLayout();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            var r = new Range(Convert.ToInt32(rangeTypeTextBox.Text), rangeCardsTextBox.Text);
            r.Name = rangeNameTextBox2.Text;
            allranges.Add(r);
            //allRangesListBox.Items.Clear();
            allRangesListBox.ItemsSource = allranges;
            allRangesListBox.Items.Refresh();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (allRangesListBox.SelectedItems.Count == 1)
            {
                var selectionindex = allRangesListBox.SelectedIndex;
                allranges.RemoveAt(selectionindex);
                allRangesListBox.ItemsSource = allranges;
                allRangesListBox.Items.Refresh();
            }

        }

        private void allRangesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          /*  if (allRangesListBox.SelectedItems.Count == 1)
            {
                var selectionindex = allRangesListBox.SelectedIndex;
                parent = allranges[selectionindex];
            }*/
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {            
                childranges.AddRange(allRangesListBox.SelectedItems.Cast<Range>());
                childrenRangesListbox.ItemsSource = childranges.Distinct();
                childrenRangesListbox.Items.Refresh();            
        }

        private void button3_Copy1_Click(object sender, RoutedEventArgs e)
        {
            childranges.Clear();
            childrenRangesListbox.ItemsSource = childranges.Distinct();
            childrenRangesListbox.Items.Refresh();
        }

        private void button3_Copy_Click(object sender, RoutedEventArgs e)
        {
            var sranges = allRangesListBox.SelectedItems.Cast<Range>().ToList();
            childranges.AddRange(sranges);            
            childrenRangesListbox.ItemsSource = childranges.Distinct();
            childrenRangesListbox.Items.Refresh();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var sranges = allRangesListBox.SelectedItems.Cast<Range>().ToList();
            parent = new Range(sranges);
            parentNameTextBox.Text = String.Concat(sranges.Select(sr => sr.Name + " "));
            parentCountTextBox.Text = parent.Hands.Count().ToString();
            percentageofalltextbox.Text = (Math.Round((decimal)parent.Hands.Count() / Range.AllHands.Hands.Count(), 4) * 100).ToString();
        }
        //GENERATE REPORT
        private void button4_Click(object sender, RoutedEventArgs e)
        {            
            var lines = "";
            foreach(var child in childranges)
            {
                //lines += child.Name + ": " + (Math.Round((decimal)child.Hands.Count(h => parent.Hands.Contains(h)) / parent.Hands.Count(), 4) * 100).ToString() + "%";
                lines += child.Name + ": " + Math.Round((decimal)child.Hands.Count() / parent.Hands.Count(), 4) * 100 + "%   " + child.Hands.Count().ToString();
                lines += Environment.NewLine;
            }
            reportTextBox.Text = lines;
        }

        private void askQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var qtype = Convert.ToInt32(questionTypeTextBox.Text);
            var qcards = questionCardsTextBox.Text;
            qcards = String.Concat(qcards.OrderBy(c => c));
            var answer = parent.Hands.Count(h => h.Category.CardsEval == qtype && h.Category.Cards == qcards);
            reportTextBox.Text = answer.ToString() + " hands, " + Math.Round((decimal)answer / parent.Hands.Count(), 4) * 100 + "%   ";
        }
    }
}

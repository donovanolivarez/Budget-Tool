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
using MahApps.Metro.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Budget_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string Input_Str;
        decimal Input_Value;
        decimal Savings;
        decimal Wants;
        decimal Needs;

        public MainWindow()
        {
            InitializeComponent();

            // set up series here, reference the name in the xaml
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)Savings) },
                    Title = "Savings",
                    DataLabels = true,
                    LabelPoint = point => "$" + point.Y
                },
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)Needs) },
                    Title = "Needs",
                    DataLabels = true,
                    LabelPoint = point => "$" + point.Y
                },
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)Wants) },
                    Title = "Wants",
                    DataLabels = true,
                    LabelPoint = point => "$" + point.Y
                },
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void CalculateButton_Click(object sender, RoutedEventArgs e) 
        {
            // gets the text from the currency box and then converts to a decimal.
            // Decimal appears to be the most accurate when working with money. 
            Input_Str = Monthly_Income.Text;
            Input_Value = decimal.Parse(Input_Str, System.Globalization.NumberStyles.Currency);
            decimal Rookie_Numbers = 100M;

            // maybe have a selector that changes the calculation based on how aggressive you want to be with you income? 
            CalculateBudget_503020();
            UpdateValues();
            Restart();
            // if less than 100 dollars, shouldn't even think about spending on anything else but needs.
            if (Input_Value < Rookie_Numbers)
            {
                MessageBox.Show("Go earn more money.");
            }
            else
            {
                // Calculate the amounts. 
                MessageBox.Show("You should save " + Savings.ToString("C", System.Globalization.CultureInfo.CurrentCulture) + " of your total income.\n"
                    + "You should spend " + Needs.ToString("C", System.Globalization.CultureInfo.CurrentCulture) + " on things you need.\n"
                    + "You should spend " + Wants.ToString("C", System.Globalization.CultureInfo.CurrentCulture) + " on things you want.");
            }
        }

        // Calculate based on 50/30/20 budegting rule. 
        private void CalculateBudget_503020()
        {
            Needs = Input_Value * 0.5M;
            Wants = Input_Value * 0.3M;
            Savings = Input_Value * 0.2M;
        }

        // TODO: get value to show as decimal instead of a double.
        private void UpdateValues()
        {
            foreach (var series in SeriesCollection)
            {
                if (series.Title.Equals("Savings"))
                {
                    foreach (var observable in series.Values.Cast<ObservableValue>())
                    {
                        observable.Value = (double)Savings;
                    }
                }

                if (series.Title.Equals("Needs"))
                {
                    foreach (var observable in series.Values.Cast<ObservableValue>())
                    {
                        observable.Value = (double)Needs;
                    }
                }

                if (series.Title.Equals("Wants"))
                {
                    foreach (var observable in series.Values.Cast<ObservableValue>())
                    {
                        observable.Value = (double)Wants;
                    }
                }
            }
        }

        private void Restart()
        {
            Chart.Update(true, true);
        }
    }
}

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CircuitSimulator.ViewModels;
using CircuitSimulator.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace CircuitSimulator
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : Window
    {
       
        public Output(ObservableCollection<Data> simulationDataCollection)
        {
            OutputChart outputChart = new OutputChart();
            InitializeComponent();
            
            DataTable.ItemsSource = simulationDataCollection;
        }
        
    }
}

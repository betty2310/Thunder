using System.Collections.ObjectModel;
using System.Windows;
using Thunder;

namespace CircuitSimulator
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : Window
    {

        public Output(ObservableCollection<Data> simulationDataCollection)
        {
            InitializeComponent();
            DataTable.ItemsSource = simulationDataCollection;
        }

        public Output()
        {
            InitializeComponent();
        }
        private void Click_Graph(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Data> dataCollection = App.Circuit.simulationDataCollection;
            GraphWindowTesst outputWindow = new GraphWindowTesst(dataCollection);
            outputWindow.ShowDialog();
            if (outputWindow.DialogResult == false)
            {
            }

        }

    }
}

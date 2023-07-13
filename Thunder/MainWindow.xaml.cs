using CircuitSimulator;
using CircuitSimulator.Views;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Thunder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<BaseComponentView> Components { get; set; }

        private bool _isSimulatorRunning = false;


        public MainWindow()
        {
            InitializeComponent();
            App.CircuitCanvas = CircuitCanvas;
            App.Circuit = new MainCircuit();

            Components = new ObservableCollection<BaseComponentView> {
                new VoltageAnalysisView{CP_color = "green", CP_name = "Voltage Analysis"},
                new ResistorView{CP_color = "red", CP_name = "Resistor"},
                new VoltageView{CP_color = "blue", CP_name = "DC Power"},
                new ACVoltageView{CP_color = "blue", CP_name = "AC Power"},
                new CapacitorView{CP_color = "yellow", CP_name = "Capacitor"},
                new GroundView{CP_name = "Ground", CP_color = "Gray"}
            };

            // Set the data context for the ListBox
            ComponentList.ItemsSource = Components;
        }
        private void Component_MouseMove(object sender, MouseEventArgs e)
        {
            nAnalysis.Text = App.voltageAnalysis.Count.ToString();
            ListBox listBox = sender as ListBox;
            BaseComponentView component = listBox.SelectedItem as BaseComponentView;
            if (component != null && e.LeftButton == MouseButtonState.Pressed)
            {
                string name = listBox.SelectedItem.GetType().Name;
                BaseComponentView newComponent = new BaseComponentView();
                switch (name)
                {
                    case "ResistorView":
                        newComponent = new ResistorView();
                        break;
                    case "VoltageView":
                        newComponent = new VoltageView();
                        break;
                    case "ACVoltageView":
                        newComponent = new ACVoltageView();
                        break;
                    case "GroundView":
                        newComponent = new GroundView();
                        break;
                    case "CapacitorView":
                        newComponent = new CapacitorView();
                        break;

                    case "VoltageAnalysisView":
                        newComponent = new VoltageAnalysisView();
                        break;
                }
                DragDrop.DoDragDrop(component, newComponent, DragDropEffects.Copy);
            }
        }


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point mousePosition = e.GetPosition(CircuitCanvas);
            lblCursorPosition.Text = $"{Convert.ToInt32(mousePosition.X)}-{Convert.ToInt32(mousePosition.Y)}";
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            BaseComponentView component = null;

            List<Type> componentTypeMap = new List<Type>
            {
                typeof(ResistorView),
                typeof(VoltageView),
                typeof(GroundView),
                typeof(CapacitorView),
                typeof(VoltageAnalysisView),
                typeof(ACVoltageView)
            };

            foreach (var entry in componentTypeMap)
            {
                if (e.Data.GetData(entry) is BaseComponentView view)
                {
                    component = view;
                    break;
                }
            }

            Point position = e.GetPosition(CircuitCanvas);
            Canvas.SetLeft(component, position.X);
            Canvas.SetTop(component, position.Y);
            if (!CircuitCanvas.Children.Contains(component))
            {
                CircuitCanvas.Children.Add(component);
            }
        }

        private void RunSimulator(object sender, RoutedEventArgs e)
        {
            if (_isSimulatorRunning)
            {
                // Stop the simulator
                StopSimulator();
            }
            else
            {
                // Start the simulator
                StartSimulator();
            }

        }
        private void StartSimulator()
        {
            // Update the UI and start the simulator
            _isSimulatorRunning = true;
            RunButton.ToolTip = "Click to stop simulator";
            textRunButton.Text = "Stop simulator";
            bar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ca5100"));
            IconRunButton.Kind = MaterialIconKind.CogPause;
            IconRunButton.Foreground = Brushes.DarkRed;

            // TODO: Start the simulation logic

            try
            {
                App.Circuit.run();
                string output = App.Circuit.SimulatorOutput.ToString();
                if(output == "Error occur!")
                {
                    MessageBox.Show("Error in circuit", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    StopSimulator();
                    return;
                }
                ObservableCollection<Data> dataCollection = App.Circuit.simulationDataCollection;

                Output outputWindow = new Output(dataCollection);
                outputWindow.ShowDialog();
                if (outputWindow.DialogResult == false)
                {
                    StopSimulator();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void StopSimulator()
        {
            // Update the UI and stop the simulator
            _isSimulatorRunning = false;
            RunButton.ToolTip = "Click to run simulator";
            textRunButton.Text = "Run simulator";
            bar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#006cbe"));
            IconRunButton.Kind = MaterialIconKind.CogPlay;
            IconRunButton.Foreground = Brushes.Green;
            // TODO: Stop the simulation logic

            App.Circuit.SimulatorOutput.Clear();
        }
    }
}

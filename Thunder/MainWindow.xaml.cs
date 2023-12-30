using CircuitSimulator;
using CircuitSimulator.Services;
using CircuitSimulator.Views;
using log4net;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        private double _zoom = 1.0;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CollectionView ComponentsCollectionView { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();

            App.logger = log;
            App.CircuitCanvas = CircuitCanvas;
            App.Circuit = new MainCircuit();

            App.TempLineService = new TempLineService(CircuitCanvas);


            Components = new ObservableCollection<BaseComponentView> {
                new VoltageAnalysisView{CP_color = "green", CP_name = "Voltage Analysis", Group=""},
                new ResistorView{CP_color = "red", CP_name = "Resistor", Group="Basic"},
                new CapacitorView{CP_color = "yellow", CP_name = "Capacitor", Group="Basic"},
                new InductorView{CP_color = "purple", CP_name = "Inductor", Group="Basic"},
                new VoltageView{CP_color = "blue", CP_name = "DC Power", Group="Sources"},
                new ACVoltageView{CP_color = "blue", CP_name = "AC Power", Group="Sources"},
                new GroundView{CP_name = "Ground", CP_color = "gray", Group="Sources"}
            };

            // Set the data context for the ListBox
            ComponentList.ItemsSource = Components;

            ComponentsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ComponentList.ItemsSource);
            var groupDescription = new PropertyGroupDescription("Group");
            ComponentsCollectionView.GroupDescriptions.Add(groupDescription);
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
                    case "InductorView":
                        newComponent = new InductorView();
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
            App.TempLineService.UpdateLine(mousePosition);
            lblCursorPosition.Text = $"{Convert.ToInt32(mousePosition.X)}-{Convert.ToInt32(mousePosition.Y)}";
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            BaseComponentView? component = null;

            List<Type> componentTypeMap = new()
            {
                typeof(ResistorView),
                typeof(VoltageView),
                typeof(GroundView),
                typeof(CapacitorView),
                typeof(VoltageAnalysisView),
                typeof(InductorView),
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
            int gridUnit = 20;
            double x = Math.Round(position.X / gridUnit) * gridUnit;
            double y = Math.Round(position.Y / gridUnit) * gridUnit;
            Canvas.SetLeft(component, x);
            Canvas.SetTop(component, y);
            log.Info(string.Format("Component {0} is dragged to position ({1}, {2})", component.Name, x, y));
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
                if (output == "Error occur!")
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

        private void root_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // Check if the wire is being drawn
                if (App.TempLineService.tempLine != null)
                {
                    // Remove the wire from the canvas
                    CircuitCanvas.Children.Remove(App.TempLineService.tempLine);
                    App.TempLineService.tempLine = null;

                    // Reset any other state related to drawing the wire
                    // ...
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComponentsCollectionView != null)
            {
                ComponentsCollectionView.Filter = (item) =>
                {
                    if (item is BaseComponentView component)
                    {
                        return component.CP_name.ToLower().Contains(SearchBox.Text.ToLower());
                    }
                    return false;
                };

            }
        }

        private void MenuItem_Options_Click(object sender, RoutedEventArgs e)
        {
            var optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
        }
    }
}

using CircuitSimulator;
using CircuitSimulator.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFHelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<IComponent> Components { get; set; }

        private bool _isSimulatorRunning = false;


        public MainWindow()
        {
            InitializeComponent();
            App.CircuitCanvas = CircuitCanvas;
            App.Circuit = new MainCircuit();

            Components = new ObservableCollection<IComponent> {
                new ResistorView{ CP_name = "Resistor", CP_color="Red"},
                new VoltageView{ CP_name = "Voltage", CP_color="Blue"},
                new GroundView { CP_name = "Ground", CP_color="gray"},
                new Test{CP_name = "Test", CP_color = "green"}
            };

            // Set the data context for the ListBox
            ComponentList.ItemsSource = Components;
        }
        private void Component_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            UserControl component = listBox.SelectedItem as UserControl;
            if (component != null && e.LeftButton == MouseButtonState.Pressed)
            {
                string name = listBox.SelectedItem.GetType().Name;
                UserControl newComponent = new UserControl();

                switch (name)
                {
                    case "ResistorView":
                        newComponent = new ResistorView();
                        break;
                    case "VoltageView":
                        newComponent = new VoltageView();
                        break;
                    case "GroundView":
                        newComponent = new GroundView();
                        break;
                    case "Test":
                        System.Diagnostics.Debug.WriteLine("Test component has been move!");
                        newComponent = new Test();
                        break;
                }

                DragDrop.DoDragDrop(component, newComponent, DragDropEffects.Move);
            }
        }


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point mousePosition = e.GetPosition(CircuitCanvas);
            lblCursorPosition.Text = $"Pos:({Convert.ToInt32(mousePosition.X)}-{Convert.ToInt32(mousePosition.Y)})";
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            UserControl component = new UserControl();
            object data = e.Data.GetData(typeof(ResistorView));
            if (data == null)
            {
                data = e.Data.GetData(typeof(VoltageView));
                if (data == null)
                {
                    data = e.Data.GetData(typeof(GroundView));
                    component = data as GroundView;
                }
                else
                {
                    component = data as VoltageView;
                }
            }
            else
            {
                component = data as ResistorView;
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
            RunButton.Content = new TextBlock() { Margin = new Thickness(3, 0, 0, 0), Text = "Stop simulator" };
            bar.Background = System.Windows.Media.Brushes.Red;
            // TODO: Start the simulation logic

            try
            {
                App.Circuit.run();
                string output = App.Circuit.SimulatorOutput.ToString();

                Output outputWindow = new Output(output);
                outputWindow.ShowDialog();

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
            RunButton.Content = new TextBlock() { Margin = new Thickness(3, 0, 0, 0), Text = "Run simulator" };
            bar.Background = System.Windows.Media.Brushes.Blue;
            // TODO: Stop the simulation logic

            App.Circuit.SimulatorOutput.Clear();
        }
    }
}

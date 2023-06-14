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

        private bool isSimulatorRunning = false;


        public MainWindow()
        {
            InitializeComponent();
            App.CircuitCanvas = CircuitCanvas;
            App.Circuit = new MainCircuit();

            Components = new ObservableCollection<IComponent> {
                new ResistorView{ CP_name = "Resistor", CP_color="Red"},
                new VoltageView{ CP_name = "Voltage", CP_color="Blue"},
                new GroundView { CP_name = "Ground", CP_color="gray"}
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

                if (name == "ResistorView")
                {
                    newComponent = new ResistorView();
                }
                else if (name == "VoltageView")
                {
                    newComponent = new VoltageView();
                }
                else if (name == "GroundView")
                {
                    newComponent = new GroundView();
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
            if (isSimulatorRunning)
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
            isSimulatorRunning = true;
            RunButton.ToolTip = "Click to stop simulator";
            RunButton.Content = new TextBlock() { Margin = new Thickness(3, 0, 0, 0), Text = "Stop simulator" };
            bar.Background = System.Windows.Media.Brushes.Red;
            // TODO: Start the simulation logic
        }

        private void StopSimulator()
        {
            // Update the UI and stop the simulator
            isSimulatorRunning = false;
            RunButton.ToolTip = "Click to run simulator";
            RunButton.Content = new TextBlock() { Margin = new Thickness(3, 0, 0, 0), Text = "Run simulator" };
            bar.Background = System.Windows.Media.Brushes.Blue;
            // TODO: Stop the simulation logic
        }
    }
}

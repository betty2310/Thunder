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

        // public static readonly DependencyProperty IsChildHitProperty = DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public MainWindow()
        {
            InitializeComponent();
            App.CircuitCanvas = CircuitCanvas;

            // Create example resistors
            //Components = new ObservableCollection<Component>
            //{
            //    new Resistor { Name = "Resistor", Value = 100, Image = new BitmapImage(new Uri("resistor1.png", UriKind.Relative)) },
            //    new Voltage { Name = "Voltage", Image = new BitmapImage(new Uri("voltage1.png", UriKind.Relative))},
            //    new Ground { Name = "Ground"}
            //};

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



        private void Canvas_Drop(object sender, DragEventArgs e)
        {

            //ResistorView component = ComponentList.SelectedItem as ResistorView;
            //component = new ResistorView();
            //Point position = e.GetPosition(CircuitCanvas);
            //Canvas.SetLeft(component, position.X);
            //Canvas.SetTop(component, position.Y);
            //CircuitCanvas.Children.Add(component);
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
            CircuitCanvas.Children.Add(component);

        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
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
            CircuitCanvas.Children.Remove(component);

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point mousePosition = e.GetPosition(CircuitCanvas);
            lblCursorPosition.Text = $"({Convert.ToInt32(mousePosition.X)}-{Convert.ToInt32(mousePosition.Y)})";
        }

        //private void Canvas_DragOver(object sender, DragEventArgs e)
        //{
        //    object data = e.Data.GetData(typeof(ResistorView));
        //    if (data != null)
        //    {
        //        ResistorView component = data as ResistorView;
        //        Point position = e.GetPosition(CircuitCanvas);
        //        Canvas.SetLeft(component, position.X);
        //        Canvas.SetTop(component, position.Y);
        //        CircuitCanvas.Children.Add(component);
        //    }
        //}
    }
}

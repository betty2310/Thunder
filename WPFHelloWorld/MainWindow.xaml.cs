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
        public ObservableCollection<ResistorView> Components { get; set; }

        // public static readonly DependencyProperty IsChildHitProperty = DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public MainWindow()
        {
            InitializeComponent();

            // Create example resistors
            //Components = new ObservableCollection<Component>
            //{
            //    new Resistor { Name = "Resistor", Value = 100, Image = new BitmapImage(new Uri("resistor1.png", UriKind.Relative)) },
            //    new Voltage { Name = "Voltage", Image = new BitmapImage(new Uri("voltage1.png", UriKind.Relative))},
            //    new Ground { Name = "Ground"}
            //};

            Components = new ObservableCollection<ResistorView> {
                new ResistorView{ CP_name = "resistor 1" },
                new ResistorView{ CP_name = "resistor 2"},
                new ResistorView{ CP_name = "resistor 3" },
            };

            // Set the data context for the ListBox
            ComponentList.ItemsSource = Components;
        }
        private void Component_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            ResistorView component = listBox.SelectedItem as ResistorView;
            if (component != null && e.LeftButton == MouseButtonState.Pressed)
            {
                ResistorView newComponent = new ResistorView();
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

            object data = e.Data.GetData(typeof(ResistorView));
            if (data != null)
            {
                ResistorView component = data as ResistorView;
                Point position = e.GetPosition(CircuitCanvas);
                Canvas.SetLeft(component, position.X);
                Canvas.SetTop(component, position.Y);
                CircuitCanvas.Children.Add(component);
            }
        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(ResistorView));
            if (data != null)
            {
                ResistorView component = data as ResistorView;
                CircuitCanvas.Children.Remove(component);
            }

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

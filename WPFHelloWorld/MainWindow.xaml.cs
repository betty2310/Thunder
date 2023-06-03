using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFHelloWorld.Models;

namespace WPFHelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Component> Components { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Create example resistors
            Components = new ObservableCollection<Component>
        {
            new Resistor { Name = "Resistor", Value = 100, Image = new BitmapImage(new Uri("resistor1.png", UriKind.Relative)) },
            new Voltage { Name = "Voltage", Image = new BitmapImage(new Uri("voltage1.png", UriKind.Relative))},
            new Ground { Name = "Ground"}
            };

            // Set the data context for the ListBox
            // Set the data context for the ListBox
            ComponentList.ItemsSource = Components;
        }
        private void Component_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            Component component = listBox.SelectedItem as Component;
            if (component != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(ComponentList, ComponentList, DragDropEffects.Copy);
            }
        }



        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = 30;
            rectangle.Height = 30;
            rectangle.Fill = new SolidColorBrush(Colors.Red);
            string component = ComponentList.SelectedItem.ToString();
            switch (component)
            {
                case "resistor":
                    rectangle.Fill = new SolidColorBrush(Colors.Red);
                    break;
                case "voltage":
                    rectangle.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case "ground":
                    rectangle.Fill = new SolidColorBrush(Colors.Green);
                    break;
                default:
                    break;
            }

            Point position = e.GetPosition(CircuitCanvas);
            Canvas.SetLeft(rectangle, position.X);
            Canvas.SetTop(rectangle, position.Y);
            CircuitCanvas.Children.Add(rectangle);

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point mousePosition = e.GetPosition(CircuitCanvas);
            lblCursorPosition.Text = $"({Convert.ToInt32(mousePosition.X)}-{Convert.ToInt32(mousePosition.Y)})";
        }


    }
}

using System.Windows.Controls;

namespace Thunder.Views
{
    /// <summary>
    /// Interaction logic for CanvasView.xaml
    /// </summary>
    public partial class CanvasView : UserControl
    {
        public CanvasView()
        {
            InitializeComponent();
        }

        //private void Canvas_Drop(object sender, DragEventArgs e)
        //{
        //    Rectangle rectangle = new Rectangle();
        //    rectangle.Width = 30;
        //    rectangle.Height = 30;
        //    rectangle.Fill = new SolidColorBrush(Colors.Red);
        //    string component = ComponentList.SelectedItem.ToString();
        //    switch (component)
        //    {
        //        case "resistor":
        //            rectangle.Fill = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "voltage":
        //            rectangle.Fill = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "ground":
        //            rectangle.Fill = new SolidColorBrush(Colors.Green);
        //            break;
        //        default:
        //            break;
        //    }

        //    Point position = e.GetPosition(CircuitCanvas);
        //    Canvas.SetLeft(rectangle, position.X);
        //    Canvas.SetTop(rectangle, position.Y);
        //    CircuitCanvas.Children.Add(rectangle);

        //}

        //private void Canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    System.Windows.Point mousePosition = e.GetPosition(canvas);
        //    lblCursorPosition.Text = $"({Convert.ToInt32(mousePosition.X)}-{Convert.ToInt32(mousePosition.Y)})";
        //}

    }
}

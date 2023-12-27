using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Thunder;

namespace CircuitSimulator.Models
{
    public class Conductor : IConductor
    {
        public IComponent StartComponent { get; set; }
        public IComponent EndComponent { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public event EventHandler OnConductorConnected;


        public Polyline polyline;

        public void Draw(Canvas canvas)
        {
            if (polyline != null)
            {
                canvas.Children.Remove(polyline);
            }
            // Calculate the intermediate point
            double midX = (X1 + X2) / 2;
            double midY = (Y1 + Y2) / 2;

            // Create a Polyline
            polyline = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Add points to the Polyline
            polyline.Points.Add(new Point(X1, Y1));
            polyline.Points.Add(new Point(X1, Y1)); // Horizontal to midpoint
            polyline.Points.Add(new Point(X1, Y2)); // Vertical to end point
            polyline.Points.Add(new Point(X2, Y2));

            // Add the Polyline to the canvas
            canvas.Children.Add(polyline);
            App.Conductors.Add(this);

        }

        public void Connect()
        {
            App.Circuit.AddConductor(this);

            StartComponent.OnMoved += Component_OnMoved;
            EndComponent.OnMoved += Component_OnMoved;

            OnConductorConnected?.Invoke(this, EventArgs.Empty);
            Draw(App.CircuitCanvas);
        }

        public void Disconnect()
        {
            StartComponent.OnMoved -= Component_OnMoved;
            EndComponent.OnMoved -= Component_OnMoved;
        }


        private void Component_OnMoved(object sender, EventArgs e)
        {
            var component = sender as IComponent;

            if (component != null)
            {
                Point point = GetPositionOnCanvas(component as FrameworkElement, App.CircuitCanvas);
                // Todo: fix negative and postitive position
                if (component == StartComponent)
                {
                    X1 = point.X;
                    Y1 = point.Y;
                }
                else
                {
                    X2 = point.X;
                    Y2 = point.Y;
                }

                Draw(App.CircuitCanvas);

            }

        }

        private Point GetPositionOnCanvas(FrameworkElement element, Canvas parentCanvas)
        {
            var transform = element.TransformToAncestor(parentCanvas);
            Point position = transform.Transform(new Point(0, 0));
            return position;
        }
    }

}

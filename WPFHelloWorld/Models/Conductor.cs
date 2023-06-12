using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using WPFHelloWorld;

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
        public void Draw(Canvas canvas)
        {
            Line line = new Line
            {
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                Stroke = System.Windows.Media.Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);
        }

        public void Connect()
        {

            StartComponent.OnMoved += Component_OnMoved;
            EndComponent.OnMoved += Component_OnMoved;

            Draw(App.CircuitCanvas);
        }

        public void Connect(IComponent start, IComponent end)
        {
            StartComponent = start;
            EndComponent = end;

            StartComponent.OnMoved += Component_OnMoved;
            EndComponent.OnMoved += Component_OnMoved;

            Draw(App.CircuitCanvas);
        }

        private void Component_OnMoved(object sender, EventArgs e)
        {
            Draw(App.CircuitCanvas);
        }
    }

}

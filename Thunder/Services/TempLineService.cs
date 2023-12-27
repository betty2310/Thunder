using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CircuitSimulator.Services
{
    public class TempLineService
    {
        private Line _line;
        private Canvas _canvas;

        public Line? tempLine
        {
            get => _line;
            set => _line = value;
        }

        public TempLineService(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void StartLine(Point startPosition)
        {
            if (tempLine == null)
            {
                tempLine = new Line
                {
                    X1 = startPosition.X + 5,
                    Y1 = startPosition.Y,
                    X2 = startPosition.X,
                    Y2 = startPosition.Y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2,
                    IsHitTestVisible = false
                };

                _canvas.Children.Add(tempLine);
            }
        }

        public void UpdateLine(Point currentPosition)
        {
            if (tempLine != null)
            {
                tempLine.X2 = currentPosition.X;
                tempLine.Y2 = currentPosition.Y;
            }
        }

        public void FinalizeLine()
        {
            if (tempLine != null)
            {
                _canvas.Children.Remove(tempLine);
                tempLine = null;
            }
        }
    }
}

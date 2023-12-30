using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CircuitSimulator.Components
{
    /// <summary>
    /// Interaction logic for CircuitCanvas.xaml
    /// </summary>
    public partial class CircuitCanvas : Canvas
    {
        private readonly MatrixTransform _transform = new MatrixTransform();
        private Point _initialMousePosition;

        private bool _dragging;
        private UIElement _selectedElement;
        private Vector _draggingDelta;

        private Color _lineColor = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);
        private Color _backgroundColor = Color.FromArgb(0xFF, 0x33, 0x33, 0x33);
        private List<Line> _gridLines = new List<Line>();


        public CircuitCanvas()
        {
            InitializeComponent();
            //MouseDown += PanAndZoomCanvas_MouseDown;
            //MouseUp += PanAndZoomCanvas_MouseUp;
            //MouseMove += PanAndZoomCanvas_MouseMove;
            //MouseWheel += PanAndZoomCanvas_MouseWheel;

            BackgroundColor = _backgroundColor;
            // Dot properties
            int dotSize = 2; // Size of the dots

            Properties.Settings.Default.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "use_dot")
                {
                    Debug.WriteLine("use_dot changed");
                    //SetGridVisibility(Properties.Settings.Default.use_dot ? Visibility.Visible : Visibility.Hidden);
                }
            };

            var dotBrush = new SolidColorBrush(_lineColor);

            // Create dots instead of lines
            for (int x = 0; x <= 700; x += 20)
            {
                for (int y = 0; y <= 600; y += 20)
                {

                    Ellipse dot = new()
                    {
                        Width = dotSize,
                        Height = dotSize,
                        Fill = dotBrush
                    };

                    // Position the dot
                    Canvas.SetLeft(dot, x - dotSize / 2);
                    Canvas.SetTop(dot, y - dotSize / 2);

                    Children.Add(dot);
                }

            }

        }

        public float Zoomfactor { get; set; } = 1.1f;

        public Color LineColor
        {
            get { return _lineColor; }

            set
            {
                _lineColor = value;

                foreach (Line line in _gridLines)
                {
                    line.Stroke = new SolidColorBrush(_lineColor);
                }
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }

            set
            {
                _backgroundColor = value;
                Background = new SolidColorBrush(_backgroundColor);
            }
        }

        public void SetGridVisibility(Visibility value)
        {
            foreach (Line line in _gridLines)
            {
                line.Visibility = value;
            }
        }

        private void PanAndZoomCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _initialMousePosition = _transform.Inverse.Transform(e.GetPosition(this));
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.Children.Contains((UIElement)e.Source))
                {
                    _selectedElement = (UIElement)e.Source;
                    Point mousePosition = Mouse.GetPosition(this);
                    double x = Canvas.GetLeft(_selectedElement);
                    double y = Canvas.GetTop(_selectedElement);
                    Point elementPosition = new Point(x, y);
                    _draggingDelta = elementPosition - mousePosition;
                }
                _dragging = true;
            }
        }

        private void PanAndZoomCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
            _selectedElement = null;
        }

        private void PanAndZoomCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Point mousePosition = _transform.Inverse.Transform(e.GetPosition(this));
                Vector delta = Point.Subtract(mousePosition, _initialMousePosition);
                var translate = new TranslateTransform(delta.X, delta.Y);
                _transform.Matrix = translate.Value * _transform.Matrix;

                foreach (UIElement child in this.Children)
                {
                    child.RenderTransform = _transform;
                }
            }

            if (_dragging && e.LeftButton == MouseButtonState.Pressed)
            {
                double x = Mouse.GetPosition(this).X;
                double y = Mouse.GetPosition(this).Y;

                if (_selectedElement != null)
                {
                    Canvas.SetLeft(_selectedElement, x + _draggingDelta.X);
                    Canvas.SetTop(_selectedElement, y + _draggingDelta.Y);
                }
            }
        }

        private void PanAndZoomCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            float scaleFactor = Zoomfactor;
            if (e.Delta < 0) // zoom out
            {
                scaleFactor = 1f / scaleFactor;
            }

            Point mousePostion = e.GetPosition(this);

            Matrix scaleMatrix = _transform.Matrix;
            scaleMatrix.ScaleAt(scaleFactor, scaleFactor, mousePostion.X, mousePostion.Y);
            _transform.Matrix = scaleMatrix;

            foreach (UIElement child in this.Children)
            {
                double x = Canvas.GetLeft(child);
                double y = Canvas.GetTop(child);

                double sx = x * scaleFactor;
                double sy = y * scaleFactor;

                Canvas.SetLeft(child, sx);
                Canvas.SetTop(child, sy);

                child.RenderTransform = _transform;
            }
        }
    }
}

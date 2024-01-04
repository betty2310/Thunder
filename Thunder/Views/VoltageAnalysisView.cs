using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CircuitSimulator.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Thunder;

namespace CircuitSimulator.Views
{
    public class VoltageAnalysisView : BaseComponentView
    {
        private DispatcherTimer _displayTimer;
        private int _currentIndex = 0;
        private List<string> _voltages;
        public string sourceComponent { set; get; }

        private bool _isSimulationRunning;


        public VoltageAnalysisView()
        {
            Value.Visibility = System.Windows.Visibility.Hidden;
            Top.Visibility = System.Windows.Visibility.Hidden;
            Bot.Visibility = System.Windows.Visibility.Hidden;
            Node.Visibility = System.Windows.Visibility.Hidden;
            Rectangle.Width = 25;
            Rectangle.Height = 20;
            var output = new TextBlock();
            Canvas.SetLeft(output, (Rectangle.Width / 2) - (output.ActualWidth / 2) - 18);
            Canvas.SetTop(output, (Rectangle.Height / 2) - (output.ActualHeight / 2) - 15);
            output.FontSize = 10;
            output.FontWeight = FontWeights.Bold;
            Canvas.Children.Add(output);

            _isSimulationRunning = false;
            App.Circuit.SimulationCompleted += (sender, e) =>
            {
                _isSimulationRunning = true;
                var voltages = App.Circuit.simulationDataCollection;
                if (voltages.Count < 1) return;
                _voltages = new List<string>();
                foreach (var voltage in voltages)
                {
                    var value = voltage.OutputValue.ToString("G4");
                    _voltages.Add(value);
                }
                output.Foreground = Brushes.Black;
                LoopVoltages(output);

                var conductors = (sender as MainCircuit)?.Conductors;
                foreach (var conductor in conductors)
                {
                    conductor.polyline.Stroke = Brushes.Red;
                }
            };
            App.Circuit.StopSimulation += (sender, e) =>
            {
                _isSimulationRunning = false;
                output.Text = "";
                var conductors = (sender as MainCircuit)?.Conductors;
                foreach (var conductor in conductors)
                {
                    conductor.polyline.Stroke = Brushes.Black;
                }
            };
        }

        private async Task LoopVoltages(TextBlock t)
        {
            while (_isSimulationRunning)
            {
                for (var i = 0; i < _voltages.Count; i++)
                {
                    t.Text = $"{_voltages[i]}V";
                    await Task.Delay(500);

                    if (!_isSimulationRunning)
                    {
                        return;
                    }
                }
            }
        }

        public override void Component_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }

            bool isOverConductor = false;
            System.Windows.Point mousePosition = e.GetPosition(App.CircuitCanvas);
            mousePosition.X += 10;
            mousePosition.Y += 10;
            foreach (Conductor conductor in App.Conductors)
            {
                VisualTreeHelper.HitTest(conductor.polyline, null, new HitTestResultCallback((result) =>
                {
                    if (result.VisualHit == conductor.polyline)
                    {
                        isOverConductor = true;
                        Rectangle.Fill = System.Windows.Media.Brushes.Green;
                        sourceComponent = conductor.EndComponent.Name;
                        if (App.voltageAnalysis.Contains(this) == false)
                        {
                            App.voltageAnalysis.Add(this);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(conductor.EndComponent.Name);
                            App.voltageAnalysis.Find(va => va == this).sourceComponent = conductor.EndComponent.Name;
                        }
                    }

                    return HitTestResultBehavior.Stop;
                }), new PointHitTestParameters(mousePosition));
            }

            if (!isOverConductor)
            {
                //Rectangle.Fill = System.Windows.Media.Brushes.Gray;

                //App.voltageAnalysis.RemoveAll(va => va == this);
            }
            else
            {
            }
        }
    }
}
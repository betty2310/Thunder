using CircuitSimulator.Models;
using SpiceSharp.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Thunder;
using SpiceSharp;

namespace CircuitSimulator.Views
{
    public class VoltageAnalysisView : BaseComponentView
    {
        public string sourceComponent { set; get; }

        public VoltageAnalysisView()
        {

            Value.Visibility = System.Windows.Visibility.Hidden;
            Top.Visibility = System.Windows.Visibility.Hidden;
            Bot.Visibility = System.Windows.Visibility.Hidden;

            Rectangle.Width = 25;
            Rectangle.Height = 25;

            //Rectangle.Visibility = System.Windows.Visibility.Hidden;
            //Ellipse ellipse = new Ellipse();
            //ellipse.Width = 20;
            //ellipse.Height = 20;
            //ellipse.Fill = System.Windows.Media.Brushes.Green;
            //Canvas.Children.Add(ellipse);
            //Canvas.SetLeft(ellipse, 0);
            //Canvas.SetTop(ellipse, 0);
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
                VisualTreeHelper.HitTest(conductor.line, null, new HitTestResultCallback((result) =>
                {
                    if (result.VisualHit == conductor.line)
                    {
                        isOverConductor = true;
                        Rectangle.Fill = System.Windows.Media.Brushes.Green;
                        sourceComponent = conductor.EndComponent.Name;
                        if(App.voltageAnalysis.Contains(this) == false)
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
            if(!isOverConductor)
            {
                //Rectangle.Fill = System.Windows.Media.Brushes.Gray;

                //App.voltageAnalysis.RemoveAll(va => va == this);
                
            } else
            {
               
            }
        }
    }
}

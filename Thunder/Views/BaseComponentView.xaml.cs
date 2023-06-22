using CircuitSimulator.Models;
using SpiceSharp.Components;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Thunder;
using IComponent = Thunder.IComponent;
using Point = System.Windows.Point;

namespace CircuitSimulator.Views
{
    /// <summary>
    /// Interaction logic for BaseComponentView.xaml
    /// </summary>
    public partial class BaseComponentView : UserControl, IComponent
    {
        public string CP_name { get; set; }
        public string CP_color { get; set; }

        public new string Name { get; set; }

        public Component SpiceComponent { get; set; }


        public ComponentType componentType { get; set; }
        public event EventHandler OnMoved;
        public Dictionary<Ellipse, IConductor> conductors = new Dictionary<Ellipse, IConductor>();


        public BaseComponentView()
        {
            InitializeComponent();
        }

        protected void MoveTopEllipse(double deltaX, double deltaY)
        {
            double currentX = Canvas.GetLeft(Top);
            double currentY = Canvas.GetTop(Top);

            Canvas.SetLeft(Top, currentX + deltaX);
            Canvas.SetTop(Top, currentY + deltaY);
        }


        private void Component_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                OnMoved?.Invoke(this, EventArgs.Empty);
            }
        }

        private Point GetPositionOnCanvas(FrameworkElement element, Canvas parentCanvas)
        {
            var transform = element.TransformToAncestor(parentCanvas);
            Point position = transform.Transform(new Point(0, 0));
            return position;
        }

        private void EllipsePos_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;

            Point position = GetPositionOnCanvas(ellipse, App.CircuitCanvas);

            if (conductors.ContainsKey(ellipse) && conductors[ellipse] != null)
            {
                return;
            }
            if (App.CurrentConductor == null)
            {
                App.CurrentConductor = new Conductor();
                App.CurrentConductor.StartComponent = this;
                App.CurrentConductor.X1 = position.X + 5;
                App.CurrentConductor.Y1 = position.Y + 5;

                conductors.Add(ellipse, App.CurrentConductor);
            }
            else
            {
                App.CurrentConductor.EndComponent = this;
                App.CurrentConductor.X2 = position.X + 5;
                App.CurrentConductor.Y2 = position.Y + 5;

                App.CurrentConductor.Connect();
                conductors.Add(ellipse, App.CurrentConductor);

                App.CurrentConductor = null;


            }
        }
        private void EllipseNeg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;

            Point position = GetPositionOnCanvas(ellipse, App.CircuitCanvas);

            if (conductors.ContainsKey(ellipse) && conductors[ellipse] != null)
            {
                return;
            }
            if (App.CurrentConductor == null)
            {
                App.CurrentConductor = new Conductor();
                App.CurrentConductor.StartComponent = this;
                App.CurrentConductor.X1 = position.X;
                App.CurrentConductor.Y1 = position.Y;

                conductors.Add(ellipse, App.CurrentConductor);
            }
            else
            {
                App.CurrentConductor.EndComponent = this;
                App.CurrentConductor.X2 = position.X;
                App.CurrentConductor.Y2 = position.Y;


                App.CurrentConductor.Connect();
                conductors.Add(ellipse, App.CurrentConductor);

                App.CurrentConductor = null;

            }
        }



        private void Component_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == App.CircuitCanvas)
            {
                App.CircuitCanvas.Children.Remove(this);
            }
        }

        private void Component_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void Value_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Set focus to the TextBox and select all text
            Value.Focus();
            Value.SelectAll();
        }

        public virtual void Value_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        public virtual void Value_KeyDown(object sender, KeyEventArgs e)
        {

        }



    }
}

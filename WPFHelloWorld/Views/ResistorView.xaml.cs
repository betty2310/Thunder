﻿using CircuitSimulator.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using WPFHelloWorld;

namespace CircuitSimulator.Views
{
    /// <summary>
    /// Interaction logic for ResistorView.xaml
    /// </summary>
    public partial class ResistorView : UserControl, IComponent
    {

        public string CP_name { get; set; }
        public string CP_color { get; set; }

        public event EventHandler OnMoved;

        public Dictionary<Ellipse, IConductor> conductors = new Dictionary<Ellipse, IConductor>();

        public ResistorView()
        {
            InitializeComponent();
        }


        private void Resistor_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                OnMoved?.Invoke(this, new EventArgs());
            }
        }

        private Point GetPositionOnCanvas(FrameworkElement element, Canvas parentCanvas)
        {
            var transform = element.TransformToAncestor(parentCanvas);
            Point position = transform.Transform(new Point(0, 0));
            return position;
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Resistor_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == App.CircuitCanvas)
            {
                App.CircuitCanvas.Children.Remove(this);
            }
        }
    }
}

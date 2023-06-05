﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public ResistorView()
        {
            InitializeComponent();
        }


        private void Resistor_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }

    }
}

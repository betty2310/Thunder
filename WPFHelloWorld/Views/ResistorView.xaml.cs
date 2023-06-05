using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CircuitSimulator.Views
{
    /// <summary>
    /// Interaction logic for ResistorView.xaml
    /// </summary>
    public partial class ResistorView : UserControl
    {
        public string CP_name { get; set; }

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

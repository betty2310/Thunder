using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFHelloWorld;

namespace CircuitSimulator.Views
{
    /// <summary>
    /// Interaction logic for VoltageView.xaml
    /// </summary>
    public partial class VoltageView : UserControl, IComponent
    {
        public string CP_name { get; set; }
        public string CP_color { get; set; }

        public VoltageView()
        {
            InitializeComponent();
        }

        private void Voltage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }

    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CircuitSimulator.Views
{
    /// <summary>
    /// Interaction logic for Ground.xaml
    /// </summary>
    public partial class GroundView : UserControl
    {
        public string CP_name { get; set; }
        public string CP_color { get; set; }


        public GroundView()
        {
            InitializeComponent();
        }


        private void Ground_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }
    }
}

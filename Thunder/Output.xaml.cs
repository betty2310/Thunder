using System.Windows;
using System.Windows.Controls;

namespace CircuitSimulator
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : Window
    {
        public Output()
        {
            InitializeComponent();
        }
        public Output(string output)
        {
            InitializeComponent();
            TextBlock.Text = output;
        }
    }
}

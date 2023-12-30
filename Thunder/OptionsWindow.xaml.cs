using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Thunder;

namespace CircuitSimulator
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private bool _showGrid = true;

        public bool ShowGrid
        {
            get => _showGrid;
            set
            {
                _showGrid = value;
                Properties.Settings.Default.use_dot = value;
                Properties.Settings.Default.Save();
            }
        }

        public OptionsWindow()
        {
            InitializeComponent();
            ChkShowGrid.IsChecked = Properties.Settings.Default.use_dot;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid = ChkShowGrid.IsChecked ?? false;
            Close();
        }
    }
}

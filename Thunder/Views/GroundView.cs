using CircuitSimulator.Models;

namespace CircuitSimulator.Views
{
    public class GroundView : BaseComponentView
    {
        public GroundView()
        {
            Rectangle.Fill = System.Windows.Media.Brushes.Gray;
            componentType = ComponentType.Ground;
            Bot.Visibility = System.Windows.Visibility.Hidden;
            Rectangle.Width = 40;
            Rectangle.Height = 15;

            Name = "0";
        }
    }
}

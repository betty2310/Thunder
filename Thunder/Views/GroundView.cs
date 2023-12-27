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
            Value.Visibility = System.Windows.Visibility.Hidden;
            Rectangle.Width = 20;
            Rectangle.Height = 10;

            Name = "0";
        }
    }
}

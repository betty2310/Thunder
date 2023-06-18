using CircuitSimulator.Models;

namespace CircuitSimulator.Views
{
    public class VoltageView : BaseComponentView
    {
        public static int counter = 0;

        public VoltageView()
        {
            componentType = ComponentType.Voltage;
            Rectangle.Fill = System.Windows.Media.Brushes.Blue;
            Name = $"V{counter++}";
        }
    }
}

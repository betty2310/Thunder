using CircuitSimulator.Models;

namespace CircuitSimulator.Views
{
    public class ResistorView : BaseComponentView
    {
        public static int counter = 0;

        public ResistorView()
        {
            componentType = ComponentType.Resistor;
            Rectangle.Fill = System.Windows.Media.Brushes.Red;
            Name = $"R{counter++}";
        }
    }
}

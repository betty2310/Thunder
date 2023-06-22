using CircuitSimulator.Models;
using SpiceSharp.Components;
using System.Globalization;

namespace CircuitSimulator.Views
{
    public class VoltageView : BaseComponentView
    {
        private static int _counter = 0;

        public VoltageView()
        {
            componentType = ComponentType.Voltage;
            Rectangle.Fill = System.Windows.Media.Brushes.Blue;
            Name = $"V{_counter++}";
            SpiceComponent = new VoltageSource("default", "", "", 1.0);
            var r = SpiceComponent as VoltageSource;
            var p = r?.Parameters;
            Value.Text = p.DcValue.Value.ToString(CultureInfo.InvariantCulture) + "V";
            Node.Text = Name;
        }
    }
}

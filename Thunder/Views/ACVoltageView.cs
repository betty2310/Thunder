using CircuitSimulator.Models;
using SpiceSharp.Components;
using System.Globalization;

namespace CircuitSimulator.Views
{
    public class ACVoltageView : BaseComponentView
    {
        private static int _counter = 0;

        public ACVoltageView()
        {
            componentType = ComponentType.Voltage;
            Rectangle.Fill = System.Windows.Media.Brushes.Blue;
            Name = $"AC {_counter++}";
            SpiceComponent = (Component)new VoltageSource("default", "", "", 0.0).SetParameter("acmag",1.0);
            var r = SpiceComponent as VoltageSource;
            var p = r?.Parameters;
            //Value.Text = p.DcValue.Value.ToString(CultureInfo.InvariantCulture) + "V";
            Node.Text = Name;
        }
    }
}

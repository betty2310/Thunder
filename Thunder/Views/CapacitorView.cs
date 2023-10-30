namespace CircuitSimulator.Views
{
    public class CapacitorView : BaseComponentView
    {
        private static int _counter = 0;
        public CapacitorView()
        {
            componentType = Models.ComponentType.Capacitor;
            Rectangle.Fill = System.Windows.Media.Brushes.Yellow;
            Name = $"C{_counter++}";
            SpiceComponent = new SpiceSharp.Components.Capacitor("default", "", "", 1.0e-6);
            var r = SpiceComponent as SpiceSharp.Components.Capacitor;
            var p = r?.Parameters;
            Value.Text = p?.Capacitance.Value.ToString();
            Node.Text = Name;
        }
    }
}

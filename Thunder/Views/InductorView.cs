namespace CircuitSimulator.Views
{
    public class InductorView : BaseComponentView
    {
        private static int _counter = 0;
        public InductorView()
        {
            componentType = Models.ComponentType.Inductor;
            Rectangle.Fill = System.Windows.Media.Brushes.Purple;
            Name = $"I{_counter++}";
            SpiceComponent = new SpiceSharp.Components.Inductor("default", "", "", 1.0e-6);
            var r = SpiceComponent as SpiceSharp.Components.Inductor;
            var p = r?.Parameters;
            Value.Text = p?.Inductance.ToString();
            Node.Text = Name;
        }
    }
}

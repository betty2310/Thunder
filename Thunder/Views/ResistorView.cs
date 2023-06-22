using CircuitSimulator.Models;
using SpiceSharp.Components;
using System;
using System.Windows;
using System.Windows.Input;
using Thunder;

namespace CircuitSimulator.Views
{
    public class ResistorView : BaseComponentView
    {
        private static int _counter = 0;

        public ResistorView()
        {
            componentType = ComponentType.Resistor;
            Rectangle.Fill = System.Windows.Media.Brushes.Red;
            Name = $"R{_counter++}";
            SpiceComponent = new Resistor("default", "", "", 1.0e4);
            var r = SpiceComponent as Resistor;
            var p = r?.Parameters;
            Value.Text = p?.Resistance.Value.ToString();
            Node.Text = Name;

        }

        public override void Value_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateResistance();
        }

        public override void Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            UpdateResistance();
            Value.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void UpdateResistance()
        {
            foreach (var entity in App.Circuit._spiceCircuit)
            {
                var component = (Component)entity;
                if (component.Name == Name && component is Resistor resistor)
                {
                    resistor.Parameters.Resistance = Convert.ToDouble(Value.Text);
                }
            }
        }
    }
}

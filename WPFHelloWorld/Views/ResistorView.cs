using CircuitSimulator.Models;
using SpiceSharp.Components;
using System;
using System.Windows;
using System.Windows.Input;
using WPFHelloWorld;

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
            SpiceComponent = new Resistor("default", "", "", 1.0e4);
            Resistor? r = SpiceComponent as Resistor;
            SpiceSharp.Components.Resistors.Parameters p = r.Parameters;
            Value.Text = p.Resistance.Value.ToString();
            Node.Text = Name;

        }

        public override void Value_LostFocus(object sender, RoutedEventArgs e)
        {
            updateResistance();
        }

        public override void Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                updateResistance();
                Value.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void updateResistance()
        {
            foreach (Component component in App.Circuit._spiceCircuit)
            {
                if (component.Name == Name && component is Resistor resistor)
                {
                    resistor.Parameters.Resistance = Convert.ToDouble(Value.Text);
                }
            }
        }
    }
}

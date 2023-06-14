using CircuitSimulator.Models;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using System;
using System.Collections.Generic;
using System.Text;
using IComponent = WPFHelloWorld.IComponent;

namespace CircuitSimulator
{
    public class MainCircuit
    {
        private Circuit _spiceCircuit { get; }

        private List<Conductor> _conductors;

        public StringBuilder SimulatorOutput { get; set; }

        public MainCircuit()
        {
            _spiceCircuit = new Circuit();
            _conductors = new List<Conductor>();
            SimulatorOutput = new StringBuilder();
        }

        public void run()
        {
            try
            {
                // Create a DC simulation that sweeps V1 from -1V to 1V in steps of 100mV
                var dc = new DC("DC 1", "V1", -1.0, 1.0, 0.5);

                // Catch exported data
                dc.ExportSimulationData += (sender, args) =>
                {
                    var input = args.GetVoltage("R1");
                    var output = args.GetVoltage("R2");
                    // System.Diagnostics.Debug.WriteLine($"input:{input}V - output:{output}V");
                    SimulatorOutput.Append($"input: {input}V - output: {output}V\n");
                };
                dc.Run(_spiceCircuit);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error occur!");
                SimulatorOutput.Append("Error occur!");
            }
        }

        public void AddConductor(Conductor conductor)
        {
            // Add the conductor to the list
            _conductors.Add(conductor);

            // Subscribe to the ConductorConnected event
            conductor.OnConductorConnected += OnConductorConnected;
        }

        // Event handler
        private void OnConductorConnected(object sender, EventArgs e)
        {
            // Cast the sender to Conductor
            Conductor conductor = sender as Conductor;

            System.Diagnostics.Debug.WriteLine($"connected {conductor.StartComponent.Name} to {conductor.EndComponent.Name}");

            IComponent start = conductor.StartComponent;
            IComponent end = conductor.EndComponent;
            // Create the SpiceSharp components if they don't exist
            if (start.SpiceComponent == null)
            {
                start.SpiceComponent = CreateSpiceComponent(start, end);
                if (start.SpiceComponent != null && !_spiceCircuit.Contains(start.SpiceComponent))
                {

                    System.Diagnostics.Debug.WriteLine(start.SpiceComponent);
                    _spiceCircuit.Add(start.SpiceComponent);
                }
            }
            if (end.SpiceComponent == null)
            {
                end.SpiceComponent = CreateSpiceComponent(end, start);
                if (end.SpiceComponent != null && !_spiceCircuit.Contains(end.SpiceComponent))
                {
                    System.Diagnostics.Debug.WriteLine(end.SpiceComponent);

                    _spiceCircuit.Add(end.SpiceComponent);
                }
            }
        }

        private Component CreateSpiceComponent(IComponent component, IComponent connectedComponent)
        {
            switch (component.componentType)
            {
                case ComponentType.Resistor:
                    return new Resistor(component.Name, connectedComponent.Name, component.Name, 1.0e4);
                case ComponentType.Voltage:
                    return new VoltageSource(component.Name, connectedComponent.Name, component.Name, 1.0);
                case ComponentType.Ground:
                    return null;
                default:
                    throw new Exception($"Unknown component type: {component.componentType}");
            }
        }
    }
}

using CircuitSimulator.Models;
using CircuitSimulator.Views;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IComponent = Thunder.IComponent;

namespace CircuitSimulator
{
    public class MainCircuit
    {
        public Circuit _spiceCircuit { get; }

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
                SimulatorOutput.Append($"{"input (V)",-10}| {"output (V)",10}\n");
                // Create a DC simulation that sweeps V1 from -1V to 1V in steps of 100mV
                var dc = new DC("DC 1", "V1", -1.0, 1.0, 0.2);

                // Catch exported data
                dc.ExportSimulationData += (sender, args) =>
                {
                    var input = args.GetVoltage("R1");
                    var output = args.GetVoltage("R2");
                    input = Math.Round(input, 2);
                    output = Math.Round(output, 2);
                    SimulatorOutput.Append($"{input,-13}|{output,10}\n");
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

            IComponent start = conductor.StartComponent;
            IComponent end = conductor.EndComponent;

            System.Diagnostics.Debug.WriteLine("Begin phase: ");
            logCircuit();

            // Create the SpiceSharp components if they don't exist
            if (isDefaultComponent(start.SpiceComponent))
            {
                start.SpiceComponent = CreateSpiceComponent(start, end);
                if (!_spiceCircuit.Contains(start.SpiceComponent))
                {
                    System.Diagnostics.Debug.WriteLine("add start component: " + start.SpiceComponent);
                    _spiceCircuit.Add(start.SpiceComponent);
                }
            }
            if (isDefaultComponent(end.SpiceComponent))
            {
                end.SpiceComponent = CreateSpiceComponent(end, start);
                if (!_spiceCircuit.Contains(end.SpiceComponent))
                {
                    System.Diagnostics.Debug.WriteLine("add end component: " + end.SpiceComponent);
                    _spiceCircuit.Add(end.SpiceComponent);
                }
            }


            int n = _spiceCircuit.Count;
            if (n >= 2)
            {
                var pre = (Component)_spiceCircuit.ElementAt(n - 2);
                var last = (Component)_spiceCircuit.ElementAt(n - 1);

                if (start is GroundView || end is GroundView)
                {
                    _spiceCircuit.Remove(last);
                    if (last is Resistor res)
                    {
                        _spiceCircuit.Add(new Resistor(last.Name, last.Nodes[0], "0", res.Parameters.Resistance));
                    }
                    else if (last is VoltageSource voltage)
                    {
                        _spiceCircuit.Add(new VoltageSource(last.Name, pre.Nodes[0], "0", voltage.Parameters.DcValue));
                    }

                }
                else if (pre != null && last != null)
                {
                    _spiceCircuit.Remove(pre);
                    if (pre.Nodes[1] == "0")
                    {
                        var vol = (VoltageSource)pre;
                        _spiceCircuit.Add(new VoltageSource(pre.Name, last.Nodes[0], "0", vol.Parameters.DcValue));
                    }
                    else
                    {
                        if (pre is Resistor res)
                        {
                            _spiceCircuit.Add(new Resistor(pre.Name, pre.Nodes[0], last.Nodes[0], res.Parameters.Resistance));
                        }
                        else if (pre is VoltageSource vol)
                        {
                            _spiceCircuit.Add(new VoltageSource(pre.Name, last.Nodes[0], last.Nodes[0], vol.Parameters.DcValue));
                        }
                    }
                }
                else
                {
                    return;
                }
            }


            System.Diagnostics.Debug.WriteLine("End phase: ");
            logCircuit();
        }

        private Component CreateSpiceComponent(IComponent component, IComponent connectedComponent)
        {
            switch (component.componentType)
            {
                case ComponentType.Resistor:
                    return new Resistor(component.Name, component.Name, connectedComponent.Name, 1.0e4);
                case ComponentType.Voltage:
                    return new VoltageSource(component.Name, component.Name, connectedComponent.Name, 1.0);
                case ComponentType.Ground:
                    return null;
                default:
                    throw new Exception($"Unknown component type: {component.componentType}");
            }
        }

        private bool isDefaultComponent(Component cp)
        {
            if (cp == null)
            {
                return false;
            }
            if (cp is Resistor)
            {
                string name = (cp as Resistor).Name;
                if (name == "default")
                {
                    return true;
                }
            }
            else if (cp is VoltageSource)
            {
                string name = (cp as VoltageSource).Name;
                if (name == "default")
                {
                    return true;
                }
            }

            return false;
        }

        private void logCircuit()
        {
            foreach (Component component in _spiceCircuit)
            {
                System.Diagnostics.Debug.WriteLine(component);
            }
        }
    }
}

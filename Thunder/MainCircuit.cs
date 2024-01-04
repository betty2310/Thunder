using CircuitSimulator.Models;
using CircuitSimulator.Views;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using Thunder;
using IComponent = Thunder.IComponent;

namespace CircuitSimulator
{
    public class MainCircuit
    {
        public Circuit SpiceCircuit { get; }

        private List<Conductor> _conductors;

        public  List<Conductor> Conductors
        {
            get => _conductors;
            set => _conductors = value;
        }

        public StringBuilder SimulatorOutput { get; set; }
        public ObservableCollection<Data> simulationDataCollection { get; set; }

        // Define a delegate for the event
        public delegate void SimulationCompletedHandler(object sender, EventArgs e);

        // Define the event based on the delegate
        public event SimulationCompletedHandler SimulationCompleted;
        public event SimulationCompletedHandler StopSimulation;

        public MainCircuit()
        {
            SpiceCircuit = new Circuit();
            _conductors = new List<Conductor>();
            SimulatorOutput = new StringBuilder();
            simulationDataCollection = new ObservableCollection<Data>();
        }

        public void run()
        {
            bool fl = false;
            foreach (var component in SpiceCircuit)
            {
                if (component.Name.Contains("AC"))
                {
                    component.SetParameter("acmag", 1.0);
                    fl = true;
                    break;
                }
            }
            System.Diagnostics.Debug.WriteLine(fl);
            if (fl)
            {

                try
                {
                    simulationDataCollection = new ObservableCollection<Data>();

                    var source = App.voltageAnalysis.Count > 0 ? App.voltageAnalysis[0].sourceComponent : "R1";
                    if (source != "R1")
                        System.Diagnostics.Debug.WriteLine(source);

                    var ac = new AC("AC", new DecadeSweep(1.0e-2, 1.0e3, 5));

                    var exportVoltage = new ComplexVoltageExport(ac, source);

                    ac.ExportSimulationData += (sender, args) =>
                    {
                        var output = exportVoltage.Value;
                        var magnitude = Complex.Abs(output);
                        var freq = args.Frequency;

                        var decibels = 10.0 * Math.Log10(output.Real * output.Real + output.Imaginary * output.Imaginary);
                        System.Diagnostics.Debug.WriteLine($"{freq}Hz: {decibels}dB:       {magnitude}   {output.Real}+{output.Imaginary}i");
                        simulationDataCollection.Add(new Data { InputValue = freq, OutputValue = decibels });

                    };
                    ac.Run(SpiceCircuit);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error occur!");
                    SimulatorOutput.Append("Error occur!");
                }
            }
            else
            {
                try
                {
                    simulationDataCollection = new ObservableCollection<Data>();

                    // Create a DC simulation that sweeps V1 from -1V to 1V in steps of 100mV
                    var dc = new DC("DC 1", "V1", 1.0, 1.0, 1);

                    var source = App.voltageAnalysis.Count > 0 ? App.voltageAnalysis[0].sourceComponent : "R1";
                    System.Diagnostics.Debug.WriteLine(source);
                    var inputExport = new RealVoltageExport(dc, "R1");
                    var outputExport = new RealVoltageExport(dc, source);

                    // Catch exported data
                    dc.ExportSimulationData += (sender, args) =>
                    {
                        var input = inputExport.Value;
                        var output = outputExport.Value;
                        input = Math.Round(input, 2);
                        output = Math.Round(output, 2);
                        simulationDataCollection.Add(new Data { InputValue = input, OutputValue = output });
                    };
                    dc.Run(SpiceCircuit);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error occur!");
                    SimulatorOutput.Append("Error occur!");
                }
            }
            OnSimulationCompleted();
        }

        protected virtual void OnSimulationCompleted()
        {
            SimulationCompleted?.Invoke(this, EventArgs.Empty);
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
                if (!SpiceCircuit.Contains(start.SpiceComponent))
                {
                    System.Diagnostics.Debug.WriteLine("add start component: " + start.SpiceComponent);
                    SpiceCircuit.Add(start.SpiceComponent);
                }
            }
            if (isDefaultComponent(end.SpiceComponent))
            {
                end.SpiceComponent = CreateSpiceComponent(end, start);
                if (!SpiceCircuit.Contains(end.SpiceComponent))
                {
                    System.Diagnostics.Debug.WriteLine("add end component: " + end.SpiceComponent);
                    SpiceCircuit.Add(end.SpiceComponent);
                }
            }


            int n = SpiceCircuit.Count;
            if (n >= 2)
            {
                var pre = (Component)SpiceCircuit.ElementAt(n - 2);
                var last = (Component)SpiceCircuit.ElementAt(n - 1);

                if (start is GroundView || end is GroundView)
                {
                    SpiceCircuit.Remove(last);
                    if (last is Resistor res)
                    {
                        SpiceCircuit.Add(new Resistor(last.Name, last.Nodes[0], "0", res.Parameters.Resistance));
                    }
                    else if (last is Capacitor cap)
                    {
                        SpiceCircuit.Add(new Capacitor(cap.Name, cap.Nodes[0], "0", cap.Parameters.Capacitance));
                    }
                    else if (last is Inductor ind)
                    {
                        SpiceCircuit.Add(new Inductor(ind.Name, ind.Nodes[0], "0", ind.Parameters.Inductance));
                    }
                    else if (last is VoltageSource voltage)
                    {
                        SpiceCircuit.Add(new VoltageSource(last.Name, pre.Nodes[0], "0", voltage.Parameters.DcValue));
                    }

                }
                else if (pre != null && last != null)
                {
                    SpiceCircuit.Remove(pre);
                    if (pre.Nodes[1] == "0")
                    {
                        var vol = (VoltageSource)pre;
                        SpiceCircuit.Add(new VoltageSource(pre.Name, last.Nodes[0], "0", vol.Parameters.DcValue));
                    }
                    else
                    {
                        if (pre is Resistor res)
                        {
                            SpiceCircuit.Add(new Resistor(pre.Name, pre.Nodes[0], last.Nodes[0], res.Parameters.Resistance));
                        }
                        else if (pre is VoltageSource vol)
                        {
                            SpiceCircuit.Add(new VoltageSource(pre.Name, last.Nodes[0], last.Nodes[0], vol.Parameters.DcValue));
                        }
                        else if (pre is Inductor ind)
                        {
                            SpiceCircuit.Add(new Inductor(ind.Name, ind.Nodes[0], last.Nodes[0], ind.Parameters.Inductance));
                        }
                        else if (pre is Capacitor cap)
                        {
                            SpiceCircuit.Add(new Capacitor(cap.Name, cap.Nodes[0], last.Nodes[0], cap.Parameters.Capacitance));
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
                case ComponentType.Capacitor:
                    return new Capacitor(component.Name, component.Name, connectedComponent.Name, 1.0e-6);
                case ComponentType.Inductor:
                    return new Inductor(component.Name, component.Name, connectedComponent.Name, 1.0e-6);
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
            else if (cp is Inductor)
            {
                string name = (cp as Inductor).Name;
                if (name == "default")
                {
                    return true;
                }
            }
            else if (cp is Capacitor)
            {
                string name = (cp as Capacitor).Name;
                if (name == "default")
                {
                    return true;
                }
            }


            return false;
        }

        private void logCircuit()
        {
            foreach (Component component in SpiceCircuit)
            {
                System.Diagnostics.Debug.WriteLine(component);
            }
        }

        public void StopCircuit()
        {
            SimulatorOutput.Clear();
            StopSimulation?.Invoke(this, EventArgs.Empty);
        }
    }
}

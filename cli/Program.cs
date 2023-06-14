using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace SpiceSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Resistor re = new Resistor("R1", "in", "out", 1.0e4);
            // Build the circuit
            var ckt = new Circuit(
                     new VoltageSource("V1", "in", "0", 1.0),

                     new Resistor("R2", "out", "0", 2.0e4)
            );

            ckt.Add(re);

            // Create a DC simulation that sweeps V1 from -1V to 1V in steps of 100mV
            var dc = new DC("DC 1", "V1", -1.0, 1.0, 0.5);

            // Catch exported data
            dc.ExportSimulationData += (sender, args) =>
            {
                var input = args.GetVoltage("in");
                var output = args.GetVoltage("out");
                Console.WriteLine($"input:{input}V - output:{output}V");
            };
            dc.Run(ckt);

        }
    }
}
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build the circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0),
                new Resistor("R1", "in", "out", 1.0e4),
                new Resistor("R2", "out", "0", 2.0e4)
                );

            // Create a DC simulation that sweeps V1 from -1V to 1V in steps of 100mV
            var dc = new DC("DC 1", "V1", -1.0, 1.0, 0.2);

            // Create exports
            var inputExport = new RealVoltageExport(dc, "in");
            var outputExport = new RealVoltageExport(dc, "out");
            var currentExport = new RealPropertyExport(dc, "V1", "i");

            // Catch exported data
            dc.ExportSimulationData += (sender, args) =>
            {
                var input = inputExport.Value;
                var output = outputExport.Value;
                var current = currentExport.Value;
            };
            dc.Run(ckt);
        }
    }

}

using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace SpiceSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // dcAnalysis();
            acAnanlysis();
        }

        private static void acAnanlysis()
        {
            // Build the circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0)
                    .SetParameter("acmag", 1.0),
                new Resistor("R1", "in", "out", 10.0e3),
                new Capacitor("C1", "out", "0", 1e-6)
            );

            var ac = new AC("AC 1", new DecadeSweep(1e-2, 1.0e3, 5));

            var exportVoltage = new ComplexVoltageExport(ac, "out");

            ac.ExportSimulationData += (sender, args) =>
            {
                var output = exportVoltage.Value;
                var decibels = 10.0 * Math.Log10(output.Real * output.Real + output.Imaginary * output.Imaginary);
                Console.WriteLine($"decibels:{decibels}V - output:{output}V");
            };
            ac.Run(ckt);
        }

        private static void dcAnalysis()
        {
            Resistor re = new Resistor("R1", "in", "out", 1.0e4);
            // Build the circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0),
                new Resistor("R2", "out", "0", 2.0e4)
            );

            ckt.Add(re);

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
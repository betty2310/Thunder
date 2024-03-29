﻿using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using System.Numerics;

namespace SpiceSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("*********************");
                Console.WriteLine("* 1. DC Analysis    *");
                Console.WriteLine("* 2. AC Analysis    *");
                Console.WriteLine("* 3. Test Circuit   *");
                Console.WriteLine("*********************");

                Console.Write("Enter a number: ");
                int num = Convert.ToInt32(Console.ReadLine());
                if (num == 0)
                {
                    break;
                }
                switch (num)
                {
                    case 1:
                        dcAnalysis();
                        break;
                    case 2:
                        acAnanlysis();
                        break;
                    case 3:
                        TestCircuit();
                        break;
                    default:
                        break;
                }


            }
        }

        private static void TestCircuit()
        {
            // Create the circuit
            var circuit = new Circuit();

            // Create the components
            var voltageSource = new VoltageSource("V1", "in", "0", 0.0).SetParameter("acmag", 1.0);
            var resistor = new Resistor("R1", "in", "out", 1.0e3);
            var capacitor = new Capacitor("C1", "out", "0", 1e-6);

            // Add the components to the circuit
            circuit.Add(voltageSource);
            circuit.Add(resistor);
            circuit.Add(capacitor);

            // Create the AC analysis
            var ac = new AC("AC 1", new LinearSweep(1.0, 1.0e3, 10));

            // Create an export for the voltage at the "out" node
            var exportVoltage = new ComplexVoltageExport(ac, "out");

            // Attach an event handler to export the simulation data
            ac.ExportSimulationData += (sender, args) =>
            {
                var voltage = exportVoltage.Value;
                Console.WriteLine($"Voltage at 'out' node: {voltage} V");
            };

            // Run the AC analysis
            ac.Run(circuit);
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

            var ac = new AC("AC 1", new DecadeSweep(1.0e-2, 1.0e3, 5));

            var exportVoltage = new ComplexVoltageExport(ac, "out");

            ac.ExportSimulationData += (sender, args) =>
            {
                var output = exportVoltage.Value;
                var magnitude = Complex.Abs(output);
                var freq = args.Frequency;

                var decibels = 10.0 * Math.Log10(output.Real * output.Real + output.Imaginary * output.Imaginary);
                Console.WriteLine($"${freq}Hz: {decibels}dB:       {magnitude}   {output.Real}+{output.Imaginary}i");
                //Console.WriteLine(output);

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
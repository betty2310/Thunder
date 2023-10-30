using CircuitSimulator;
using CircuitSimulator.Models;
using CircuitSimulator.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Thunder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Conductor CurrentConductor { get; set; }
        public static List<Conductor> Conductors = new List<Conductor>();
        public static List<VoltageAnalysisView> voltageAnalysis = new List<VoltageAnalysisView>();
        public static Canvas CircuitCanvas { get; set; }
        public static MainCircuit Circuit { get; set; }
    }
}

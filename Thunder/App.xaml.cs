using CircuitSimulator;
using CircuitSimulator.Models;
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
        public static Canvas CircuitCanvas { get; set; }
        public static MainCircuit Circuit { get; set; }
    }
}

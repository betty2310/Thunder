using CircuitSimulator;
using CircuitSimulator.Models;
using CircuitSimulator.Services;
using CircuitSimulator.Views;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public static Conductor CurrentConductor { get; set; }
        public static List<Conductor> Conductors = new List<Conductor>();
        public static List<VoltageAnalysisView> voltageAnalysis = new List<VoltageAnalysisView>();
        public static Canvas CircuitCanvas { get; set; }
        public static MainCircuit Circuit { get; set; }

        public static TempLineService? TempLineService { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Logging  =============        ");
            base.OnStartup(e);
        }
    }

}

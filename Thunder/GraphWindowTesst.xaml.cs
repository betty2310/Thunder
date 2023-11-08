using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CircuitSimulator
{
    /// <summary>
    /// Interaction logic for GraphWindowTesst.xaml
    /// </summary>
    public partial class GraphWindowTesst : Window
    {
        public GraphWindowTesst(ObservableCollection<Data> dataCollection)
        {
            InitializeComponent();
            var series = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = dataCollection.Select(x => new ObservablePoint(x.InputValue, x.OutputValue)),
                    Fill = null
                }
            };
            OutputChart.Series = series;
            OutputChart.ZoomMode = ZoomAndPanMode.Both;
            OutputChart.XAxes = XAxes;
            OutputChart.YAxes = YAxes;
        }
        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Frequency (Hz)",
                }
            };

        public Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Ampliture (dB)",
                }
            };
    }

}

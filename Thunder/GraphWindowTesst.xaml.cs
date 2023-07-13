using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            OutputChart.ZoomMode = ZoomAndPanMode.X;
        }
    }
}

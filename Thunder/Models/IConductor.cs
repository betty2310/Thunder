using System.Windows.Controls;
using Thunder;

namespace CircuitSimulator.Models
{
    public interface IConductor
    {
        IComponent StartComponent { get; set; }
        IComponent EndComponent { get; set; }
        double X1 { get; set; }
        double Y1 { get; set; }
        double X2 { get; set; }
        double Y2 { get; set; }

        void Draw(Canvas canvas);
    }
}

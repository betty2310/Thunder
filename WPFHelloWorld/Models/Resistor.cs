using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFHelloWorld
{
    public class Resistor : Component
    {
        public double Value { set; get; }
        public override string ToString()
        {
            return "resistor";
        }
    }
}

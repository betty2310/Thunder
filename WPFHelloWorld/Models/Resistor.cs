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

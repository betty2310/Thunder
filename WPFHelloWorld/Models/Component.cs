using System.Windows.Media;

namespace WPFHelloWorld
{
    abstract public class Component
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }
        public override string ToString()
        {
            return "component";
        }
    }
}

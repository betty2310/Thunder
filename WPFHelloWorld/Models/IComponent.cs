using System;

namespace WPFHelloWorld
{
    public interface IComponent
    {
        string CP_name { get; set; }
        string CP_color { get; set; }

        event EventHandler OnMoved;
    }
}

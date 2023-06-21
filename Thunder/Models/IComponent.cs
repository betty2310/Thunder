using CircuitSimulator.Models;
using SpiceSharp.Components;
using System;

namespace Thunder
{
    public interface IComponent
    {
        string CP_name { get; set; }
        string CP_color { get; set; }

        string Name { get; set; }

        Component SpiceComponent { get; set; }
        ComponentType componentType { get; }

        event EventHandler OnMoved;
    }
}

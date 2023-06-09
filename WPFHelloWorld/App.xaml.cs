﻿using CircuitSimulator.Models;
using System.Windows;
using System.Windows.Controls;

namespace WPFHelloWorld
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Conductor CurrentConductor { get; set; }
        public static Canvas CircuitCanvas { get; set; }
    }
}

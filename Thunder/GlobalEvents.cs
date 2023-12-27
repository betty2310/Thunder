using System;
using System.Windows;

namespace CircuitSimulator
{
    public static class GlobalEvents
    {
        public delegate void StartDrawingHandler(object sender, StartDrawingEventArgs e);
        public static event StartDrawingHandler StartDrawing;

        public static void OnStartDrawing(Point startPosition)
        {
            StartDrawing?.Invoke(null, new StartDrawingEventArgs(startPosition));
        }

        public class StartDrawingEventArgs : EventArgs
        {
            public Point StartPosition { get; }

            public StartDrawingEventArgs(Point startPosition)
            {
                StartPosition = startPosition;
            }
        }
    }
}

using System.Windows.Input;

namespace FractalVisualizer
{
    public static class CustomCommands
    {
        public static RoutedCommand GifExport = new RoutedCommand();
        public static RoutedCommand ConstantRotationExport = new RoutedCommand();
        public static RoutedCommand ResetView = new RoutedCommand();
        public static RoutedCommand ChooseFractal = new RoutedCommand();
        public static RoutedCommand ShowSettings = new RoutedCommand();
        public static RoutedCommand ShowTimeStats = new RoutedCommand();
    }
}

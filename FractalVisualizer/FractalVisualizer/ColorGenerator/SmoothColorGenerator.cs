using System;

namespace FractalVisualizer.ColorGenerator
{
    public class SmoothColorGenerator : MaxIterationDependantColorGenerator
    {
        public SmoothColorGenerator(int maxIterations) : base("Smooth", maxIterations) { }

        public override byte[] GetColorBytesFromIterations(int iterations)
        {
            double x = (double) iterations / MaxIterations * 100;
            int r = (int)(15.3 * x - 0.2295 * Math.Pow(x, 2));
            if (r < 0)
            {
                r = 0;
            }
            int g = (int)(-0.2295 * Math.Pow(x, 2) + 30.6 * x - 765);
            if (g < 0)
            {
                g = 0;
            }
            int b = (int)(-0.2295 * Math.Pow(x, 2) + 45.9 * x - 2040);
            if (b < 0)
            {
                b = 0;
            }
            return new byte[] {255, (byte)r, (byte)g, (byte)b};
        }
    }
}

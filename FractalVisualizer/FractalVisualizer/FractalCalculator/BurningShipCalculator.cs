using System;

namespace FractalVisualizer.FractalCalculator
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Burning_Ship_fractal
    /// </summary>
    class BurningShipCalculator : FractalCalculator
    {
        public BurningShipCalculator(int maxIterations) : base("Burning Ship", maxIterations, (-0.75, 0.375))
        {
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = Math.Abs(x * x - y * y + x0);
            double newY = Math.Abs(2 * x * y + y0);
            return (newX, newY);
        }
    }
}

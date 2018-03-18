using System;

namespace FractalVisualizer.FractalCalculator
{
    public class TestCalculator : FractalCalculator
    {
        public TestCalculator(int maxIterations) : base("Test", maxIterations)
        {
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = (x * x * x - 3 * x * y * y) - 0.54;
            y = 3 * x * x * y - y * y * y + 0.196;
            return (newX, y);
        }
    }
}

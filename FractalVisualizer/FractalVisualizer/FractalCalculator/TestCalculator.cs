using System;

namespace FractalVisualizer.FractalCalculator
{
    class TestCalculator : FractalCalculator
    {
        public TestCalculator(int maxIterations) : base("Test", maxIterations)
        {
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            x = x - x0;
            y = y + y0;
            double newX = (x * x - y * y) + x0;
            y = 2 * x * y - y0;
            return (newX, y);
        }
    }
}

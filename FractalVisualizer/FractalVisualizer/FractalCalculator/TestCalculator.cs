using System;

namespace FractalVisualizer.FractalCalculator
{
    public class TestCalculator : FractalCalculatorWithConstant
    {
        public TestCalculator(int maxIterations) : base("Test", maxIterations, (0, 0))
        {
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = x * x - y * y + x0 + Cx;
            y = 2 * x * y + y0 + Cy;
            return (newX, y);
        }
    }
}

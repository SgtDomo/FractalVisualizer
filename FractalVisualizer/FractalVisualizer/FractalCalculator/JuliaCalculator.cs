using System.ComponentModel;
using System.Runtime.CompilerServices;
using FractalVisualizer.Annotations;

namespace FractalVisualizer.FractalCalculator
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Julia_set
    /// </summary>
    public class JuliaCalculator : FractalCalculatorWithConstant
    {

        public JuliaCalculator(int maxIterations) : base("Julia Set", maxIterations)
        {
            Cx = -0.7269;
            Cy = 0.1889;
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = x * x - y * y + Cx;
            y = 2 * x * y + Cy;
            return (newX, y);
        }
    }
}

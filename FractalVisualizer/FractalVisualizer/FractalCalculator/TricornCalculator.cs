using System.Numerics;

namespace FractalVisualizer.FractalCalculator
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Tricorn_%28mathematics%29
    /// </summary>
    class TricornCalculator : FractalCalculator
    {
        public TricornCalculator(int maxIterations) : base("Tricorn", maxIterations)
        {
        }

        public override int GetIterationsForPoint(Complex point)
        {
            return GetIterationsForPoint(point.Real, point.Imaginary);
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = x * x - y * y + x0;
            y = -2 * x * y + y0;
            return(newX, y);
        }
    }
}

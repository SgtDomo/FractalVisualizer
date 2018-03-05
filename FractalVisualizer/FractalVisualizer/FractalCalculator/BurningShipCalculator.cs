using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FractalVisualizer.FractalCalculator
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Burning_Ship_fractal
    /// </summary>
    class BurningShipCalculator : FractalCalculator
    {
        public BurningShipCalculator(int maxIterations) : base("Burning Ship", maxIterations)
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

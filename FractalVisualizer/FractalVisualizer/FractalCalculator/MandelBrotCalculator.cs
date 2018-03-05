using System.Numerics;

namespace FractalVisualizer.FractalCalculator
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Mandelbrot_set
    /// </summary>
    class MandelbrotCalculator : FractalCalculator
    {
        public MandelbrotCalculator() : this(500)
        {
        }

        public MandelbrotCalculator(int maxIterations) : base("Mandelbrot", maxIterations)
        {
        }

        //funny artifact
        //public override int GetIterationsForPoint(double x, double y)
        //{
        //    double x0 = x;
        //    double y0 = x;
        //    int iterations = 0;
        //    while (x * x + y * y <= 4)
        //    {
        //        iterations++;
        //        if (iterations == MaxIterations)
        //        {
        //            return -1;
        //        }
        //        double newX = x * x - y * y + x0;
        //        double newY = 2 * x * y + y0;
        //        x = newX;
        //        y = newY;
        //    }
        //    return iterations;
        //}

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = x * x - y * y + x0;
            y = 2 * x * y + y0;
            return (newX, y);
        }
    }
}

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

        public MandelbrotCalculator(int maxIterations) : base("Mandelbrot", maxIterations, (-0.5, 0))
        {
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            double newX = x * x - y * y + x0;
            y = 2 * x * y + y0;
            return (newX, y);
        }
    }
}

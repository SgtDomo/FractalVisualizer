namespace FractalVisualizer.FractalCalculator
{
    public class ZCubedPlusCCalculator : FractalCalculatorWithConstant
    {
        public ZCubedPlusCCalculator( int maxIterations) : base("z^3 + c", maxIterations, (0, 0))
        {
            Cx = -0.54;
            Cy = 0.196;
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            var newX = x * x * x - 3 * x * y * y + Cx;
            y = 3 * x * x * y - y * y * y + Cy;
            return (newX, y);
        }
    }
}

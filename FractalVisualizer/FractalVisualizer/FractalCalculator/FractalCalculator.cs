using System;
using System.Numerics;

namespace FractalVisualizer.FractalCalculator
{
    public abstract class FractalCalculator
    {
        protected FractalCalculator(string name, int maxIterations)
        {
            MaxIterations = maxIterations;
            Name = name;
        }

        public int MaxIterations { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Calculates the Iterations needed to exeed the bound defined by the fractal. If the iterations reach MaxIterations -1 will be returned
        /// </summary>
        public virtual int GetIterationsForPoint(Complex point)
        {
            return GetIterationsForPoint(point.Real, point.Imaginary);
        }

        public virtual int GetIterationsForPoint(double x, double y)
        {
            double x0 = x;
            double y0 = y;
            int iterations = 0,
                maxIterations = MaxIterations;
            while (x * x + y * y <= 4)
            {
                iterations++;
                if (iterations == maxIterations)
                {
                    return -1;
                }
                (x, y) = GetNextZ(x, y, x0, y0);
            }
            return iterations;
        }

        public abstract (double x, double y) GetNextZ(double x, double y, double x0, double y0);

        protected Complex Square(Complex c)
        {
            double newReal = c.Real * c.Real - c.Imaginary * c.Imaginary;
            double newImagenary = 2 * c.Real * c.Imaginary;
            return new Complex(newReal, newImagenary);
        }

        protected double Abs(Complex complex)
        {
            double c = Math.Abs(complex.Real);
            double d = Math.Abs(complex.Imaginary);
            double r;
            if (c > d)
            {
                r = d / c;
                return c * Math.Sqrt(1.0 + r * r);
            }
            if (d == 0.0)
            {
                return c;  // c is either 0.0 or NaN
            }
            r = c / d;
            return d * Math.Sqrt(1.0 + r * r);

        }

        public static FractalCalculator[] GetFractalCalculators()
        {
            return new FractalCalculator[]
            {
                new MandelbrotCalculator(1000),
                new JuliaCalculator(1000),
                new BurningShipCalculator(1000),
                new TricornCalculator(1000)
            };
        }
    }
}

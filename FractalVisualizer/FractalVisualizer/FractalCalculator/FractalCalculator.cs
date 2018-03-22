using System;
using System.Numerics;

namespace FractalVisualizer.FractalCalculator
{
    public abstract class FractalCalculator
    {
        protected FractalCalculator(string name, int maxIterations, (double x, double y) defaultPosition)
        {
            MaxIterations = maxIterations;
            Name = name;
            DefaultPosition = defaultPosition;
        }

        public int MaxIterations { get; set; }

        /// <summary>
        /// Name of the fractal
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Determines the so to say center of the fractal - where the 
        /// position should be to have a good view over it.
        /// </summary>
        public (double x, double y) DefaultPosition { get; set; }

        /// <summary>
        /// Calculates the Iterations needed to exeed the bound defined by the fractal. If the iterations reach MaxIterations -1 will be returned
        /// </summary>
        public virtual int GetIterationsForPoint(Complex point)
        {
            return GetIterationsForPoint(point.Real, point.Imaginary);
        }

        /// <summary>
        /// Calculates the Iterations needed to exeed the bound defined by the fractal. If the iterations reach MaxIterations -1 will be returned
        /// </summary>
        /// <param name="x">The real part of the point</param>
        /// <param name="y">The imagenary part of the point</param>
        /// <returns>the amount of iterations needed for this point to reach the limit. If it reaches the MaxIterations, -1</returns>
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

        /// <summary>
        /// This here is basically the function of the fractal. Given the complex point Zn (x, y) it returns Zn+1 for the given fractal.
        /// </summary>
        /// <param name="x">Real part of current Z</param>
        /// <param name="y">Imagenary part of current Z</param>
        /// <param name="x0">Real part of the original point that is currently calculated</param>
        /// <param name="y0">Imagenary part of the original point that is currently calculated</param>
        /// <returns></returns>
        public abstract (double x, double y) GetNextZ(double x, double y, double x0, double y0);

        /// <summary>
        /// Returns all fractal calculators (so basically all fractals this tool can display)
        /// </summary>
        /// <returns>An array representing all currently implemented fractalCalculators</returns>
        public static FractalCalculator[] GetFractalCalculators()
        {
            return new FractalCalculator[]
            {
                new MandelbrotCalculator(1000),
                new JuliaCalculator(1000),
                new BurningShipCalculator(1000),
                new TricornCalculator(1000),
                new ZCubedPlusCCalculator(1000), 
                new TestCalculator(1000), 
            };
        }
    }
}

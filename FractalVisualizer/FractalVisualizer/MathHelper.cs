using System;
using System.Numerics;

namespace FractalVisualizer
{
    public static class MathHelper
    {

        public static Complex GetEToThePowerOfXTimesI(double x)
        {
            return Complex.Pow(new Complex(Math.E, 0), new Complex(0, x));
        }


        /// <summary>
        /// Squares a complex number
        /// </summary>
        public static Complex Square(Complex c)
        {
            double newReal = c.Real * c.Real - c.Imaginary * c.Imaginary;
            double newImagenary = 2 * c.Real * c.Imaginary;
            return new Complex(newReal, newImagenary);
        }

        /// <summary>
        /// Calculates the absolute length of the complex vector
        /// </summary>
        public static double Abs(Complex complex)
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

    }
}

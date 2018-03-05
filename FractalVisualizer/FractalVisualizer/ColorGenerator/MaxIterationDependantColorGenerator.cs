using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalVisualizer.ColorGenerator
{
    public abstract class MaxIterationDependantColorGenerator : ColorGenerator
    {
        protected MaxIterationDependantColorGenerator(string name, int maxIterations) : base(name)
        {
            MaxIterations = maxIterations;
        }

        public int MaxIterations { get; set; }
    }
}

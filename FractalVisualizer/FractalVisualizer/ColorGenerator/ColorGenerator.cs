using System.Drawing;
using FractalVisualizer.ImageGenerator;

namespace FractalVisualizer.ColorGenerator
{
    public abstract class ColorGenerator
    {
        protected ColorGenerator(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public abstract byte[] GetColorBytesFromIterations(int iterations);

        public virtual int GetColorIntFromIterations(int iterations)
        {

            var bytes = GetColorBytesFromIterations(iterations);
            return ImageUtils.ArgbIntFromBytes(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        public virtual Color GetColorFromIterations(int iterations)
        {
            return ImageUtils.ColorFromArgbInt(GetColorIntFromIterations(iterations));
        }

        public static ColorGenerator[] GetColorGenerators()
        {
            return new ColorGenerator[]
            {
                new SmoothColorGenerator(1000),
                new CuttedMultipliedColorGenerator()
            };
        }
    }
}

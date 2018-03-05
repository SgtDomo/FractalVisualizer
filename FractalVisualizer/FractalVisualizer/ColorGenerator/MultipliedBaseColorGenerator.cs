using FractalVisualizer.ImageGenerator;

namespace FractalVisualizer.ColorGenerator
{
    class MultipliedBaseColorGenerator : ColorGenerator
    {
        public int BaseColor { get; set; }

        public MultipliedBaseColorGenerator() : base("multiplied base")
        {

            BaseColor = 4096;
        }

        public override byte[] GetColorBytesFromIterations(int iterations)
        {
            if (iterations == -1)
            {
                return new byte[]
                {
                    255,0,0,0
                };
            }
            int number = int.MaxValue - BaseColor * iterations;
            var bytes = ImageUtils.BytesFromArgbInt(number);
            bytes[0] = 255;
            return bytes;
        }
    }
}

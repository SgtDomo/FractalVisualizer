namespace FractalVisualizer.ColorGenerator
{
    class CuttedMultipliedColorGenerator : ColorGenerator
    {
        public double RedMultiplier { get; set; }

        public double GreenMultiplier { get; set; }

        public double BlueMultiplier { get; set; }

        public CuttedMultipliedColorGenerator() : this(1, 2, 4)
        {
        }

        public CuttedMultipliedColorGenerator(double redMultiplier, double greenMultiplier, double blueMultiplier) : base("Multiplied RGB")
        {
            RedMultiplier = redMultiplier;
            GreenMultiplier = greenMultiplier;
            BlueMultiplier = blueMultiplier;
        }

        public override byte[] GetColorBytesFromIterations(int iterations)
        {
            if (iterations == -1)
            {
                return new[]
                {
                    (byte) 255,
                    (byte) 0,
                    (byte) 0,
                    (byte) 0
                };
            }
            return new[]
            {
                (byte) 255,
                (byte) (iterations * RedMultiplier),
                (byte) (iterations * GreenMultiplier),
                (byte) (50 + iterations * BlueMultiplier)
            };
        }
    }
}

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FractalVisualizer.ImageGenerator
{
    public class ImageGenerator
    {
        private FractalCalculator.FractalCalculator _fractalCalculator;

        public RenderSettings RenderSettings { get; set; }

        public ProgressModel ProgressModel { get; set; }

        public FractalCalculator.FractalCalculator FractalCalculator
        {
            get => _fractalCalculator;
            set
            {
                _fractalCalculator = value;
                _fractalCalculator.MaxIterations = RenderSettings.MaxIterations;
            }
        }

        public ImageGenerator(RenderSettings renderSettings, ProgressModel progressModel, FractalCalculator.FractalCalculator fractalCalculator)
        {
            RenderSettings = renderSettings;
            ProgressModel = progressModel;
            FractalCalculator = fractalCalculator;
        }

        public virtual ImageSource GenerateImage() => RenderSettings != null ? GenerateImage(RenderSettings) : null;

        public virtual ImageSource GenerateImage(RenderSettings renderSettings)
        {
            Bitmap bitmap = GenerateBitmap(renderSettings);
            return ImageUtils.ConvertBitmapToImageSource(bitmap);
        }

        public virtual Bitmap GenerateBitmap() => RenderSettings != null ? GenerateBitmap(RenderSettings) : null;

        public virtual Bitmap GenerateBitmap(RenderSettings renderSettings)
        {
            return GenerateBitmap(renderSettings, true);
        }

        public Bitmap GenerateBitmap(RenderSettings renderSettings, bool initProgressModel)
        {
            if (initProgressModel && ProgressModel != null)
            {
                ProgressModel.Min = 0;
                ProgressModel.Max = renderSettings.ThreadCount;
                ProgressModel.Value = 0;
            }

            FractalCalculator.FractalCalculator calculator = FractalCalculator;
            calculator.MaxIterations = renderSettings.MaxIterations;

            var bitmap = new DirectBitmap(renderSettings.ResolutionX, renderSettings.ResolutionY);

            double dxy = RenderSettings.Dx;

            var tasks = new Task[renderSettings.ThreadCount];
            int fromYPixel = 0;
            int stripHeight = (int)Math.Ceiling((double)renderSettings.ResolutionY / renderSettings.ThreadCount);
            for (int i = 0; i < renderSettings.ThreadCount; i++)
            {
                int localFromYPixel = fromYPixel;
                tasks[i] = Task.Factory.StartNew(
                    () => GenerateBitmapStrip(renderSettings, bitmap, calculator, localFromYPixel, stripHeight, dxy));
                fromYPixel += stripHeight;
            }

            foreach (Task task in tasks)
            {
                task.Wait();
                task.Dispose();
            }

            return bitmap.Bitmap;
        }

        public virtual GifBitmapEncoder GenerateGif() => RenderSettings != null ? GenerateGif(RenderSettings) : null;

        public virtual GifBitmapEncoder GenerateGif(RenderSettings renderSettings)
        {
            if (ProgressModel != null)
            {
                ProgressModel.Min = 0;
                ProgressModel.Max = ((int)Math.Log(renderSettings.GifEndMagnification / renderSettings.GifStartMagnification,
                                         renderSettings.GifMagnificationFactor) + 1) * renderSettings.ThreadCount;
                ProgressModel.Value = 0;
            }

            var gEnc = new GifBitmapEncoder();

            renderSettings.ChangeMagnificationBy(renderSettings.GifStartMagnification / renderSettings.CurrMagnification);

            while (renderSettings.CurrMagnification <= renderSettings.GifEndMagnification)
            {
                var bmp = GenerateBitmap(renderSettings, false).GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
                renderSettings.ChangeMagnificationBy(renderSettings.GifMagnificationFactor);
            }

            return gEnc;
        }

        protected virtual void GenerateBitmapStrip(RenderSettings renderSettings, DirectBitmap bitmap, FractalCalculator.FractalCalculator calculator,
            int fromYPixel, int stripPixelHeight, double dxy)
        {
            double y = renderSettings.FromY - dxy * fromYPixel;
            int toYPixel = fromYPixel + stripPixelHeight;
            for (int i = fromYPixel; i < toYPixel && i < renderSettings.ResolutionY; i++)
            {
                double x = renderSettings.FromX;
                for (int j = 0; j < renderSettings.ResolutionX; j++)
                {
                    //int iterations = calculator.GetIterationsForPoint(new Complex(x, y));
                    int iterations = calculator.GetIterationsForPoint(x, -y);
                    int color = renderSettings.ColorGenerator.GetColorIntFromIterations(iterations);
                    bitmap.SetPixel(j, i, color);
                    x += dxy;
                }
                y -= dxy;
            }

            if (ProgressModel != null && ProgressModel.IncreaseValueAfterEachThread)
            {
                ProgressModel.Value++;
            }
        }
    }
}

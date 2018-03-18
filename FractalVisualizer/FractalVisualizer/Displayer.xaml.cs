using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FractalVisualizer.Annotations;
using FractalVisualizer.ColorGenerator;
using FractalVisualizer.FractalCalculator;
using FractalVisualizer.ImageGenerator;
using Microsoft.Win32;

namespace FractalVisualizer
{
    /// <summary>
    /// Interaction logic for Displayer.xaml
    /// </summary>
    public partial class Displayer : INotifyPropertyChanged
    {
        private ImageSource _img;
        private RenderSettings _renderSettings;
        private ImageGenerator.ImageGenerator _imageGenerator;

        private readonly double _moveByPercentage;
        private readonly double _shortMoveByPercentage;

        private readonly Dictionary<Tuple<int, int, int>, List<long>> _timeLogger = new Dictionary<Tuple<int, int, int>, List<long>>();
        private Bitmap _bitmap;
        private bool _enableInput;

        public Displayer()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            RenderSettings = new RenderSettings { ColorGenerator = new CuttedMultipliedColorGenerator() };
            ProgressModel = new ProgressModel();
            InitFromY(RenderSettings);
            ImageGenerator =
                new ImageGenerator.ImageGenerator(RenderSettings, ProgressModel, new MandelbrotCalculator(RenderSettings.MaxIterations));

            RefreshImage();
            _moveByPercentage = 0.25;
            _shortMoveByPercentage = 0.03;
            EnableInput = true;
        }

        public bool EnableInput
        {
            get => _enableInput;
            set
            {
                if (value == _enableInput) return;
                _enableInput = value;
                OnPropertyChanged();
            }
        }

        public ImageSource Img
        {
            get => _img;
            set
            {
                if (Equals(value, _img)) return;
                _img = value;
                OnPropertyChanged();
            }
        }

        public Bitmap Bitmap
        {
            get => _bitmap;
            set
            {
                if (Equals(value, _bitmap)) return;
                _bitmap = value;
                OnPropertyChanged();
                ImageSource img = ImageUtils.ConvertBitmapToImageSource(value);
                img.Freeze();
                Img = img;
            }
        }

        public RenderSettings RenderSettings
        {
            get => _renderSettings;
            set
            {
                if (Equals(value, _renderSettings)) return;
                _renderSettings = value;
                OnPropertyChanged();
            }
        }

        public ProgressModel ProgressModel { get; set; }

        public ImageGenerator.ImageGenerator ImageGenerator
        {
            get => _imageGenerator;
            set
            {
                _imageGenerator = value;
                _imageGenerator.RenderSettings = RenderSettings;
                _imageGenerator.ProgressModel = ProgressModel;
            }
        }

        private static void InitFromY(RenderSettings renderSettings)
        {
            double dx = (renderSettings.ToX - renderSettings.FromX) / renderSettings.ResolutionX;
            renderSettings.FromY = dx * renderSettings.ResolutionY / 2;
        }

        public void ChooseFractal()
        {
            var fractalChooserDlg = new FractalChooserDialog(ImageGenerator.FractalCalculator);
            fractalChooserDlg.ShowDialog();
            if (fractalChooserDlg.DialogResult == true && fractalChooserDlg.SelectedFractal != null)
            {
                ImageGenerator.FractalCalculator = fractalChooserDlg.SelectedFractal;
                RefreshImageAsync();
            }
        }

        /// <summary>
        /// Executes the given function asnycly and sets the EnableInput field to false while the function is running
        /// </summary>
        public void DisableInputWhileAsync(Action func)
        {
            EnableInput = false;
            Task.Factory.StartNew(() =>
            {
                func();
                EnableInput = true;
            });
        }

        public void RefreshImageAsync()
        {
            DisableInputWhileAsync(RefreshImage);
        }

        public void RefreshImage()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Bitmap = ImageGenerator.GenerateBitmap();
            stopwatch.Stop();
            long millis = stopwatch.ElapsedMilliseconds;
            var identifier = new Tuple<int, int, int>(RenderSettings.ResolutionX, RenderSettings.ResolutionY, RenderSettings.MaxIterations);
            if (_timeLogger.ContainsKey(identifier))
            {
                _timeLogger[identifier].Add(millis);
            }
            else
            {
                _timeLogger[identifier] = new List<long>
                {
                    millis
                };
            }
            double avg = _timeLogger[identifier].Average();
            double meanDeviation = _timeLogger[identifier].Average(x => Math.Abs(x - avg));
            long min = _timeLogger[identifier].Min();
            long max = _timeLogger[identifier].Max();

            Debug.WriteLine($"Frame with width: {RenderSettings.ResolutionX}, height: {RenderSettings.ResolutionY} and max-iterations: {RenderSettings.MaxIterations} took {millis}ms to render.\n" +
                            $"This configuration takes on average {avg}ms with a mean deviation of {meanDeviation}ms to render. (Min: {min}ms, Max: {max}ms)");
        }

        public void ZoomIn()
        {
            RenderSettings.ChangeMagnificationBy(RenderSettings.MagnificationFactor);
        }

        public void ZoomOut()
        {
            RenderSettings.ChangeMagnificationBy(1 / RenderSettings.MagnificationFactor);
        }

        public void GoUp(bool shortMove = false)
        {
            RenderSettings.MoveYBy(shortMove ? _shortMoveByPercentage : _moveByPercentage);
        }

        public void GoDown(bool shortMove = false)
        {
            RenderSettings.MoveYBy(shortMove ? -_shortMoveByPercentage : -_moveByPercentage);
        }

        public void GoRight(bool shortMove = false)
        {
            RenderSettings.MoveXBy(shortMove ? _shortMoveByPercentage : _moveByPercentage);
        }

        public void GoLeft(bool shortMove = false)
        {
            RenderSettings.MoveXBy(shortMove ? -_shortMoveByPercentage : -_moveByPercentage);
        }

        public void CopyImageToClipboard()
        {
            Clipboard.SetImage((BitmapSource)Img);
        }

        public void ExportImageAsync()
        {
            RenderSettings renderSettings = RenderSettings.Copy();
            if (new RenderSettingsDialog("Image Export Settings", renderSettings, true).ShowDialog() != true)
            {
                return;
            }
            var saveFileDialog = new SaveFileDialog { Filter = "png | *.png" };
            if (saveFileDialog.ShowDialog() == true) // it's a bool? so the '== true' is necessary
            {
                DisableInputWhileAsync(() =>
                {
                    Bitmap bitmap = Bitmap;
                    if (!RenderSettings.Equals(renderSettings)) // if the rendersettings aren't the same as the ones that were used for the displayed image,
                                                                // the frame has to be rerendered
                    {
                        bitmap = ImageGenerator.GenerateBitmap(renderSettings);
                    }
                    bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
                });


            }
        }

        public void ExportGifAsync()
        {
            RenderSettings renderSettings = RenderSettings.Copy();
            renderSettings.GifStartMagnification = 1;
            renderSettings.GifEndMagnification = renderSettings.CurrMagnification;
            if (new RenderSettingsDialog("Gif Export Settings", renderSettings, true, true).ShowDialog() != true)
            {
                return;
            }
            var saveFileDialog = new SaveFileDialog { Filter = "gif | *.gif" };
            if (saveFileDialog.ShowDialog() == true) // it's a bool? so the '== true' is necessary
            {
                DisableInputWhileAsync(() =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    GifBitmapEncoder gifEnc = ImageGenerator.GenerateGif(renderSettings);
                    using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        gifEnc.Save(fileStream);
                    }
                    stopwatch.Stop();
                    long millis = stopwatch.ElapsedMilliseconds;
                    int expectedFrameCount = (int)Math.Log(
                                                 renderSettings.GifEndMagnification /
                                                 renderSettings.GifStartMagnification,
                                                 renderSettings.GifMagnificationFactor) + 1;
                    Debug.WriteLine($"Gif finished - expected frame count: {expectedFrameCount}");
                    Debug.WriteLine($"Gif with width: {renderSettings.ResolutionX}, " +
                                    $"height: {renderSettings.ResolutionY} and " +
                                    $"max-iterations: {renderSettings.MaxIterations} " +
                                    $"rendered {gifEnc.Frames.Count} frames in {millis}ms. " +
                                    $"({(double)millis / gifEnc.Frames.Count}ms / frame)");
                });

            }
        }

        public void ExportRotatingConstantGif()
        {
            var fractalChooserDialog = new FractalChooserDialog(null,
                    FractalCalculator.FractalCalculator.GetFractalCalculators().Where(x => x is FractalCalculatorWithConstant).ToArray(), false);

            if (fractalChooserDialog.ShowDialog() != true)
            {
                return;
            }

            var calculator = (FractalCalculatorWithConstant) fractalChooserDialog.SelectedFractal;

            var renderSettings = RenderSettings.Copy();
            if (new RenderSettingsDialog("Julia Set Rotation Export Settings", renderSettings, true, juliaRotationMode: true)
                    .ShowDialog() != true)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog {Filter = "gif | *.gif"};
            if (saveFileDialog.ShowDialog() == true)
            {
                DisableInputWhileAsync(() =>
                {
                    var imageGenerator = new ImageGenerator.ImageGenerator(renderSettings, ProgressModel, calculator);

                    if (ProgressModel != null)
                    {
                        ProgressModel.Min = 0;
                        ProgressModel.Max = renderSettings.JuliaRotationFrameAmount * renderSettings.ThreadCount;
                        ProgressModel.Value = 0;
                    }

                    var gifEnc = new GifBitmapEncoder();

                    const double max = Math.PI * 2;
                    double increment = max / renderSettings.JuliaRotationFrameAmount;
                    for (double x = 0; x < max; x += increment)
                    {
                        Complex c = MathHelper.GetEToThePowerOfXTimesI(x);
                        c *= renderSettings.JuliaRotationConstantFactor;
                        calculator.Cx = c.Real;
                        calculator.Cy = c.Imaginary;
                        var bmp = imageGenerator.GenerateBitmap(renderSettings, false).GetHbitmap();
                        var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            bmp,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                        gifEnc.Frames.Add(BitmapFrame.Create(src));
                    }

                    using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        gifEnc.Save(fileStream);
                    }
                });
            }
        }

        public void ShowStatisics()
        {
            string outputString = "";
            foreach (var configTimesPair in _timeLogger)
            {
                var identifier = configTimesPair.Key;
                double avg = _timeLogger[identifier].Average();
                double meanDeviation = _timeLogger[identifier].Average(x => Math.Abs(x - avg));
                long min = _timeLogger[identifier].Min();
                long max = _timeLogger[identifier].Max();


                outputString +=
                    $"X: {configTimesPair.Key.Item1}, Y: {configTimesPair.Key.Item2}, Iterations: {configTimesPair.Key.Item3}:\n";
                outputString +=
                    $"Avg: {avg}ms, Mean deviation: {meanDeviation}ms, Min: {min}ms, Max: {max}ms\n\n";
            }

            MessageBox.Show(outputString, "Time Statistics");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

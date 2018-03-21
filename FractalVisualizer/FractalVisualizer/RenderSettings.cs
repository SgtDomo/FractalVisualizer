using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FractalVisualizer.Annotations;
using FractalVisualizer.ColorGenerator;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FractalVisualizer
{
    public class RenderSettings : INotifyPropertyChanged, IEquatable<RenderSettings>
    {
        #region Attributes

        private const double WidthAt1Magnification = 3;

        private int _resolutionX;
        private int _resolutionY;
        private int _maxIterations;
        private double _magnificationFactor;
        private double _currMagnification;
        private double _gifMagnificationFactor;
        private double _gifStartMagnification;
        private double _gifEndMagnification;
        private int _threadCount;
        private ColorGenerator.ColorGenerator _colorGenerator;
        private int _juliaRotationFrameAmount;
        private double _juliaRotationConstantFactor;
        private double _centerY;
        private double _centerX;

        #endregion

        #region Constructors
        public RenderSettings() : this(640, 360, 500, 2, 1, 2, 1, 100, -0.5, 0, 16, new MultipliedBaseColorGenerator(), 100, 1)
        {
        }

        public RenderSettings(int resolutionX, int resolutionY, int maxIterations, 
            double magnificationFactor, double currMagnification, double gifMagnificationFactor, 
            double gifStartMagnification, double gifEndMagnification, 
            double centerX, double centerY, int threadCount,
            ColorGenerator.ColorGenerator colorGenerator, int juliaRotationFrameAmount, double juliaRotationConstantFactor)
        {
            _centerX = centerX;
            _centerY = centerY;
            _resolutionX = resolutionX;
            _resolutionY = resolutionY;
            _magnificationFactor = magnificationFactor;
            _currMagnification = currMagnification;
            _gifMagnificationFactor = gifMagnificationFactor;
            _gifStartMagnification = gifStartMagnification;
            _gifEndMagnification = gifEndMagnification;
            _threadCount = threadCount;
            _colorGenerator = colorGenerator;
            _juliaRotationFrameAmount = juliaRotationFrameAmount;
            _juliaRotationConstantFactor = juliaRotationConstantFactor;

            MaxIterations = maxIterations; //important to set maxIterations also at ColorGenerator if it is dependant on maxIterations
        }
        #endregion

        #region Properties

        public int ResolutionX
        {
            get => _resolutionX;
            set
            {
                if (value == _resolutionX) return;
                _resolutionX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Dx));
                OnPropertyChanged(nameof(XDistance));
                OnPropertyChanged(nameof(YDistance));
                OnPropertyChanged(nameof(FromX));
                OnPropertyChanged(nameof(FromY));
                OnPropertyChanged(nameof(ToX));
                OnPropertyChanged(nameof(ToY));
            }
        }

        public int ResolutionY
        {
            get => _resolutionY;
            set
            {
                if (value == _resolutionY) return;
                _resolutionY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(YDistance));
                OnPropertyChanged(nameof(FromY));
                OnPropertyChanged(nameof(ToY));
            }
        }

        public double CenterX
        {
            get => _centerX;
            set
            {
                if (value.Equals(_centerX)) return;
                _centerX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FromX));
                OnPropertyChanged(nameof(ToX));
            }
        }

        public double CenterY
        {
            get => _centerY;
            set
            {
                if (value.Equals(_centerY)) return;
                _centerY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FromY));
                OnPropertyChanged(nameof(ToY));
            }
        }

        public int MaxIterations
        {
            get => _maxIterations;
            set
            {
                if (value == _maxIterations) return;
                _maxIterations = value;

                if (ColorGenerator is MaxIterationDependantColorGenerator colorGenerator)
                {
                    colorGenerator.MaxIterations = value;
                }
                OnPropertyChanged();
            }
        }

        public double MagnificationFactor
        {
            get => _magnificationFactor;
            set
            {
                if (value.Equals(_magnificationFactor)) return;
                _magnificationFactor = value;
                OnPropertyChanged();
            }
        }

        public double CurrMagnification
        {
            get => _currMagnification;
            set
            {
                if (value.Equals(_currMagnification)) return;
                _currMagnification = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Dx));
                OnPropertyChanged(nameof(XDistance));
                OnPropertyChanged(nameof(YDistance));
                OnPropertyChanged(nameof(FromX));
                OnPropertyChanged(nameof(FromY));
                OnPropertyChanged(nameof(ToX));
                OnPropertyChanged(nameof(ToY));
            }
        }

        public double GifMagnificationFactor
        {
            get => _gifMagnificationFactor;
            set
            {
                if (value.Equals(_gifMagnificationFactor)) return;
                _gifMagnificationFactor = value;
                OnPropertyChanged();
            }
        }

        public double GifStartMagnification
        {
            get => _gifStartMagnification;
            set
            {
                if (value.Equals(_gifStartMagnification)) return;
                _gifStartMagnification = value;
                OnPropertyChanged();
            }
        }

        public double GifEndMagnification
        {
            get => _gifEndMagnification;
            set
            {
                if (value.Equals(_gifEndMagnification)) return;
                _gifEndMagnification = value;
                OnPropertyChanged();
            }
        }

        public int ThreadCount
        {
            get => _threadCount;
            set
            {
                if (value == _threadCount) return;
                _threadCount = value;
                OnPropertyChanged();
            }
        }

        public ColorGenerator.ColorGenerator ColorGenerator
        {
            get => _colorGenerator;
            set
            {
                if (Equals(value, _colorGenerator)) return;
                _colorGenerator = value;
                if (_colorGenerator is MaxIterationDependantColorGenerator generator)
                {
                    generator.MaxIterations = MaxIterations;
                }
                OnPropertyChanged();
            }
        }

        public int JuliaRotationFrameAmount
        {
            get => _juliaRotationFrameAmount;
            set
            {
                _juliaRotationFrameAmount = value;
                OnPropertyChanged();
            }
        }

        public double JuliaRotationConstantFactor
        {
            get => _juliaRotationConstantFactor;
            set
            {
                if (value.Equals(_juliaRotationConstantFactor)) return;
                _juliaRotationConstantFactor = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Computed Properties
        public double Dx => WidthAt1Magnification / CurrMagnification / ResolutionX;

        public double XDistance => WidthAt1Magnification / CurrMagnification; // Also Dx * ResolutionX

        public double YDistance => Dx * ResolutionY;

        public double FromX => CenterX - XDistance / 2;

        public double ToX => CenterX + XDistance / 2;

        public double FromY => CenterY + YDistance / 2;

        public double ToY => CenterY - YDistance / 2;
        #endregion

        #region Methods
        public void ChangeMagnificationBy(double factor)
        {
            CurrMagnification *= factor;
        }

        public void MoveXBy(double percentage)
        {
            CenterX += XDistance * percentage;
        }

        public void MoveYBy(double percentage)
        {
            CenterY += XDistance * percentage;
        }

        public RenderSettings Copy()
        {
            return new RenderSettings(ResolutionX, ResolutionY, MaxIterations, 
                MagnificationFactor, CurrMagnification, GifMagnificationFactor, 
                GifStartMagnification, GifEndMagnification, CenterX, CenterY, 
                ThreadCount, ColorGenerator, JuliaRotationFrameAmount, JuliaRotationConstantFactor);
        }

        #endregion

        #region IEquatable Members

        public bool Equals(RenderSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _resolutionX == other._resolutionX && _resolutionY == other._resolutionY
                && _maxIterations == other._maxIterations && _magnificationFactor.Equals(other._magnificationFactor)
                && _currMagnification.Equals(other._currMagnification) && _gifMagnificationFactor.Equals(other._gifMagnificationFactor)
                && _gifStartMagnification.Equals(other._gifStartMagnification) && _gifEndMagnification.Equals(other._gifEndMagnification)
                && _centerX.Equals(other._centerX) && _centerY.Equals(other._centerY)
                && _threadCount == other._threadCount && Equals(_colorGenerator, other._colorGenerator);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as RenderSettings;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _resolutionX;
                hashCode = (hashCode * 397) ^ _resolutionY;
                hashCode = (hashCode * 397) ^ _maxIterations;
                hashCode = (hashCode * 397) ^ _magnificationFactor.GetHashCode();
                hashCode = (hashCode * 397) ^ _currMagnification.GetHashCode();
                hashCode = (hashCode * 397) ^ _gifMagnificationFactor.GetHashCode();
                hashCode = (hashCode * 397) ^ _gifStartMagnification.GetHashCode();
                hashCode = (hashCode * 397) ^ _gifEndMagnification.GetHashCode();
                hashCode = (hashCode * 397) ^ _centerX.GetHashCode();
                hashCode = (hashCode * 397) ^ _centerY.GetHashCode();
                hashCode = (hashCode * 397) ^ _threadCount;
                hashCode = (hashCode * 397) ^ (_colorGenerator != null ? _colorGenerator.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(RenderSettings left, RenderSettings right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RenderSettings left, RenderSettings right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}

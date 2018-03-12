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


        private int _resolutionX;
        private int _resolutionY;
        private int _maxIterations;
        private double _magnificationFactor;
        private double _currMagnification;
        private double _gifMagnificationFactor;
        private double _gifStartMagnification;
        private double _gifEndMagnification;
        private double _fromX;
        private double _toX;
        private double _fromY;
        private int _threadCount;
        private ColorGenerator.ColorGenerator _colorGenerator;
        private int _juliaRotationFrameAmount;
        private double _juliaRotationConstantFactor;

        #endregion

        #region Constructors
        public RenderSettings() : this(640, 360, 500, 2, 1, 2, 1, 100, -2, 1, 1, 16, new MultipliedBaseColorGenerator(), 100, 1)
        {
        }

        public RenderSettings(int resolutionX, int resolutionY, int maxIterations, 
            double magnificationFactor, double currMagnification, double gifMagnificationFactor, 
            double gifStartMagnification, double gifEndMagnification, 
            double fromX, double toX, double fromY, int threadCount,
            ColorGenerator.ColorGenerator colorGenerator, int juliaRotationFrameAmount, double juliaRotationConstantFactor)
        {
            _resolutionX = resolutionX;
            _resolutionY = resolutionY;
            _magnificationFactor = magnificationFactor;
            _currMagnification = currMagnification;
            _gifMagnificationFactor = gifMagnificationFactor;
            _gifStartMagnification = gifStartMagnification;
            _gifEndMagnification = gifEndMagnification;
            _fromX = fromX;
            _toX = toX;
            _fromY = fromY;
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
            }
        }

        public double FromX
        {
            get => _fromX;
            set
            {
                if (value.Equals(_fromX)) return;
                _fromX = value;
                OnPropertyChanged();
            }
        }

        public double ToX
        {
            get => _toX;
            set
            {
                if (value.Equals(_toX)) return;
                _toX = value;
                OnPropertyChanged();
            }
        }

        public double FromY
        {
            get => _fromY;
            set
            {
                if (value.Equals(_fromY)) return;
                _fromY = value;
                OnPropertyChanged();
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
        public double Dx => XDistance / ResolutionX;

        public double ToY => FromY - ResolutionY * Dx;

        public double YDistance => ToY - FromY;

        public double XDistance => ToX - FromX;
        #endregion

        #region Methods
        public void ChangeMagnificationBy(double factor)
        {
            double centerX = (ToX + FromX) / 2;
            double centerY = (ToY + FromY) / 2;
            double halfPixelsX = ResolutionX / 2.0;
            double halfPixelsY = ResolutionY / 2.0;
            double dx = Dx / factor;
            FromX = centerX - halfPixelsX * dx;
            ToX = centerX + halfPixelsX * dx;
            FromY = centerY + halfPixelsY * dx;

            CurrMagnification *= factor;
        }

        public void MoveXBy(double percentage)
        {
            double dx = Dx;
            FromX += percentage * ResolutionX * dx;
            ToX += percentage * ResolutionX  * dx;
        }

        public void MoveYBy(double percentage)
        {
            double dx = Dx;
            FromY += percentage * ResolutionY * dx;
        }

        public RenderSettings Copy()
        {
            return new RenderSettings(ResolutionX, ResolutionY, MaxIterations, 
                MagnificationFactor, CurrMagnification, GifMagnificationFactor, 
                GifStartMagnification, GifEndMagnification, FromX, ToX, FromY, 
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
                && _fromX.Equals(other._fromX) && _toX.Equals(other._toX) && _fromY.Equals(other._fromY)
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
                hashCode = (hashCode * 397) ^ _fromX.GetHashCode();
                hashCode = (hashCode * 397) ^ _toX.GetHashCode();
                hashCode = (hashCode * 397) ^ _fromY.GetHashCode();
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

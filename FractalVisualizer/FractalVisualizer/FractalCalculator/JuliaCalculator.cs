using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using FractalVisualizer.Annotations;

namespace FractalVisualizer.FractalCalculator
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Julia_set
    /// </summary>
    public class JuliaCalculator : FractalCalculator, INotifyPropertyChanged
    {
        private double _cx;
        private double _cy;

        public JuliaCalculator(int maxIterations) : base("Julia Set", maxIterations)
        {
            
            Cx = -0.7269;
            Cy = 0.1889;
        }

        public double Cx
        {
            get => _cx;
            set
            {
                if (value.Equals(_cx)) return;
                _cx = value;
                OnPropertyChanged();
            }
        }

        public double Cy
        {
            get => _cy;
            set
            {
                if (value.Equals(_cy)) return;
                _cy = value;
                OnPropertyChanged();
            }
        }

        public override (double x, double y) GetNextZ(double x, double y, double x0, double y0)
        {
            var newX = x * x - y * y + Cx;
            y = 2 * x * y + Cy;
            return (newX, y);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

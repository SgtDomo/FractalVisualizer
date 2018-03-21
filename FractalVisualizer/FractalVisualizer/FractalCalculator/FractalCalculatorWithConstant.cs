using System.ComponentModel;
using System.Runtime.CompilerServices;
using FractalVisualizer.Annotations;

namespace FractalVisualizer.FractalCalculator
{
    public abstract class FractalCalculatorWithConstant : FractalCalculator, INotifyPropertyChanged
    {
        private double _cx;
        private double _cy;

        protected FractalCalculatorWithConstant(string name, int maxIterations, (double x, double y) defaultPosition) 
            : base(name, maxIterations, defaultPosition)
        {
            _cx = 0;
            _cy = 0;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

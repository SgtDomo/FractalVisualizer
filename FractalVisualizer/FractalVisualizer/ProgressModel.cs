using System.ComponentModel;
using System.Runtime.CompilerServices;
using FractalVisualizer.Annotations;

namespace FractalVisualizer
{
    public class ProgressModel : INotifyPropertyChanged
    {
        private int _min;
        private int _max;
        private int _value;
        private bool _increaseValueAfterEachThread;

        private object _valueLock = new object();

        public ProgressModel() : this(0, 1, 0, true)
        {
        }

        public ProgressModel(int min, int max, int value, bool increaseValueAfterEachThread)
        {
            _min = min;
            _max = max;
            _value = value;
            _increaseValueAfterEachThread = increaseValueAfterEachThread;
        }

        public int Min
        {
            get => _min;
            set
            {
                if (value == _min) return;
                _min = value;
                OnPropertyChanged();
            }
        }

        public int Max
        {
            get => _max;
            set
            {
                if (value == _max) return;
                _max = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFinished));
            }
        }

        public int Value
        {
            get => _value;
            private set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFinished));
            }
        }

        public bool IncreaseValueAfterEachThread
        {
            get => _increaseValueAfterEachThread;
            set
            {
                if (value == _increaseValueAfterEachThread) return;
                _increaseValueAfterEachThread = value;
                OnPropertyChanged();
            }
        }

        public bool IsFinished => Value >= Max;

        public void IncrementValue()
        {
            lock (_valueLock)
            {
                Value++;
            }
        }

        public void SetValue(int newValue)
        {
            lock (_valueLock)
            {
                Value = newValue;
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

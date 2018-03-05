using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FractalVisualizer.Annotations;

namespace FractalVisualizer
{
    public class ProgressModel : INotifyPropertyChanged
    {
        private int _min;
        private int _max;
        private int _value;
        private bool _currentlyWorking;
        private bool _increaseValueAfterEachThread;

        public ProgressModel() : this(0, 1, 0, false, true)
        {
        }

        public ProgressModel(int min, int max, int value, bool currentlyWorking, bool increaseValueAfterEachThread)
        {
            _min = min;
            _max = max;
            _value = value;
            _currentlyWorking = currentlyWorking;
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
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FractalVisualizer.Annotations;

namespace FractalVisualizer
{
    /// <summary>
    /// Interaction logic for FractalChooserDialog.xaml
    /// </summary>
    public partial class FractalChooserDialog : INotifyPropertyChanged
    {
        private FractalCalculator.FractalCalculator _selectedFractal;
        private FractalCalculator.FractalCalculator[] _fractals;

        public FractalChooserDialog([CanBeNull] FractalCalculator.FractalCalculator selectedFractal = null)
        {
            Fractals = FractalCalculator.FractalCalculator.GetFractalCalculators();
            SelectedFractal = selectedFractal == null ? Fractals[0] : Fractals.First(x => x.Name == selectedFractal.Name);
            InitializeComponent();
            MainGrid.DataContext = this;
        }

        public FractalCalculator.FractalCalculator[] Fractals
        {
            get => _fractals;
            set
            {
                if (Equals(value, _fractals)) return;
                _fractals = value;
                OnPropertyChanged();
            }
        }

        public FractalCalculator.FractalCalculator SelectedFractal
        {
            get => _selectedFractal;
            set
            {
                if (Equals(value, _selectedFractal)) return;
                _selectedFractal = value;
                OnPropertyChanged();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

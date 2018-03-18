using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FractalVisualizer.Annotations;
using FractalVisualizer.FractalCalculator;

namespace FractalVisualizer
{
    /// <summary>
    /// Interaction logic for FractalChooserDialog.xaml
    /// </summary>
    public partial class FractalChooserDialog : INotifyPropertyChanged
    {
        private FractalCalculator.FractalCalculator _selectedFractal;
        private FractalCalculator.FractalCalculator[] _fractals;
        private double _calculationX;

        private readonly bool _showConstantEditing;

        public FractalChooserDialog([CanBeNull] FractalCalculator.FractalCalculator selectedFractal = null, [CanBeNull] FractalCalculator.FractalCalculator[] fractalCalculators = null, bool showConstantEditing = true)
        {
            _showConstantEditing = showConstantEditing;

            Fractals = fractalCalculators ?? FractalCalculator.FractalCalculator.GetFractalCalculators();
            Fractals = Fractals.Select(x =>
                selectedFractal == null || selectedFractal.Name != x.Name ? x : selectedFractal).ToArray(); // replaces the calculator inside the fractals array with the selected array (so that the settings of the selected are applied)

            SelectedFractal = selectedFractal == null ? Fractals[0] : Fractals.First(x => x.Name == selectedFractal.Name);
            InitializeComponent();
            MainGrid.DataContext = this;
            FractalSettingsGrid.DataContext = SelectedFractal;
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
                OnPropertyChanged(nameof(ShowConstantEditing));

                if (FractalSettingsGrid != null)
                {
                    FractalSettingsGrid.DataContext = value;
                }
            }
        }

        public bool ShowConstantEditing => SelectedFractal is FractalCalculatorWithConstant && _showConstantEditing;

        public double CalculationX
        {
            get => _calculationX;
            set
            {
                if (value.Equals(_calculationX)) return;
                _calculationX = value;
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

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            Complex complex = MathHelper.GetEToThePowerOfXTimesI(CalculationX);
            if (!(SelectedFractal is FractalCalculatorWithConstant fractalCalculatorWithConstant)) return;
            fractalCalculatorWithConstant.Cx = complex.Real;
            fractalCalculatorWithConstant.Cy = complex.Imaginary;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FractalChooserDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    OK_Click(null, null);
                    break;
                case Key.Escape:
                    Cancel_Click(null, null);
                    break;
            }
        }
    }
}

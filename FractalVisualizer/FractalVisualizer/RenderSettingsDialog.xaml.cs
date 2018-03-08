using System.Windows;
using System.Windows.Input;

namespace FractalVisualizer
{
    /// <summary>
    /// Interaction logic for RenderSettingsDialog.xaml
    /// </summary>
    public partial class RenderSettingsDialog
    {
        public RenderSettingsDialog(string dialogTitle, RenderSettings renderSettings, bool exportMode = false, bool gifMode = false)
        {
            GifRowHeight = gifMode ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            NonExportRowHeight = exportMode ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
            InitializeComponent();
            RenderSettings = renderSettings;
            DataContext = this;
            Title = dialogTitle;
        }

        public GridLength NonExportRowHeight { get; set; }

        public GridLength GifRowHeight { get; set; }

        public GridLength NonGifRowHeight { get; set; }

        public RenderSettings RenderSettings { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void RenderSettingsDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    DialogResult = false;
                    Close();
                    break;
                case Key.Enter:
                    DialogResult = true;
                    Close();
                    break;
            }
        }
    }
}

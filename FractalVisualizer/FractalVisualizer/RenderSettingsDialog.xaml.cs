using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FractalVisualizer
{
    /// <summary>
    /// Interaction logic for RenderSettingsDialog.xaml
    /// </summary>
    public partial class RenderSettingsDialog
    {
        public RenderSettingsDialog(RenderSettings renderSettings, bool exportMode = false, bool gifMode = false)
        {
            GifRowHeight = gifMode ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            NonExportRowHeight = exportMode ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
            InitializeComponent();
            RenderSettings = renderSettings;
            DataContext = this;
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

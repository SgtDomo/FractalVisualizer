﻿using System.Windows;
using System.Windows.Input;

namespace FractalVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!Displayer.EnableInput)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Space:
                    Displayer.ZoomIn();
                    Displayer.RefreshImageAsync();
                    break;
                case Key.LeftCtrl:
                    Displayer.ZoomOut();
                    Displayer.RefreshImageAsync();
                    break;
                case Key.Up:
                case Key.W:
                    Displayer.GoUp(Keyboard.IsKeyDown(Key.LeftShift));
                    Displayer.RefreshImageAsync();
                    break;
                case Key.Down:
                case Key.S:
                    Displayer.GoDown(Keyboard.IsKeyDown(Key.LeftShift));
                    Displayer.RefreshImageAsync();
                    break;
                case Key.Left:
                case Key.A:
                    Displayer.GoLeft(Keyboard.IsKeyDown(Key.LeftShift));
                    Displayer.RefreshImageAsync();
                    break;
                case Key.Right:
                case Key.D:
                    Displayer.GoRight(Keyboard.IsKeyDown(Key.LeftShift));
                    Displayer.RefreshImageAsync();
                    break;
                case Key.F12:
                    Displayer.CopyImageToClipboard();
                    break;
            }
        }

        private void SettingsMenuItems_Click(object sender, RoutedEventArgs e)
        {
            if (!Displayer.EnableInput)
            {
                return;
            }
            new RenderSettingsDialog(Displayer.RenderSettings).ShowDialog();
            Displayer.RefreshImageAsync();
        }

        private void CopyToClickBoard_Click(object sender, RoutedEventArgs e)
        {
            if (!Displayer.EnableInput)
            {
                return;
            }
            Displayer.CopyImageToClipboard();
        }

        private void ImageExport_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Displayer.EnableInput)
            {
                return;
            }
            Displayer.ExportImageAsync();
        }

        private void GifExport_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Displayer.EnableInput)
            {
                return;
            }
            Displayer.ExportGifAsync();
        }

        private void TimeStatistics_Click(object sender, RoutedEventArgs e)
        {
            Displayer.ShowStatisics();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Controls:\n" +
                            "Arrow-keys to move, hold left-shift to move for a smaller amount.\n" +
                            "Space to zoom in, left-ctrl to zoom out.", 
                            "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ChooseFractal_Click(object sender, RoutedEventArgs e)
        {
            Displayer.ChooseFractal();
        }
    }
}
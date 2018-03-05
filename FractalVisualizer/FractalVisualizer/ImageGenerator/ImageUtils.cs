using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace FractalVisualizer.ImageGenerator
{
    class ImageUtils
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource ConvertBitmapToImageSource(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public static int ArgbIntFromColor(Color c)
        {
            return ArgbIntFromBytes(c.A, c.R, c.G, c.B);
        }

        public static int ArgbIntFromBytes(byte a, byte r, byte g, byte b)
        {
            return (a << 24) | (r << 16) | (g << 8) | b;
        }

        public static byte[] BytesFromArgbInt(int color)
        {
            var bytes = BitConverter.GetBytes(color);
            Array.Reverse(bytes);
            return bytes;
        }

        public static Color ColorFromArgbInt(int argb)
        {
            return Color.FromArgb((byte)(argb >> 24),
                                  (byte)(argb >> 16),
                                  (byte)(argb >> 8),
                                  (byte) argb);
        }
    }
}

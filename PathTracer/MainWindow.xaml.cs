using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PathTracer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scene s;
        Renderer r;
        readonly Int32Rect rect;
        readonly WriteableBitmap bmp;
        public MainWindow()
        {
            InitializeComponent();

            var width = (int)PaintCanvas.Width;
            var height = (int)PaintCanvas.Height;
            bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
            var stride = Helper.CalcStride(width, bmp.Format.BitsPerPixel);
            rect = new Int32Rect(0, 0, width, height);

            r = new Renderer(width, height, stride);
            s = SceneFactory.CreateThisThing();

            Paint();
        }

        private void Paint()
        {
            byte[] data;
            var sw = new Stopwatch();

            sw.Restart();
            data = r.Render(s);
            sw.Stop();

            LblRender.Content = string.Format("Render time: {0:N2}ms", sw.Elapsed.TotalMilliseconds);

            bmp.WritePixels(rect, data, r.Stride, 0);
            SaveImage(bmp, @"./output.png");
            PaintCanvas.Background = new ImageBrush { ImageSource = bmp };
        }

        private void SaveImage(BitmapSource bmp, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(bmp));
            png.Save(fs);
            fs.Close();
        }
    }
}

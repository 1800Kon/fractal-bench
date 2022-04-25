using FractalBench.Classes;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Image = System.Drawing.Image;

namespace FractalBench
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
                      
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            CreateFractal(400, 400, 4);

        }

        private void CreateFractal(int width, int height, int noOfThreads) {
            var startingSectionX = 0;
            var startingSectionY = 0;
            var endingSectionX = 0;
            var endingSectionY = 0;

            var bitmap = new Bitmap(width, height);
            var rect = new Rectangle(0, 0, width, height);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var depth = Image.GetPixelFormatSize(bitmapData.PixelFormat) / 8; // this is something which might need to be changed
            var buffer = new byte[bitmapData.Width * bitmapData.Height * depth];
            
            // Copy all the data from the bitmap to the buffer
            Marshal.Copy(bitmapData.Scan0, buffer, 0, buffer.Length);
            
            ThreadPool.SetMaxThreads(noOfThreads, noOfThreads);
            ThreadPool.SetMinThreads(noOfThreads, noOfThreads);
            
            for (var i = 0; i <= 3; i++)
            {
                switch (i)
                {
                    case 0:
                        startingSectionX = 0;
                        startingSectionY = 0;
                        endingSectionX = 200;
                        endingSectionY = 200;
                        break;

                    case 1:
                        startingSectionX = 200;
                        startingSectionY = 0;
                        endingSectionX = 400;
                        endingSectionY = 200;
                        break;

                    case 2:
                        startingSectionX = 0;
                        startingSectionY = 200;
                        endingSectionX = 200;
                        endingSectionY = 400;
                        break;

                    case 3:
                        startingSectionX = 200;
                        startingSectionY = 200;
                        endingSectionX = 400;
                        endingSectionY = 400;
                        break;
                }
                ThreadPool.QueueUserWorkItem(a => CreateSingleSection(startingSectionX, startingSectionY, endingSectionX, endingSectionY, height, width, buffer, depth));
            }
            Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save(@"E:\fractal\fractal.jpg", ImageFormat.Jpeg);
        }

        private static void CreateSingleSection(int startingSectionX, int startingSectionY, int endingSectionX, int endingSectionY, int height, int width, byte[] buffer, int depth)
        {
            for (var x = startingSectionX; x < endingSectionX; x++)
            {
                for (var y = startingSectionY; y < endingSectionY; y++)
                {
                    var a = (double)(x - width / 4) / (width / 4);
                    var b = (double)(y - height / 4) / (height / 4);
                    var c = new ComplexNumber(a, b);
                    var z = new ComplexNumber(0, 0);
                    var iterations = 0;
                    
                    do
                    {
                        iterations++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2.0)
                        {
                            break;
                        }
                    } while (iterations < 10000);
                    var offset = ((y * width) + x) * depth;
                    var o = 0.2126 * buffer[offset + 0] + 0.7152 * buffer[offset + 1] + 0.0722 * buffer[offset + 2];
                    buffer[offset + 0] = buffer[offset + 1] = buffer[offset + 2] = (byte)o;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }

        private void LoadChartContents()
        {
            List<Chart> lstSource = new List<Chart>();
            lstSource.Add(new Chart() { Utilization = 30, Time = 1 });
            lstSource.Add(new Chart() { Utilization = 25, Time = 2 });
            lstSource.Add(new Chart() { Utilization = 35, Time = 3 });
            lstSource.Add(new Chart() { Utilization = 20, Time = 4 });
            lstSource.Add(new Chart() { Utilization = 15, Time = 5 });
            ((ColumnSeries)LineChart.Series[0]).ItemsSource = lstSource;
        }
    }
}
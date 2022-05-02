using FractalBench.Classes;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
=======
using System.Threading.Tasks;
using System.Timers;
using Windows.Foundation;
using Windows.UI;
>>>>>>> Tom's
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

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
<<<<<<< HEAD
            CreateFractal(400, 400, 4);

=======
            CreateFractal(400, 400, 2);
>>>>>>> Tom's
        }

        private void CreateFractal(int width, int height, int noOfThreads) {
            var startingSectionX = 0;
            var startingSectionY = 0;
            var endingSectionX = 0;
            var endingSectionY = 0;
            var bitmap = new WriteableBitmap(width, height);
            var buffer = bitmap.PixelBuffer.ToArray();
            
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
                ThreadPool.QueueUserWorkItem(a => CreateSingleSection(startingSectionX, startingSectionY, endingSectionX, endingSectionY, height, width, buffer));
            }
            Thread.Sleep(5000);
            buffer.CopyTo(bitmap.PixelBuffer);
            fractalImage.Source = bitmap;
        }

        private static void CreateSingleSection(int startingSectionX, int startingSectionY, int endingSectionX, int endingSectionY, int height, int width, IList<byte> buffer)
        {
            for (var x = startingSectionX; x < endingSectionX; x++)
            {
                for (var y = startingSectionY; y < endingSectionY; y++)
                {
                    var a = (double)(x - width / 2.0) / (width / 4.0);
                    var b = (double)(y - height / 2.0) / (height / 4.0);
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
                    } while (iterations < 1000);
                    
                    // Color the bitmap
                    if (iterations < 1000)
                    {
                        var color = iterations * 255 / 1000;
                        buffer[x + y * width] = 100;
                        buffer[x + y * width + 1] = 100;
                        buffer[x + y * width + 2] = 255;
                    }
                    else
                    {
                        const int color = 240;
                        buffer[x + y * width] = color;
                        buffer[x + y * width] = color;
                        buffer[x + y * width] = 255;
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

<<<<<<< HEAD
        private void LoadChartContents()
        {
            List<Chart> lstSource = new List<Chart>();
            lstSource.Add(new Chart() { Utilization = 30, Time = 1 });
            lstSource.Add(new Chart() { Utilization = 25, Time = 2 });
            lstSource.Add(new Chart() { Utilization = 35, Time = 3 });
            lstSource.Add(new Chart() { Utilization = 20, Time = 4 });
            lstSource.Add(new Chart() { Utilization = 15, Time = 5 });
            ((ColumnSeries)LineChart.Series[0]).ItemsSource = lstSource;
=======
>>>>>>> Tom's
        }
    }
}
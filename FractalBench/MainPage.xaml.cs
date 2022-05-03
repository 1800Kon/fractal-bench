using FractalBench.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace FractalBench
{
    public sealed partial class MainPage : Page
    {
        private object _bitmap;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadChartContents();
        }

        //private void LoadChartContents()
        //{
        //    List<Chart> lstSource = new List<Chart>();
        //    lstSource.Add(new Chart() { Utilization = 30, Time = 1 });
        //    lstSource.Add(new Chart() { Utilization = 25, Time = 2 });
        //    lstSource.Add(new Chart() { Utilization = 35, Time = 3 });
        //    lstSource.Add(new Chart() { Utilization = 20, Time = 4 });
        //    lstSource.Add(new Chart() { Utilization = 15, Time = 5 });
        //    (LineChart.Series[0] as LineSeries).ItemsSource = lstSource;
        //}


        private async Task<StorageFile> WriteableBitmapToStorageFile(WriteableBitmap WB, FileFormat fileFormat)
        {
            string FileName = "MyFile.";
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            switch (fileFormat)
            {
                case FileFormat.Jpeg:
                    FileName += "jpeg";
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                    break;

                case FileFormat.Jpg:
                    FileName += "jpg";
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                    break;

                case FileFormat.Png:
                    FileName += "png";
                    BitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                    break;
            }

            var file = await Windows.Storage.DownloadsFolder.CreateFileAsync(FileName, CreationCollisionOption.GenerateUniqueName);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = WB.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                                    (uint)WB.PixelWidth,
                                    (uint)WB.PixelHeight,
                                    96.0,
                                    96.0,
                                    pixels);
                await encoder.FlushAsync();
            }
            //TODO: Task to assign the task to export the image;
            //TODO: Task to output the results;
            return file;
        }

        private enum FileFormat
        {
            Jpeg,
            Jpg,
            Png
        }

        private async void Render_Click(object sender, RoutedEventArgs e)
        {
            Mandelbrot mandelbrot = new Mandelbrot(400, 400, 4);
            var fractal = mandelbrot.CreateFractal();
            var fileType = FileFormat.Png;

            fractalImage.Source = fractal;

            try
            {
                if (fractal != null)
                {
                    await WriteableBitmapToStorageFile(fractal, fileType);
                }
                else
                {
                    Debug.WriteLine("not exported!!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
           
        }
    }
}
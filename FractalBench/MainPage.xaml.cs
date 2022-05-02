using FractalBench.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.Foundation;
using Windows.UI;
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
            Mandelbrot mandelbrot = new Mandelbrot();
            fractalImage.Source = mandelbrot.CreateFractal(400, 400, 4);
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
            (LineChart.Series[0] as LineSeries).ItemsSource = lstSource;
        }
    }
}
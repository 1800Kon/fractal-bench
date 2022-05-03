using FractalBench.Classes;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace FractalBench
{
    public sealed partial class MainPage : Page
    {
        FractalRenderer fractalRenderer = new FractalRenderer();
        public MainPage()
        {
            InitializeComponent();
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        { 
            var bitmap = fractalRenderer.CreateFractal(400, 400, 4);
            fractalImage.Source = bitmap;
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
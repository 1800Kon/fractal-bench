using FractalBench.Classes;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace FractalBench
{
    public sealed partial class MainPage : Page
    {
        FractalRenderer fractalRenderer = new FractalRenderer();
        ChartRenderer chartRenderer = new ChartRenderer();
        ObservableCollection<Chart> observableCollection = new ObservableCollection<Chart>();
        public ObservableCollection<Chart> LstSource
        {
            get { return observableCollection; }
        }
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            while (chartRenderer.isContinue)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                int i = chartRenderer.GetCpuUsage();
                chartRenderer.GetChartData(observableCollection, i);

                (LineChart1.Series[0] as LineSeries).ItemsSource = observableCollection;
                UsageText.Text = i.ToString();

                watch.Stop();

                var elapsedMs = watch.ElapsedMilliseconds;
                TextBlock2.Text = elapsedMs.ToString();

                await System.Threading.Tasks.Task.Delay(500);
            }
        }
    }
}
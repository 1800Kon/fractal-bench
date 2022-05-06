using FractalBench.Classes;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using static FractalBench.Classes.ExportImageTypes;

namespace FractalBench
{
    public sealed partial class MainPage : Page
    {
        FractalRenderer fractalRenderer = new FractalRenderer();
        ChartRenderer chartRenderer = new ChartRenderer();
        ExportFractalBase export = new ExportFractalBase();
        private int threadSelection;

        ObservableCollection<Chart> observableCollection = new ObservableCollection<Chart>();
        public ObservableCollection<Chart> LstSource
        {
            get { return observableCollection; }
        }
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Render_Click(object sender, RoutedEventArgs e)
        {
            chartRenderer.RenderChart(this);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            threadSelection += Convert.ToInt32(NoOfThreads.SelectedIndex) + 1;
            var bitmap = await fractalRenderer.CreateFractal(400, 400, threadSelection);
            fractalImage.Source = bitmap;

            var fileType = FileFormat.Png;

            if (bitmap != null)
            {
                await export.WriteableBitmapToStorageFile(bitmap, fileType);
            }
            else
            {
                Debug.WriteLine("not exported!!");
            }

            watch.Stop();
            chartRenderer.isContinue = false;
            var elapsedMs = watch.ElapsedMilliseconds;
            ElapsedText.Text = elapsedMs.ToString();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            chartRenderer.ClearChart();
            chartRenderer.isContinue = true;
            ElapsedText.Text = "elapsedMs";
        }
    }
}
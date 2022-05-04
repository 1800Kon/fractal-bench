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
        //ExportFractal file = new ExportFractal();
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
            var bitmap = fractalRenderer.CreateFractal(400, 400, 4);
            fractalImage.Source = bitmap;

            var fileType = FileFormat.Png;

            try
            {
                if (bitmap != null)
                {
                    await export.WriteableBitmapToStorageFile(bitmap, fileType);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chartRenderer.RenderChart(this);
        }
    }
}
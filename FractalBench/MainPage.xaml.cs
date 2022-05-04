using FractalBench.Classes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chartRenderer.RenderChart(this);
        }
    }
}
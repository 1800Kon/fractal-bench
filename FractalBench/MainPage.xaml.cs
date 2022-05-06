using FractalBench.Classes;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static FractalBench.Classes.ExportImageTypes;

namespace FractalBench
{
    public sealed partial class MainPage : Page
    {
        FractalRenderer fractalRenderer = new FractalRenderer();
        ChartRenderer chartRenderer = new ChartRenderer();
        ExportFractalBase export = new ExportFractalBase();
        private int threadSelection;
        private bool busy = false;
        private bool needsReset = false;

        public ObservableCollection<Chart> LstSource { get; } = new ObservableCollection<Chart>();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Render_Click(object sender, RoutedEventArgs e)
        {
            if (NoOfThreads.SelectedIndex.Equals(-1))
            {
                ContentDialog warning = new ContentDialog()
                {
                    Title = "Select threads!",
                    Content = "Please select the number of threads to use before rendering.",
                    CloseButtonText = "Ok"
                };
                await warning.ShowAsync();
                return;
            }
            if (busy)
            {
                ContentDialog warning = new ContentDialog()
                {
                    Title = "The app is busy!",
                    Content = "You are already generating a fractal, please wait until it is finished.",
                    CloseButtonText = "Ok"
                };
                await warning.ShowAsync();
                return;
            }

            if (needsReset)
            {
                ContentDialog warning = new ContentDialog()
                {
                    Title = "Reset the fractal!",
                    Content = "Please reset the fractal before continuing.",
                    CloseButtonText = "Ok"
                };
                await warning.ShowAsync();
                return;
            }
            busy = true;
            chartRenderer.RenderChart(this);
            var watch = Stopwatch.StartNew();
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
            busy = false;
            needsReset = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Reset the fractal
            if (needsReset && !busy)
            {
                chartRenderer.ClearChart();
                chartRenderer.isContinue = true;
                ElapsedText.Text = "elapsedMs";
                fractalImage.Source = null;
                needsReset = false;
            }
            else switch (busy)
            {
                // Do not reset the fractal
                case true when !needsReset:
                {
                    ContentDialog warning = new ContentDialog()
                    {
                        Title = "The app is busy!",
                        Content = "You are already generating a fractal, please wait until it is finished.",
                        CloseButtonText = "Ok"
                    };
                    await warning.ShowAsync();
                    break;
                }
                case false when !needsReset:
                {
                    ContentDialog warning = new ContentDialog()
                    {
                        Title = "Nothing to reset!",
                        Content = "There is nothing to reset.",
                        CloseButtonText = "Ok"
                    };
                    await warning.ShowAsync();
                    break;
                }
            }


        }
    }
}
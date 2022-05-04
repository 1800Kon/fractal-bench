using Org.Mentalis.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace FractalBench
{
    public class ChartRenderer
    {
        public bool isContinue = true;
        private static CpuUsage cpu;

        public ObservableCollection<Chart> LstSource { get; } = new ObservableCollection<Chart>();

        public async void RenderChart(MainPage mainPage)
        {
            while (isContinue)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                int usage = GetCpuUsage();

                GetChartData(usage, mainPage);
                mainPage.UsageText.Text = usage.ToString();

                watch.Stop();

                var elapsedMs = watch.ElapsedMilliseconds;
                mainPage.ElapsedText.Text = elapsedMs.ToString();
                await Task.Delay(500);
            }
        }

        public int GetCpuUsage()
        {
            int process = 0;
            cpu = CpuUsage.Create();
            while (isContinue)
            {
                process = cpu.Query();
                break;
            }
            return process;
        }

        public void GetChartData(int processUsage, MainPage mainPage)
        {
            observableCollection.Add(new Chart { Time = DateTime.Now, Utilization = processUsage });
            (mainPage.LineChart1.Series[0] as LineSeries).ItemsSource = observableCollection;
        }
    }
}

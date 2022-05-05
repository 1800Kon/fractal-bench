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

        ObservableCollection<Chart> observableCollection = new ObservableCollection<Chart>();
        public ObservableCollection<Chart> LstSource
        {
            get { return observableCollection; }
        }

        public async void RenderChart(MainPage mainPage)
        {
            while (isContinue)
            {
                int usage = GetCpuUsage();

                GetChartData(usage, mainPage);
                mainPage.UsageText.Text = usage.ToString() + "%";

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

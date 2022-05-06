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

        /// <summary>
        /// Render chart through GetCpuUsage() and GetChartData() and replace text from MainPage and repeat task per 500ms
        /// </summary>
        /// <param name="mainPage"></param>
        public async void RenderChart(MainPage mainPage)
        {
            while (isContinue)
            {
                int usage = GetCpuUsage();
                if (usage >= 0)
                {
                    GetChartData(usage, mainPage);
                    mainPage.UsageText.Text = usage.ToString() + "%";

                    await Task.Delay(500);
                }
            }
        }

        /// <summary>
        /// Use CpuUsage class to take info from system registry and repeat query
        /// </summary>
        /// <returns>Percentage of Cpu usage in int</returns>
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

        /// <summary>
        /// Add a new chart to the graph on call
        /// </summary>
        /// <param name="processUsage"></param>
        /// <param name="mainPage"></param>
        public void GetChartData(int processUsage, MainPage mainPage)
        {
            observableCollection.Add(new Chart { Time = DateTime.Now, Utilization = processUsage });
            (mainPage.LineChart1.Series[0] as LineSeries).ItemsSource = observableCollection;
        }

        public void ClearChart()
        {
            observableCollection.Clear();
        }
    }
}

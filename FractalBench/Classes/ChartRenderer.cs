using Org.Mentalis.Utilities;
using System;
using System.Collections.ObjectModel;

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

        public void GetChartData(ObservableCollection<Chart> observableCollection, int processUsage)
        {
            observableCollection.Add(new Chart { Time = DateTime.Now, Utilization = processUsage });
        }
    }
}

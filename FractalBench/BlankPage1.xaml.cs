using Org.Mentalis.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FractalBench
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        bool iscontinue = true;
        private static CpuUsage cpu;
        List<Chart> chart = new List<Chart>();
        ObservableCollection<Chart> observableCollection = new ObservableCollection<Chart>();
        public ObservableCollection<Chart> LstSource
        {
            get { return observableCollection; }
        }
        public BlankPage1()
        {
            this.InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            while (iscontinue) 
            {
                test();
                await System.Threading.Tasks.Task.Delay(500);
            } 
        }
        private void test()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int i = GetCpuUsage();
            GetChartData(i);
            TextBlock1.Text = i.ToString();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            TextBlock2.Text = elapsedMs.ToString();
        }

        private int GetCpuUsage()
        {
            int process = 0;
            // Creates and returns a CpuUsage instance that can be used to query the CPU time on this operating system.
            cpu = CpuUsage.Create();
            while (iscontinue)
            {
                process = cpu.Query();
                break;
            }
            return process;
        }

        private void GetChartData(int processUsage)
        {
            observableCollection.Add(new Chart { Time = DateTime.Now, Utilization = processUsage });
            (LineChart1.Series[0] as LineSeries).ItemsSource = observableCollection;
        }
    }
}

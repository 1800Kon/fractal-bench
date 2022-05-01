using Org.Mentalis.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i = PopulateCPUInfo();
            LoadChart(i);
            TextBlock1.Text = i.ToString();
        }

        private void BlankPage1_Load(object sender, EventArgs e) 
        {
            this.LineChart1.Series.Clear();
            this.LineChart1.Title = "CPU Usage";
        }

        private int PopulateCPUInfo()
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

        private void LoadChart(int processUsage)
        {
            observableCollection.Add(new Chart { Time = DateTime.Now, Utilization = processUsage });
            (LineChart1.Series[0] as LineSeries).ItemsSource = observableCollection;
        }
    }
}

using Org.Mentalis.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public BlankPage1()
        {
            this.InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock1.Text = PopulateCPUInfo().ToString();

            timing.Text = checkTime().ToString();
            //var time = checkTime();
            //for (int y = 0; y < time; y++)
            //{
            //    LoadChartContents(time);
            //}
            LoadChartContents();
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


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private int checkTime()
        {
            int datetime = DateTime.Now.Millisecond;
            return datetime;
        }

        private void LoadChartContents()
        {
            int time = checkTime();

            var data = PopulateCPUInfo();

            List<Chart> lstSource = new List<Chart>();

            for (int i = 0; i < data; i++, time++)
            {
                lstSource.Add(new Chart() { Utilization = data, Time = time });
            }
            (LineChart1.Series[0] as LineSeries).ItemsSource = lstSource;
        }
    }
}

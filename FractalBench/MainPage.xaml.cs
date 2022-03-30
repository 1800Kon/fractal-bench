using FractalBench.Classes;
using System.Drawing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FractalBench
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            DrawSet();
        }

        private void DrawSet()
        {
            int width = 400;
            int height = 400;
            WriteableBitmap writeableBmp = BitmapFactory.New(width, height);
            using (writeableBmp.GetBitmapContext())
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double a = (double)(x - (width / 2)) / (double)(width / 4);
                        double b = (double)(y - (height / 2)) / (double)(height / 4);
                        ComplexNumber c = new ComplexNumber(a, b);
                        ComplexNumber z = new ComplexNumber(0, 0);

                        int iterations = 0;
                        do
                        {
                            iterations++;
                            z.Square();
                            z.Add(c);
                            if (z.Magnitude() > 2.0)
                            {
                                break;
                            }
                        } while (iterations < 10000);

                        if (iterations < 10000)
                        {
                            writeableBmp.SetPixel(x, y, iterations < 100 ? Colors.Yellow : Colors.White);
                        }
                    }
                }
                fractalImage.Source = writeableBmp;
            }
        }
    }
}

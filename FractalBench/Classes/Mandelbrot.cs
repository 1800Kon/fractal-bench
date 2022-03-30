using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FractalBench.Classes
{
    class Mandelbrot : Fractal
    {
        public Mandelbrot()
        {
            WriteableBitmap Calculate()
            {
                //Change the size of the bitmap depending on the window
                int width = 500;
                int height = 500;
                int noOfThreads = 4;
                WriteableBitmap bmp = new WriteableBitmap(width, height);
                ThreadPool.SetMaxThreads(noOfThreads, noOfThreads);
                for (int x = 0; x < height; x++)
                {
                    int y = 0;
                    Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = noOfThreads }, i =>
                    {
                        double a = (x - width / 2.0) / (0.5 * width);
                        double b = (y - height / 2.0) / (0.5 * height);
                        ComplexNumber c = new ComplexNumber(a, b);
                        ComplexNumber z = new ComplexNumber(0, 0);
                        int iterations = 0;
                        do
                        {
                            iterations++;
                            z.Square();
                            z.Add(z);
                            if (z.Magnitude() > 2)
                            {
                                break;
                            }
                        } while (iterations < 100);
                        bmp.se
                        //(x, y, iterations < 100 ? Color.Black : Color.White);
                });
                }
                return bmp;
            }
        }
    }
}

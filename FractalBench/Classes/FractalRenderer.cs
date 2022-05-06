using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FractalBench.Classes
{
    class FractalRenderer
    {
        public async Task<WriteableBitmap> CreateFractal(int width, int height, int noOfThreads)
        {
            // The list of tasks in which needs to awaited
            var tasks = new List<Task>();
            var semaphore = new Semaphore(noOfThreads, noOfThreads);
            var startingSectionX = 0;
            var startingSectionY = 0;
            var endingSectionX = 0;
            var endingSectionY = 0;
            WriteableBitmap bitmap = BitmapFactory.New(width, height);
            var buffer = bitmap.PixelBuffer.ToArray();
            // Set the max amount of threads to the amount of threads specified in the app
            ThreadPool.SetMaxThreads(noOfThreads, noOfThreads);
            ThreadPool.SetMinThreads(noOfThreads, noOfThreads);

            for (var i = 0; i <= 3; i++)
            {
                // Switch to determine the starting section of the image from which to generate the fractal, each being 1/4 of the image
                switch (i)
                {
                    case 0:
                        startingSectionX = 0;
                        startingSectionY = 0;
                        endingSectionX = 200;
                        endingSectionY = 200;
                        break;

                    case 1:
                        startingSectionX = 200;
                        startingSectionY = 0;
                        endingSectionX = 400;
                        endingSectionY = 200;
                        break;

                    case 2:
                        startingSectionX = 0;
                        startingSectionY = 200;
                        endingSectionX = 200;
                        endingSectionY = 400;
                        break;

                    case 3:
                        startingSectionX = 200;
                        startingSectionY = 200;
                        endingSectionX = 400;
                        endingSectionY = 400;
                        break;
                }

                var x = startingSectionX;
                var y = startingSectionY;
                var sectionX = endingSectionX;
                var sectionY = endingSectionY;
                // Add the tasks to the list
                tasks.Add(Task.Run(() =>
                {
                    // Wait for the semaphore to be available
                    semaphore.WaitOne();
                    CreateSingleSection(x, y, sectionX, sectionY, height,
                        width, buffer, semaphore);
                }));
            }
            // Wait for all the tasks to finish
            var final = Task.WhenAll(tasks);
            try
            {
                await final;
            }
            catch
            {
                // ignored
            }
            // Copy the buffer to the bitmap
            buffer.CopyTo(bitmap.PixelBuffer);
            return final.Status == TaskStatus.RanToCompletion ? bitmap : null;
        }

        private static void CreateSingleSection(int startingSectionX, int startingSectionY, int endingSectionX,
            int endingSectionY, int height, int width, IList<byte> buffer, Semaphore semaphore)
        {
            // Fractal algorithm
            for (var x = startingSectionX; x < endingSectionX; x++)
            {
                for (var y = startingSectionY; y < endingSectionY; y++)
                {
                    var a = (double)(x - width / 2.0) / (width / 4.0);
                    var b = (double)(y - height / 2.0) / (height / 4.0);
                    var c = new ComplexNumber(a, b);
                    var z = new ComplexNumber(0, 0);
                    var iterations = 0;
                    do
                    {
                        iterations++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2.0)
                        {
                            break;
                        }
                    } while (iterations < 100000);
                    // Color the bitmap
                    if (iterations < 100000)
                    {
                        buffer[(((y * width) + x) * 4)] = 50;
                        buffer[(((y * width) + x) * 4) + 1] = 50;
                        buffer[(((y * width) + x) * 4) + 2] = 50;
                    }
                    else
                    {
                        buffer[(((y * width) + x) * 4)] = 150;
                        buffer[(((y * width) + x) * 4) + 1] = 150;
                        buffer[(((y * width) + x) * 4) + 2] = 150;
                    }
                }
            }
            // Release the semaphore
            semaphore.Release();
        }
    }
}
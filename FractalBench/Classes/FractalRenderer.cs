﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;
using Image = Windows.UI.Xaml.Controls.Image;

namespace FractalBench.Classes
{
    class FractalRenderer
    {
        public WriteableBitmap CreateFractal(int width, int height, int noOfThreads)
        {
            var tasks = new List<Task>();
            var semaphore = new Semaphore(noOfThreads, noOfThreads);
            var startingSectionX = 0;
            var startingSectionY = 0;
            var endingSectionX = 0;
            var endingSectionY = 0;
            var bitmap = new WriteableBitmap(width, height);
            var buffer = bitmap.PixelBuffer.ToArray();

            //ThreadPool.SetMaxThreads(noOfThreads, noOfThreads);
            //ThreadPool.SetMinThreads(noOfThreads, noOfThreads);

            for (var i = 0; i <= 3; i++)
            {
                switch (i)
                {
                    case 0:
                        startingSectionX = 0;
                        startingSectionY = 0;
                        endingSectionX = 199;
                        endingSectionY = 199;
                        break;

                    case 1:
                        startingSectionX = 200;
                        startingSectionY = 0;
                        endingSectionX = 400;
                        endingSectionY = 199;
                        break;

                    case 2:
                        startingSectionX = 0;
                        startingSectionY = 200;
                        endingSectionX = 199;
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
                tasks.Add(Task.Run(() =>
                {
                    semaphore.WaitOne();
                    CreateSingleSection(x, y, sectionX, sectionY, height,
                        width, buffer, semaphore);
                }));
            }

            var final = Task.WhenAll(tasks);
            try
            {
                final.Wait();
            }
            catch
            {
                // ignored
            }
            buffer.CopyTo(bitmap.PixelBuffer);
            return final.Status == TaskStatus.RanToCompletion ? bitmap : null;
        }

        private static void CreateSingleSection(int startingSectionX, int startingSectionY, int endingSectionX,
            int endingSectionY, int height, int width, IList<byte> buffer, Semaphore semaphore)
        {
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
                        buffer[x + y * width] = 100;
                        buffer[x + y * width + 1] = 100;
                        buffer[x + y * width + 2] = 255;
                    }
                    else
                    {
                        const int color = 240;
                        buffer[x + y * width] = color;
                        buffer[x + y * width] = color;
                        buffer[x + y * width] = 255;
                    }
                }
            }
            semaphore.Release();
        }
    }
}
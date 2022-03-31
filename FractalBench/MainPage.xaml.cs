using FractalBench.Classes;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

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
            CreateFractal(400, 400, 2);
        }

        private void CreateFractal(int height, int width, int noOfThreads)
        {
            List<WriteableBitmap> listOfBitmaps = new List<WriteableBitmap>();
            List<List<int>> listData = new List<List<int>>();
            ConcurrentQueue<int> listToUse = null;
            WriteableBitmap finalResult = BitmapFactory.New(width, height);

            int i = 0;

            WriteableBitmap writeableBmp0 = BitmapFactory.New(width, height);
            WriteableBitmap writeableBmp1 = BitmapFactory.New(width, height);
            WriteableBitmap writeableBmp2 = BitmapFactory.New(width, height);
            WriteableBitmap writeableBmp3 = BitmapFactory.New(width, height);

            ConcurrentQueue<int> bitmapData0 = new ConcurrentQueue<int>();
            ConcurrentQueue<int> bitmapData1 = new ConcurrentQueue<int>();
            ConcurrentQueue<int> bitmapData2 = new ConcurrentQueue<int>();
            ConcurrentQueue<int> bitmapData3 = new ConcurrentQueue<int>();

            listOfBitmaps.Add(writeableBmp0);
            //listOfBitmaps.Add(writeableBmp1);
            //listOfBitmaps.Add(writeableBmp2);
            //listOfBitmaps.Add(writeableBmp3);

            Parallel.For(i, 2, new ParallelOptions { MaxDegreeOfParallelism = noOfThreads, }, p =>
           {
               int startingSectionX = 0;
               int startingSectionY = 0;
               int endingSectionX = 0;
               int endingSectionY = 0;
               switch (i)
               {
                   case 0:
                       startingSectionX = 0;
                       startingSectionY = 0;
                       endingSectionX = 200;
                       endingSectionY = 200;
                       listToUse = bitmapData0;
                       break;

                   case 1:
                       startingSectionX = 200;
                       startingSectionY = 0;
                       endingSectionX = 400;
                       endingSectionY = 200;
                        // listToUse = bitmapData1;
                        break;

                   case 2:
                       startingSectionX = 0;
                       startingSectionY = 200;
                       endingSectionX = 200;
                       endingSectionY = 400;
                        //  listToUse = bitmapData2;
                        break;

                   case 3:
                       startingSectionX = 200;
                       startingSectionY = 200;
                       endingSectionX = 400;
                       endingSectionY = 400;
                        //   listToUse = bitmapData3;
                        break;
               }
               for (int x = startingSectionX; x < endingSectionX; x++)
               {
                   for (int y = startingSectionY; y < endingSectionY; y++)
                   {
                       double a = (double)(x - (width / 4)) / (double)(width / 4);
                       double b = (double)(y - (height / 4)) / (double)(height / 4);
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
                       } while (iterations < 1000);

                       if (iterations < 1000)
                       {

                           listToUse.Enqueue(x);
                           listToUse.Enqueue(y);
                           listToUse.Enqueue(iterations < 1000 ? 1 : 2);
                       }
                   }
               }
               i++;
           });

            // ADD ALL THE BITMAP DATA TO THE LIST AND THEN USE IT TO LOOP THROUGH AND RENDER ALL THE BITMAPS, TO CHANGE THE POSITION U ENED TO FIGURE OUT SOME LOOP IDK
            //listData.Add(bitmapData0);
            listData.Add(bitmapData1);
            listData.Add(bitmapData2);
            listData.Add(bitmapData3);



            int[] queueToArray = listToUse.ToArray();

            for (int q = 0; q < listOfBitmaps.Count; q++)
            {
                WriteableBitmap bitmap = listOfBitmaps[q];
                for (int o = 0; o < queueToArray.Length; o += 3)
                {
                    Color color;

                    if (queueToArray[0 +2].Equals(1))
                    {
                        color = Colors.DeepSkyBlue;
                    } else
                    {
                        color = Colors.DeepPink;
                    }
                    bitmap.SetPixel(queueToArray[o], queueToArray[o + 1], color);
                }
            }

            // This does not have to be multithreaded
            foreach (var bitmap in listOfBitmaps)
            {
                using (finalResult.GetBitmapContext())
                {
                    using (bitmap.GetBitmapContext())
                    {
                        finalResult.Blit(new Rect(0, 0, 100, 100), bitmap, new Rect(0, 0, 100, 100), WriteableBitmapExtensions.BlendMode.Additive);
                    }
                }
            }
            fractalImage.Source = finalResult;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace FractalBench.Classes
{
    public class ExportImageTypes
    {
        public enum FileFormat
        {
            Jpeg,
            Jpg,
            Png
        }
    }
}

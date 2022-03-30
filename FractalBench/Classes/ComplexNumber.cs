using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBench.Classes
{
    public class ComplexNumber
    {
        public double a;
        public double b;

        public ComplexNumber(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public void Square()
        {
            double temp = (a * a) - (b * b);
            b = 2.0 * a * b;
            a = temp;
        }

        public double Magnitude()
        {
            return Math.Sqrt((a * a) + (b * b));
        }

        public void Add(ComplexNumber c)
        {
            a += c.a;
            b += c.b;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Mathematics
{
    public struct ComplexNumber
    {
        public double Real;
        public double Imaginary;

        public ComplexNumber(double r, double i)
        {
            this.Real = r;
            this.Imaginary = i;
        }
    }
}

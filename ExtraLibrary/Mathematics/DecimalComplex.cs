using System;
using System.Globalization;
 
namespace System.Numerics
{
    public struct DecimalComplex : IEquatable<DecimalComplex>, IFormattable
    {

        // --------------SECTION: Private Data members ----------- //

        private Decimal m_real;
        private Decimal m_imaginary;

        // ---------------SECTION: Necessary Constants ----------- //

        private const Decimal LOG_10_INV = 0.43429448190325m;


        // --------------SECTION: Public Properties -------------- //

        public Decimal Real
        {
            get
            {
                return m_real;
            }
        }

        public Decimal Imaginary
        {
            get
            {
                return m_imaginary;
            }
        }

        public Decimal Magnitude
        {
            get
            {
                return DecimalComplex.Abs(this);
            }
        }

        public Decimal Phase
        {
            get
            {
                return Convert.ToDecimal(Math.Atan2(Convert.ToDouble(m_imaginary), Convert.ToDouble(m_real)));
            }
        }

        // --------------SECTION: Attributes -------------- //

        public static readonly DecimalComplex Zero = new DecimalComplex(0.0m, 0.0m);
        public static readonly DecimalComplex One = new DecimalComplex(1.0m, 0.0m);
        public static readonly DecimalComplex ImaginaryOne = new DecimalComplex(0.0m, 1.0m);

        // --------------SECTION: Constructors and factory methods -------------- //

        public DecimalComplex(Decimal real, Decimal imaginary)  /* Constructor to create a complex number with rectangular co-ordinates  */
        {
            this.m_real = real;
            this.m_imaginary = imaginary;
        }

        public static DecimalComplex FromPolarCoordinates(Decimal magnitude, Decimal phase) /* Factory method to take polar inputs and create a Complex object */
        {
            return new DecimalComplex(
                (magnitude * Convert.ToDecimal(Math.Cos(Convert.ToDouble(phase)))), 
                (magnitude * Convert.ToDecimal(Math.Sin(Convert.ToDouble(phase))))
            );
        }

        public static DecimalComplex Negate(DecimalComplex value)
        {
            return -value;
        }

        public static DecimalComplex Add(DecimalComplex left, DecimalComplex right)
        {
            return left + right;
        }

        public static DecimalComplex Subtract(DecimalComplex left, DecimalComplex right)
        {
            return left - right;
        }

        public static DecimalComplex Multiply(DecimalComplex left, DecimalComplex right)
        {
            return left * right;
        }

        public static DecimalComplex Divide(DecimalComplex dividend, DecimalComplex divisor)
        {
            return dividend / divisor;
        }

        // --------------SECTION: Arithmetic Operator(unary) Overloading -------------- //
        public static DecimalComplex operator -(DecimalComplex value)  /* Unary negation of a complex number */
        {

            return (new DecimalComplex((-value.m_real), (-value.m_imaginary)));
        }

        // --------------SECTION: Arithmetic Operator(binary) Overloading -------------- //       
        public static DecimalComplex operator +(DecimalComplex left, DecimalComplex right)
        {
            return (new DecimalComplex((left.m_real + right.m_real), (left.m_imaginary + right.m_imaginary)));
        }

        public static DecimalComplex operator -(DecimalComplex left, DecimalComplex right)
        {
            return (new DecimalComplex((left.m_real - right.m_real), (left.m_imaginary - right.m_imaginary)));
        }

        public static DecimalComplex operator *(DecimalComplex left, DecimalComplex right)
        {
            // Multiplication:  (a + bi)(c + di) = (ac -bd) + (bc + ad)i
            Decimal result_Realpart = (left.m_real * right.m_real) - (left.m_imaginary * right.m_imaginary);
            Decimal result_Imaginarypart = (left.m_imaginary * right.m_real) + (left.m_real * right.m_imaginary);
            return (new DecimalComplex(result_Realpart, result_Imaginarypart));
        }

        public static DecimalComplex operator /(DecimalComplex left, DecimalComplex right)
        {
            // Division : Smith's formula.
            decimal a = left.m_real;
            decimal b = left.m_imaginary;
            decimal c = right.m_real;
            decimal d = right.m_imaginary;

            if (Math.Abs(d) < Math.Abs(c))
            {
                decimal doc = d / c;
                return new DecimalComplex((a + b * doc) / (c + d * doc), (b - a * doc) / (c + d * doc));
            }
            else
            {
                decimal cod = c / d;
                return new DecimalComplex((b + a * cod) / (d + c * cod), (-a + b * cod) / (d + c * cod));
            }
        }


        // --------------SECTION: Other arithmetic operations  -------------- //

        public static Decimal Abs(DecimalComplex value)
        {

            //if (Decimal.IsInfinity(value.m_real) || Double.IsInfinity(value.m_imaginary))
            //{
            //    return double.PositiveInfinity;
            //}

            // |value| == sqrt(a^2 + b^2)
            // sqrt(a^2 + b^2) == a/a * sqrt(a^2 + b^2) = a * sqrt(a^2/a^2 + b^2/a^2)
            // Using the above we can factor out the square of the larger component to dodge overflow.


            decimal c = Math.Abs(value.m_real);
            decimal d = Math.Abs(value.m_imaginary);

            if (c > d)
            {
                decimal r = d / c;
                return c * Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(1.0m + r * r)));
            }
            else if (d == 0.0m)
            {
                return c;  // c is either 0.0 or NaN
            }
            else
            {
                decimal r = c / d;
                return d * Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(1.0m + r * r)));
            }
        }
        public static DecimalComplex Conjugate(DecimalComplex value)
        {
            // Conjugate of a Complex number: the conjugate of x+i*y is x-i*y 

            return (new DecimalComplex(value.m_real, (-value.m_imaginary)));

        }
        public static DecimalComplex Reciprocal(DecimalComplex value)
        {
            // Reciprocal of a Complex number : the reciprocal of x+i*y is 1/(x+i*y)
            if ((value.m_real == 0) && (value.m_imaginary == 0))
            {
                return DecimalComplex.Zero;
            }

            return DecimalComplex.One / value;
        }

        // --------------SECTION: Comparison Operator(binary) Overloading -------------- //

        public static bool operator ==(DecimalComplex left, DecimalComplex right)
        {
            return ((left.m_real == right.m_real) && (left.m_imaginary == right.m_imaginary));


        }
        public static bool operator !=(DecimalComplex left, DecimalComplex right)
        {
            return ((left.m_real != right.m_real) || (left.m_imaginary != right.m_imaginary));

        }

        // --------------SECTION: Comparison operations (methods implementing IEquatable<ComplexNumber>,IComparable<ComplexNumber>) -------------- //

        public override bool Equals(object obj)
        {
            if (!(obj is DecimalComplex)) return false;
            return this == ((DecimalComplex)obj);
        }
        public bool Equals(DecimalComplex value)
        {
            return ((this.m_real.Equals(value.m_real)) && (this.m_imaginary.Equals(value.m_imaginary)));

        }

        // --------------SECTION: Type-casting basic numeric data-types to ComplexNumber  -------------- //

        public static implicit operator DecimalComplex(Int16 value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        public static implicit operator DecimalComplex(Int32 value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        public static implicit operator DecimalComplex(Int64 value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        
        public static implicit operator DecimalComplex(UInt16 value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        
        public static implicit operator DecimalComplex(UInt32 value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        
        public static implicit operator DecimalComplex(UInt64 value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        
        public static implicit operator DecimalComplex(SByte value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        public static implicit operator DecimalComplex(Byte value)
        {
            return (new DecimalComplex(value, 0.0m));
        }
        public static implicit operator DecimalComplex(Single value)
        {
            return (new DecimalComplex(Convert.ToDecimal(value), 0.0m));
        }
        public static implicit operator DecimalComplex(Double value)
        {
            return (new DecimalComplex(Convert.ToDecimal(value), 0.0m));
        }
        
        public static explicit operator DecimalComplex(Decimal value)
        {
            return (new DecimalComplex(value, 0.0m));
        }

        // --------------SECTION: Formattig/Parsing options  -------------- //

        public override String ToString()
        {
            return (String.Format(CultureInfo.CurrentCulture, "({0}, {1})", this.m_real, this.m_imaginary));
        }

        public String ToString(String format)
        {
            return (String.Format(CultureInfo.CurrentCulture, "({0}, {1})", this.m_real.ToString(format, CultureInfo.CurrentCulture), this.m_imaginary.ToString(format, CultureInfo.CurrentCulture)));
        }

        public String ToString(IFormatProvider provider)
        {
            return (String.Format(provider, "({0}, {1})", this.m_real, this.m_imaginary));
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return (String.Format(provider, "({0}, {1})", this.m_real.ToString(format, provider), this.m_imaginary.ToString(format, provider)));
        }


        public override Int32 GetHashCode()
        {
            Int32 n1 = 99999997;
            Int32 hash_real = this.m_real.GetHashCode() % n1;
            Int32 hash_imaginary = this.m_imaginary.GetHashCode();
            Int32 final_hashcode = hash_real ^ hash_imaginary;
            return (final_hashcode);
        }



        // --------------SECTION: Trigonometric operations (methods implementing ITrigonometric)  -------------- //

        public static DecimalComplex Sin(DecimalComplex value)
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(
                Convert.ToDecimal(Math.Sin(Convert.ToDouble(a)) * Math.Cosh(Convert.ToDouble(b))), 
                Convert.ToDecimal(Math.Cos(Convert.ToDouble(a)) * Math.Sinh(Convert.ToDouble(b)))
            );
        }
                
        public static DecimalComplex Sinh(DecimalComplex value) /* Hyperbolic sin */
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(
                Convert.ToDecimal(Math.Sinh(Convert.ToDouble(a)) * Math.Cos(Convert.ToDouble(b))), 
                Convert.ToDecimal(Math.Cosh(Convert.ToDouble(a)) * Math.Sin(Convert.ToDouble(b)))
            );
        }
        public static DecimalComplex Asin(DecimalComplex value) /* Arcsin */
        {
            return (-ImaginaryOne) * Log(ImaginaryOne * value + Sqrt(One - value * value));
        }

        public static DecimalComplex Cos(DecimalComplex value)
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(
                Convert.ToDecimal(Math.Cos(Convert.ToDouble(a)) * Math.Cosh(Convert.ToDouble(b))), 
                Convert.ToDecimal(-(Math.Sin(Convert.ToDouble(a)) * Math.Sinh(Convert.ToDouble(b))))
            );
        }
                
        public static DecimalComplex Cosh(DecimalComplex value) /* Hyperbolic cos */
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(
                Convert.ToDecimal(Math.Cosh(Convert.ToDouble(a)) * Math.Cos(Convert.ToDouble(b))), 
                Convert.ToDecimal(Math.Sinh(Convert.ToDouble(a)) * Math.Sin(Convert.ToDouble(b)))
            );
        }
        public static DecimalComplex Acos(DecimalComplex value) /* Arccos */
        {
            return (-ImaginaryOne) * Log(value + ImaginaryOne * Sqrt(One - (value * value)));

        }
        public static DecimalComplex Tan(DecimalComplex value)
        {
            return (Sin(value) / Cos(value));
        }
        
        public static DecimalComplex Tanh(DecimalComplex value) /* Hyperbolic tan */
        {
            return (Sinh(value) / Cosh(value));
        }
        public static DecimalComplex Atan(DecimalComplex value) /* Arctan */
        {
            DecimalComplex Two = new DecimalComplex(2.0m, 0.0m);
            return (ImaginaryOne / Two) * (Log(One - ImaginaryOne * value) - Log(One + ImaginaryOne * value));
        }

        // --------------SECTION: Other numerical functions  -------------- //        

        public static DecimalComplex Log(DecimalComplex value) /* Log of the complex number value to the base of 'e' */
        {
            return (new DecimalComplex(
                Convert.ToDecimal((Math.Log(Convert.ToDouble(Abs(value))))),
                Convert.ToDecimal((Math.Atan2(Convert.ToDouble(value.m_imaginary), Convert.ToDouble(value.m_real)))))
            );
        }
        
        //public static DecimalComplex Log(DecimalComplex value, Decimal baseValue) /* Log of the complex number to a the base of a double */
        //{
        //    return (Log(value) / Log(baseValue));
        //}
        
        public static DecimalComplex Log10(DecimalComplex value) /* Log to the base of 10 of the complex number */
        {
            DecimalComplex temp_log = Log(value);
            return (Scale(temp_log, (Decimal)LOG_10_INV));

        }
        public static DecimalComplex Exp(DecimalComplex value) /* The complex number raised to e */
        {
            Decimal temp_factor = Convert.ToDecimal(Math.Exp(Convert.ToDouble(value.m_real)));
            Decimal result_re = temp_factor * Convert.ToDecimal(Math.Cos(Convert.ToDouble(value.m_imaginary)));
            Decimal result_im = temp_factor * Convert.ToDecimal(Math.Sin(Convert.ToDouble(value.m_imaginary)));
            return (new DecimalComplex(result_re, result_im));
        }
        
        public static DecimalComplex Sqrt(DecimalComplex value) /* Square root ot the complex number */
        {
            return DecimalComplex.FromPolarCoordinates(
                Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(value.Magnitude))), 
                (value.Phase / 2.0m)
            );
        }

        public static DecimalComplex Pow(DecimalComplex value, DecimalComplex power) /* A complex number raised to another complex number */
        {

            if (power == DecimalComplex.Zero)
            {
                return DecimalComplex.One;
            }

            if (value == DecimalComplex.Zero)
            {
                return DecimalComplex.Zero;
            }

            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            decimal c = power.m_real;
            decimal d = power.m_imaginary;

            decimal rho = DecimalComplex.Abs(value);
            decimal theta = Convert.ToDecimal(Math.Atan2(Convert.ToDouble(b), Convert.ToDouble(a)));
            decimal newRho = c * theta + d * Convert.ToDecimal(Math.Log(Convert.ToDouble(rho)));

            decimal t = Convert.ToDecimal(
                Math.Pow(Convert.ToDouble(rho), 
                Convert.ToDouble(c)) * Math.Pow(Math.E, Convert.ToDouble(-d * theta))
            );

            return new DecimalComplex(
                t * Convert.ToDecimal(Math.Cos(Convert.ToDouble(newRho))),
                t * Convert.ToDecimal(Math.Sin(Convert.ToDouble(newRho)))
            );
        }

        public static DecimalComplex Pow(DecimalComplex value, Decimal power) // A complex number raised to a real number 
        {
            return Pow(value, new DecimalComplex(power, 0));
        }



        //--------------- SECTION: Private member functions for internal use -----------------------------------//

        private static DecimalComplex Scale(DecimalComplex value, Decimal factor)
        {

            Decimal result_re = factor * value.m_real;
            Decimal result_im = factor * value.m_imaginary;
            return (new DecimalComplex(result_re, result_im));
        }
    }
}


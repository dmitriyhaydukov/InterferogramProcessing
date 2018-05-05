using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Numbers {
    //Рациональное число
    public class BigRationalNumber {
        BigInteger numerator;       //Числитель
        BigInteger denominator;     //Знаменатель
        //-----------------------------------------------------------------------------------------
        //Конструкторы
        public BigRationalNumber( BigInteger numerator, BigInteger denominator ) {
            this.numerator = numerator;
            this.denominator = denominator;
        }
        //-----------------------------------------------------------------------------------------
        public BigRationalNumber( int numerator, int denominator ) {
            this.numerator = new BigInteger( numerator );
            this.denominator = new BigInteger( denominator );
        }
        //-----------------------------------------------------------------------------------------
        public BigRationalNumber( long numerator, long denominator ) {
            this.numerator = new BigInteger( numerator );
            this.denominator = new BigInteger( denominator );
        }
        //-----------------------------------------------------------------------------------------
        public BigRationalNumber( string numerator, string denominator ) {
            this.numerator = BigInteger.Parse( numerator );
            this.denominator = BigInteger.Parse( denominator );
        }
        //-----------------------------------------------------------------------------------------
        public BigRationalNumber( double value ) {
            int power = 0;
            while ( ( value % 1 ) != 0 ) {
                value *= 10;
                power++;
            }
            this.numerator = new BigInteger( value );
            this.denominator = new BigInteger( Math.Pow( 10, power ) );
         }
        //-----------------------------------------------------------------------------------------
        //Наибольший общий делитель
        private BigInteger GetGreatestCommonDivisor() {
            
            BigInteger dividend = this.numerator;
            BigInteger divisor = this.denominator;
            BigInteger divisionResidue;

            while ( true ) {
                divisionResidue = dividend % divisor;
                if ( divisionResidue == 0 ) {
                    break;
                }
                dividend = divisor;
                divisor = divisionResidue;
            }
            return divisor;
        }
        //-----------------------------------------------------------------------------------------
        //Сокращение дроби
        private void ToReduce() {
            BigInteger greatestCommonDivisor =
                BigInteger.GreatestCommonDivisor( this.numerator, this.denominator );
            this.numerator = this.numerator / greatestCommonDivisor;
            this.denominator = this.denominator / greatestCommonDivisor;
        }
        //-----------------------------------------------------------------------------------------
        //Представление в виде строки
        public override string ToString() {
            string result;
            result = this.numerator.ToString() + "/" + this.denominator.ToString();
            return result;
        }
        //-----------------------------------------------------------------------------------------
        //Обратная дробь
        public BigRationalNumber GetInverseFraction() {
            BigRationalNumber inverseFraction =
                new BigRationalNumber( this.denominator, this.numerator );
            return inverseFraction;
        }
        //-----------------------------------------------------------------------------------------
        //Сложение
        public static BigRationalNumber operator +(
            BigRationalNumber operandOne,
            BigRationalNumber operandTwo
        ) {
            BigInteger numeratorOne = operandOne.numerator;
            BigInteger numeratorTwo = operandTwo.numerator;

            BigInteger denominatorOne = operandOne.denominator;
            BigInteger denominatorTwo = operandTwo.denominator;

            BigInteger newNumerator =
                numeratorOne * denominatorTwo +
                numeratorTwo * denominatorOne;

            BigInteger newDenominator = denominatorOne * denominatorTwo;

            BigRationalNumber newBigRationalNumber =
                new BigRationalNumber( newNumerator, newDenominator );
            
            newBigRationalNumber.ToReduce();
            return newBigRationalNumber;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //Вычитание
        public static BigRationalNumber operator -(
            BigRationalNumber operandOne,
            BigRationalNumber operandTwo
        ) {
            BigInteger numeratorOne = operandOne.numerator;
            BigInteger numeratorTwo = operandTwo.numerator;

            BigInteger denominatorOne = operandOne.denominator;
            BigInteger denominatorTwo = operandTwo.denominator;

            BigInteger newNumerator =
                numeratorOne * denominatorTwo -
                numeratorTwo * denominatorOne;

            BigInteger newDenominator = denominatorOne * denominatorTwo;

            BigRationalNumber newBigRationalNumber =
                new BigRationalNumber( newNumerator, newDenominator );

            newBigRationalNumber.ToReduce();
            return newBigRationalNumber;
        }
        //-----------------------------------------------------------------------------------------
        //Умножение
        public static BigRationalNumber operator *(
            BigRationalNumber operandOne,
            BigRationalNumber operandTwo
        ) {
            BigInteger numeratorOne = operandOne.numerator;
            BigInteger numeratorTwo = operandTwo.numerator;

            BigInteger denominatorOne = operandOne.denominator;
            BigInteger denominatorTwo = operandTwo.denominator;

            BigInteger newNumerator = numeratorOne * numeratorTwo;
            BigInteger newDenominator = denominatorOne * denominatorTwo;

            BigRationalNumber newBigRationalNumber =
                new BigRationalNumber( newNumerator, newDenominator );

            newBigRationalNumber.ToReduce();
            return newBigRationalNumber;
        }
        //-----------------------------------------------------------------------------------------
        //Умножение int на BigRationalNumber
        public static BigRationalNumber operator *(
            int operandOne,
            BigRationalNumber operandTwo
        ) {
            int numeratorOne = operandOne;
            BigInteger numeratorTwo = operandTwo.numerator;

            int denominatorOne = 1;
            BigInteger denominatorTwo = operandTwo.denominator;

            BigInteger newNumerator = numeratorOne * numeratorTwo;
            BigInteger newDenominator = denominatorOne * denominatorTwo;

            BigRationalNumber newBigRationalNumber =
                new BigRationalNumber( newNumerator, newDenominator );

            newBigRationalNumber.ToReduce();
            return newBigRationalNumber;
        }
        //-----------------------------------------------------------------------------------------
        //Деление
        public static BigRationalNumber operator /(
            BigRationalNumber operandOne,
            BigRationalNumber operandTwo
        ) {
            BigRationalNumber inverseOperandTwo = operandTwo.GetInverseFraction();          
            
            BigInteger numeratorOne = operandOne.numerator;
            BigInteger numeratorTwo = inverseOperandTwo.numerator;

            BigInteger denominatorOne = operandOne.denominator;
            BigInteger denominatorTwo = inverseOperandTwo.denominator;

            BigInteger newNumerator = numeratorOne * numeratorTwo;
            BigInteger newDenominator = denominatorOne * denominatorTwo;

            BigRationalNumber newBigRationalNumber =
                new BigRationalNumber( newNumerator, newDenominator );

            newBigRationalNumber.ToReduce();
            return newBigRationalNumber;
        }
        //-----------------------------------------------------------------------------------------
        //Представление в виде double
        public double ToDouble() {
            double dividend = (double) this.numerator;
            double divider = ( double )this.denominator;
            double result = dividend / divider;
            return result;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Mathematics.ModularArithmetic
{
    public static class ModularArithmeticHelper
    {
        public static int GetValue
        (
            int M1,
            int N1,
            int M2,
            int N2,
            int m1,
            int m2,
            int b1,
            int b2
        )
        {
            int value = (M1 * N1 * b1 + M2 * N2 * b2) % (m1 * m2);
            return value;
        }

        public static int GetValue
        (
            int M1,
            int N1,
            int M2,
            int N2,
            int M3,
            int N3,
            int m1,
            int m2,
            int m3,
            int b1,
            int b2,
            int b3
        )
        {
            int value = (M1 * N1 * b1 + M2 * N2 * b2 + M3 * N3 * b3) % (m1 * m2 * m3);
            return value;
        }

        public static int GetDiagonalIndex(int b1, int b2, int m1)
        {
            int index = b2 + m1 - 1 - b1;
            return index;
        }

        public static int[] GetDiagonalNumbersByb2(int m1, int m2, int M2, int N2)
        {
            int[] diagonalNumbersByb2 = new int[m2];
            for (int i = 0; i < m2; i++) // i = b2
            {
                int diagonalNumber = ((M2 * N2 * i) % (m1 * m2)) / m1;
                diagonalNumbersByb2[i] = diagonalNumber;
            }
            return diagonalNumbersByb2;
        }

        public static int[] GetDiagonalNumbersByb1(int m1, int m2, int M1, int N1)
        {
            int[] diagonalNumbersByb1 = new int[m1];
            for (int i = 0; i < m1; i++) // i = b1
            {
                int diagonalNumber = ((M1 * N1 * i) % (m1 * m2)) / m1;
                diagonalNumbersByb1[i] = diagonalNumber;
            }
            return diagonalNumbersByb1;
        }

        public static int[] GetDiagonalNumbersAugmented(int m1, int m2, int[] diagonalNumbersByb1, int[] diagonalNumbersByb2)
        {
            int[] diagonalNumbersAugmented = new int[m1 + m2 - 1];

            for (int k = diagonalNumbersByb1.Length - 1, j = 0; k >= 0; k--, j++)
            {
                diagonalNumbersAugmented[j] = diagonalNumbersByb1[k];
            }

            for (int k = 0, j = m1 - 1; k < diagonalNumbersByb2.Length; k++, j++)
            {
                diagonalNumbersAugmented[j] = diagonalNumbersByb2[k];
            }

            return diagonalNumbersAugmented;
        }

        public static int CalculateM1(int m1, int m2)
        {
            return m2;
        }

        public static int CalculateM2(int m1, int m2)
        {
            return m1;
        }

        public static int CalculateM1(int m1, int m2, int m3)
        {
            return (m2 * m3);// / m1;
        }

        public static int CalculateM2(int m1, int m2, int m3)
        {
            return (m1 * m3);// / m2;
        }

        public static int CalculateM3(int m1, int m2, int m3)
        {
            return (m1 * m2);// / m3;
        }

        public static int CalculateN(int M, int m)
        {
            int n = 1;
            int value = (M * n) % m;
            while (value != 1)
            {
                n++;
                value = (M * n) % m;
            }
            return n;
        }
    }
}

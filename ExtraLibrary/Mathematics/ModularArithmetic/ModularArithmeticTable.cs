using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Mathematics.ModularArithmetic
{
    public class ModularArithmeticTable
    {
        public int M1 { get; set; }
        public int M2 { get; set; }
        public int[,] Table { get; set; }

        public ModularArithmeticTable(int M1, int M2)
        {
            this.M1 = M1;
            this.M2 = M2;

            this.Table = new int[this.M1, this.M2];
            this.FillTable();
        }

        private void FillTable()
        {
            int M1C = ModularArithmeticHelper.CalculateM1(this.M1, this.M2);
            int M2C = ModularArithmeticHelper.CalculateM2(this.M1, this.M2);
            
            int N1 = ModularArithmeticHelper.CalculateN(M1C, M1);
            int N2 = ModularArithmeticHelper.CalculateN(M2C, M2);
            
            for (int b1 = 0; b1 < this.M1; b1++)
            {
                for (int b2 = 0; b2 < this.M2; b2++)
                {
                    int value = ModularArithmeticHelper.GetValue(M1C, N1, M2C, N2, M1, M2, b1, b2);
                    this.Table[b1, b2] = value;
                }
            }
        }
    }
}

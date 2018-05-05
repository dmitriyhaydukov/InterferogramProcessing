using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Randomness {
    //Генератор случайных чисел
    public class RandomNumberGenerator {
        Random random;
        //-----------------------------------------------------------------------------------
        public RandomNumberGenerator() {
            this.random = new Random( DateTime.Now.Millisecond );
        }
        //-----------------------------------------------------------------------------------
        //Случайное число от 0.0 до 1.0
        public double GetNextDouble() {
            double number = this.random.NextDouble();
            return number;
        }
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
        public int GetNextInteger(int maxValue) {
            return this.random.Next( maxValue );
        }
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
    }
}

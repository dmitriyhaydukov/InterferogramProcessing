using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class HalvedByte
    {
        public byte Full { get; set; }

        public byte Low
        {
            get { return (byte)(Full & 0x0F); }

            set
            {
                if (value >= 16)
                {
                    throw new ArithmeticException("Value must be between 0 and 16.");
                }

                this.Full = (byte)((High << 4) | (value & 0x0F));
            }
        }

        public byte High
        {
            get { return (byte)(Full >> 4); }

            set
            {
                if (value >= 16)
                {
                    throw new ArithmeticException("Value must be between 0 and 16.");
                }

                Full = (byte)((value << 4) | Low);
            }
        }

        public HalvedByte(byte full)
        {
            this.Full = full;
        }

        public HalvedByte(byte low, byte high)
        {
            if (low >= 16 || high >= 16)
            {
                throw new ArithmeticException("Values must be between 0 and 16.");
            }

            this.Full = (byte)((high << 4) | low);
        }
    }
}
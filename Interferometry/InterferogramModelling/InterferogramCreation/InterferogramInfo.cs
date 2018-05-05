using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interferometry.InterferogramCreation {
    //Параметры интерференционной картины
    public class InterferogramInfo {
        int width;                  //Ширина интерферограммы
        int height;                 //Высота интерферограммы

        double meanIntensity;       //Средняя интенсивность
        double intensityModulation; //Модуляция интенсивности
        double maxIntensity;        //Максимаьная интенсивность

        double noisePercent;        //Процент шума интенсивности
        double maxNoise;            //Максимальное значение шума
        //--------------------------------------------------------------------------------
        public InterferogramInfo(
            int width,
            int height,
            double percentNoise
        ) {
            this.width = width;
            this.height = height;
            this.noisePercent = percentNoise;

            this.meanIntensity = 127.5;
            this.maxIntensity = 255;
            this.intensityModulation = 1;
            this.maxNoise = this.maxIntensity / 100 * this.noisePercent;
        }
        //--------------------------------------------------------------------------------
        //Ширина
        public int Width {
            get {
                return this.width;
            }
        }
        //--------------------------------------------------------------------------------
        //Высота
        public int Height {
            get {
                return this.height; 
            }
        }
        //--------------------------------------------------------------------------------
        //Максимальная интенсивность
        public double MaxIntensity {
            get {
                return this.maxIntensity;
            }
        }
        //--------------------------------------------------------------------------------
        //Средняя интенсивность
        public double MeanIntensity {
            get {
                return this.meanIntensity;
            }
        }
        //--------------------------------------------------------------------------------
        //Модуляция интенсивности
        public double IntensityModulation {
            get {
                return this.intensityModulation;
            }
        }
        //--------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        //Максимальный шум
        public double MaxNoise {
            get {
                return this.maxNoise;
            }
        }
        //--------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------
    }
}

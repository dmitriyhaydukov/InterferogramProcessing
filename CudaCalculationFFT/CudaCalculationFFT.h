#include "cuda_runtime.h"

#define BLOCK_DIM 16
typedef double2 Complex;

int Add2(int * a, int * b, int * c, int d);
void CalcFFT(Complex* inputMatrix, Complex* devInputMatrix, int width, int height);
#include "CudaCalculationFFT.h"

extern "C" __declspec (dllexport) int  __cdecl   Add(int * a, int * b, int * c, int d)
{
	return  Add2(a, b, c, d);
}

extern "C" __declspec (dllexport) void __cdecl FFT(Complex* inputMatrix, Complex* outputMatrix, int width, int height)
{
	return CalcFFT(inputMatrix, outputMatrix, width, height);
}

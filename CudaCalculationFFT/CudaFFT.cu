#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include "device_functions.h"
#include <stdio.h>
#include <conio.h>
#include <cufft.h>
#include <locale.h>
#include "CudaCalculationFFT.h"

// Транспонирование матрицы c использования разделяемой памяти
//
// inputMatrix - указатель на исходную матрицу
// outputMatrix - указатель на матрицу результат
// width - ширина исходной матрицы (она же высота матрицы-результата)
// height - высота исходной матрицы (она же ширина матрицы-результата)
//
__global__ void transposeMatrixFast(Complex* inputMatrix, Complex* outputMatrix, int width, int height)
{
	__shared__ Complex temp[BLOCK_DIM][BLOCK_DIM];
	int xIndex = blockIdx.x * blockDim.x + threadIdx.x;
	int yIndex = blockIdx.y * blockDim.y + threadIdx.y;
	if ((xIndex < width) && (yIndex < height))
	{
		// Линейный индекс элемента строки исходной матрицы  
		int idx = yIndex * width + xIndex;
		//Копируем элементы исходной матрицы
		temp[threadIdx.y][threadIdx.x].x = inputMatrix[idx].x;
		temp[threadIdx.y][threadIdx.x].y = inputMatrix[idx].y;
	}
	//Синхронизируем все нити в блоке
	__syncthreads(); // компилятор ее не находит?!

	xIndex = blockIdx.y * blockDim.y + threadIdx.x;
	yIndex = blockIdx.x * blockDim.x + threadIdx.y;
	if ((xIndex < height) && (yIndex < width))
	{
		// Линейный индекс элемента строки исходной матрицы  
		int idx = yIndex * height + xIndex;
		//Копируем элементы исходной матрицы
		outputMatrix[idx].x = temp[threadIdx.x][threadIdx.y].x;
		outputMatrix[idx].y = temp[threadIdx.x][threadIdx.y].y;
	}
}

void CalcFFT(Complex* inputMatrix, Complex* outputMatrix, int width, int height)
{
	int matrixSize = width * height;
			
	Complex* devInputMatrix;

	cudaHostGetDevicePointer(&devInputMatrix, inputMatrix, 0);

	setlocale(LC_ALL, "Russian");
	
	// строим план для БПФ по столбцам
	cufftHandle planY;
	cufftPlan1d(&planY, height, CUFFT_Z2Z, width);
	// строим план для БПФ по строкам
	cufftHandle planX;
	cufftPlan1d(&planX, width, CUFFT_Z2Z, height);
	
	// формируем грид - определяющий размер по столбцам
	dim3 gridSize = dim3(width / BLOCK_DIM, height / BLOCK_DIM, 1);
	dim3 blockSize = dim3(BLOCK_DIM, BLOCK_DIM, 1);

	// транспонируем матрицу (БПФ по столбцам)
	transposeMatrixFast << < gridSize, blockSize >> >(devInputMatrix, devInputMatrix, width, height);
	cudaThreadSynchronize();
	cufftExecZ2Z(planY, (Complex *)devInputMatrix, (Complex *)devInputMatrix, CUFFT_INVERSE);
	cudaThreadSynchronize();

	// формируем грид - определяющий размер по строкам	
	gridSize = dim3(height / BLOCK_DIM, width / BLOCK_DIM, 1);
	blockSize = dim3(BLOCK_DIM, BLOCK_DIM, 1);
	// транспонируем матрицу (БПФ по строкам)
	transposeMatrixFast << < gridSize, blockSize >> >(devInputMatrix, devInputMatrix, height, width);
	cudaThreadSynchronize();
	cufftExecZ2Z(planX, (Complex *)devInputMatrix, (Complex *)devInputMatrix, CUFFT_INVERSE);
	cudaThreadSynchronize();
	
	printf("CUDA RESULT:\n");
	for (int i = 0; i < matrixSize; i++) { if (i % width == 0) printf("\n"); printf("(%4.1f,%4.1f)", inputMatrix[i].x, inputMatrix[i].y); }
	
	cudaFree(inputMatrix);
	cudaFreeHost(devInputMatrix);
		
	getch();
}
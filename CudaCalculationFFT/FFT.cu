#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include "device_functions.h"
#include <stdio.h>
#include <conio.h>
#include <cufft.h>
#include <locale.h>
#include "CudaCalculationFFT.h"


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
	cudaEvent_t startCopyInput, stopCopyInput;
	float elapsedTimeCopyInput = 0;

	cudaEvent_t startCopyOutput, stopCopyOutput;
	float elapsedTimeCopyOutput = 0;
	
	cudaEvent_t startCalcFFT, stopCalcFFT;
	float elapsedTimeCalcFFT = 0;

	cudaEvent_t startCreatePlan, stopCreatePlan;
	float elapsedTimeCreatePlan = 0;

	int matrixSize = width * height;
	int byteSize = matrixSize * sizeof(Complex);
	
	Complex* devInputMatrix;
	Complex* input;
	
	Complex* transposedMatrix;
	Complex* devTransposedMatrix;

	cudaHostAlloc((void**)&input, byteSize, cudaHostAllocMapped || cudaHostAllocWriteCombined);
	cudaHostAlloc((void**)&transposedMatrix, byteSize, cudaHostAllocMapped || cudaHostAllocWriteCombined);

	cudaEventCreate(&startCopyInput);
	cudaEventCreate(&stopCopyInput);
	cudaEventRecord(startCopyInput, 0);
	
	cudaMemcpy(input, inputMatrix, byteSize, cudaMemcpyHostToDevice);
	cudaThreadSynchronize();
	
	cudaEventRecord(stopCopyInput, 0);
	cudaEventSynchronize(stopCopyInput);
	cudaEventElapsedTime(&elapsedTimeCopyInput, startCopyInput, stopCopyInput);
	printf("\n\n (DLL) Copy input array time: %5.3f сек. \n\n", elapsedTimeCopyInput / 1000);

	cudaHostGetDevicePointer(&devInputMatrix, input, 0);
	cudaHostGetDevicePointer(&devTransposedMatrix, transposedMatrix, 0);
		
	setlocale(LC_ALL, "Russian");
	
	cudaEventCreate(&startCreatePlan);
	cudaEventCreate(&stopCreatePlan);
	cudaEventRecord(startCreatePlan, 0);
	
	// строим план для БПФ по столбцам
	cufftHandle planY;
	cufftPlan1d(&planY, height, CUFFT_Z2Z, width);
	// строим план для БПФ по строкам
	cufftHandle planX;
	cufftPlan1d(&planX, width, CUFFT_Z2Z, height);
		
	cudaEventRecord(stopCreatePlan, 0);
	cudaEventSynchronize(stopCreatePlan);
	cudaEventElapsedTime(&elapsedTimeCreatePlan, startCreatePlan, stopCreatePlan);
	printf("\n\n (DLL) Plan creation FFT time: %5.3f сек. \n\n", elapsedTimeCreatePlan / 1000);
	
	cudaEventCreate(&startCalcFFT);
	cudaEventCreate(&stopCalcFFT);
	cudaEventRecord(startCalcFFT, 0);

	// формируем грид - определяющий размер по столбцам
	dim3 gridSize = dim3(width / BLOCK_DIM, height / BLOCK_DIM, 1);
	dim3 blockSize = dim3(BLOCK_DIM, BLOCK_DIM, 1);
	
	// транспонируем матрицу (БПФ по столбцам)
	//transposeMatrixFast << < gridSize, blockSize >> >(devInputMatrix, devInputMatrix, width, height);
	//cudaThreadSynchronize();
	//cufftExecZ2Z(planY, (Complex *)devInputMatrix, (Complex *)devInputMatrix, CUFFT_INVERSE);
	//cudaThreadSynchronize();
	
	// транспонируем матрицу (БПФ по столбцам)
	transposeMatrixFast << < gridSize, blockSize >> >(devInputMatrix, devTransposedMatrix, width, height);
	cudaThreadSynchronize();
	cufftExecZ2Z(planY, (Complex *)devTransposedMatrix, (Complex *)devTransposedMatrix, CUFFT_INVERSE);
	cudaThreadSynchronize();
	

	// формируем грид - определяющий размер по строкам	
	gridSize = dim3(height / BLOCK_DIM, width / BLOCK_DIM, 1);
	blockSize = dim3(BLOCK_DIM, BLOCK_DIM, 1);
	
	// транспонируем матрицу (БПФ по строкам)
	transposeMatrixFast << < gridSize, blockSize >> >(devTransposedMatrix, devInputMatrix, height, width);
	cudaThreadSynchronize();
	
	cufftExecZ2Z(planX, (Complex *)devInputMatrix, (Complex *)devInputMatrix, CUFFT_INVERSE);
	cudaThreadSynchronize();
	
	cudaEventRecord(stopCalcFFT, 0);
	cudaEventSynchronize(stopCalcFFT);
	cudaEventElapsedTime(&elapsedTimeCalcFFT, startCalcFFT, stopCalcFFT);
	printf("\n\n (DLL) CUDA FFT Calculation time: %5.3f сек. \n\n", elapsedTimeCalcFFT / 1000);

	cudaEventCreate(&startCopyOutput);
	cudaEventCreate(&stopCopyOutput);
	cudaEventRecord(startCopyOutput, 0);
			
	cudaMemcpy(outputMatrix, input, byteSize, cudaMemcpyDeviceToHost);
	cudaThreadSynchronize();

	//cudaMemcpy(outputMatrix, inputMatrix, byteSize, cudaMemcpyDeviceToHost);
	//cudaThreadSynchronize();
	

	cudaEventRecord(stopCopyOutput, 0);
	cudaEventSynchronize(stopCopyOutput);
	cudaEventElapsedTime(&elapsedTimeCopyOutput, startCopyOutput, stopCopyOutput);
	printf("\n\n (DLL) Copy to output array time: %5.3f сек. \n\n", elapsedTimeCopyOutput / 1000);
	
	//printf("\nCUDA OUTPUT\n");
	//printf("(%4.1f,%4.1f)", input[0].x, input[0].y);

	/*
	for (int i = 0; i < matrixSize; i++) { 
		if (i % width == 0) printf("\n"); 
		printf("(%4.1f,%4.1f)", inputMatrix[i].x, inputMatrix[i].y);
	}
	*/
	
	getch();
}

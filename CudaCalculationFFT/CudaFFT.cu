#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include "device_functions.h"
#include <stdio.h>
#include <conio.h>
#include <cufft.h>
#include <locale.h>
#include "CudaCalculationFFT.h"

// ���������������� ������� c ������������� ����������� ������
//
// inputMatrix - ��������� �� �������� �������
// outputMatrix - ��������� �� ������� ���������
// width - ������ �������� ������� (��� �� ������ �������-����������)
// height - ������ �������� ������� (��� �� ������ �������-����������)
//
__global__ void transposeMatrixFast(Complex* inputMatrix, Complex* outputMatrix, int width, int height)
{
	__shared__ Complex temp[BLOCK_DIM][BLOCK_DIM];
	int xIndex = blockIdx.x * blockDim.x + threadIdx.x;
	int yIndex = blockIdx.y * blockDim.y + threadIdx.y;
	if ((xIndex < width) && (yIndex < height))
	{
		// �������� ������ �������� ������ �������� �������  
		int idx = yIndex * width + xIndex;
		//�������� �������� �������� �������
		temp[threadIdx.y][threadIdx.x].x = inputMatrix[idx].x;
		temp[threadIdx.y][threadIdx.x].y = inputMatrix[idx].y;
	}
	//�������������� ��� ���� � �����
	__syncthreads(); // ���������� �� �� �������?!

	xIndex = blockIdx.y * blockDim.y + threadIdx.x;
	yIndex = blockIdx.x * blockDim.x + threadIdx.y;
	if ((xIndex < height) && (yIndex < width))
	{
		// �������� ������ �������� ������ �������� �������  
		int idx = yIndex * height + xIndex;
		//�������� �������� �������� �������
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
	
	// ������ ���� ��� ��� �� ��������
	cufftHandle planY;
	cufftPlan1d(&planY, height, CUFFT_Z2Z, width);
	// ������ ���� ��� ��� �� �������
	cufftHandle planX;
	cufftPlan1d(&planX, width, CUFFT_Z2Z, height);
	
	// ��������� ���� - ������������ ������ �� ��������
	dim3 gridSize = dim3(width / BLOCK_DIM, height / BLOCK_DIM, 1);
	dim3 blockSize = dim3(BLOCK_DIM, BLOCK_DIM, 1);

	// ������������� ������� (��� �� ��������)
	transposeMatrixFast << < gridSize, blockSize >> >(devInputMatrix, devInputMatrix, width, height);
	cudaThreadSynchronize();
	cufftExecZ2Z(planY, (Complex *)devInputMatrix, (Complex *)devInputMatrix, CUFFT_INVERSE);
	cudaThreadSynchronize();

	// ��������� ���� - ������������ ������ �� �������	
	gridSize = dim3(height / BLOCK_DIM, width / BLOCK_DIM, 1);
	blockSize = dim3(BLOCK_DIM, BLOCK_DIM, 1);
	// ������������� ������� (��� �� �������)
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
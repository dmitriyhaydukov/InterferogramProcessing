#include "cuda_runtime.h"
#include "device_launch_parameters.h"
cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size);
__global__ void addKernel(int *c, const int *a, const int *b)
{
	int i = threadIdx.x;
	c[i] = a[i] + b[i];
}

int   Add2(int * aa, int * bb, int * cc, int dd) {
	cudaError_t cudaStatus = addWithCuda(cc, aa, bb, dd);
	cudaStatus = cudaDeviceReset();
	return 0;
};


/////////////////////////////////////////////////   
cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size)
{
	int *dev_a = 0;
	int *dev_b = 0;
	int *dev_c = 0;
	cudaError_t cudaStatus;
	cudaStatus = cudaSetDevice(0);
	cudaStatus = cudaMalloc((void**)&dev_c, size * sizeof(int));
	cudaStatus = cudaMalloc((void**)&dev_a, size * sizeof(int));
	cudaStatus = cudaMalloc((void**)&dev_b, size * sizeof(int));
	cudaStatus = cudaMemcpy(dev_a, a, size * sizeof(int), cudaMemcpyHostToDevice);
	cudaStatus = cudaMemcpy(dev_b, b, size * sizeof(int), cudaMemcpyHostToDevice);
	addKernel << <1, size >> >(dev_c, dev_a, dev_b);
	cudaStatus = cudaDeviceSynchronize();
	cudaStatus = cudaMemcpy(c, dev_c, size * sizeof(int), cudaMemcpyDeviceToHost);
	cudaFree(dev_c); cudaFree(dev_a); cudaFree(dev_b);
	return cudaStatus;
}
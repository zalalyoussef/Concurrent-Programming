#include <stdio.h>
#include <string>
#include <iostream>
#include <fstream>
#include <vector>
#include <omp.h>
#include <random>
#include <algorithm>
#define N 100
int main() {
    int arr1[N];
    int arr2[N];

    for (int i = 0;i < N;++i)
    {
        arr1[i] = i + 1;
        arr2[i] = i + 2;

    }

    int sum = 0;
#pragma omp parallel for reduction(+:sum)
    for (int i = 0;i<N;++i)
    {
        sum += arr1[i] + arr2[i];
    }
    printf("the total sum by reduction is equal to : %d \n", sum);

    int threads = omp_get_max_threads();
    std::vector<int> indexes(threads, -1);
#pragma omp parallel
    {
        int id = omp_get_thread_num();
#pragma omp for

            for (int i = 0;i < N;++i)
            {
                if (indexes[id] < i)
                    indexes[id] = i;
            }
            printf("thread id is euqla to : %d, and the max index is equal to : %d\n", id, indexes[id]);
    }
}
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Transactions;

namespace HSMulBenchmark2 {

    public unsafe class ComplexBenchmark {

        private const int DataSize = 1024;

        private SimdComplex.Complex* _simdData1;
        private SimdComplex.Complex* _simdData2;
        private SimdComplex.Complex* _simdData3;

        private NoSimdComplex.Complex* _noSimdData1 => (NoSimdComplex.Complex*)_simdData1;
        private NoSimdComplex.Complex* _noSimdData2 => (NoSimdComplex.Complex*)_simdData2;
        private NoSimdComplex.Complex* _noSimdData3 => (NoSimdComplex.Complex*)_simdData3;

        private System.Numerics.Complex* _snData1 => (System.Numerics.Complex*)_simdData1;
        private System.Numerics.Complex* _snData2 => (System.Numerics.Complex*)_simdData2;
        private System.Numerics.Complex* _snData3 => (System.Numerics.Complex*)_simdData3;

        [GlobalSetup]
        public void Setup() {
            var random = new Random();
            var _data1 = (SimdComplex.Complex*)NativeMemory.AlignedAlloc(checked(DataSize * (nuint)sizeof(Vector128<double>)), (nuint)sizeof(Vector128<double>));
            _simdData1 = _data1;
            var _data2 = (SimdComplex.Complex*)NativeMemory.AlignedAlloc(checked(DataSize * (nuint)sizeof(Vector128<double>)), (nuint)sizeof(Vector128<double>));
            _simdData2 = _data2;
            var _data3 = (SimdComplex.Complex*)NativeMemory.AlignedAlloc(checked(DataSize * (nuint)sizeof(Vector128<double>)), (nuint)sizeof(Vector128<double>));
            _simdData3 = _data3;

            for (int i = 0; i < DataSize; i++) {
                _data1[i] = new SimdComplex.Complex(random.NextDouble(), random.NextDouble());
                _data2[i] = new SimdComplex.Complex(random.NextDouble(), 0.0);
                _data3[i] = new SimdComplex.Complex(random.NextDouble(), random.NextDouble());
            }
        }

        [GlobalCleanup]
        public void Cleanup() {
            NativeMemory.AlignedFree(_simdData1);
            NativeMemory.AlignedFree(_simdData2);
        }

        [Benchmark]
        public void SimdComplexMultiplicationH() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] * data2[i];
            }
        }

        [Benchmark]
        public void NoSimdComplexMultiplicationH() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] * data2[i];
            }
        }

        [Benchmark]
        public void SystemComplexMultiplicationH() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] * data2[i];
            }
        }

        [Benchmark]
        public void SimdComplexMultiplicationS() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) * data2[i];
            }
        }

        [Benchmark]
        public void NoSimdComplexMultiplicationS() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) * data2[i];
            }
        }

        [Benchmark]
        public void SystemComplexMultiplicationS() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) * data2[i];
            }
        }

        static SimdComplex.Complex Mul2(double x, SimdComplex.Complex y) {
            return (SimdComplex.Complex)x * y;
        }

        static NoSimdComplex.Complex Mul2(double x, NoSimdComplex.Complex y) {
            return (NoSimdComplex.Complex)x * y;
        }

        static System.Numerics.Complex Mul2(double x, System.Numerics.Complex y) {
            return (System.Numerics.Complex)x * y;
        }

        [Benchmark]
        public void SimdComplexMultiplicationS2() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = Mul2(*(double*)(data1 + i), data2[i]);
            }
        }

        [Benchmark]
        public void NoSimdComplexMultiplicationS2() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = Mul2(*(double*)(data1 + i), data2[i]);
            }
        }

        [Benchmark]
        public void SystemComplexMultiplicationS2() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = Mul2(*(double*)(data1 + i), data2[i]);
            }
        }

        [Benchmark]
        public void SimdComplexAdditionH() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] + data2[i];
            }
        }

        [Benchmark]
        public void NoSimdComplexAdditionH() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] + data2[i];
            }
        }

        [Benchmark]
        public void SystemComplexAdditionH() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] + data2[i];
            }
        }

        [Benchmark]
        public void SimdComplexAdditionS() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) + data2[i];
            }
        }

        [Benchmark]
        public void NoSimdComplexAdditionS() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) + data2[i];
            }
        }

        [Benchmark]
        public void SystemComplexAdditionS() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) + data2[i];
            }
        }

        [Benchmark]
        public void SimdComplexNopH() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                _ = data1[i];
                data3[i] = data2[i];
            }
        }

        [Benchmark]
        public void NoSimdComplexNopH() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                _ = data1[i];
                data3[i] = data2[i];
            }
        }

        [Benchmark]
        public void SystemComplexNopH() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                _ = data1[i];
                data3[i] = data2[i];
            }
        }

        [Benchmark]
        public void SimdComplexNopS() {
            var data1 = _simdData1;
            var data2 = _simdData2;
            var data3 = _simdData3;
            for (int i = 0; i < DataSize; ++i) {
                _ = *(double*)(data1 + i);
                data3[i] = data2[i];
            }
        }

        [Benchmark]
        public void NoSimdComplexNopS() {
            var data1 = _noSimdData1;
            var data2 = _noSimdData2;
            var data3 = _noSimdData3;
            for (int i = 0; i < DataSize; ++i) {
                _ = *(double*)(data1 + i);
                data3[i] = data2[i];
            }
        }

        [Benchmark]
        public void SystemComplexNopS() {
            var data1 = _snData1;
            var data2 = _snData2;
            var data3 = _snData3;
            for (int i = 0; i < DataSize; ++i) {
                _ = *(double*)(data1 + i);
                data3[i] = data2[i];
            }
        }
    }

    class Program {

        static void Main(string[] args) {
            var summary = BenchmarkRunner.Run<ComplexBenchmark>();
        }
    }
}

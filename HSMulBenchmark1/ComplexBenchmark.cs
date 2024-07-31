using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace HSMulBenchmark1 {

    public unsafe class ComplexBenchmark {

        private const int DataSize = 1024;

        private System.Numerics.Complex* _data1;
        private System.Numerics.Complex* _data2;
        private System.Numerics.Complex* _data3;

        [GlobalSetup]
        public void Setup() {
            var random = new Random();
            var _data1 = (System.Numerics.Complex*)NativeMemory.AlignedAlloc(checked(DataSize * (nuint)sizeof(Vector128<double>)), (nuint)sizeof(Vector128<double>));
            this._data1 = _data1;
            var _data2 = (System.Numerics.Complex*)NativeMemory.AlignedAlloc(checked(DataSize * (nuint)sizeof(Vector128<double>)), (nuint)sizeof(Vector128<double>));
            this._data2 = _data2;
            var _data3 = (System.Numerics.Complex*)NativeMemory.AlignedAlloc(checked(DataSize * (nuint)sizeof(Vector128<double>)), (nuint)sizeof(Vector128<double>));
            this._data3 = _data3;

            for (int i = 0; i < DataSize; i++) {
                _data1[i] = new System.Numerics.Complex(random.NextDouble(), random.NextDouble());
                _data2[i] = new System.Numerics.Complex(random.NextDouble(), 0.0);
                _data3[i] = new System.Numerics.Complex(random.NextDouble(), random.NextDouble());
            }
        }

        [GlobalCleanup]
        public void Cleanup() {
            NativeMemory.AlignedFree(_data1);
            NativeMemory.AlignedFree(_data2);
            NativeMemory.AlignedFree(_data3);
        }

        [Benchmark]
        public void ComplexMultiplicationH() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] * data2[i];
            }
        }

        [Benchmark]
        public void SimdComplexMultiplicationS() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) * data2[i];
            }
        }


        static System.Numerics.Complex Mul2(double x, System.Numerics.Complex y) {
            return (System.Numerics.Complex)x * y;
        }

        [Benchmark]
        public void SimdComplexMultiplicationS2() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = Mul2(*(double*)(data1 + i), data2[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static System.Numerics.Complex Mul3(double x, System.Numerics.Complex y) {
            double re = (x * y.Real) - (0.0 * y.Imaginary);
            double im = (x * y.Imaginary) + (0.0 * y.Real);
            return new System.Numerics.Complex(re, im);
        }

        [Benchmark]
        public void SimdComplexMultiplicationS3() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = Mul3(*(double*)(data1 + i), data2[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static System.Numerics.Complex Mul4(double x, System.Numerics.Complex y) {
            double re = x * y.Real;
            double im = x * y.Imaginary;
            return new System.Numerics.Complex(re, im);
        }

        [Benchmark]
        public void SimdComplexMultiplicationS4() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = Mul4(*(double*)(data1 + i), data2[i]);
            }
        }

        [Benchmark]
        public void SimdComplexAdditionH() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = data1[i] + data2[i];
            }
        }


        [Benchmark]
        public void SimdComplexAdditionS() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                data3[i] = *(double*)(data1 + i) + data2[i];
            }
        }


        [Benchmark]
        public void ComplexNopH() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
            for (int i = 0; i < DataSize; ++i) {
                _ = data1[i];
                data3[i] = data2[i];
            }
        }

        [Benchmark]
        public void ComplexNopS() {
            var data1 = _data1;
            var data2 = _data2;
            var data3 = _data3;
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

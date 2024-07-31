using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;

namespace SimdComplex {

    public struct Complex {
        private Vector128<double> _value;

        public Complex(double real, double imaginary) {
            _value = Vector128.Create(real, imaginary);
        }

        public static Complex Zero = new Complex(Vector128<double>.Zero);

        public Complex(Vector128<double> value) {
            _value = value;
        }

        public static Complex operator *(Complex left, Complex right) {
            if (Fma.IsSupported) {
                var x = left._value;
                var y = right._value;

                var cc = Avx.Permute(y, 0b00);
                var ba = Avx.Permute(x, 0b01);
                var dd = Avx.Permute(y, 0b11);
                var dba = Sse2.Multiply(ba, dd);
                var mult = Fma.MultiplyAddSubtract(x, cc, dba);

                return new Complex(mult);
            } else {
                throw new PlatformNotSupportedException("FMA is not supported on this platform.");
            }
        }

        public static Complex operator *(double left, Complex right) {
            if (Sse2.IsSupported) {
                var x = left;
                var y = right._value;
                var mult = Sse2.MultiplyScalar(Vector128.Create(x), y);
                return new Complex(mult);
            } else {
                throw new PlatformNotSupportedException("FMA is not supported on this platform.");
            }
        }

        public static Complex operator +(Complex left, Complex right) {
            if (Sse2.IsSupported) {
                var x = left._value;
                var y = right._value;
                return new Complex(Sse2.Add(x, y));
            } else {
                throw new PlatformNotSupportedException("SSE2 is not supported on this platform.");
            }
        }

        public static Complex operator +(double left, Complex right) {
            if (Sse2.IsSupported) {
                var x = left;
                var y = right._value;
                return new Complex(Sse2.Add(Vector128.Create(x), y));
            } else {
                throw new PlatformNotSupportedException("SSE2 is not supported on this platform.");
            }
        }

        public static explicit operator Complex(double real) {
            return new Complex(Vector128.Create(real, 0.0));
        }

        public override string ToString() {
            return $"({_value.GetElement(0)}, {_value.GetElement(1)})";
        }
    }
}


namespace NoSimdComplex {

    public struct Complex {

        private double _real;
        private double _imaginary;

        public static Complex Zero = default;

        public Complex(double real, double imaginary) {
            _real = real;
            _imaginary = imaginary;
        }

        public static Complex operator *(Complex left, Complex right) {
            double real = (left._real * right._real) - (left._imaginary * right._imaginary);
            double imaginary = (left._real * right._imaginary) + (left._imaginary * right._real);
            return new Complex(real, imaginary);
        }

        public static Complex operator *(double left, Complex right) {
            double real = left * right._real;
            double imaginary = left * right._imaginary;
            return new Complex(real, imaginary);
        }

        public static Complex operator +(Complex left, Complex right) {
            double real = left._real + right._real;
            double imaginary = left._imaginary + right._imaginary;
            return new Complex(real, imaginary);
        }

        public static Complex operator +(double left, Complex right) {
            double real = left + right._real;
            double imaginary = right._imaginary;
            return new Complex(real, imaginary);
        }

        public static implicit operator Complex(double real) {
            return new Complex(real, 0.0);
        }

        public override string ToString() {
            return $"({_real}, {_imaginary})";
        }
    }
}

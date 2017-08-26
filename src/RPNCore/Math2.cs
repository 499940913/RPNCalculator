using System;

namespace RPNCore
{
    public static class Math2
    {
        private const double True = 1.0;
        private const double False = 0.0;
        public static double Plus(double a, double b)
        {
            return a + b;
        }
        public static double Minus(double a, double b)
        {
            return a - b;
        }
        public static double Negate(double a)//取反数
        {
            return a * -1;
        }
        public static double Mul(double a, double b)
        {
            return a * b;
        }
        public static double Div(double a, double b)
        {
            return a / b;
        }
        public static double Mod(double a, double b)
        {
            return a % b;
        }
        public static double And(double a, double b)
        {
            return !a.Equals(False) && !b.Equals(False) ? True : False;
        }
        public static double Or(double a, double b)
        {
            return !a.Equals(False) || !b.Equals(False) ? True : False;
        }
        public static double GreaterThan(double a, double b)
        {
            return a > b ? True : False;
        }
        public static double GreaterThanEqual(double a, double b)
        {
            return a >= b ? True : False;
        }
        public static double LessThanEqual(double a, double b)
        {
            return a <= b ? True : False;
        }
        public static double LessThan(double a, double b)
        {
            return a < b ? True : False;
        }
        public static double Equal(double a, double b)
        {
            return a.Equals(b) ? True : False;
        }
        public static double NotEqual(double a, double b)
        {
            return a.Equals(b) ? False : True;
        }
        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1));
        }
        public static double Acosh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x - 1));
        }
        public static double Atanh(double x)
        {
            return Math.Log((1 + x) * (1 - x)) / 2;
        }
    }
}

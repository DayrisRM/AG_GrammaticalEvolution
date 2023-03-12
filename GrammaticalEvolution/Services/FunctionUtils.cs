using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution.Services
{
    public static class FunctionUtils
    {
        public static double F1(double x) 
        {
            var pow = Math.Pow(x - 2, 2);
            var exp1 = -2 * pow;
            var eX1 = Math.Exp(exp1);

            return 8 * eX1 + ((2 * x) + 1) + 3 * Math.Tan((3 * x) + 2); 
        }
        public static double F2(double x) 
        {
            var pow = Math.Pow(x - 1, 2);
            var exp1 = -2 * pow;
            var exp2 = -pow;

            var eX1 = Math.Exp(exp1);
            var eX2 = Math.Exp(exp2);
            
            return 2 * eX1 - eX2;
        }
        public static double F3(double x) 
        { 
            return Math.Sqrt(x); 
        }
        public static double F4(double x) 
        {
            var eX = Math.Exp(x);
            var sen = Math.Sin(2 * x);
            return eX * sen;
        }

        public static double Kg(double x, double y, double c) 
        {
            var pow = Math.Pow(c - x, 2);
            var exp1 = -y * pow;
            return Math.Exp(exp1);
        }

        public static double Kp(double x, double a, double b, double d)
        {            
            var basePart = (a * x) + b;
            var pow = Math.Pow(basePart, d); 
            
            return RoundDouble(pow);
        }

        public static double Ks(double x, double a, double b)
        {            
            return Math.Tanh(a* x + b);
        }

        private static double RoundDouble(double value) 
        {
            string roundedDouble = value.ToString("0.00E+00", CultureInfo.InvariantCulture);
            return Convert.ToDouble(roundedDouble);
        }

    }
}

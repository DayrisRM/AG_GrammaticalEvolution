using GrammaticalEvolution.Services;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_UnitTests
{
    public class GrammarEvaluatorServiceUnitTests
    {
        [Test]
        public void GetKG_ShouldReturnValue() 
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var grammar = "KG(6.3E-2,9.9E-4,nulo2)";
            var x = 0.10;
            var kgVal = grammarEvaluatorService.GetKernel(grammar, x, "KG");
            Assert.IsNotNull(kgVal);
            Assert.That(kgVal.Contains("KG") != true);

            var parameter1 = Decimal.Parse(
                                        "6.3E-2",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var parameter2 = Decimal.Parse(
                                        "9.9E-4",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var valueKG = FunctionUtils.Kg(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2));
            valueKG = Math.Round(valueKG, 4);

            Assert.That(kgVal.Contains($"*({valueKG})") == true);
        }

        [Test]
        public void GetKP_ShouldReturnValue()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var grammar = "KP(2.5E+6,6.7E+2,1)";
            var x = 0.10;
            var kVal = grammarEvaluatorService.GetKernel(grammar, x, "KP");
            Assert.IsNotNull(kVal);
            Assert.That(kVal.Contains("KP") != true);

            var parameter1 = Decimal.Parse(
                                        "2.5E+6",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var parameter2 = Decimal.Parse(
                                        "6.7E+2",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var parameter3 = Decimal.Parse(
                                        "1",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var valueK = FunctionUtils.Kp(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2), Convert.ToDouble(parameter3));
            valueK = Math.Round(valueK, 4);

            Assert.That(kVal.Contains($"*({valueK})") == true);
        }

        [Test]
        public void GetKS_ShouldReturnValue()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var grammar = "KS(3.1E+4,7.3E-8,nulo1)";
            var x = 0.10;
            var kVal = grammarEvaluatorService.GetKernel(grammar, x, "KS");
            Assert.IsNotNull(kVal);
            Assert.That(kVal.Contains("KS") != true);

            var parameter1 = Decimal.Parse(
                                        "3.1E+4",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var parameter2 = Decimal.Parse(
                                        "7.3E-8",
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);
           

            var valueK = FunctionUtils.Ks(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2));
            valueK = Math.Round(valueK, 4);

            Assert.That(kVal.Contains($"*({valueK})") == true);
        }

        [Test]
        public void GetKernels_ShouldReturnValue()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var grammar = "KG(6.3E-2,9.9E-4,nulo2)+1.3E+5KS(3.1E+4,7.3E-8,nulo1)+1.3E-5KG(6.3E-2,9.9E-4,nulo2)";
            var x = 0.10;
            var kgVal = grammarEvaluatorService.GetKernel(grammar, x, "KG");
            kgVal = grammarEvaluatorService.GetKernel(kgVal, x, "KS");
            kgVal = grammarEvaluatorService.GetKernel(kgVal, x, "KP");
            Assert.IsNotNull(kgVal);
            Assert.That(kgVal.Contains("KG") != true);
            Assert.That(kgVal.Contains("KP") != true);
            Assert.That(kgVal.Contains("KS") != true);
        }

        [TestCase("*(0.9994)+1.3E+5*(1)+1.3E-5*(0.9994)")]
        [TestCase("-9.5E-3*(1.064E+22)-6.0E+0*(0.9938)")]
        public void ReplaceENotation_ShouldReturnValue(string grammar)
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            
            var val = grammarEvaluatorService.ReplaceENotation(grammar);

            Assert.IsNotNull(val);
            Assert.That(val.Contains("E") != true);

        }

        [TestCase("KG(6.3E-2,9.9E-4,nulo2)+1.3E+5KS(3.1E+4,7.3E-8,nulo1)+1.3E-5KG(6.3E-2,9.9E-4,nulo2)", 0.10)]
        [TestCase("+4.9E+2KS(4.4E+3,1.6E-8,nulo1)-6.9E-0KS(4.3E-7,3.6E+9,nulo1)-2.0E-2KP(3.5E+2,6.7E-9,2)", 0.10)]
        [TestCase("-3.4E-4KS(4.1E+9,9.7E-7,nulo2)-4.2E+1KG(2.3E-8,1.9E-9,nulo2)", 2)]
        [TestCase("-9.5E-3KP(8.6E-1,2.2E+7,3)-6.0E+0KG(9.6E-9,8.0E+2,nulo1)", -2)]
        public void Eval_ShouldReturnValue(string grammar, double x)
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();           
            
            var val = grammarEvaluatorService.Eval(grammar, x);
            Assert.IsNotNull(val);
            Assert.That(val != 0);            
        }

        [Test]
        public void toDelete()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var param1 = "3.5E-4";
            var param2 = "2.2E-6";
            var param3 = 2;

            var grammar = $"KP({param1},{param2},{param3})";
            var x = -2;
            var kgVal = grammarEvaluatorService.GetKernel(grammar, x, "KP");
            Assert.IsNotNull(kgVal);
            Assert.That(kgVal.Contains("KP") != true);

            var parameter1 = Decimal.Parse(
                                        param1,
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var parameter2 = Decimal.Parse(
                                        param2,
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var valueKG = FunctionUtils.Kp(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2), param3);
            valueKG = Math.Round(valueKG, 4);

            Assert.That(kgVal.Contains($"*({valueKG})") == true);
        }

        [Test]
        public void toDeleteKG()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var param1 = "3.8E+8";
            var param2 = "8.4E+2";
            var param3 = 2;

            var grammar = $"KG({param1},{param2},{param3})";
            var x = 1.7;
            var kgVal = grammarEvaluatorService.GetKernel(grammar, x, "KG");
            Assert.IsNotNull(kgVal);
            Assert.That(kgVal.Contains("KG") != true);

            var parameter1 = Decimal.Parse(
                                        param1,
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var parameter2 = Decimal.Parse(
                                        param2,
                                        NumberStyles.Float | NumberStyles.AllowExponent,
                                        CultureInfo.InvariantCulture);

            var valueKG = FunctionUtils.Kg(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2));
            
            Assert.That(kgVal.Contains($"*({valueKG})") == true);
        }

    }
}

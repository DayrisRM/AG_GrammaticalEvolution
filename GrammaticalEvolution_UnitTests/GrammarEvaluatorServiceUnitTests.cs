using GrammaticalEvolution.Services;
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

        [Test]
        public void ReplaceENotation_ShouldReturnValue()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var grammar = "*(0,9994)+1.3E+5*(1)+1.3E-5*(0,9994)";
            var val = grammarEvaluatorService.ReplaceENotation(grammar);

            Assert.IsNotNull(val);
            Assert.That(val.Contains("E") != true);

        }

        [Test]
        public void Eval_ShouldReturnValue()
        {
            GrammarEvaluatorService grammarEvaluatorService = new GrammarEvaluatorService();
            var grammar = "KG(6.3E-2,9.9E-4,nulo2)+1.3E+5KS(3.1E+4,7.3E-8,nulo1)+1.3E-5KG(6.3E-2,9.9E-4,nulo2)";
            var x = 0.10;
            var val = grammarEvaluatorService.Eval(grammar, x);
            Assert.IsNotNull(val);
            Assert.That(val != 0);            
        }

    }
}

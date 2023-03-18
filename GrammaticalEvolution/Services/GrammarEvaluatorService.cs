using GrammaticalEvolution.Abstractions;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using System.IO;
//using DynamicExpresso;
using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace GrammaticalEvolution.Services
{
    public class GrammarEvaluatorService : IGrammarEvaluator
    {
        public const string KernelKG = "KG";
        public const string KernelKP = "KP";
        public const string KernelKS = "KS";

        public GrammarEvaluatorService() 
        {
            RemoveLibraryMsg();
        }    

        public double Eval(string grammar, double x)
        {
            double eval = 0;
            var fn = "";

            if (grammar.Contains("<") || grammar.Contains(">"))
            {
               return double.MaxValue;
            }

            try
            {
                var replacedGrammar = ReplaceKernels(grammar, x);
                fn = replacedGrammar.Replace(",", ".");                 

                if (fn.StartsWith("*"))
                {
                    fn = fn.Insert(0, "1");
                }

                fn = fn.Replace(",", ".");

                Expression expression = new Expression(fn);
                eval = expression.calculate();

            }
            catch (Exception e) 
            {                
                Console.WriteLine("fn: " + fn);
                Console.WriteLine(grammar + "---" + x);
                throw;
            }            
            
            return eval;
        }

        public string ReplaceKernels(string grammar, double x) 
        {
            var kernelVal = GetKernel(grammar, x, "KG");
            kernelVal = GetKernel(kernelVal, x, "KS");
            kernelVal = GetKernel(kernelVal, x, "KP");

            return kernelVal;
        }
        public string GetKernel(string grammar, double x, string kernel) 
        {
            var grammarWithReplacement = grammar;            
            Regex fn = new Regex($"{kernel}\\(\\d+(\\.\\d+)?E[+-]\\d+,\\d+(\\.\\d+)?E[+-]\\d+,(nulo\\d+|\\+?\\d+(\\.\\d+)?E[+-]\\d+|\\d+)\\)", RegexOptions.Compiled);
            var matchesFn = fn.Matches(grammar);
            if (matchesFn.Count > 0)
            {
                foreach (Match match in matchesFn)
                {
                    double valueKernel = 0;
                    var replacedFn = match.Value.Replace($"{kernel}(", "").Replace(")", "");
                    var parametersFn = replacedFn.Split(",");
                    if (parametersFn.Length > 0)
                    {                        
                        var parameter1 = Decimal.Parse(
                                              parametersFn[0],
                                              NumberStyles.Float | NumberStyles.AllowExponent,
                                              CultureInfo.InvariantCulture);

                        var parameter2 = Decimal.Parse(
                                              parametersFn[1],
                                              NumberStyles.Float | NumberStyles.AllowExponent,
                                              CultureInfo.InvariantCulture);

                        switch (kernel) 
                        {
                            case KernelKG:
                                valueKernel = FunctionUtils.Kg(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2));
                                break;
                            case KernelKS:
                                valueKernel = FunctionUtils.Ks(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2));
                                break;
                            case KernelKP:
                                var parameter3 = Decimal.Parse(
                                                          parametersFn[2],
                                                          NumberStyles.Float | NumberStyles.AllowExponent,
                                                          CultureInfo.InvariantCulture);

                                valueKernel = FunctionUtils.Kp(x, Convert.ToDouble(parameter1), Convert.ToDouble(parameter2), Convert.ToDouble(parameter3));
                                break;
                        }
                        
                    }                   
                    grammarWithReplacement = grammarWithReplacement.Replace(match.Value.ToString(), $"*({valueKernel})");
                }
            }

            return grammarWithReplacement;
        }
        
        public string ReplaceENotation(string grammar) 
        {
            var grammarWithReplacement = grammar;

            //Regex _notationERegex = new Regex($"[+\\-](?:0|[1-9]\\d*)(?:\\.\\d+)?(?:[eE][+\\-]?\\d+)?", RegexOptions.Compiled);
            Regex _notationERegex = new Regex($"(?:0|[1-9]\\d*)(?:\\.\\d+)?[eE][+\\-]?[0-9]+", RegexOptions.Compiled);            

            var matches = _notationERegex.Matches(grammar);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    decimal h = Decimal.Parse(match.Value, NumberStyles.Float | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
                    grammarWithReplacement = grammarWithReplacement.Replace(match.Value.ToString(), $"{h}");                    
                }
            }

            return grammarWithReplacement;
        }

        private void RemoveLibraryMsg()
        {
            /* Non-Commercial Use Confirmation */
            var isCallSuccessful = License.iConfirmNonCommercialUse("John Doe");

            /* Verification if use type has been already confirmed */
            var isConfirmed = License.checkIfUseTypeConfirmed();

            /* Checking use type confirmation message */
            String message = License.getUseTypeConfirmationMessage();
        }

    }
}

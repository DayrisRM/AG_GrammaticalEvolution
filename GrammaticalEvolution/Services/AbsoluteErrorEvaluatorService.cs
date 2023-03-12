using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution.Services
{
    public class AbsoluteErrorEvaluatorService : IAbsoluteErrorEvaluator
    {
        private Function _functionToEval { get; set; }

        private const int K0 = 1;
        private const int K1 = 10;
        private const double U = 0.1;

        IGrammarEvaluator GrammarEvaluator { get; set; }

        public AbsoluteErrorEvaluatorService(Function functionToEval)
        {
            _functionToEval = functionToEval;
            GrammarEvaluator = new GrammarEvaluatorService();
        }
        public void Eval(Individual individual)
        {
            var sumError = GetSumError(_functionToEval, individual);
            var eval = ((double)1 / _functionToEval.M) * sumError;

            eval = Math.Round(eval, 4);

            individual.AbsoluteErrorEval = eval;
        }

        private int GetW(double abs)
        {
            int w = 0;

            if (abs <= U)
            {
                w = K0;
            }
            else
            {
                w = K1;
            }

            return w;
        }

        private double GetAbsFn(double fnEval, double grammarEval)
        {
            return Math.Abs(fnEval - grammarEval);
        }

        private double GetSumError(Function functionToEval, Individual individual)
        {
            double sumError = 0;

            foreach (var x in functionToEval.MValues)
            {
                //eval fn
                var fnVal = FunEval(functionToEval.Name, x);

                //eval grammarFn
                var grammarVal = GrammarEval(individual.Grammar, x);

                if (grammarVal == 0)
                {
                    Console.WriteLine($"GETSUM -- {individual.Grammar} -- {x} -- fnVal:{fnVal}");
                }
                else 
                {
                    var pepe = "";
                }

                //get absFN
                var abs = GetAbsFn(fnVal, grammarVal);

                //get w
                var w = GetW(abs);

                sumError += w * abs;
            }

            return sumError;
        }

        private double GrammarEval(string grammar, double x)
        {
            return GrammarEvaluator.Eval(grammar, x);           
        }

        private double FunEval(string functionName, double x)
        {
            double eval = 0;

            switch (functionName)
            {
                case "F1":
                    eval = FunctionUtils.F1(x);
                    break;
                case "F2":
                    eval = FunctionUtils.F2(x);
                    break;
                case "F3":
                    eval = FunctionUtils.F3(x);
                    break;
                case "F4":
                    eval = FunctionUtils.F4(x);
                    break;
            }

            eval = Math.Round(eval, 4);

            return eval;
        }

    }
}

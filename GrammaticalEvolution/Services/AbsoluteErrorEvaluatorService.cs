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
            bool reachHit = true;

            foreach (var x in functionToEval.MValues)
            {
                //eval fn
                var fnVal = FunEval(functionToEval, x);

                //eval grammarFn
                var grammarVal = GrammarEval(individual.Grammar, x);  
                
                //if(grammarVal == 0) 
                //{
                //    Console.WriteLine($"GrammarEval is 0 ---> grammar:{individual.Grammar} -- x:{x} -- fEval: {fnVal}");
                //}

                //get absFN
                var abs = GetAbsFn(fnVal, grammarVal);

                //get w
                var w = GetW(abs);
                if (w != 1) reachHit = false;

                sumError += w * abs;
                
                individual.EvaluationData.Add(x, new Evaluation() { FunctionEval = fnVal, GrammarEval = grammarVal });

            }

            individual.ReachHit = reachHit;

            return sumError;
        }

        private double GrammarEval(string grammar, double x)
        {
            return GrammarEvaluator.Eval(grammar, x);           
        }

        private double FunEval(Function function, double x)
        {
            double eval = function.Coords.Where(t => t.X == x).Select(x => x.Y).FirstOrDefault();
           
            //eval = Math.Round(eval, 4);

            return eval;
        }

    }
}

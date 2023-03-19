using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class FunctionEvaluatorService : IEvaluator<Function, Function>
    {
        public Function Evaluate(Function functionToEval)
        {
            foreach (var x in functionToEval.MValues) 
            {
                var eval = FnEval(functionToEval.Name, x);
                var coord = new Coords(x, eval);
                functionToEval.Coords.Add(coord);
            }

            return functionToEval;
        }

        private double FnEval(string functionName, double x)
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

            //eval = Math.Round(eval, 4);

            return eval;
        }

    }
}

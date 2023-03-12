using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class VAMMEvaluatorService : IEvaluator<List<Population>, double>
    {
        public double Evaluate(List<Population> populations)
        {           
            double sumOfError = 0;
            foreach (var population in populations) 
            {
                sumOfError += population.BestIndividual.AbsoluteErrorEval;
            }

            sumOfError /= populations.Count;

            return sumOfError;
        }
    }
}

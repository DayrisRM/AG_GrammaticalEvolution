using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class FitnessCalculatorService : IFitnessCalculator
    {
        IAbsoluteErrorEvaluator AbsoluteErrorEvaluator { get; set; }
        GrammarService GrammarService { get; set; }

        public FitnessCalculatorService(Function functionToEval, GrammarService grammarService) 
        {
            AbsoluteErrorEvaluator = new AbsoluteErrorEvaluatorService(functionToEval);
            GrammarService = grammarService;
        }

        public void Evaluate(Individual individual)
        {
            var grammarFn = GrammarService.GetGrammar(individual.Genotype);
            individual.Grammar = grammarFn;
            AbsoluteErrorEvaluator.Eval(individual);
        }
    }
}

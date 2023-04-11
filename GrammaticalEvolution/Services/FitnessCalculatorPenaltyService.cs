using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class FitnessCalculatorPenaltyService : IFitnessCalculator
    {
        IAbsoluteErrorEvaluator AbsoluteErrorEvaluator { get; set; }
        GrammarService GrammarService { get; set; }

        IDynamicPenaltyService DynamicPenaltyService { get; set; }

        public FitnessCalculatorPenaltyService(Function functionToEval, GrammarService grammarService) 
        {
            AbsoluteErrorEvaluator = new AbsoluteErrorEvaluatorService(functionToEval);
            GrammarService = grammarService;
            DynamicPenaltyService = new DynamicPenaltyService();
        }

        public void Evaluate(Individual individual)
        {
            var grammarFn = GrammarService.GetGrammar(individual.Genotype);
            individual.Grammar = grammarFn;
            
            //get number of kernels
            var numberOfKernelsInGrammar = GrammarService.GetNumberOfKernelsInGrammar(grammarFn);

            //calculate penalty
            var penalty = DynamicPenaltyService.CalculatePenalty(0, numberOfKernelsInGrammar);//TODOOOOO

            if (individual.EvaluationData.Keys.Count > 0) 
            {
                individual.EvaluationData = new Dictionary<double, Evaluation>();
            }

            AbsoluteErrorEvaluator.Eval(individual);
        }
    }
}

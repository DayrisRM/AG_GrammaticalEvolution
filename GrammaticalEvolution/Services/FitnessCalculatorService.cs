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

        IDynamicPenaltyService DynamicPenaltyService { get; set; }

        public FitnessCalculatorService(Function functionToEval, GrammarService grammarService) 
        {
            AbsoluteErrorEvaluator = new AbsoluteErrorEvaluatorService(functionToEval);
            GrammarService = grammarService;
            DynamicPenaltyService = new DynamicPenaltyService();
        }

        public void Evaluate(Individual individual, bool allowPenalty = false, int generationNumber = 0)
        {
            var grammarFn = GrammarService.GetGrammar(individual.Genotype);
            individual.Grammar = grammarFn;            

            if(individual.EvaluationData.Keys.Count > 0) 
            {
                individual.EvaluationData = new Dictionary<double, Evaluation>();
            }

            AbsoluteErrorEvaluator.Eval(individual);

            //Add penalty if is allowed
            if (allowPenalty)
            {
                //get number of kernels
                var numberOfKernelsInGrammar = GrammarService.GetNumberOfKernelsInGrammar(grammarFn);

                //calculate penalty
                var penalty = DynamicPenaltyService.CalculatePenalty(generationNumber, numberOfKernelsInGrammar);

                Console.WriteLine("--> Adding penalty to fitness:" + penalty);

                //modify fitness with the penalty
                individual.AbsoluteErrorEval += penalty;
            }
            
        }
    }
}

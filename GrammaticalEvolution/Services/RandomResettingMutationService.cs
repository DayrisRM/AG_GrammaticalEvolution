using GrammaticalEvolution.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution.Services
{
    public class RandomResettingMutationService : IMutationService<List<int>, List<int>>
    {
        private readonly IRandomGeneratorNumbersService _randomGeneratorNumbersService;
        private double _mutationProbability { get; set; }
        private Tuple<int, int> _mutationIndex { get; set; }

        public RandomResettingMutationService(IRandomGeneratorNumbersService randomGeneratorNumbersService, 
            double mutationProbability, Tuple<int, int> mutationIndex)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
            _mutationProbability = mutationProbability;
            _mutationIndex = mutationIndex;
        }
        public List<int> Mutate(List<int> individuals)
        {
            var mutatedInd = new List<int>();
            Console.WriteLine("--> Pm control - pm:" + _mutationProbability);

            foreach (var individual in individuals) 
            {
                var p = _randomGeneratorNumbersService.GetDouble();
                if (p < _mutationProbability)
                {
                    var newInd = GetIndividualValue(individual);
                    mutatedInd.Add(newInd);

                }
                else 
                {
                  mutatedInd.Add(individual);
                }
            }

            return mutatedInd;
        }

        private int GetIndividualValue(int individual)
        {
            var newInd = _randomGeneratorNumbersService.GetInt(_mutationIndex.Item1, _mutationIndex.Item2);
            if (newInd == individual)
            {
                newInd = GetIndividualValue(individual);
            }

            return newInd;
        }
    }
}


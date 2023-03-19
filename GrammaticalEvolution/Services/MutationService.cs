using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class MutationService
    {
        private double _mutationProbability { get; set; }
        
        private readonly IRandomGeneratorNumbersService _randomGeneratorNumbersService;

        private IMutationService<List<int>, List<int>> _randomResettingMutationService { get; set; }

        private Tuple<int, int> _mutationIndex { get; set; }

        public MutationService(double mutationProbability, IRandomGeneratorNumbersService randomGeneratorNumbersService, Tuple<int, int> mutationIndex)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
            _mutationProbability = mutationProbability;
            _mutationIndex = mutationIndex;
            _randomResettingMutationService = new RandomResettingMutationService(_randomGeneratorNumbersService, _mutationProbability, _mutationIndex);
            
        }
        
        public List<Individual> Mutate(List<Individual> individuals)
        {
            if (!individuals.Any())
                throw new ArgumentNullException(nameof(individuals));           

            foreach (var individual in individuals)
            {
                individual.Genotype = _randomResettingMutationService.Mutate(individual.Genotype);
            }

            return individuals;
        }

    }
}

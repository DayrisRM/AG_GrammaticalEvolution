using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class SwapMutationService : IMutationService<List<int>, List<int>>
    {       
       
        private readonly IRandomGeneratorNumbersService _randomGeneratorNumbersService;

        public SwapMutationService(IRandomGeneratorNumbersService randomGeneratorNumbersService)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
        }
        

        public List<int> Mutate(List<int> genotype) 
        {
            if (genotype.Count < 2)
                throw new ArgumentException("The genotype must have more than 2 genes.");

            var indices = _randomGeneratorNumbersService.GetUniqueInts(2, 0, genotype.Count);
            var firstIndice = indices[0];
            var secondIndice = indices[1];

            var firstIndiceValue = genotype[firstIndice];
            var secondIndiceValue = genotype[secondIndice];

            genotype[firstIndice] = secondIndiceValue;
            genotype[secondIndice] = firstIndiceValue;

            return genotype;

        }

    }
}

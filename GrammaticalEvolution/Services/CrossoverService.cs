using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class CrossoverService
    {
        private const int NumberOfParents = 2;

        private double _crossoverProbability { get; set; }

        private ICrossoverService _onePointCrossoverService { get; set; }

        private RandomGeneratorNumbersService _randomGeneratorNumbersService { get; set; }

        public CrossoverService(double crossoverProbability)
        {
            _onePointCrossoverService = new OnePointCrossoverService();
            _randomGeneratorNumbersService = new RandomGeneratorNumbersService();
            _crossoverProbability = crossoverProbability;
        }

        public List<Individual> SelectParentsAndCrossIfPossible(List<Individual> parents) 
        { 
            var chields = new List<Individual>();

            if(!parents.Any())
                throw new ArgumentNullException(nameof(parents));

            for (var i = 0; i < parents.Count; i += NumberOfParents) 
            {
                var parent1 = parents[i];
                var parent2 = parents[i + 1];
               

                var p = _randomGeneratorNumbersService.GetDouble();
                if(p <= _crossoverProbability) 
                {
                    var crossoverResult = _onePointCrossoverService.Cross(new List<Individual>() { (Individual)parent1.Clone(), (Individual)parent2.Clone() });
                    chields.AddRange(crossoverResult);
                }
                else 
                {
                    //Include parents to childs
                    chields.Add((Individual)parent1.Clone());
                    chields.Add((Individual)parent2.Clone());
                }

            }

            return chields;
        }

    }
}

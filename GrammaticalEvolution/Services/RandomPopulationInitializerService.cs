using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class RandomPopulationInitializerService : IPopulationInitializerService
    {

        private readonly IRandomGeneratorNumbersService _randomGeneratorNumbersService;

        public RandomPopulationInitializerService(IRandomGeneratorNumbersService randomGeneratorNumbersService)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
        }

        public Population Initialize(int numberMinCodons, int numberMaxCodons, int maxValueCodon, int initialNumberPopulation)
        {
            var actualGeneration = new Generation
            {
                GenerationNumber = 1,
                CreationDate = DateTime.Now,
                Individuals = CreateIndividuals(numberMinCodons, numberMaxCodons, maxValueCodon, initialNumberPopulation)
            };


            return new Population() { CurrentGeneration = actualGeneration };
        }

        private List<Individual> CreateIndividuals(int numberMinCodons, int numberMaxCodons, int maxValueCodon, int initialNumberPopulation) 
        {
            var individuals = new List<Individual>();

            for (int i = 1; i <= initialNumberPopulation; i++)
            {
                var numberCodons = _randomGeneratorNumbersService.GetInt(numberMinCodons, numberMaxCodons);               
                var codonsIndexes = _randomGeneratorNumbersService.GetUniqueInts(numberCodons, 0, maxValueCodon);
                if (codonsIndexes == null)
                {
                    throw new Exception("Codons indexes must not be null");
                }

                if (!(codonsIndexes.Distinct().Count() >= numberMinCodons && codonsIndexes.Distinct().Count() <= numberMaxCodons))
                {
                    throw new Exception("Repeated codons in the initialization");
                }

                var individual = new Individual()
                {
                    Id = i,
                    Genotype = codonsIndexes.ToList()
                };

                individuals.Add(individual);
            }

            return individuals;
        }

    }
}

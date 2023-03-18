using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class OnePointCrossoverService : ICrossoverService
    {
        private RandomGeneratorNumbersService _randomGeneratorNumbersService { get; set; }

        public OnePointCrossoverService()
        {
            _randomGeneratorNumbersService = new RandomGeneratorNumbersService();
        }

        public OnePointCrossoverService(RandomGeneratorNumbersService randomGeneratorNumbersService)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
        }

        public List<Individual> Cross(List<Individual> parents)
        {            
            var firstParent = parents[0];
            var secondParent = parents[1];            

            return CreateChilds(firstParent, secondParent);
        }

        private List<Individual> CreateChilds(Individual firstParent, Individual secondParent)
        {
            var childs = new List<Individual>();            
            var cutPoints = GetPointCross(firstParent, secondParent);
            var pointCrossFistParent = cutPoints.Item1;
            var pointCrossSecondParent = cutPoints.Item2;


            var firstPartParent1 = GetSegmentFromList(firstParent.Genotype, 0, pointCrossFistParent);
            var secondPartParent1 = GetSegmentFromList(firstParent.Genotype, pointCrossFistParent, firstParent.Genotype.Count);

            var firstPartParent2 = GetSegmentFromList(secondParent.Genotype, 0, pointCrossSecondParent);
            var secondPartParent2 = GetSegmentFromList(secondParent.Genotype, pointCrossSecondParent, secondParent.Genotype.Count);

            var child1Genotype = new List<int>();
            child1Genotype.AddRange(firstPartParent1);
            child1Genotype.AddRange(secondPartParent2);

            var child2Genotype = new List<int>();
            child2Genotype.AddRange(firstPartParent2);
            child2Genotype.AddRange(secondPartParent1);

            childs.Add(new Individual() { Genotype = child1Genotype });
            childs.Add(new Individual() { Genotype = child2Genotype });

            return childs;

        }

        private Tuple<int, int> GetPointCross(Individual firstParent, Individual secondParent)
        {
            var pointCrossFistParent = _randomGeneratorNumbersService.GetInt(1, firstParent.Genotype.Count - 1);
            var pointCrossSecondParent = pointCrossFistParent;
            if (firstParent.Genotype.Count != secondParent.Genotype.Count)
            {               
                pointCrossSecondParent = _randomGeneratorNumbersService.GetInt(1, secondParent.Genotype.Count - 1);
            }

            return new Tuple<int, int> (pointCrossFistParent, pointCrossSecondParent);
        }

        private List<int> GetSegmentFromList(List<int> genotype, int firstCutPoint, int secondCutPoint)
        {
            return genotype
                .Skip(firstCutPoint)
                .Take((secondCutPoint - firstCutPoint))
                .ToList();
        }       
    }
}

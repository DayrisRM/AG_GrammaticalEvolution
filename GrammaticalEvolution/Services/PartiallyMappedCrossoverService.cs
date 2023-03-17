using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class PartiallyMappedCrossoverService : ICrossoverService
    {
        private RandomGeneratorNumbersService _randomGeneratorNumbersService { get; set; }       

        public PartiallyMappedCrossoverService()
        {
            _randomGeneratorNumbersService = new RandomGeneratorNumbersService();
        }

        public PartiallyMappedCrossoverService(RandomGeneratorNumbersService randomGeneratorNumbersService)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
        }

        public List<Individual> Cross(List<Individual> parents)
        {          
            var descendants = new List<Individual>();

            var parent1 = parents[0];
            var parent2 = parents[1];
            var size = parent1.Genotype.Count;

            //select segment positions of chromosome
            var segmentIndices = GetSegmentIndices(size);
            var firstCutPoint = segmentIndices.Item1;
            var secondCutPoint = segmentIndices.Item2;

            //Cut segment
            var parent1CuttedSegment = GetSegmentFromList(parent1.Genotype, firstCutPoint, secondCutPoint);
            var parent2CuttedSegment = GetSegmentFromList(parent2.Genotype, firstCutPoint, secondCutPoint);

            //create childs with the segment of the parent
            var child2 = CreateEmptyGenotypeWithSegment(size, firstCutPoint, secondCutPoint, parent1CuttedSegment);
            var child1 = CreateEmptyGenotypeWithSegment(size, firstCutPoint, secondCutPoint, parent2CuttedSegment);

            //
            for (int i = 0; i < size; i++)
            {
                if (i >= firstCutPoint && i <= secondCutPoint)
                {
                    continue;
                }

                int x1 = 0;                
                var geneForChild1 = GetGeneNotInMappingSection(parent1.Genotype[i], parent2CuttedSegment, parent1CuttedSegment, x1);
                ReplaceGene(i, geneForChild1, child1);

                int y1 = 0;               
                var geneForChild2 = GetGeneNotInMappingSection(parent2.Genotype[i], parent1CuttedSegment, parent2CuttedSegment, y1);
                ReplaceGene(i, geneForChild2, child2);
            }

            descendants.Add(new Individual() { Genotype = child1 });
            descendants.Add(new Individual() { Genotype = child2 });


            var checkPoint2 = CheckIndividualHasRepeatedGene(descendants);
            if (checkPoint2.Item1 == true)
            {
                var pepe = checkPoint2.Item2[0].Genotype.GroupBy(x => x)
                     .Where(g => g.Count() > 1)
                     .Select(x => x.Key)
                     .ToList();

                throw new Exception("IndividualHasRepeatedGene checkpoint2");
            }

            return descendants;
        }

        public List<int> CreateEmptyGenotypeWithSegment(int size, int firstCutPoint, int secondCutPoint, List<int> segment)
        {
            //var newGenotype = new List<int>(new int[size]);
            //newGenotype.RemoveRange(firstCutPoint, secondCutPoint - firstCutPoint);
            //newGenotype.InsertRange(firstCutPoint, segment);
            var realSize = size - 1;
            var gen1 = new List<int>(new int[firstCutPoint]);
            var gen2 = new List<int>(new int[realSize - secondCutPoint]);

            var newGenotype = new List<int>();
            newGenotype.AddRange(gen1);
            newGenotype.AddRange(segment);
            newGenotype.AddRange(gen2);

            return newGenotype;
        }

        private Tuple<int, int> GetSegmentIndices(int countGenotype) 
        {
            var indices = _randomGeneratorNumbersService.GetUniqueInts(2, 0, countGenotype);
            Array.Sort(indices);
            
            return new Tuple<int, int>(indices[0], indices[1]);
        }

        private List<int> GetSegmentFromList(List<int> genotype, int firstCutPoint, int secondCutPoint) 
        {
            return genotype
                .Skip(firstCutPoint)
                .Take((secondCutPoint - firstCutPoint) + 1)
                .ToList();
        }
        
        private int GetGeneNotInMappingSection(int candidateGene, List<int> mappingSection, List<int> otherParentMappingSection, int numberIteracion)
        {
                
            numberIteracion++;
            
            if (numberIteracion > 1000)
            {
                throw new Exception("GetGeneNotInMappingSection exception");
            }
            
            int index = mappingSection.FindIndex(a => a == candidateGene);
            if (index == -1) 
            {
                return candidateGene;
            }

            return GetGeneNotInMappingSection(otherParentMappingSection[index], mappingSection, otherParentMappingSection, numberIteracion);

        }
        
        private void ReplaceGene(int position, int value, List<int> genotype) 
        {
            if (position > genotype.Count)
                throw new ArgumentException("Replace gen with invalid position.");

            genotype[position] = value;
        }


        private Tuple<bool, List<Individual>> CheckIndividualHasRepeatedGene(List<Individual> individuals)
        {
            var hasRepeated = false;
            var individualWithRepeatedGene = new List<Individual>();
            Parallel.ForEach(individuals, ind =>
            {
                var notRepeatedGenesLength = ind.Genotype.Distinct().Count();

                if (notRepeatedGenesLength < ind.Genotype.Count)
                {
                    hasRepeated = true;
                    individualWithRepeatedGene.Add(ind);
                }
            });

            return new Tuple<bool, List<Individual>>(hasRepeated, individualWithRepeatedGene);
        }

    }
}

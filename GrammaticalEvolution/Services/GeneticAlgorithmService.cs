using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class GeneticAlgorithmService
    {
        //Parameters algorithm
        private int _numberIterations { get; set; }
        private int _initialNumberPopulation { get; set; }
        private int _numberMaxCodons { get; set; }
        private int maxValueCodon { get; set; }
        private double _crossoverProbability { get; set; }
        private double _mutationProbability { get; set; }

        //Services
        private IPopulationInitializerService PopulationInitializerService { get; set; }
        private IFitnessCalculator FitnessCalculatorService { get; set; }
        private ISelectionService TournamentSelectionService { get; set; }
        private CrossoverService CrossoverService { get; set; }
        private MutationService MutationService { get; set; }
        private ISurvivorsSelectionService ElitistSurvivorsSelectionService { get; set; }
        private IPopulationService PopulationService { get; set; }



        public GeneticAlgorithmService()
        {
           
        }

        public Population EvolveAlgorithm() 
        {
            //Initialize population
            var population = PopulationInitializerService.Initialize(_numberMaxCodons, maxValueCodon, _initialNumberPopulation);

            //Evaluate population
            //CalculateFitness(population.CurrentGeneration.Individuals);

            //bucle
            var actualIteration = 1;
            while (actualIteration <= _numberIterations) 
            {
               
            }
            //return population. We will show the best individual
            return population;
        }

        private void CalculateFitness(List<Individual> individuals) 
        {
            Parallel.ForEach(individuals, ind =>
            {
                FitnessCalculatorService.Evaluate(ind);
            }
            );            
        }

        private Tuple<bool, List<Individual>> CheckIndividualHasRepeatedGene(List<Individual> individuals) 
        {
            var hasRepeated = false;
            var individualWithRepeatedGene = new List<Individual>();

            foreach (var ind in individuals) 
            {
                var notRepeatedGenesLength = ind.Genotype.Distinct().Count();

                if (notRepeatedGenesLength < ind.Genotype.Count)
                {
                    hasRepeated = true;
                    individualWithRepeatedGene.Add(ind);
                }
            }            

            return new Tuple<bool, List<Individual>>(hasRepeated, individualWithRepeatedGene);
        }

    }
}

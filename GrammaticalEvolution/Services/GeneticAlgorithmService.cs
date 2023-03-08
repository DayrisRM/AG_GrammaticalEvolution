using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;
using Newtonsoft.Json;

namespace GrammaticalEvolution.Services
{
    public class GeneticAlgorithmService
    {
        //Parameters algorithm
        private int _numberIterations { get; set; }
        private int _initialNumberPopulation { get; set; }
        private int _numberMaxCodons { get; set; }
        private int _numberMinCodons { get; set; }
        private int _maxValueCodon { get; set; }
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

        LoadFileGrammarBNFService loadFileGrammarBNFService = new LoadFileGrammarBNFService();
        

        public GeneticAlgorithmService()
        {
            PopulationInitializerService = new RandomPopulationInitializerService();
        }

        public Population EvolveAlgorithm() 
        {
            //Initialize population
            _initialNumberPopulation = 10;
            _numberMinCodons = 16;
            _numberMaxCodons = 100;
            _maxValueCodon = 256;

            var population = PopulationInitializerService.Initialize(_numberMinCodons, _numberMaxCodons, _maxValueCodon, _initialNumberPopulation);
            var grammarBNF = loadFileGrammarBNFService.LoadFile("grammarbnf.txt");

            //test
            //string json = JsonConvert.SerializeObject(grammarBNF);
            //var fileName = $"grammarBNFTEST.json";
            //var pathFile = @"../../../Data/grammars/" + fileName;

            //File.WriteAllTextAsync(pathFile, json);

            //

            GrammarService grammarService = new GrammarService(grammarBNF, true, 100);
            foreach (var pop in population.CurrentGeneration.Individuals) 
            {
                Console.WriteLine(string.Join(",", pop.Genotype));
                var grammarFn = grammarService.GetGrammar(pop.Genotype); 
                pop.Grammar = grammarFn;
                Console.WriteLine(grammarFn);
                Console.WriteLine("-----------");
            }

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

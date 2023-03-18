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
        private bool _allowWrapping { get; set; }
        private int _maxWrapping { get; set; }
        private double _crossoverProbability { get; set; }
        private double _mutationProbability { get; set; }

        private Function _functionToEval { get; set; }

        //Services
        private IPopulationInitializerService PopulationInitializerService { get; set; }
        private IFitnessCalculator FitnessCalculatorService { get; set; }
        private ISelectionService TournamentSelectionService { get; set; }
        private CrossoverService CrossoverService { get; set; }
        private MutationService MutationService { get; set; }
        private ISurvivorsSelectionService ElitistSurvivorsSelectionService { get; set; }
        private IPopulationService PopulationService { get; set; }

        private ILoadFileService<GrammarBNF> LoadFileGrammarBNFService { get; set; }

        private IAbsoluteErrorEvaluator AbsoluteErrorEvaluatorService { get; set; }

        private GrammarService GrammarService { get; set; }


        public GeneticAlgorithmService(int initialNumberPopulation, int numberIterations, double crossoverProbability, 
            double mutationProbability, Function functionToEval, int numberMinCodons, int numberMaxCodons, int maxValueCodon,
            bool allowWrapping, int maxWrapping, GrammarBNF grammar)
        {
            _initialNumberPopulation = initialNumberPopulation > 0 ? initialNumberPopulation : throw new ArgumentOutOfRangeException(nameof(initialNumberPopulation));
            _numberIterations = numberIterations > 0 ? numberIterations : throw new ArgumentOutOfRangeException(nameof(numberIterations));           
            _crossoverProbability = crossoverProbability;
            _mutationProbability = mutationProbability;
            _functionToEval = functionToEval;
            _numberMinCodons = numberMinCodons;
            _numberMaxCodons = numberMaxCodons;
            _maxValueCodon = maxValueCodon;
            _allowWrapping = allowWrapping;
            _maxWrapping = maxWrapping;

            PopulationInitializerService = new RandomPopulationInitializerService();
            GrammarService = new GrammarService(grammar, _allowWrapping, _maxWrapping);
            FitnessCalculatorService = new FitnessCalculatorService(functionToEval, GrammarService);
            TournamentSelectionService = new TournamentSelectionService(_initialNumberPopulation);
            CrossoverService = new CrossoverService(_crossoverProbability);
            MutationService = new MutationService(_mutationProbability);
            ElitistSurvivorsSelectionService = new ElitistSurvivorsSelectionService();
            PopulationService = new PopulationService();
            LoadFileGrammarBNFService = new LoadFileGrammarBNFService();
            AbsoluteErrorEvaluatorService = new AbsoluteErrorEvaluatorService(_functionToEval);            
        }

        public Population EvolveAlgorithm() 
        {
            Console.WriteLine("Initialize population and initial evaluation");

            //Initialize population
            var population = PopulationInitializerService.Initialize(_numberMinCodons, _numberMaxCodons, _maxValueCodon, _initialNumberPopulation);

            //Evaluate population
            CalculateFitness(population.CurrentGeneration.Individuals);


            //iterations
            var actualIteration = 1;
            while (actualIteration <= _numberIterations)
            {
                Console.WriteLine("Iteration " + actualIteration);

                //select parents by tournament              
                var tournamentResult = TournamentSelectionService.Select(population.CurrentGeneration.Individuals);

                //cross parents by partially mapped               
                var crossResult = CrossoverService.SelectParentsAndCrossIfPossible(tournamentResult);               

                //mutate using swap mutation
                var mutatedElements = MutationService.Mutate(crossResult);                

                //evaluate mutated elements
                mutatedElements = CleanEvaluationDataFitness(mutatedElements, actualIteration);
                CalculateFitness(mutatedElements);

                //select survivors
                var newIndividuals = ElitistSurvivorsSelectionService.SelectIndividuals(population.CurrentGeneration.Individuals, mutatedElements);

                //add new generation to population
                PopulationService.CreateNewGeneration(population, newIndividuals);
                actualIteration++;
            }
            //return population. We will show the best individual
            return population;
        }

        private void CalculateFitness(List<Individual> individuals) 
        {        
            foreach(var indx in individuals) 
            {               
                FitnessCalculatorService.Evaluate(indx);                
            }
        }

        private List<Individual> CleanEvaluationDataFitness(List<Individual> individuals, int actualIteration)
        {
            var i = 1;
            foreach (var ind in individuals)
            {
                var calculatedId = $"{actualIteration}{i}";
                ind.Id = Convert.ToInt32(calculatedId);
                ind.AbsoluteErrorEval = 0;
                ind.EvaluationData = new Dictionary<double, Evaluation>();
                ind.Grammar = "";

                i++;
            }

            return individuals;
        }
               

    }
}

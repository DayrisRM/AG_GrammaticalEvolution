using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution.Services
{
    public class LocalSearchService : ILocalSearchService
    {
        private const int NBestIndividuals = 2;
        private const int HillDescDistance = 1;
        private readonly IRandomGeneratorNumbersService _randomGeneratorNumbersService;
        private IFitnessCalculator _fitnessCalculatorService;

        public LocalSearchService(IRandomGeneratorNumbersService randomGeneratorNumbersService, IFitnessCalculator fitnessCalculatorService)
        {
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
            _fitnessCalculatorService = fitnessCalculatorService;
        }

        /// <summary>
        /// Se aplica la BL solo cuando el mejor individuo de la descendencia obtenida al finalizar cada generación es menor que el mejor individuos 
        /// de la población actual.        
        /// </summary>
        /// <param name="individuals">Descendencia obtenida</param>
        /// <param name="parents">Población actual</param>
        /// <returns></returns>
        public bool ShouldApplyLocalSearchByBestPerformance(List<Individual> individuals, List<Individual> parents)
        {
            var bestParentIndividual = parents.OrderBy(t => t.AbsoluteErrorEval).Select(t => t.AbsoluteErrorEval).FirstOrDefault();
            var bestChildIndividual = individuals.OrderBy(t => t.AbsoluteErrorEval).Select(t => t.AbsoluteErrorEval).FirstOrDefault();

            if (bestChildIndividual < bestParentIndividual)
                return true;

            return false;
        }


        public List<Individual> ApplyLocalSearch(List<Individual> individuals, LocalSearchDepthCriterion localSearchDepthCriterion, LocalSearchProbability localSearchProbability)
        {
            //Get best individuals
            if (localSearchProbability == LocalSearchProbability.NBestIndividuals)
            {
                return ApplyLocalSearchNBestIndividuals(individuals, localSearchDepthCriterion);
            }
            else if (localSearchProbability == LocalSearchProbability.FixedProbability)
            {
                //NotImplemented yet
                return ApplyLocalSearchFixedProbability(individuals, localSearchDepthCriterion);
            }


            return individuals;
        }

        private List<Individual> ApplyLocalSearchNBestIndividuals(List<Individual> individuals, LocalSearchDepthCriterion localSearchDepthCriterion)
        {
            var newIndividuals = new List<Individual>();
            var individualsToApplyLocalSearch = new List<Individual>();

            //Get best individuals
            var bestInd = individuals.OrderBy(t => t.AbsoluteErrorEval).Take(NBestIndividuals).ToList();
            individualsToApplyLocalSearch.AddRange(bestInd);

            newIndividuals.AddRange(ApplyLocalSearch(individualsToApplyLocalSearch, localSearchDepthCriterion));
            newIndividuals.AddRange(individuals.OrderBy(t => t.AbsoluteErrorEval).Skip(NBestIndividuals).ToList());

            return newIndividuals;
        }


        private List<Individual> ApplyLocalSearch(List<Individual> individualsToApplyLocalSearch, LocalSearchDepthCriterion localSearchDepthCriterion)
        {
            if (localSearchDepthCriterion == LocalSearchDepthCriterion.OnlyOneTime)
            {
                return ApplyLocalSearchOnlyOneTime(individualsToApplyLocalSearch);
            }
            else if (localSearchDepthCriterion == LocalSearchDepthCriterion.NeighborWithBestFitness)
            {
                //NotImplemented yet
                return ApplyLocalSearchNeighborWithBestFitness(individualsToApplyLocalSearch);
            }

            return individualsToApplyLocalSearch;
        }

        private List<Individual> ApplyLocalSearchOnlyOneTime(List<Individual> individualsToApplyLocalSearch)
        {
            var individualsWithLocalSearch = new List<Individual>();
            foreach (var individual in individualsToApplyLocalSearch)
            {
                individualsWithLocalSearch.Add(ApplyLocalSearchToIndividualOnlyOneTime(individual));
            }

            return individualsWithLocalSearch;
        }

        private Individual ApplyLocalSearchToIndividualOnlyOneTime(Individual individual)
        {
            var copiedInd = (Individual)individual.Clone();
            var fourthCodon = copiedInd.Genotype[3]; //4to codon
            var operatorHillDesc = _randomGeneratorNumbersService.GetInt(0, 2);
            if (operatorHillDesc == 0)
            {
                //sumar valor                
                copiedInd.Genotype[3] = fourthCodon + HillDescDistance;
            }
            else
            {
                //restar valor
                if (fourthCodon > 0)
                    copiedInd.Genotype[3] = fourthCodon - HillDescDistance;
            }

            _fitnessCalculatorService.Evaluate(copiedInd);

            if (copiedInd.AbsoluteErrorEval < individual.AbsoluteErrorEval)
            {
                Console.WriteLine("--> Modify individual");
                return copiedInd;
            }

            return individual;
        }


        private List<Individual> ApplyLocalSearchNeighborWithBestFitness(List<Individual> individualsToApplyLocalSearch)
        {
            throw new NotImplementedException();
        }
        private List<Individual> ApplyLocalSearchFixedProbability(List<Individual> individuals, LocalSearchDepthCriterion localSearchDepthCriterion)
        {
            throw new NotImplementedException();
        }
    }
}

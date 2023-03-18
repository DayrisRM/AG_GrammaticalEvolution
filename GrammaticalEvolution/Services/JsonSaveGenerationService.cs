using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class JsonSavePopulationService : ISavePopulationService
    {
        public void SavePopulationJson(int numberExecution, Population population)
        {
            var populationToSave = PreparePopulationToSave(population);
            string json = JsonConvert.SerializeObject(populationToSave);
            var fileName = $"{numberExecution}_population.json";
            var pathFile = @"../../../Data/populations/" + fileName;

            File.WriteAllTextAsync(pathFile, json);

        }

        public void SavePopulationFnEvalJson(int numberExecution, Population population)
        {
            var individualToSave = PrepareBestIndividualFnEvalToSave(population);
            string json = JsonConvert.SerializeObject(individualToSave);
            var fileName = $"{numberExecution}_population_best_ind_eval.json";
            var pathFile = @"../../../Data/populations/" + fileName;

            File.WriteAllTextAsync(pathFile, json);

        }

        private Individual PrepareBestIndividualFnEvalToSave(Population population)
        {
            var bestIndividual = new Individual()
            {
                Grammar = population.BestIndividual.Grammar,
                AbsoluteErrorEval = population.BestIndividual.AbsoluteErrorEval,
                EvaluationData = population.BestIndividual.EvaluationData
            };

            return bestIndividual;
        }

        private Population PreparePopulationToSave(Population population) 
        {            

            var populationToSave = new Population() 
            {
                CurrentGeneration = new Generation() 
                { 
                    GenerationNumber = population.CurrentGeneration.GenerationNumber, 
                    CreationDate = population.CurrentGeneration.CreationDate
                },                
                BestIndividual = new Individual()
                {
                    AbsoluteErrorEval = population.BestIndividual.AbsoluteErrorEval,                    
                }
            };

            if(population.CurrentGeneration.BestIndividual != null) 
            {
                populationToSave.CurrentGeneration.BestIndividual = new Individual()
                {
                    AbsoluteErrorEval = population.CurrentGeneration.BestIndividual.AbsoluteErrorEval,                    
                };
            }


            foreach (var generation in population.Generations) 
            {
                populationToSave.Generations.Add(new Generation()
                {
                    GenerationNumber = generation.GenerationNumber,
                    CreationDate = generation.CreationDate,
                    BestIndividual = new Individual() 
                    { 
                        AbsoluteErrorEval = generation.BestIndividual.AbsoluteErrorEval
                    },
                });
            }

            return populationToSave;
        }

    }
}

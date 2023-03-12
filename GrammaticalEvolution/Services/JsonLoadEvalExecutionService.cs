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
    public class JsonLoadEvalExecutionService : ILoadExecution<List<Individual>>
    {

        private int _numberExecutions { get; set; }

        public JsonLoadEvalExecutionService(int numberExecutions)
        {
            _numberExecutions = numberExecutions;
        }
               

        public List<Individual> Load()
        {
            var pathFolder = @"../../../Data/populations/";
            var savedIndividual = new List<Individual>();

            for (int i = 1; i <= _numberExecutions; i++)
            {
                var fileName = $"{i}_population_best_ind_eval.json";

                var population = LoadJson(pathFolder + fileName);
                savedIndividual.Add(population);
            }

            return savedIndividual;
        }

        private Individual LoadJson(string pathFile)
        {
            using (StreamReader r = new StreamReader(pathFile))
            {
                string json = r.ReadToEnd();
                var individual = JsonConvert.DeserializeObject<Individual>(json);

                if (individual == null)
                    throw new Exception("Json invalid");

                return individual;
            }
        }
    }
}

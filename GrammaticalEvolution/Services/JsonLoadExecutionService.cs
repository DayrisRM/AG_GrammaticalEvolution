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
    public class JsonLoadExecutionService : ILoadExecution<List<Population>>
    {

        private int _numberExecutions { get; set; }

        public JsonLoadExecutionService(int numberExecutions)
        {
            _numberExecutions = numberExecutions;
        }
               

        public List<Population> Load()
        {
            var pathFolder = @"../../../Data/generations/";
            var savedPopulation = new List<Population>();

            for (int i = 1; i <= _numberExecutions; i++)
            {
                var fileName = $"{i}_population.json";

                var population = LoadJson(pathFolder + fileName);
                savedPopulation.Add(population);
            }

            return savedPopulation;
        }

        private Population LoadJson(string pathFile)
        {
            using (StreamReader r = new StreamReader(pathFile))
            {
                string json = r.ReadToEnd();
                var population = JsonConvert.DeserializeObject<Population>(json);

                if (population == null)
                    throw new Exception("Json invalid");

                return population;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public class Population
    {
        public Generation CurrentGeneration { get; set; }

        public List<Generation> Generations { get; set; } = new List<Generation>();

        public Individual BestIndividual { get; set; }
        
    }
}

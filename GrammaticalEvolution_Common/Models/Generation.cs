using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public class Generation
    {
        public int GenerationNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Individual> Individuals { get; set; } = new List<Individual>();
        public Individual BestIndividual { get; set; }
    }
}

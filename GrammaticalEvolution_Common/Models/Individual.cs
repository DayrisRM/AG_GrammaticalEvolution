using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public class Individual
    {
        public int Id { get; set; }
        public List<int> Genotype { get; set; } = new List<int>();
        public double Distance { get; set; }
        public string Grammar { get; set; }

        //campos relacionados con el fitness
        public double GrammarEval { get; set; } 
        public double FunctionEval { get; set; }
        public double AbsoluteErrorEval { get; set; }

    }
}

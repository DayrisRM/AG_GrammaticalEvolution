using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    [Serializable()]
    public class Evaluation
    {
        public double GrammarEval { get; set; }
        public double FunctionEval { get; set; }
    }
}

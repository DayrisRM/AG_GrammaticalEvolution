using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public class GrammarBNF
    {
        //Start symbol
        public string S { get; set; }

        //Set of non terminals
        public List<string> N { get; set; } = new List<string>();

        //set of terminals
        public List<string> T { get; set; } = new List<string>();

        //set of production rules that maps the elements of N to T
        public Dictionary<string, List<string>> P { get; set; } = new Dictionary<string, List<string>>();
        
    }
}

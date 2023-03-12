﻿using System;
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
        public string Grammar { get; set; }       
        public double AbsoluteErrorEval { get; set; }

        public Dictionary<double, Evaluation> EvaluationData { get; set; } = new Dictionary<double, Evaluation>();
        

    }
}

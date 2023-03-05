using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface ISurvivorsSelectionService
    {
        public List<Individual> SelectIndividuals(List<Individual> parents, List<Individual> children);
    }
}

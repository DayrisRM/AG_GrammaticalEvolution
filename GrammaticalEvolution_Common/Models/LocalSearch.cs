using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public enum LocalSearchDepthCriterion
    {
        OnlyOneTime,
        NeighborWithBestFitness
    }

    public enum LocalSearchProbability
    {
        FixedProbability,
        NBestIndividuals
    }

}

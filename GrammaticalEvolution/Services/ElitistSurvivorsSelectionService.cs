using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class ElitistSurvivorsSelectionService : ISurvivorsSelectionService
    {
        public List<Individual> SelectIndividuals(List<Individual> parents, List<Individual> children)
        {
            //TSP is minimization problem; we need the shortest distance
            var bestParent = parents.OrderBy(x => x.Distance).Take(1).FirstOrDefault();
            var worstChildren = children.OrderByDescending(x => x.Distance).Take(1).FirstOrDefault();

            if(worstChildren.Distance > bestParent.Distance) 
            {
                children.Remove(worstChildren);
                children.Add(bestParent);
            }            
            
            return children;
        }
    }
}

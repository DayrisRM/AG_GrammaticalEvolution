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
            //Minimization problem; we need the Min error
            var bestParent = parents.OrderBy(x => x.AbsoluteErrorEval).Take(1).FirstOrDefault();
            var worstChildren = children.OrderByDescending(x => x.AbsoluteErrorEval).Take(1).FirstOrDefault();

            if(worstChildren.AbsoluteErrorEval > bestParent.AbsoluteErrorEval) 
            {
                children.Remove(worstChildren);
                children.Add(bestParent);
            }            
            
            return children;
        }
    }
}

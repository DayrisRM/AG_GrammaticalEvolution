using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface ISavePopulationService
    {
        void SavePopulationFnEvalJson(int numberExecution, Population population);
        public void SavePopulationJson(int numberExecution, Population population);
    }
}

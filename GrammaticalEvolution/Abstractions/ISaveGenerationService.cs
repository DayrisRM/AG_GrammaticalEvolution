using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface ISaveGenerationService
    {
        public void SaveGenerationJson(int numberExecution, Population population);
    }
}

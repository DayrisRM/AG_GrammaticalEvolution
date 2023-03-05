using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface IMutationService<TInput, TOutput>
    {
        public TInput Mutate(TOutput individuals);        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution.Abstractions
{
    public interface ILoadExecution<TOutput>
    {
        public TOutput Load();
    }
}

using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface IAbsoluteErrorEvaluator
    {
        void Eval(Individual individual);
    }
}
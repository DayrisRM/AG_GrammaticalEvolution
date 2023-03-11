using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface IFunctionInitializer
    {
        Dictionary<string, Function> Initialize();
    }
}
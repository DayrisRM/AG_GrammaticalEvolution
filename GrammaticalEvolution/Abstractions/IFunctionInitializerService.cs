using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface IFunctionInitializerService
    {
        Dictionary<string, Function> Initialize();
    }
}
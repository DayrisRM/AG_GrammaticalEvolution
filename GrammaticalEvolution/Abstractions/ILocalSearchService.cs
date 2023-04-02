using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Abstractions
{
    public interface ILocalSearchService
    {
        List<Individual> ApplyLocalSearch(List<Individual> individuals, LocalSearchDepthCriterion localSearchDepthCriterion, LocalSearchProbability localSearchProbability);
        bool ShouldApplyLocalSearchByBestPerformance(List<Individual> individuals, List<Individual> parents);
    }
}
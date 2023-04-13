namespace GrammaticalEvolution.Abstractions
{
    public interface IDynamicPenaltyService
    {
        double CalculatePenalty(int generationNumber, int numberOfKernels);
    }
}
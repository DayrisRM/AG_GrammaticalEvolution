namespace GrammaticalEvolution.Abstractions
{
    public interface IRandomGeneratorNumbersService
    {
        double GetDouble();
        int GetInt(int min, int max);
        int[] GetUniqueInts(int length, int min, int max);
    }
}
using NUnit.Framework;
using GrammaticalEvolution.Services;

namespace GrammaticalEvolution_UnitTests
{
    public class RandomPopulationInitializerServiceUnitTests
    {        

        [Test]
        public void Initialize_WithCorrectParameters_ShouldHaveInitialNumberPopulation()
        {
            var randomPopulationInitializer = new RandomPopulationInitializerService();
            var numberMinCodons = 16;
            var numberMaxCodons = 100;
            var maxValueCodon = 256;
            var initialNumberPopulation = 10;
            var randomPopulation = randomPopulationInitializer.Initialize(numberMinCodons, numberMaxCodons, maxValueCodon, initialNumberPopulation);
            Assert.That(randomPopulation, Is.Not.Null);
            Assert.That(randomPopulation.CurrentGeneration.Individuals.Count, Is.EqualTo(initialNumberPopulation));
        }       
        

    }
}

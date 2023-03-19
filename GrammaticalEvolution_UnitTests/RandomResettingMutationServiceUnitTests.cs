using GrammaticalEvolution.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_UnitTests
{
    public class RandomResettingMutationServiceUnitTests
    {
        [TestCase("0,1,2,3,4,5", 0.2, 0.9, "255,255,255,255,255,255")]
        [TestCase("0,1,2,3,4,5", 0.95, 0.9, "0,1,2,3,4,5")]
        public void RandomResettingMutationService_xx(string genotype1St, double probMutationCodon, double pm, string expectedGenotype1St) 
        {
            var genotype = genotype1St.Split(',').Select(int.Parse).ToList();
            var expectedGenotype = expectedGenotype1St.Split(',').Select(int.Parse).ToList();
            var mutationIndex = new Tuple<int, int>(0, 255);

            var mockService = new Mock<RandomGeneratorNumbersService>();
            mockService
                .Setup(m => m.GetDouble())
                .Returns(probMutationCodon);

            mockService
                .Setup(m => m.GetInt(mutationIndex.Item1, mutationIndex.Item2))
                .Returns(255);            

            var _onePointCrossoverService = new RandomResettingMutationService(mockService.Object, pm, mutationIndex);
            var mutatedElement = _onePointCrossoverService.Mutate(genotype);
            Assert.NotNull(mutatedElement);
            CheckChild(mutatedElement, expectedGenotype);
        }

        private void CheckChild(List<int> genotype, List<int> expectedGenotype)
        {
            Assert.IsTrue(expectedGenotype.Count == genotype.Count);

            for (var i = 0; i < expectedGenotype.Count; i++)
            {
                Assert.That(expectedGenotype[i], Is.EqualTo(genotype[i]));
            }
        }

    }
}

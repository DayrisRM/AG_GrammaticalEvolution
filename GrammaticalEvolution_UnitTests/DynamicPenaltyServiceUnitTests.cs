using GrammaticalEvolution.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_UnitTests
{
    public class DynamicPenaltyServiceUnitTests
    {
        
        [TestCase(1, 4, 0.04)]
        [TestCase(20, 4, 0.8)]
        [TestCase(40, 4, 1.6)]
        [TestCase(60, 4, 2.4)]        
        public void CalculatePenalty_WithGenAndKernelsGreaterThanZero_ShouldPenaltyIncrease(int generationNumber, int numberOfKernels, double expectedP) 
        {
            DynamicPenaltyService dynamicPenaltyService = new DynamicPenaltyService();
            var penalty = dynamicPenaltyService.CalculatePenalty(generationNumber, numberOfKernels);
            Assert.That(penalty, Is.EqualTo(expectedP));
        }

        [TestCase(1, 4, 0.04)]
        [TestCase(20, 4, 0.8)]
        [TestCase(40, 3, 1.2)]
        [TestCase(41, 3, 1.23)]
        [TestCase(42, 3, 1.26)]
        [TestCase(50, 2, 1)]
        [TestCase(51, 2, 1.02)]
        [TestCase(60, 2, 1.2)]
        [TestCase(60, 1, 0.6)]
        public void CalculatePenalty_WithGenAndKernelsGreaterThanZero_ShouldPenaltyVary(int generationNumber, int numberOfKernels, double expectedP)
        {
            DynamicPenaltyService dynamicPenaltyService = new DynamicPenaltyService();
            var penalty = dynamicPenaltyService.CalculatePenalty(generationNumber, numberOfKernels);
            Assert.That(penalty, Is.EqualTo(expectedP));
        }

    }
}

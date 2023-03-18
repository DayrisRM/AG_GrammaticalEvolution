using GrammaticalEvolution.Services;
using GrammaticalEvolution_Common.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_UnitTests
{
    public class OnePointCrossoverServiceUnitTests
    {
        
        [TestCase("0,0,0,0,0,0", "1,1,1,1,1,1", "0,0,0,0,1,1", "1,1,1,1,0,0", 4)]
        [TestCase("0,0,0,0,0,0", "1,1,1,1,1,1", "0,0,0,0,0,1", "1,1,1,1,1,0", 5)]
        [TestCase("0,0,0,0,0,0", "1,1,1,1,1,1", "0,1,1,1,1,1", "1,0,0,0,0,0", 1)]
        [TestCase("1,2,3,4,5,6", "1,1,1,1,1,1", "1,2,3,4,5,1", "1,1,1,1,1,6", 5)]
        public void OnePointCrossoverService_GenotypeSameSize_ShouldCross(string genotype1St, string genotype2St, string expectedGenotype1St, string expectedGenotype2St, int cutPoint)
        {           
            var parents = new List<Individual>()
            {
                new Individual()
                {
                    Genotype = genotype1St.Split(',').Select(int.Parse).ToList()
                },
                new Individual()
                {
                    Genotype = genotype2St.Split(',').Select(int.Parse).ToList()
                }
            };

            var mockService = new Mock<RandomGeneratorNumbersService>();
            mockService
                .Setup(m => m.GetInt(1, parents[0].Genotype.Count - 1))
                .Returns(cutPoint);

            var _onePointCrossoverService = new OnePointCrossoverService(mockService.Object);


            var childs = _onePointCrossoverService.Cross(parents);
            Assert.IsNotNull(childs);
            Assert.That(2 == childs.Count, Is.True);

            var expectedGenotype1 = expectedGenotype1St.Split(',').Select(int.Parse).ToList();
            var expectedGenotype2 = expectedGenotype2St.Split(',').Select(int.Parse).ToList();

            CheckChild(childs[0].Genotype, expectedGenotype1);
            CheckChild(childs[1].Genotype, expectedGenotype2);
        }

        [TestCase("0,0,0,0,0,0", "1,1,1,1,1,1,1,1,1,1", "0,0,0,0,1,1,1,1,1", "1,1,1,1,1,0,0", 4, 5)]
        [TestCase("1,2,3,4,5,6", "1,1,1,1,1,1,1,1,1,1", "1,2,3,4,5,1,1", "1,1,1,1,1,1,1,1,6", 5, 8)]
        [TestCase("1,2,3,4,5,6,7,8,9,10", "1,1,1,1,1,1,1", "1,2,3,4,5,6,7,1,1,1,1,1", "1,1,8,9,10", 7, 2)]
        public void OnePointCrossoverService_GenotypeDiferentSize_ShouldCross(string genotype1St, string genotype2St, string expectedGenotype1St, string expectedGenotype2St, int cutPointParent1, int cutPointParent2)
        {
            var parents = new List<Individual>()
            {
                new Individual()
                {
                    Genotype = genotype1St.Split(',').Select(int.Parse).ToList()
                },
                new Individual()
                {
                    Genotype = genotype2St.Split(',').Select(int.Parse).ToList()
                }
            };

            var mockService = new Mock<RandomGeneratorNumbersService>();
            mockService
                .Setup(m => m.GetInt(1, parents[0].Genotype.Count - 1))
                .Returns(cutPointParent1);

            mockService
                .Setup(m => m.GetInt(1, parents[1].Genotype.Count - 1))
                .Returns(cutPointParent2);

            var _onePointCrossoverService = new OnePointCrossoverService(mockService.Object);


            var childs = _onePointCrossoverService.Cross(parents);
            Assert.IsNotNull(childs);
            Assert.That(2 == childs.Count, Is.True);

            var expectedGenotype1 = expectedGenotype1St.Split(',').Select(int.Parse).ToList();
            var expectedGenotype2 = expectedGenotype2St.Split(',').Select(int.Parse).ToList();

            CheckChild(childs[0].Genotype, expectedGenotype1);
            CheckChild(childs[1].Genotype, expectedGenotype2);
        }


        private void CheckChild(List<int> genotype, List<int> expectedGenotype)
        {            
            Assert.IsTrue(expectedGenotype.Count == genotype.Count);

            for(var i = 0; i < expectedGenotype.Count; i++) 
            {                
                Assert.That(expectedGenotype[i], Is.EqualTo(genotype[i]));
            }
        }
       



    }
}

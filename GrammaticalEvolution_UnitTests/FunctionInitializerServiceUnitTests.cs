using GrammaticalEvolution.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_UnitTests
{
    public class FunctionInitializerServiceUnitTests
    {
        [Test]
        public void Initialize_DefaultFunctions_ShouldHaveNumberOfFunctions()
        {
            var functionInitializerService = new FunctionInitializerService();
            var functions = functionInitializerService.Initialize();
            Assert.That(functions, Is.Not.Null);
            Assert.That(functions.Keys.Count, Is.EqualTo(4));
        }
    }
}

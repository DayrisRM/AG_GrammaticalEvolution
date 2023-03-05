using GrammaticalEvolution.Services;
using GrammaticalEvolution_Common.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_UnitTests
{
    public class GrammarServiceUnitTests
    {
        public GrammarService InitializeGrammarService() 
        {
            var grammar = new GrammarBNF
            {
                S = "expr",
                T = new List<string>() { "+", "-", "*", "%" },
                N = new List<string>() { "expr", "op", "var" },
                P = new Dictionary<string, List<string>>
                {
                    { "<expr>", new List<string>() { "<expr><op><expr>", "(<expr><op><expr>)", "<var>" } },
                    { "<op>", new List<string>() { "+", "-", "*", "%" } },
                    { "<var>", new List<string>() { "x", "1.0" } },
                }
            };
            GrammarService grammarService = new GrammarService(grammar, false, 0);
            return grammarService;
        }

        [Test]        
        public void GrammarTest() 
        {
            var grammarService = InitializeGrammarService();
            var chromosome = new List<int>() 
            {
                201, 34, 95, 9, 100, 44, 101, 80, 105, 35, 88, 34, 14, 188, 39, 11
            };

            var grammarFn = grammarService.GetGrammar(chromosome);
            Assert.IsNotNull(grammarFn);
            Assert.IsTrue(grammarFn.Equals("(1.0+1.0)+x*x"));
        }

        [Test]
        [TestCase("<exp1><op><exp>", "<exp1>")]
        [TestCase("(<exp1><op><exp>)<op><exp>", "<exp1>")]
        [TestCase("1.0<op><exp>", "<op>")]
        [TestCase("(1.0+1.0)+x*<exp>", "<exp>")]
        [TestCase("(1.0+1.0)+x*x", "")]
        public void GetNonTerminalElementInLeftPosition_WithParameters_ShouldReturnExactValue(string symbolText, string expected) 
        {
            var grammarService = InitializeGrammarService();
            var leftSymbol = grammarService.GetNonTerminalElementInLeftPosition(symbolText);
            Assert.That(leftSymbol.Item1, Is.EqualTo(expected));
        }

        [Test]
        public void GetProductionRulesByN_SymbolExist_ShouldReturnProductionRules() 
        {
            var grammarService = InitializeGrammarService();
            var rules = grammarService.GetProductionRulesByN("<expr>");
            Assert.IsNotNull(rules);
            Assert.IsTrue(rules.Count().Equals(3));
        }

        [Test]
        public void GetProductionRulesByN_SymbolNotExist_ShouldReturnProductionRules()
        {
            var grammarService = InitializeGrammarService();
            var rules = grammarService.GetProductionRulesByN("<exp>");
            Assert.IsNotNull(rules);
            Assert.IsTrue(rules.Count().Equals(0));
        }

        [Test]
        public void GetSymbol_WithProductionRules_ShouldReturnCorrectOne() 
        {
            var grammarService = InitializeGrammarService();
            var productionRules = new List<string>() { "<expr><op><expr>", "(<expr><op><expr>)", "<var>" };
            var newSymbol = grammarService.GetSymbol(201, productionRules);
            Assert.IsNotNull(newSymbol);
            Assert.IsTrue(newSymbol.Equals("<expr><op><expr>"));
        }

        [Test]
        [TestCase("<exp1><op><exp>", 0, 5, "<apt>", "<apt><op><exp>")]
        [TestCase("(<exp1><op><exp>)<op><exp>", 1, 6, "<op>", "(<op><op><exp>)<op><exp>")]
        [TestCase("1.0<op><exp>",3, 6, "xx", "1.0xx<exp>")]
        [TestCase("(1.0+1.0)+x*<exp>",12, 16, "y", "(1.0+1.0)+x*y")]        
        public void ReplaceSymbolWithAnother_WithParameters_ShouldReplaceSymbol(string symbol, int initialPos, int endPos, string newSymbol, string expected) 
        {
            var grammarService = InitializeGrammarService();
            var newSymbolAfterReplacement = grammarService.ReplaceSymbolWithAnother(symbol, Tuple.Create("", initialPos, endPos), newSymbol);
            Assert.IsNotNull(newSymbolAfterReplacement);
            Assert.That(newSymbolAfterReplacement.Equals(expected));
        }
    }
}

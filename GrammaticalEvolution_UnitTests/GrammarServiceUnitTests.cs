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


        public GrammarService InitializeGrammar2Service()
        {
            var grammar = new GrammarBNF
            {
                S = "expr",
                T = new List<string>() { 
                    "“KG”",
                    "“KP“",
                    "“KS”",
                    "“(”",
                    "“)”",
                    "“+”",
                    "“-”",
                    "“nulo1”",
                    "“nulo2”",
                    "“.”",
                    "“E”",
                    "“0”",
                    "“1”",
                    "“2”",
                    "“3”",
                    "“4”",
                    "“5”",
                    "“6”",
                    "“7”",
                    "“8”",
                    "“9”" 
                },
                N = new List<string>() { 
                    "expr",
                    "signo",
                    "real",
                    "K_G",
                    "K_P",
                    "K_S",
                    "nulo",
                    "grado",
                    "uno-nueve",
                    "cero-nueve"
                },
                P = new Dictionary<string, List<string>>
                {
                    { "<expr>", new List<string>() { 
                        "<signo><real><K_G>",
                        "<signo><real><K_G><expr>",
                        "<signo><real><K_P>",
                        "<signo><real><K_P><expr>",
                        "<signo><real><K_S>",
                        "<signo><real><K_S><expr>" } 
                    },
                    { "<K_G>", new List<string>() {
                        "KG(<real><real><nulo>)" }
                    },
                    { "<K_P>", new List<string>() {
                        "KP(<real><real><grado>)" }
                    },
                    { "<K_S>", new List<string>() {
                        "KS(<real><real><nulo>)" }
                    },
                    { "<signo>", new List<string>() {
                        "+",
                        "-" }
                    },
                    { "<real>", new List<string>() {
                        "<uno-nueve>.<cero-nueve>E<signo><cero-nueve>" }
                    },
                    { "<nulo>", new List<string>() {
                        "nulo1",
                        "nulo2" }
                    },
                    { "<grado>", new List<string>() {
                        "0",
                        "1",
                        "2",
                        "3",
                        "4" }
                    },
                    { "<uno-nueve>", new List<string>() {
                        "1",
                        "2",
                        "3",
                        "4",
                        "5",
                        "6",
                        "7",
                        "8",
                        "9" }
                    },
                    { "<cero-nueve>", new List<string>() {
                        "0",
                        "1",
                        "2",
                        "3",
                        "4",
                        "5",
                        "6",
                        "7",
                        "8",
                        "9" }
                    },

                }
            };
            GrammarService grammarService = new GrammarService(grammar, true, 100);
            return grammarService;
        }

        [Test]
        public void Grammar2Test()
        {
            var grammarService = InitializeGrammar2Service();
            var chromosome = new List<int>()
            {
                87,26,238,123,121,242,186,140,241,78,207,83,46,27,3,167,73,161,158,70,208,74,22,232,237,135,152,192,206,52,213,131,104,11,51,50,202,86,139,255,76,129,1,164,182,8,107,165,99,229,114,96,178,205,190,236,256,31,15,210,103,23,53,64,142,49,37,132,94,143,188,58,89,25,2,144,246,134,198,66,199,42,7,45,217,250,13,187,156,145,44,219,36,173,32,204,40,234,79,10
            };

            var grammarFn = grammarService.GetGrammar(chromosome);
            Assert.IsNotNull(grammarFn);
            Assert.IsTrue(grammarFn.Contains("<").Equals(false));
            Assert.IsTrue(grammarFn.Contains(">").Equals(false));
        }

    }
}

using GrammaticalEvolution_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution.Services
{
    public class GrammarService
    {
        private GrammarBNF _grammarBNF { get; set; }
        private bool _allowWrapping { get; set; }
        private int _maxWrapping { get; set; }

        private int _actualWrapping { get; set; }
        private const string InitialSymbol = "<";
        private const string EndSymbol = ">";

        public GrammarService(GrammarBNF grammarBNF, bool allowWrapping, int maxWrapping) 
        {
            _grammarBNF = grammarBNF;
            _allowWrapping = allowWrapping;
            _maxWrapping = maxWrapping;
        }

        public string GetGrammar(List<int> chromosome) 
        {            
            if (HasNonTerminalSymbols(_grammarBNF.S)) 
            {
                var grammarFn = InitialSymbol + _grammarBNF.S + EndSymbol;
                grammarFn = GetGrammarRecursively(chromosome, grammarFn, 0);
                return grammarFn;
            }
            
            return _grammarBNF.S;
        }

        private string GetGrammarRecursively(List<int> chromosome, string grammarFn, int position) 
        {
            if (!HasNonTerminalSymbols(grammarFn))
            {
                return grammarFn;
            }

            if (position > chromosome.Count())
            {
                //add wrapping
                if (_allowWrapping && _actualWrapping <= _maxWrapping) 
                {
                    position = 0;
                    _actualWrapping += 1;
                }
                else 
                {
                    throw new Exception($"Wrapping exception - position > chromosome.Count -- position:{position} - chromosomeCount:{chromosome.Count}");
                }
            }           

            var codon = chromosome[position];
            var elementNonTerminal = GetNonTerminalElementInLeftPosition(grammarFn);
            if (elementNonTerminal.Item1 == "" && elementNonTerminal.Item2 == -1 && elementNonTerminal.Item3 == -1)
                throw new Exception("Error in GetNonTerminalElementInLeftPosition");

            var productionRulesByN = GetProductionRulesByN(elementNonTerminal.Item1);
            if (!productionRulesByN.Any())
                throw new Exception("ProductionRulesByN can not be empty");

            var newSymbol = GetSymbol(codon, productionRulesByN);

            var newGrammarFN = ReplaceSymbolWithAnother(grammarFn, elementNonTerminal, newSymbol);

            return GetGrammarRecursively(chromosome, newGrammarFN, position + 1);
        }

        private bool HasNonTerminalSymbols(string symbol) 
        {
            if (symbol.Contains(InitialSymbol) || symbol.Contains(EndSymbol)) 
            {
                //obtener el primero
                var symbolInLeftPosition = GetNonTerminalElementInLeftPosition(symbol);
                symbol = symbolInLeftPosition.Item1.Replace(InitialSymbol, "").Replace(EndSymbol, "");
                //symbol = symbol.Replace(InitialSymbol, "").Replace(EndSymbol, "");
            }
            var hasNSymbols = _grammarBNF.N.Any(x => x.Equals(symbol));
            return hasNSymbols;
        }

        public Tuple<string, int, int> GetNonTerminalElementInLeftPosition(string symbol) 
        {
            var firstPosition = symbol.IndexOf(InitialSymbol);
            var lastPosition = symbol.IndexOf(EndSymbol);

            if (firstPosition == -1 || lastPosition == -1) 
            {
                return Tuple.Create("", -1, -1);
            }

            var element = symbol.Substring(firstPosition, (lastPosition-firstPosition)+1);

            return Tuple.Create(element, firstPosition, lastPosition);
        }

        public List<string> GetProductionRulesByN(string nonTerminalElement) 
        {
            var productionRules = new List<string>();
            if (_grammarBNF.P.ContainsKey(nonTerminalElement)) 
            {
                productionRules = _grammarBNF.P[nonTerminalElement];
            }
            
            return productionRules;
        }

        public string GetSymbol(int codon, List<string> productionRulesByN) 
        {
            var mod = codon % productionRulesByN.Count;
            return productionRulesByN[mod];
        }

        public string ReplaceSymbolWithAnother(string symbol, Tuple<string, int, int> elementNonTerminal, string newSymbol) 
        {
            var element = symbol.Substring(0, elementNonTerminal.Item2);
            element += newSymbol;
            var initialSecondPart = elementNonTerminal.Item3 + 1;
            element += symbol.Substring(initialSecondPart, symbol.Length - initialSecondPart);

            return element;
        }

    }
}

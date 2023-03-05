using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class LoadFileGrammarBNFService : ILoadFileService<GrammarBNF>
    {
        public GrammarBNF LoadFile(string nameFile)
        {           
            Dictionary<string, List<string>> sections = new Dictionary<string, List<string>>();
            List<string>? section = null;
            
            string[] lines = System.IO.File.ReadAllLines(@"../../../Data/grammars/" + nameFile);

            foreach (string line in lines)
            {
                if(string.IsNullOrWhiteSpace(line)) continue;

                if (IsSectionStart(line))
                {
                    string name = GetSectionName(line);
                    section = new List<string>();
                    sections.Add(name, section);
                }
                else if (IsSectionEnd(line))
                {
                    continue;
                }
                else
                {
                    section.Add(line);
                }
            }

            if (!sections.Any()) 
            {
                throw new Exception("Grammar exception");
            }


            return BuildGrammar(sections);
        }

        private GrammarBNF BuildGrammar(Dictionary<string, List<string>> sections)
        {
            var grammarBNF = new GrammarBNF();

            foreach (var section in sections) 
            {
                if (section.Key == "N") 
                {
                    grammarBNF.N = LoadElements(section.Value);
                }
                else if (section.Key == "T") 
                {
                    grammarBNF.T = LoadElements(section.Value);
                }
                else if (section.Key == "S") 
                {
                    grammarBNF.S = LoadStartSymbol(section.Value);
                }
                else if (section.Key == "P") 
                {
                   grammarBNF.P = LoadProductionRules(section.Value);
                }
            }

            return grammarBNF;
        }

        private string GetSectionName(string line)
        {            
            var section = line.Replace("###", "").Replace("START -", "").Trim();            

            return section;
        }

        private bool IsSectionEnd(string line)
        {
            if (line.StartsWith("### END "))
                return true;

            return false;
        }

        private bool IsSectionStart(string line)
        {
            if(line.StartsWith("### START -"))
                return true;

            return false;
        }

        private string LoadStartSymbol(List<string> lines) 
        {
            var startSymbol = "";
            if (lines.Count > 1) 
            {
                throw new Exception("Start symbol should has one line");
            }
            var line = lines[0].Replace("{", "").Replace("}", "");
            var rule = line.Split("=");
            if (rule.Length > 0) 
            {
                startSymbol = rule[1].Trim();
            }

            return startSymbol;
        }
        private List<string> LoadElements(List<string> lines) 
        { 
            var terminals = new List<string>();
            foreach (var line in lines) 
            {
                var cleanedLine = line.Replace("{", "").Replace("}", "");
                var rule = cleanedLine.Split("=");
                if (rule.Length > 0)
                {
                    var rules = rule[1].Split(",");
                    foreach (var r in rules) 
                    {
                        terminals.Add(r.Trim());
                    }
                }
            }
            return terminals;
        }       
        private Dictionary<string, List<string>> LoadProductionRules(List<string> lines)
        {
            Dictionary<string, List<string>> productionRules = new Dictionary<string, List<string>>();
            foreach (var line in lines) 
            {
                var values = line.Split("::=");
                var ruleName = values[0].Trim();
                var ruleAllValues = values[1].Trim();

                if (string.IsNullOrWhiteSpace(ruleAllValues))
                {
                    throw new Exception("Rule can't be empty");
                }

                var cleanedRules = new List<string>();

                var rules = ruleAllValues.Split("|");
                foreach (var rule in rules)
                {
                    cleanedRules.Add(rule.Trim());
                }

                productionRules[ruleName] = cleanedRules;
            }

            return productionRules;

        }
    }
}

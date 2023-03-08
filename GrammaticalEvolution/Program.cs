// See https://aka.ms/new-console-template for more information
using GrammaticalEvolution.Services;
using GrammaticalEvolution_Common.Models;

Console.WriteLine("Hello, World!");


LoadFileGrammarBNFService loadFileGrammarBNFService = new LoadFileGrammarBNFService();
var txt = loadFileGrammarBNFService.LoadFile("grammarbnf.txt");


//create functions to symbolic regression





ExecuteGA();

void ExecuteGA()
{
    //Initialize timer
    var watch = System.Diagnostics.Stopwatch.StartNew();
    GeneticAlgorithmService geneticAlgorithmService = new GeneticAlgorithmService();
    geneticAlgorithmService.EvolveAlgorithm();
}

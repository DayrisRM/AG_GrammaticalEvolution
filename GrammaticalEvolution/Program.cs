// See https://aka.ms/new-console-template for more information
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution.Services;
using GrammaticalEvolution_Common.Models;

Console.WriteLine("Hello, World!");


LoadFileGrammarBNFService loadFileGrammarBNFService = new LoadFileGrammarBNFService();
var txt = loadFileGrammarBNFService.LoadFile("grammarbnf.txt");


//create functions to symbolic regression
IFunctionInitializer functionInitializerService = new FunctionInitializerService();
var functions = functionInitializerService.Initialize();
var selectedFn = functions["F1"];


ExecuteGA();

void ExecuteGA()
{
    //Initialize timer
    var watch = System.Diagnostics.Stopwatch.StartNew();
    GeneticAlgorithmService geneticAlgorithmService = new GeneticAlgorithmService(selectedFn);
    geneticAlgorithmService.EvolveAlgorithm();
}




// See https://aka.ms/new-console-template for more information
using GrammaticalEvolution.Services;

Console.WriteLine("Hello, World!");


LoadFileGrammarBNFService loadFileGrammarBNFService = new LoadFileGrammarBNFService();
var txt = loadFileGrammarBNFService.LoadFile("grammarbnf.txt");

//ExecuteGA();

void ExecuteGA()
{
    //Initialize timer
    var watch = System.Diagnostics.Stopwatch.StartNew();

}

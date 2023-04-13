// See https://aka.ms/new-console-template for more information
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution.Services;
using GrammaticalEvolution_Common.Models;
using TSP_Visualization;


Console.WriteLine("Hello, World!");

var executionData = new ExecutionGA()
{
    GrammarBNFFile = "grammarbnf.txt",
    InitialNumberPopulation = 500,
    NumberIterations = 60,
    NumberExecutions = 2,
    NumberMinCodons = 30,
    NumberMaxCodons = 100,
    MaxValueCodon = 255,
    MaxWrapping = 2,
    AllowWrapping = true,
    CrossoverProbability = 0.7,
    MutationProbability = 0.4,
    AllowLocalSearch = false,
    AllowFitnessPenalty = true,
};

CreatePlot createPlot = new CreatePlot(executionData.NumberExecutions, executionData.NumberIterations, executionData.CrossoverProbability, executionData.MutationProbability);

//Load grammar BNF
LoadFileGrammarBNFService loadFileGrammarBNFService = new LoadFileGrammarBNFService();
var grammarBNF = loadFileGrammarBNFService.LoadFile("grammarbnf.txt");

//create functions to symbolic regression
IFunctionInitializer functionInitializerService = new FunctionInitializerService();
IEvaluator<Function, Function> functionEvaluatorService = new FunctionEvaluatorService();
var functions = functionInitializerService.Initialize();
var selectedFn = functions["F2"];
selectedFn = functionEvaluatorService.Evaluate(selectedFn);
createPlot.CreateFunctionEval(selectedFn);

ExecuteGA();

void ExecuteGA()
{
    //Initialize timer
    var watch = System.Diagnostics.Stopwatch.StartNew();
    ISavePopulationService JsonSavePopulationService = new JsonSavePopulationService();

    Console.WriteLine("Starting executions with pc: " + executionData.CrossoverProbability + " -- pm: " + executionData.MutationProbability);

    for (var i = 1; i <= executionData.NumberExecutions; i++)
    {
        Console.WriteLine("Execution: " + i);

        var watch2 = System.Diagnostics.Stopwatch.StartNew();

        GeneticAlgorithmService geneticAlgorithmService = new GeneticAlgorithmService(executionData.InitialNumberPopulation, executionData.NumberIterations,
        executionData.CrossoverProbability, executionData.MutationProbability, selectedFn, executionData.NumberMinCodons, executionData.NumberMaxCodons,
        executionData.MaxValueCodon, executionData.AllowWrapping, executionData.MaxWrapping, grammarBNF, executionData.AllowLocalSearch, executionData.AllowFitnessPenalty);

        var finalPopulation = geneticAlgorithmService.EvolveAlgorithm();

        JsonSavePopulationService.SavePopulationJson(i, finalPopulation);
        JsonSavePopulationService.SavePopulationFnEvalJson(i, finalPopulation);

        Console.WriteLine("Best Individual: " + finalPopulation.BestIndividual.AbsoluteErrorEval);
        Console.WriteLine("Generations: " + finalPopulation.Generations.Count);

        watch2.Stop();

        var elapsedMinutesInExecution = TimeSpan.FromMilliseconds(watch2.ElapsedMilliseconds).TotalMinutes;
        Console.WriteLine($"Elapsed time in execution:{elapsedMinutesInExecution}");

        Console.WriteLine("------");
        Console.WriteLine();
    }

    //Finish timer
    watch.Stop();
    var elapsedMs = watch.ElapsedMilliseconds;
    var elapsedMinutes = TimeSpan.FromMilliseconds(elapsedMs).TotalMinutes;
    Console.WriteLine($"Elapsed time in milliseconds:{elapsedMs}");
    Console.WriteLine($"Elapsed time in minutes:{elapsedMinutes}");

    Console.WriteLine("Loading saved population");

    //Get Saved execution-population
    ILoadExecution<List<Population>> JsonLoadExecutionService = new JsonLoadExecutionService(executionData.NumberExecutions);
    var savedPopulation = JsonLoadExecutionService.Load();

    //Calculate VAMM
    Console.WriteLine("Calculating VAMM");
    IEvaluator<List<Population>, double> VAMMEvaluatorService = new VAMMEvaluatorService();
    var vammGA = VAMMEvaluatorService.Evaluate(savedPopulation);

    Console.WriteLine($"VAMM:{vammGA}");
    var pc = executionData.CrossoverProbability.ToString().Replace('.', '_');
    var pm = executionData.MutationProbability.ToString().Replace('.', '_');
    
    //Create Plots
    Console.WriteLine("Generating plots...");    
    createPlot.CreateProgressCurve(savedPopulation);

    Console.WriteLine("Generated plots in /Data/figures");

    //Create Fn errors
    ILoadExecution<List<Individual>> JsonLoadEvalExecutionService = new JsonLoadEvalExecutionService(executionData.NumberExecutions);
    var savedIndividuals = JsonLoadEvalExecutionService.Load();
    //TODO: Definir con qué valores pintar la gráfica
    for(var i = 0; i < savedIndividuals.Count; i++) 
    {
        createPlot.CreateFunctionEvalPlot(savedIndividuals[i], i);
    }
   
}


public class ExecutionGA
{
    public string GrammarBNFFile { get; set; }
    public int InitialNumberPopulation { get; set; }    
    public int NumberIterations { get; set; }
    public int NumberExecutions { get; set; }    
    public int NumberMinCodons { get; set; }
    public int NumberMaxCodons { get; set; }
    public int MaxValueCodon { get; set; }
    public int MaxWrapping { get; set; }
    public double MutationProbability { get; set; }
    public double CrossoverProbability { get; set; }
    public bool AllowWrapping { get; set; }
    public bool AllowLocalSearch { get; set; }
    public bool AllowFitnessPenalty { get; set; }
}


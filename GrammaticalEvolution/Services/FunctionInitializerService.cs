using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;
using Newtonsoft.Json.Linq;

namespace GrammaticalEvolution.Services
{
    public class FunctionInitializerService : IFunctionInitializerService
    {
        public Dictionary<string, Function> Initialize()
        {
            var functions = new Dictionary<string, Function>()
            {
                {
                    "F1", new Function(){
                        Name = "F1",
                        IntervalMin = -2,
                        IntervalMax = 4,
                        M = 61

                    }
                },
                {
                    "F2", new Function(){
                        Name = "F2",
                        IntervalMin = -1,
                        IntervalMax = 3,
                        M = 41

                    }
                },
                {
                    "F3", new Function(){
                        Name = "F3",
                        IntervalMin = 0,
                        IntervalMax = 4,
                        M = 41

                    }
                },
                {
                    "F4", new Function(){
                        Name = "F4",
                        IntervalMin = 0,
                        IntervalMax = 4,
                        M = 41

                    }
                },
            };

            foreach(var function in functions.Values) 
            {
                GenerateMValues(function);
            }
            
            return functions;
        }


        private void GenerateMValues(Function function) 
        {
            var mValues = new List<double>();
            double deltaX = (double)(function.IntervalMax - function.IntervalMin) / (double)(function.M - 1);
            deltaX = Math.Round(deltaX, 3);

            bool exit = true;
            var i = 0;
            while(exit) 
            {
                if(i == 0) 
                {
                    mValues.Add(function.IntervalMin);
                }
                else 
                {
                    if(i == function.M - 1) 
                    { 
                        mValues.Add(function.IntervalMax);
                        break;
                    }
                    
                    double nextValue = function.IntervalMin + (i * deltaX);


                    if (nextValue > function.IntervalMax) 
                    {
                        mValues.Add(function.IntervalMax);
                        break;
                    }

                    mValues.Add(Math.Round(nextValue, 3));

                }
                i++;
            };

            function.MValues = mValues;
        }

    }
}

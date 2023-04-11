using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;

namespace GrammaticalEvolution.Services
{
    public class DynamicPenaltyService : IDynamicPenaltyService
    {

        /// <summary>
        /// Calculate penalty 
        /// penalty = C * numberOfKernels
        /// </summary>
        /// <param name="generationNumber"></param>
        /// <param name="numberOfKernels"></param>
        /// <returns></returns>
        public double CalculatePenalty(int generationNumber, int numberOfKernels)
        {
            var c = CalculateCMagnitud(generationNumber);

            double penalty = c * (double)numberOfKernels;

            return Math.Round(penalty, 2);
        }

        private double CalculateCMagnitud(int generationNumber)
        {
            return (double)generationNumber / 100;
        }

    }
}

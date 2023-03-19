using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammaticalEvolution.Abstractions;
using GrammaticalEvolution_Common.Models;

namespace GrammaticalEvolution.Services
{
    public class TournamentSelectionService : ISelectionService
    {
        private int _numberOfTournament { get; set; }
        private int _sizeOfTournament { get; set; } = 2;

        private readonly IRandomGeneratorNumbersService _randomGeneratorNumbersService;
        

        public TournamentSelectionService(int numberOfTournament, IRandomGeneratorNumbersService randomGeneratorNumbersService)
        {
            _numberOfTournament = numberOfTournament;           
            _randomGeneratorNumbersService = randomGeneratorNumbersService;
        }

        public List<Individual> Select(List<Individual> individuals)
        {
            var tournamentResult = new List<Individual>();

            if(!individuals.Any())
                throw new ArgumentNullException(nameof(individuals));

            for(var i = 0; i < _numberOfTournament; i++) 
            {
                var tournamentInd = DoTournamentWithSizeTwo(individuals);
                tournamentResult.Add((Individual)tournamentInd.Clone());          
            }

            return tournamentResult;
        }

        private Individual DoTournamentWithSizeTwo(List<Individual> individuals) 
        {           
            var firstIndex = _randomGeneratorNumbersService.GetInt(0, _numberOfTournament);
            var secondIndex = _randomGeneratorNumbersService.GetInt(0, _numberOfTournament);

            var firstIndividual = individuals[firstIndex];
            var secondIndividual = individuals[secondIndex];

            //minimization problem; we need the min error
            if (firstIndividual.AbsoluteErrorEval > secondIndividual.AbsoluteErrorEval)
                return secondIndividual;

            return firstIndividual;

        }

    }
}

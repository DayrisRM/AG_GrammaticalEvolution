using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public class Function
    {
        public string Name { get; set; }
        public int IntervalMin { get; set; }
        public int IntervalMax { get; set; }
        public int M { get; set; }
        public List<double> MValues { get; set; } = new List<double>();
        public List<Coords> Coords { get; set; } = new List<Coords> { };
    }

    public struct Coords
    {
        public double X { get; }
        public double Y { get; }

        public Coords(double x, double y)
        {
            X = x;
            Y = y;
        }
        
    }

}

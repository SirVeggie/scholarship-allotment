using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class KnapsackResult2D<T> {
        public double Value { get; }
        public int WeightX { get; }
        public int WeightY { get; }
        public List<KnapsackItem2D<T>> Items { get; }

        public KnapsackResult2D(double value, int weightX, int weightY, List<KnapsackItem2D<T>> items) {
            Value = value;
            WeightX = weightX;
            WeightY = weightY;
            Items = items;
        }
    }
}

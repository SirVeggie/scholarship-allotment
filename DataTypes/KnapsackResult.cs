using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class KnapsackResult<T> {
        public double Value { get; }
        public int Weight { get; }
        public List<KnapsackItem<T>> Items { get; }

        public KnapsackResult(double value, int weight, List<KnapsackItem<T>> items) {
            Value = value;
            Weight = weight;
            Items = items;
        }
    }
}

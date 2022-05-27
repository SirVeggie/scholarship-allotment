using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class SackItem<T> {

        public double Value { get; }
        public int Weight { get; }
        public T Relation { get; }

        public SackItem(double value, int weight, T relation) {
            Value = value;
            Weight = weight;
            Relation = relation;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class SackItem2D<T> {

        public double Value { get; }
        public int WeightX { get; }
        public int WeightY { get; }
        public T Relation { get; }

        public SackItem2D(double value, int weightX, int weightY, T relation) {
            Value = value;
            WeightX = weightX;
            WeightY = weightY;
            Relation = relation;
        }
    }
}

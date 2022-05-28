using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class KnapsackStep2D<T> {
        public double Value { get; }
        public SackItem2D<T>? AddedItem { get; }
        public (int, int, int) Origin { get; }

        public KnapsackStep2D(double value, SackItem2D<T>? addedItem, (int, int, int) origin) {
            Value = value;
            AddedItem = addedItem;
            Origin = origin;
        }
    }
}

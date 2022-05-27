using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class KnapsackStep<T> {

        public double Value { get; }
        public SackItem<T>? AddedItem { get; }
        public (int, int) Origin { get; }

        public KnapsackStep(double value, SackItem<T>? addedItem, (int, int) origin) {
            Value = value;
            AddedItem = addedItem;
            Origin = origin;
        }
    }
}

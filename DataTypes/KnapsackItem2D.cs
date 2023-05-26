using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public struct KnapsackItem2D<T> {
        public T item;
        public double value;
        public int weightX;
        public int weightY;

        public KnapsackItem2D(T item, double value, int weightX, int weightY) {
            this.item = item;
            this.value = value;
            this.weightX = weightX;
            this.weightY = weightY;
        }

        public override string ToString() {
            return $"{item} | {value} | {weightX} | {weightY}";
        }
    }
}

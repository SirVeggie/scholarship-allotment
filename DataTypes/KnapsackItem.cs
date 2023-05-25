using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public struct KnapsackItem<T> {
        public T item;
        public double value;
        public int weight;

        public KnapsackItem(T item, double value, int weight) {
            this.item = item;
            this.value = value;
            this.weight = weight;
        }
    }
}

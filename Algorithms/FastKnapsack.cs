using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public static class FastKnapsack<T> {

        public static KnapsackResult<T> Solve(KnapsackItem<T>[] items, int maxWeight) {
            KnapsackNode[,] M = new KnapsackNode[items.Length + 1, maxWeight + 1];

            for (int i = 0; i <= items.Length; i++) {
                for (int w = 0; w <= maxWeight; w++) {

                    if (i == 0) {
                        continue;

                    } else if (items[i - 1].weight <= w) {
                        double addValue = items[i - 1].value + M[i - 1, w - items[i - 1].weight].value;
                        M[i, w] = M[i - 1, w].value >= addValue
                            ? new(M[i - 1, w].value, false)
                            : new(addValue, true);

                    } else {
                        M[i, w] = new(M[i - 1, w].value, false);
                    }
                }
            }

            List<KnapsackItem<T>> result = new(items.Length);

            int weight = maxWeight;
            double totalV = 0;
            int totalW = 0;
            for (int i = items.Length; i > 0; i--) {
                if (M[i, weight].include) {
                    result.Add(items[i - 1]);
                    weight -= items[i - 1].weight;
                    totalV += items[i - 1].value;
                    totalW += items[i - 1].weight;
                }
            }

            return new(totalV, totalW, result);
        }
    }
}

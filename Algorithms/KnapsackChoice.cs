using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public static class KnapsackChoice<T> {

        public static KnapsackResult<T> Solve(List<List<KnapsackItem<T>>> items, int maxWeight) => Solve(items.Select(x => x.ToArray()).ToArray(), maxWeight);
        public static KnapsackResult<T> Solve(KnapsackItem<T>[][] items, int maxWeight) {
            KnapsackNodeChoice[,] M = new KnapsackNodeChoice[items.Length + 1, maxWeight + 1];

            for (int i = 0; i <= items.Length; i++) {
                if (i == 0)
                    continue;
                for (int w = 0; w <= maxWeight; w++) {

                    bool wasIncluded = false;
                    for (int k = 0; k < items[i - 1].Length; k++) {
                        if (items[i - 1][k].weight <= w) {
                            double addValue = items[i - 1][k].value + M[i - 1, w - items[i - 1][k].weight].value;
                            if (addValue > M[i - 1, w].value) {
                                if (!wasIncluded || addValue > M[i, w].value)
                                    M[i, w] = new(addValue, k);
                                wasIncluded = true;
                            }
                        }
                    }

                    if (!wasIncluded) {
                        M[i, w] = new(M[i - 1, w].value);
                    }
                }
            }

            List<KnapsackItem<T>> result = new(items.Length);

            int weight = maxWeight;
            double totalV = 0;
            int totalW = 0;
            for (int i = items.Length; i > 0; i--) {
                if (M[i, weight].include) {
                    KnapsackItem<T> item = items[i - 1][M[i, weight].item];
                    result.Add(item);
                    weight -= item.weight;
                    totalV += item.value;
                    totalW += item.weight;
                }
            }

            return new(totalV, totalW, result);
        }
    }
}

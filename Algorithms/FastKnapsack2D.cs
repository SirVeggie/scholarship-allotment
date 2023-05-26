using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public static class FastKnapsack2D<T> {

        public static KnapsackResult2D<T> Solve(List<List<KnapsackItem2D<T>>> items, int maxWeightX, int maxWeightY) => Solve(items.Select(x => x.ToArray()).ToArray(), maxWeightX, maxWeightY);
        public static KnapsackResult2D<T> Solve(KnapsackItem2D<T>[][] items, int maxWeightX, int maxWeightY) {
            KnapsackNodeChoice[,,] M = new KnapsackNodeChoice[items.Length + 1, maxWeightX + 1, maxWeightY + 1];

            for (int i = 0; i <= items.Length; i++) {
                if (i == 0)
                    continue;

                for (int wx = 0; wx <= maxWeightX; wx++) {
                    for (int wy = 0; wy <= maxWeightY; wy++) {
                        bool wasIncluded = false;
                        for (int k = 0; k < items[i - 1].Length; k++) {
                            if (items[i - 1][k].weightX <= wx && items[i - 1][k].weightY <= wy) {
                                double addValue = items[i - 1][k].value + M[i - 1, wx - items[i - 1][k].weightX, wy - items[i - 1][k].weightY].value;
                                if (addValue > M[i - 1, wx, wy].value) {
                                    if (!wasIncluded || addValue > M[i, wx, wy].value)
                                        M[i, wx, wy] = new(addValue, k);
                                    wasIncluded = true;
                                }
                            }
                        }

                        if (!wasIncluded) {
                            M[i, wx, wy] = new(M[i - 1, wx, wy].value);
                        }
                    }
                }
            }

            List<KnapsackItem2D<T>> result = new(items.Length);

            int weightX = maxWeightX;
            int weightY = maxWeightY;
            double totalV = 0;
            int totalWX = 0;
            int totalWY = 0;
            for (int i = items.Length; i > 0; i--) {
                if (M[i, weightX, weightY].include) {
                    KnapsackItem2D<T> item = items[i - 1][M[i, weightX, weightY].item];
                    result.Add(item);
                    weightX -= item.weightX;
                    weightY -= item.weightY;
                    totalV += item.value;
                    totalWX += item.weightX;
                    totalWY += item.weightY;
                }
            }

            return new(totalV, totalWX, totalWY, result);
        }
    }
}

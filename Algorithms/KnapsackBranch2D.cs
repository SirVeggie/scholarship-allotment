using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public static class KnapsackBranch2D<T> {

        public static KnapsackResult2D<T> Solve(List<List<KnapsackItem2D<T>>> items, int maxWeightX, int maxWeightY) {
            return Solve(items.Select(x => x.ToArray()).ToArray(), maxWeightX, maxWeightY);
        }
        public static KnapsackResult2D<T> Solve(KnapsackItem2D<T>[][] items, int maxWeightX, int maxWeightY) {
            Queue<Position3D> queue = new();
            Stack<Position3D> backtrack = new();
            HashSet<Position3D> tracked = new();

            queue.Enqueue(new(items.Length, maxWeightX, maxWeightY));
            backtrack.Push(new(items.Length, maxWeightX, maxWeightY));

            while (queue.Count > 0) {
                Position3D current = queue.Dequeue();
                int i = current.x, wx = current.y, wy = current.z;
                if (i <= 1)
                    continue;

                Position3D exclude = new(i - 1, wx, wy);
                if (!tracked.Contains(exclude)) {
                    tracked.Add(exclude);
                    queue.Enqueue(exclude);
                    backtrack.Push(exclude);
                }

                foreach (var item in items[i - 1]) {
                    if (item.weightX > wx || item.weightY > wy)
                        continue;
                    Position3D include = new(i - 1, wx - item.weightX, wy - item.weightY);
                    if (!tracked.Contains(include)) {
                        tracked.Add(include);
                        queue.Enqueue(include);
                        backtrack.Push(include);
                    }
                }
            }

            queue = null;
            tracked = null;
            Dictionary<Position3D, KnapsackNodeChoice> M = new();

            while (backtrack.Count > 0) {
                Position3D current = backtrack.Pop();
                int i = current.x, wx = current.y, wy = current.z;

                if (i == 1) {
                    M.TryAdd(new(0, wx, wy), new());
                    for (int k = 0; k < items[0].Length; k++) {
                        M.TryAdd(new(0, wx - items[0][k].weightX, wy - items[0][k].weightY), new());
                    }
                }

                bool wasIncluded = false;
                for (int k = 0; k < items[i - 1].Length; k++) {
                    if (items[i - 1][k].weightX <= wx && items[i - 1][k].weightY <= wy) {
                        double addValue = items[i - 1][k].value + M[new(i - 1, wx - items[i - 1][k].weightX, wy - items[i - 1][k].weightY)].value;
                        if (addValue > M[new(i - 1, wx, wy)].value) {
                            if (wasIncluded && M[new(i, wx, wy)].value < addValue)
                                M[new(i, wx, wy)] = new(addValue, k);
                            else if (!wasIncluded)
                                M.Add(new(i, wx, wy), new(addValue, k));
                            wasIncluded = true;
                        }
                    }
                }

                if (!wasIncluded) {
                    M.Add(new(i, wx, wy), new(M[new(i - 1, wx, wy)].value));
                }
            }

            List<KnapsackItem2D<T>> result = new(items.Length);

            int weightX = maxWeightX, weightY = maxWeightY;
            double totalValue = 0;
            int totalWeightX = 0, totalWeightY = 0;
            for (int i = items.Length; i > 0; i--) {
                if (M[new(i, weightX, weightY)].include) {
                    int item = M[new(i, weightX, weightY)].item;
                    result.Add(items[i - 1][item]);
                    weightX -= items[i - 1][item].weightX;
                    weightY -= items[i - 1][item].weightY;
                    totalValue += items[i - 1][item].value;
                    totalWeightX += items[i - 1][item].weightX;
                    totalWeightY += items[i - 1][item].weightY;
                }
            }

            return new(totalValue, totalWeightX, totalWeightY, result);
        }
    }
}

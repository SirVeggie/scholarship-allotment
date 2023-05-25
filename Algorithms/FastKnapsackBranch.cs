using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public static class FastKnapsackBranch<T> {

        public static KnapsackResult<T> Solve(KnapsackItem<T>[] items, int maxWeight) {
            Stopwatch sw = Stopwatch.StartNew();
            Queue<Position> queue = new();
            Stack<Position> backtrack = new();
            HashSet<Position> tracked = new();

            queue.Enqueue(new(items.Length, maxWeight));
            backtrack.Push(new(items.Length, maxWeight));

            while (queue.Count > 0) {
                Position current = queue.Dequeue();
                int i = current.x, w = current.y;
                if (i <= 1)
                    continue;

                var left = new Position(i - 1, w);
                if (!tracked.Contains(left)) {
                    tracked.Add(left);
                    queue.Enqueue(left);
                    backtrack.Push(left);
                }

                if (items[i - 1].weight > w)
                    continue;
                var right = new Position(i - 1, w - items[i - 1].weight);
                if (!tracked.Contains(right)) {
                    tracked.Add(right);
                    queue.Enqueue(right);
                    backtrack.Push(right);
                }
            }
            sw.Stop();

            queue = null;
            tracked = null;
            Dictionary<Position, KnapsackNode> M = new();

            while (backtrack.Count > 0) {
                Position current = backtrack.Pop();
                int i = current.x, w = current.y;

                if (i == 1) {
                    M.TryAdd(new(0, w), new());
                    M.TryAdd(new(0, w - items[0].weight), new());
                }

                if (items[i - 1].weight <= w) {
                    double addValue = items[i - 1].value + M[new(i - 1, w - items[i - 1].weight)].value;
                    M.Add(new(i, w), M[new(i - 1, w)].value >= addValue
                        ? new(M[new(i - 1, w)].value, false)
                        : new(addValue, true));

                } else {
                    M.Add(new(i, w), new(M[new(i - 1, w)].value, false));
                }
            }

            List<KnapsackItem<T>> result = new(items.Length);

            int weight = maxWeight;
            double totalV = 0;
            int totalW = 0;
            for (int i = items.Length; i > 0; i--) {
                if (M[new(i, weight)].include) {
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

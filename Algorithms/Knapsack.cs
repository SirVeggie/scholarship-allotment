using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public static class Knapsack {

        public static List<SackItem<T>> Solve<T>(List<SackItem<T>> items, int weight, bool debug = false) {
            return new KnapsackNormal<T>(items, weight).SetDebug(debug).Solve();
        }

        public static List<SackItem<T>> Solve<T>(List<List<SackItem<T>>> items, int weight, bool debug = false) {
            return new KnapsackChoice<T>(items, weight).SetDebug(debug).Solve();
        }

        public static List<SackItem2D<T>> Solve<T>(List<List<SackItem2D<T>>> items, int weightX, int weightY, bool debug = false) {
            return new Knapsack2D<T>(items, weightX, weightY).SetDebug(debug).Solve();
        }
    }

    public class KnapsackNormal<T> {

        private bool debug;

        public List<SackItem<T>> Items { get; }
        public KnapsackStep<T>[,] Matrix { get; }
        public int WeightLimit { get; }
        public Func<int, int, (int, int), bool>? CancelInclude { get; set; }

        public KnapsackNormal(List<SackItem<T>> items, int weightLimit) {
            Items = items;
            WeightLimit = weightLimit;
            Matrix = Initialize();
        }

        public List<SackItem<T>> Solve() {
            InternalSolve();
            if (debug)
                PrintMatrix();
            return RetrieveItems();
        }

        public KnapsackNormal<T> SetDebug(bool state) {
            debug = state;
            return this;
        }

        private KnapsackStep<T>[,] Initialize() {
            KnapsackStep<T>[,] matrix = new KnapsackStep<T>[WeightLimit + 1, Items.Count + 1];

            for (int weight = 0; weight < WeightLimit + 1; weight++) {
                matrix[weight, 0] = new(0, null, new(-1, -1));
            }

            return matrix;
        }

        private void InternalSolve() {
            for (int itemIndex = 1; itemIndex < Items.Count + 1; itemIndex++) {
                for (int weight = 0; weight < WeightLimit + 1; weight++) {
                    KnapsackStep<T> inc = IncludeItem(itemIndex, weight);
                    KnapsackStep<T> exc = ExcludeItem(itemIndex, weight);

                    if (exc.Value >= inc.Value) {
                        Matrix[weight, itemIndex] = exc;
                    } else {
                        Matrix[weight, itemIndex] = inc;
                    }
                }
            }
        }

        private KnapsackStep<T> IncludeItem(int index, int weight) {
            SackItem<T> item = Items[index - 1];
            (int, int) position = new(weight - item.Weight, index - 1);

            if (item.Weight > weight)
                return new(0, null, new(-1, -1));
            if (CancelInclude?.Invoke(index, weight, position) ?? false)
                return new(0, null, new(-1, -1));
            KnapsackStep<T> prev = Matrix[position.Item1, position.Item2];
            return new(item.Value + prev.Value, item, position);
        }

        private KnapsackStep<T> ExcludeItem(int index, int weight) {
            double value = Matrix[weight, index - 1].Value;
            return new(value, null, new(weight, index - 1));
        }

        private List<SackItem<T>> RetrieveItems() {
            List<SackItem<T>> result = new();
            KnapsackStep<T> step = Matrix[WeightLimit, Items.Count];

            while (step.Origin.Item1 != -1) {
                if (step.AddedItem != null)
                    result.Add(step.AddedItem);
                step = Matrix[step.Origin.Item1, step.Origin.Item2];
            }

            result.Reverse();
            return result;
        }

        public void PrintMatrix() {
            Console.Write("x".PadRight(10, ' '));
            for (int i = 0; i < WeightLimit + 1; i++)
                Console.Write($"{i}".PadRight(10, ' '));
            Console.WriteLine();

            for (int itemIndex = 0; itemIndex < Items.Count + 1; itemIndex++) {
                Console.Write($"{itemIndex}".PadRight(10, ' '));
                for (int weight = 0; weight < WeightLimit + 1; weight++) {
                    var item = Matrix[weight, itemIndex];
                    Console.Write($"{item.Value}{(item.AddedItem != null ? $"|{weight-item.Origin.Item1}" : "")}".PadRight(10, ' '));
                }
                Console.WriteLine();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public class KnapsackChoice<T> {

        private bool debug;

        public List<List<SackItem<T>>> Items { get; }
        public KnapsackStep<T>[,] Matrix { get; }
        public int WeightLimit { get; }

        public KnapsackChoice(List<List<SackItem<T>>> items, int weightLimit) {
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

        public KnapsackChoice<T> SetDebug(bool state) {
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
            List<SackItem<T>> choices = Items[index - 1];
            KnapsackStep<T> result = new(0, null, new(-1, -1));
            double bestValue = 0;

            for (int i = 0; i < choices.Count; i++) {
                SackItem<T> item = choices[i];
                (int, int) position = new(weight - item.Weight, index - 1);

                if (item.Weight > weight)
                    continue;
                KnapsackStep<T> prev = Matrix[position.Item1, position.Item2];
                double value = item.Value + prev.Value;

                if (value > bestValue) {
                    bestValue = value;
                    result = new(item.Value + prev.Value, item, position);
                }
            }

            return result;
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
                    Console.Write($"{item.Value.ToString().Split(',')[0]}{(item.AddedItem != null ? $"|{weight-item.Origin.Item1}" : "")}".PadRight(10, ' '));
                }
                Console.WriteLine();
            }
        }
    }
}

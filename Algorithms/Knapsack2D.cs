using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public class Knapsack2D<T> {
        private bool debug;

        public List<List<SackItem2D<T>>> Items { get; }
        public KnapsackStep2D<T>[,,] Matrix { get; }
        public int WeightLimitX { get; }
        public int WeightLimitY { get; }

        public Knapsack2D(List<List<SackItem2D<T>>> items, int weightLimitX, int weightLimitY) {
            Items = items;
            WeightLimitX = weightLimitX;
            WeightLimitY = weightLimitY;
            Matrix = Initialize();
        }

        public List<SackItem2D<T>> Solve() {
            InternalSolve();
            if (debug)
                PrintMatrix();
            return RetrieveItems();
        }

        public Knapsack2D<T> SetDebug(bool state) {
            debug = state;
            return this;
        }

        private KnapsackStep2D<T>[,,] Initialize() {
            KnapsackStep2D<T>[,,] matrix = new KnapsackStep2D<T>[WeightLimitX + 1, WeightLimitY + 1, Items.Count + 1];

            for (int weightX = 0; weightX < WeightLimitX + 1; weightX++) {
                for (int weightY = 0; weightY < WeightLimitY + 1; weightY++) {
                    matrix[weightX, weightY, 0] = new(0, null, new(-1, -1, -1));
                }
            }

            return matrix;
        }

        private void InternalSolve() {
            for (int itemIndex = 1; itemIndex < Items.Count + 1; itemIndex++) {
                for (int weightX = 0; weightX < WeightLimitX + 1; weightX++) {
                    for (int weightY = 0; weightY < WeightLimitY + 1; weightY++) {
                        KnapsackStep2D<T> inc = IncludeItem(itemIndex, weightX, weightY);
                        KnapsackStep2D<T> exc = ExcludeItem(itemIndex, weightX, weightY);

                        if (exc.Value >= inc.Value) {
                            Matrix[weightX, weightY, itemIndex] = exc;
                        } else {
                            Matrix[weightX, weightY, itemIndex] = inc;
                        }
                    }
                }
            }
        }

        private KnapsackStep2D<T> IncludeItem(int index, int weightX, int weightY) {
            List<SackItem2D<T>> choices = Items[index - 1];
            KnapsackStep2D<T> result = new(0, null, new(-1, -1, -1));
            double bestValue = 0;

            for (int i = 0; i < choices.Count; i++) {
                SackItem2D<T> item = choices[i];
                (int, int, int) position = new(weightX - item.WeightX, weightY - item.WeightY, index - 1);

                if (item.WeightX > weightX || item.WeightY > weightY)
                    continue;
                KnapsackStep2D<T> prev = Matrix[position.Item1, position.Item2, position.Item3];
                double value = item.Value + prev.Value;

                if (value > bestValue) {
                    bestValue = value;
                    result = new(item.Value + prev.Value, item, position);
                }
            }

            return result;
        }

        private KnapsackStep2D<T> ExcludeItem(int index, int weightX, int weightY) {
            double value = Matrix[weightX, weightY, index - 1].Value;
            return new(value, null, new(weightX, weightY, index - 1));
        }

        private List<SackItem2D<T>> RetrieveItems() {
            List<SackItem2D<T>> result = new();
            KnapsackStep2D<T> step = Matrix[WeightLimitX, WeightLimitY, Items.Count];

            while (step.Origin.Item1 != -1) {
                if (step.AddedItem != null)
                    result.Add(step.AddedItem);
                step = Matrix[step.Origin.Item1, step.Origin.Item2, step.Origin.Item3];
            }

            result.Reverse();
            return result;
        }

        public void PrintMatrix(bool all = false) {
            Console.Write("x".PadRight(10, ' '));
            for (int i = 0; i < WeightLimitX + 1; i++)
                Console.Write($"{i}".PadRight(10, ' '));
            Console.WriteLine();

            for (int weightY = all ? 0 : WeightLimitY; weightY < WeightLimitY + 1; weightY++) {
                if (all && weightY > 0)
                    Console.WriteLine("\n");
                for (int itemIndex = 0; itemIndex < Items.Count + 1; itemIndex++) {
                    Console.Write($"{itemIndex}".PadRight(10, ' '));
                    for (int weightX = 0; weightX < WeightLimitX + 1; weightX++) {
                        var item = Matrix[weightX, weightY, itemIndex];
                        if (item == null)
                            Console.Write("null".PadRight(10, ' '));
                        else
                            Console.Write($"{item.Value.ToString().Split(',')[0]}{(item.AddedItem != null ? $"|{weightX - item.Origin.Item1}|{weightY - item.Origin.Item2}" : "")}".PadRight(10, ' '));
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}

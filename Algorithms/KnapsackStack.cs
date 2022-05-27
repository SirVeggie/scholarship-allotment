using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {

    public class KnapsackStack<T> {

        public KnapsackNormal<T> BaseSolver { get; }
        public List<List<SackItem<T>>> Items { get; set; }
        public bool[] Stacks { get; }
        public int[] StackMap { get; }

        public KnapsackStack(List<List<SackItem<T>>> items, int weightLimit) {
            Items = items;
            Stacks = InitializeStacks();
            StackMap = InitializeStackMap();
            BaseSolver = new(items.SelectMany(x => x).ToList(), weightLimit);
            BaseSolver.CancelInclude = (index, weight, prev) => {
                var step = BaseSolver.Matrix[prev.Item1, prev.Item2];
                //Console.WriteLine($"");
                return !Stacks[index - 1] && step.AddedItem == null;
            };
        }

        public List<List<SackItem<T>>> Solve() {
            BaseSolver.Solve();
            return RetrieveItems();
        }

        public KnapsackStack<T> SetDebug(bool state) {
            BaseSolver.SetDebug(state);
            return this;
        }

        private bool[] InitializeStacks() {
            bool[] stacks = new bool[Items.Sum(x => x.Count)];

            int pointer = 0;
            for (int i = 0; i < Items.Count; i++) {
                stacks[pointer] = true;
                pointer += Items[i].Count;
            }

            return stacks;
        }

        private int[] InitializeStackMap() {
            int[] map = new int[Items.Sum(x => x.Count)];

            int pointer = 0;
            for (int i = 0; i < Items.Count; i++) {
                for (int k = 0; k < Items[i].Count; k++) {
                    map[pointer] = i;
                    pointer++;
                }
            }

            return map;
        }

        private List<List<SackItem<T>>> RetrieveItems() {
            List<List<SackItem<T>>> result = new();
            for (int i = 0; i < Items.Count; i++)
                result.Add(new());
            (int, int) position = new(BaseSolver.WeightLimit, Items.Sum(x => x.Count));
            KnapsackStep<T> step = BaseSolver.Matrix[position.Item1, position.Item2];

            while (step.Origin.Item1 != -1) {
                if (step.AddedItem != null) {
                    int target = StackMap[position.Item2 - 1];
                    if (result[target] == null)
                        result[target] = new();
                    result[target].Add(step.AddedItem);
                }

                position = step.Origin;
                step = BaseSolver.Matrix[position.Item1, position.Item2];
            }

            return result;
        }
    }
}

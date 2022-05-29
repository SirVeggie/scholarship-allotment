using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public class KnapsackBranch2DX<T> {

        private bool debug;
        private State state;

        public List<List<SackItem2D<T>>> Items { get; }
        public List<Dictionary<int, Dictionary<int, KnapsackStep2D<T>>>> Memory { get; }
        public Stack<(int, int, int)> Stack { get; }
        public int WeightLimitX { get; }
        public int WeightLimitY { get; }

        public KnapsackBranch2DX(List<List<SackItem2D<T>>> items, int weightLimitX, int weightLimitY) {
            state = new(this);

            Items = items;
            WeightLimitX = weightLimitX;
            WeightLimitY = weightLimitY;
            Memory = new();
            for (int i = 0; i < items.Count; i++)
                Memory.Add(new());
            Stack = new();
            Stack.Push((items.Count - 1, weightLimitX, weightLimitY));
        }

        public List<SackItem2D<T>> Solve() {
            while (Stack.Count != 0) {
                state.Update();

                if (HandleLeaf())
                    continue;
                if (HandleBranching())
                    continue;
                HandleCombining();
            }

            return RetrieveItems();
        }

        private bool HandleLeaf() {
            if (state.Depth == 0) {
                var query = state.Items.Select((x, i) => (i, (state.MemoryItems[i]?.Value ?? 0) + x.Value)).Where(x => state.ChildItems[x.i] != null);
                var (itemIndex, itemValue) = query.Any() ? query.MaxBy(x => x.Item2) : (0, 0);

                if (itemValue != 0)
                    Assign(state.Items[itemIndex].Value, (-1, -1, -1), state.Items[itemIndex]);
                else
                    Assign(0, (-1, -1, -1));
                Stack.Pop();
                return true;
            }

            return false;
        }

        private bool HandleBranching() {
            for (int i = 0; i < state.Items.Count; i++)
                if (state.ChildItems[i] != null && state.MemoryItems[i] == null)
                    Stack.Push(state.ChildItems[i]!.Value);
            if (state.ChildEmpty != null && state.MemoryEmpty == null && !state.ChildItems!.Contains(state.ChildEmpty))
                Stack.Push(state.ChildEmpty!.Value);
            if (Stack.Peek() != state.Current)
                return true;

            return false;
        }

        private void Push((int, int, int)? position) {
            if (Stack.Peek() == position!.Value) {
                Console.WriteLine($"Pushed duplicate! {state.ChildEmpty}");
                for (int i = 0; i < state.ChildItems.Count; i++) {
                    Console.WriteLine($"{state.ChildItems[i]} - {state.Items[i].Value} - {state.Items[i].WeightX} - {state.Items[i].WeightY}");
                }
                Console.WriteLine();
            }
            Stack.Push(position!.Value);
        }

        private void HandleCombining() {
            var query = state.Items.Select((x, i) => (i, (state.MemoryItems[i]?.Value ?? 0) + x.Value)).Where(x => state.ChildItems[x.i] != null);
            var (itemIndex, itemValue) = query.Any() ? query.MaxBy(x => x.Item2) : (0, 0);
            double emptyValue = state.MemoryEmpty!.Value;

            if (itemValue > emptyValue)
                Assign(itemValue, state.ChildItems[itemIndex], state.Items[itemIndex]);
            else
                Assign(emptyValue, state.ChildEmpty);
            Stack.Pop();
        }

        private List<SackItem2D<T>> RetrieveItems() {
            List<SackItem2D<T>> result = new();
            KnapsackStep2D<T> step = Recall((Items.Count - 1, WeightLimitX, WeightLimitY))!;
            while (step != null) {
                if (step.AddedItem != null)
                    result.Add(step.AddedItem);
                step = Recall(step.Origin)!;
            }

            result.Reverse();
            return result;
        }

        private KnapsackStep2D<T>? Recall((int, int, int)? position) {
            if (position == null)
                return null;
            if (position.Value.Item1 < 0 || position.Value.Item2 < 0 || position.Value.Item3 < 0)
                return null;
            if (Memory[position.Value.Item1].ContainsKey(position.Value.Item2))
                if (Memory[position.Value.Item1][position.Value.Item2].ContainsKey(position.Value.Item3))
                    return Memory[position.Value.Item1][position.Value.Item2][position.Value.Item3];
            return null;
        }

        private void Assign(double value, (int, int, int)? origin, SackItem2D<T>? item = null) {
            if (origin == null)
                throw new ArgumentNullException(nameof(origin));
            KnapsackStep2D<T> step = new(value, item, origin.Value);
            if (!Memory[state.Current.Item1].ContainsKey(state.Current.Item2))
                Memory[state.Current.Item1].Add(state.Current.Item2, new());
            if (!Memory[state.Current.Item1][state.Current.Item2].ContainsKey(state.Current.Item3))
                Memory[state.Current.Item1][state.Current.Item2].Add(state.Current.Item3, step);
            else if (step.Value > Memory[state.Current.Item1][state.Current.Item2][state.Current.Item3].Value)
                Memory[state.Current.Item1][state.Current.Item2][state.Current.Item3] = step;
        }

        public KnapsackBranch2DX<T> SetDebug(bool state) {
            debug = state;
            return this;
        }

        private class State {
            public KnapsackBranch2DX<T> Parent { get; }
            public int Depth { get; private set; }
            public int WeightX { get; private set; }
            public int WeightY { get; private set; }
            public List<SackItem2D<T>> Items { get; private set; }
            public (int, int, int) Current { get; private set; }
            public List<(int, int, int)?> ChildItems { get; private set; }
            public (int, int, int)? ChildEmpty { get; private set; }
            public List<KnapsackStep2D<T>?> MemoryItems { get; private set; }
            public KnapsackStep2D<T>? MemoryEmpty { get; private set; }

            public State(KnapsackBranch2DX<T> parent) {
                Parent = parent;
            }

            public void Update() {
                Current = Parent.Stack.Peek();
                Depth = Current.Item1;
                WeightX = Current.Item2;
                WeightY = Current.Item3;
                Items = Parent.Items[Depth];
                ChildEmpty = Depth > 0 ? (Depth - 1, WeightX, WeightY) : null;
                MemoryEmpty = Parent.Recall(ChildEmpty);

                ChildItems = new();
                MemoryItems = new();
                for (int i = 0; i < Items.Count; i++) {
                    ChildItems.Add(Items[i].WeightX <= WeightX && Items[i].WeightY <= WeightY ? (Depth - 1, WeightX - Items[i].WeightX, WeightY - Items[i].WeightY) : null);
                    MemoryItems.Add(Parent.Recall(ChildItems[i]));
                }
            }
        }
    }
}

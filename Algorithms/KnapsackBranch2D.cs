using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    internal class KnapsackBranch2D<T> {

        private bool debug;
        private State state;

        public List<SackItem2D<T>> Items { get; }
        public List<Dictionary<int, Dictionary<int, KnapsackStep2D<T>>>> Memory { get; }
        public Stack<(int, int, int)> Stack { get; }
        public int WeightLimitX { get; }
        public int WeightLimitY { get; }

        public KnapsackBranch2D(List<SackItem2D<T>> items, int weightLimitX, int weightLimitY) {
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
                if (state.ChildItem != null)
                    Assign(state.Item.Value, (-1, -1, -1), state.Item);
                else
                    Assign(0, (-1, -1, -1));
                Stack.Pop();
                return true;
            }

            return false;
        }

        private bool HandleBranching() {
            if (state.ChildItem != null && state.MemoryItem == null)
                Stack.Push(state.ChildItem!.Value);
            if (state.ChildEmpty != null && state.MemoryEmpty == null && state.ChildEmpty != state.ChildItem)
                Stack.Push(state.ChildEmpty!.Value);
            if (Stack.Peek() != state.Current)
                return true;

            return false;
        }

        private void HandleCombining() {
            double itemValue = (state.MemoryItem?.Value ?? 0) + state.Item.Value;
            double emptyValue = state.MemoryEmpty!.Value;

            if (state.ChildItem != null && itemValue > emptyValue)
                Assign(itemValue, state.ChildItem, state.Item);
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
            Memory[state.Current.Item1][state.Current.Item2].Add(state.Current.Item3, step);
        }

        public KnapsackBranch2D<T> SetDebug(bool state) {
            debug = state;
            return this;
        }

        private class State {
            public KnapsackBranch2D<T> Parent { get; }
            public int Depth { get; private set; }
            public int WeightX { get; private set; }
            public int WeightY { get; private set; }
            public SackItem2D<T> Item { get; private set; }
            public (int, int, int) Current { get; private set; }
            public (int, int, int)? ChildItem { get; private set; }
            public (int, int, int)? ChildEmpty { get; private set; }
            public KnapsackStep2D<T>? MemoryItem { get; private set; }
            public KnapsackStep2D<T>? MemoryEmpty { get; private set; }

            public State(KnapsackBranch2D<T> parent) {
                Parent = parent;
            }

            public void Update() {
                Current = Parent.Stack.Peek();
                Depth = Current.Item1;
                WeightX = Current.Item2;
                WeightY = Current.Item3;
                Item = Parent.Items[Depth];
                ChildItem = Item.WeightX <= WeightX && Item.WeightY <= WeightY ? (Depth - 1, WeightX - Item.WeightX, WeightY - Item.WeightY) : null;
                ChildEmpty = Depth > 0 ? (Depth - 1, WeightX, WeightY) : null;
                MemoryItem = Parent.Recall(ChildItem);
                MemoryEmpty = Parent.Recall(ChildEmpty);
            }
        }
    }
}

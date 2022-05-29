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

        public List<SackItem<T>> Items { get; }
        public List<Dictionary<int, KnapsackStep<T>>> Memory { get; }
        public Stack<(int, int)> Stack { get; }
        public int WeightLimit { get; }

        public KnapsackBranch2D(List<SackItem<T>> items, int weightLimit) {
            state = new(this);

            Items = items;
            WeightLimit = weightLimit;
            Memory = new();
            for (int i = 0; i < items.Count; i++)
                Memory.Add(new());
            Stack = new();
            Stack.Push((items.Count - 1, weightLimit));
        }

        public List<SackItem<T>> Solve() {
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
                    Assign(state.Item.Value, (-1, -1), state.Item);
                else
                    Assign(0, (-1, -1));
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

        private List<SackItem<T>> RetrieveItems() {
            List<SackItem<T>> result = new();
            KnapsackStep<T> step = Recall((Items.Count - 1, WeightLimit))!;
            while (step != null) {
                if (step.AddedItem != null)
                    result.Add(step.AddedItem);
                step = Recall(step.Origin)!;
            }

            result.Reverse();
            return result;
        }

        private KnapsackStep<T>? Recall((int, int)? position) {
            if (position == null)
                return null;
            (int, int) pos = ((int, int))position;
            if (pos.Item1 < 0 || pos.Item2 < 0)
                return null;
            if (Memory[pos.Item1].ContainsKey(pos.Item2))
                return Memory[pos.Item1][pos.Item2];
            return null;
        }

        private void Assign(double value, (int, int)? origin, SackItem<T>? item = null) {
            if (origin == null)
                throw new ArgumentNullException(nameof(origin));
            KnapsackStep<T> step = new(value, item, ((int, int))origin);
            Memory[state.Current.Item1].Add(state.Current.Item2, step);
        }

        public KnapsackBranch2D<T> SetDebug(bool state) {
            debug = state;
            return this;
        }

        private class State {
            public KnapsackBranch2D<T> Parent { get; }
            public int Depth { get; private set; }
            public int Weight { get; private set; }
            public SackItem<T> Item { get; private set; }
            public (int, int) Current { get; private set; }
            public (int, int)? ChildItem { get; private set; }
            public (int, int)? ChildEmpty { get; private set; }
            public KnapsackStep<T>? MemoryItem { get; private set; }
            public KnapsackStep<T>? MemoryEmpty { get; private set; }

            public State(KnapsackBranch2D<T> parent) {
                Parent = parent;
            }

            public void Update() {
                Current = Parent.Stack.Peek();
                Depth = Current.Item1;
                Weight = Current.Item2;
                Item = Parent.Items[Depth];
                ChildItem = Item.Weight <= Weight ? (Depth - 1, Weight - Item.Weight) : null;
                ChildEmpty = Depth > 0 ? (Depth - 1, Weight) : null;
                MemoryItem = Parent.Recall(ChildItem);
                MemoryEmpty = Parent.Recall(ChildEmpty);
            }
        }
    }
}

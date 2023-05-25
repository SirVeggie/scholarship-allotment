﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution {
    public static class Tools {

        public static List<T[]> RemoveDuplicates<T>(List<T[]> list) {
            HashSet<Container<T>> set = new();
            foreach (T[] array in list) {
                set.Add(new(array));
            }

            return set.Select(x => x.Values).ToList();
        }

        public static IEnumerable<T[]> Permutations<T>(T[] values, int fromInd = 0) {
            if (fromInd + 1 == values.Length)
                yield return values;
            else {
                foreach (var v in Permutations(values, fromInd + 1))
                    yield return v;

                for (var i = fromInd + 1; i < values.Length; i++) {
                    Swap(values, fromInd, i);
                    foreach (var v in Permutations(values, fromInd + 1))
                        yield return v;
                    Swap(values, fromInd, i);
                }
            }
        }

        public static List<bool[]> BoolPermutations(int amount, int length) {
            if (amount > length)
                throw new ArgumentException("Amount cannot be larger than length");
            bool[] values = new bool[length];

            for (int i = 0; i < length; i++) {
                if (i < amount) {
                    values[i] = true;
                } else {
                    values[i] = false;
                }
            }

            List<bool[]> result = new();
            foreach (var perm in Permutations(values)) {
                result.Add((bool[])perm.Clone());
            }

            //Console.WriteLine($"{result.Count}");
            result = RemoveDuplicates(result);
            //Console.WriteLine($"{result.Count}");
            return result;
        }

        public static List<double[]> TrioPermutations(int n) {
            List<double[]> result = new();
            bool odd = n % 2 == 1;

            for (int i = 0; i < n / 2 + 1; i++) {
                List<double> values = new();
                int halves = i * 2 + (odd ? 1 : 0);

                for (int k = 0; k < halves; k++)
                    values.Add(0.5);
                for (int k = 0; k < (n - halves) / 2; k++) {
                    values.Add(1);
                    values.Add(0);
                }

                foreach (var perm in Permutations(values.ToArray())) {
                    result.Add((double[])perm.Clone());
                }
            }

            return RemoveDuplicates(result);
        }

        public static List<int[]> Combinations5(int n, int[] bans) {
            List<int[]> res = new();

            for (int f1 = 0; f1 < n - 4; f1++) {
                if (bans.Contains(f1))
                    continue;
                for (int f2 = f1 + 1; f2 < n - 3; f2++) {
                    if (bans.Contains(f2))
                        continue;
                    for (int f3 = f2 + 1; f3 < n - 2; f3++) {
                        if (bans.Contains(f3))
                            continue;
                        for (int f4 = f3 + 1; f4 < n - 1; f4++) {
                            if (bans.Contains(f4))
                                continue;
                            for (int f5 = f4 + 1; f5 < n; f5++) {
                                if (bans.Contains(f5))
                                    continue;
                                res.Add(new int[] { f1, f2, f3, f4, f5 });
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static uint SetBit(uint value, int bit) {
            return value | ((uint)1 << bit);
        }

        public static void PrintArray<T>(T[]? array) {
            if (array == null)
                Console.WriteLine("[null]");
            else
                Console.WriteLine(string.Join(", ", array));
        }

        public static void PrintArray<T>(List<T>? list) {
            if (list == null)
                Console.WriteLine("[null]");
            else
                Console.WriteLine(string.Join(", ", list));
        }

        public static void Swap<T>(T[] values, int pos1, int pos2) {
            if (pos1 != pos2) {
                T tmp = values[pos1];
                values[pos1] = values[pos2];
                values[pos2] = tmp;
            }
        }

        public static void Swap<T>(List<T> values, int pos1, int pos2) {
            if (pos1 != pos2) {
                T tmp = values[pos1];
                values[pos1] = values[pos2];
                values[pos2] = tmp;
            }
        }

        public class Container<T> {
            public T[] Values { get; private set; }

            public Container(T[] values) {
                Values = values;
            }

            public override int GetHashCode() {
                HashCode hash = new();
                foreach (var value in Values)
                    hash.Add(value);
                return hash.ToHashCode();
            }

            public override bool Equals(object? obj)
                => obj is not null
                && obj is Container<T> other
                && other.Values is not null
                && Values is not null
                && Values.SequenceEqual(other.Values);
        }
    }
}
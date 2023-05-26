using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

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

        public static List<Trio> TrioPermutationsFast(int n) {
            List<Trio> result = new();
            bool odd = n % 2 == 1;

            for (int i = 0; i <= n / 2; i++) {
                int full = n / 2 - i;
                int halves = i * 2 + (odd ? 1 : 0);

                if (full == 0) {
                    result.Add(new(new int[0], FullList(halves), new int[0]));
                }

                List<int[]> fullCombinations = Combinations(n, full);

                foreach (var comb in fullCombinations) {
                    List<int[]> halfCombinations = Combinations(n, halves, comb);
                    foreach (var comb2 in halfCombinations) {
                        int[] comb3 = new int[full];
                        int p = 0;
                        for (int k = 0; k < n; k++) {
                            if (!comb.Contains(k) && !comb2.Contains(k)) {
                                comb3[p] = k;
                                p++;
                            }
                        }
                        result.Add(new(comb, comb2, comb3));
                    }
                }
            }

            return result;
        }

        public static int[] FullList(int n) {
            int[] result = new int[n];
            for (int i = 0; i < n; i++) {
                result[i] = i;
            }
            return result;
        }

        public struct Trio {
            public int[] fulls;
            public int[] halves;
            public int[] nones;

            public Trio(int[] fulls, int[] halves, int[] nones) {
                this.fulls = fulls;
                this.halves = halves;
                this.nones = nones;
            }

            public override string ToString() {
                return $"({string.Join(",", fulls)}), ({string.Join(",", halves)}), ({string.Join(",", nones)})";
            }
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

        public static List<int[]> Combinations(int n, int select, int[] bans = null) {
            if (n <= 0 || select < 0)
                throw new ArgumentOutOfRangeException(n < 0 ? nameof(n) : nameof(select));
            if (select > n)
                throw new ArgumentException($"{nameof(select)} cannot be higher than {nameof(n)}");
            if (n == select)
                return new() { FullList(n) };
            if (select == 0)
                return new() { Array.Empty<int>() };
            if (bans == null)
                bans = Array.Empty<int>();
            List<int[]> res = new();
            int[] pointers = new int[select];

            for (int i = 0; i < select; i++) {
                pointers[i] = i;
            }

            int p = 0;
            while (true) {
                if (bans.Contains(pointers[p])) {
                    p = IncrementPointer(p);
                    if (p == -1)
                        break;
                    continue;
                }

                if (p < select - 1) {
                    p++;
                    continue;
                }

                res.Add((int[])pointers.Clone());
                p = IncrementPointer(p);
                if (p == -1)
                    break;
            }

            int IncrementPointer(int index) {
                pointers[index] += 1;
                if (pointers[index] >= n - (select - index - 1)) {
                    if (index == 0)
                        return -1;
                    return IncrementPointer(index - 1);
                }

                for (int k = index + 1; k < select; k++) {
                    pointers[k] = pointers[k - 1] + 1;
                }

                return index;
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

        public static TestResult GetTestResult(Result result) {
            return new TestResult {
                success = true,
                value = result.Total(),
                expectedStudents = result.Full.Sum(x => x.PHigh) + result.Half.Sum(x => x.PMid) + result.None.Sum(x => x.PLow),
                expectedScholarships = result.Full.Sum(x => x.PHigh) + result.Half.Sum(x => x.PMid/2),
            };
        }

        public static TestResult GetTestResult(List<Student> result) {
            return GetTestResult(new Result(
                result,
                new(),
                result.Where(x => !result.Contains(x)).ToList()
            ));
        }

        public static TestResult GetTestResult(KnapsackResult<(ScType, Student)> result) {
            return GetTestResult(new Result(
                result.Items.Where(x => x.item.Item1.IsFull()).Select(x => x.item.Item2).ToList(),
                result.Items.Where(x => x.item.Item1.IsHalf()).Select(x => x.item.Item2).ToList(),
                result.Items.Where(x => x.item.Item1.IsNone()).Select(x => x.item.Item2).ToList()
            ));
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
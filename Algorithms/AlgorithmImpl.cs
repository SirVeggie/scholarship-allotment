using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class AlgorithmImpl {

        public static TestResult SolveFullBrute(List<Student> students) {
            Result result = BasicDistribution.Brute(students);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullSort(List<Student> students) {
            Result result = BasicDistribution.Sorted(students);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullMedian(List<Student> students) {
            Result result = BasicDistribution.Median(students);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullKnapsack(List<Student> students) {
            List<KnapsackItem<Student>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new(students[i], students[i].Diff, 1));
            }

            var result = Knapsack<Student>.Solve(items.ToArray(), students.Count / 2);

            var included = result.Items.Select(x => x.item).ToList();
            return Tools.GetTestResult(new Result(
                included,
                new(),
                students.Where(x => !included.Contains(x)).ToList()
            ));
        }

        public static TestResult SolveFullChoiceKnapsack(List<Student> students) {
            List<List<KnapsackItem<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new() {
                    new((ScType.Full, students[i]), students[i].HighScore, 1),
                    //new((ScType.Half, items[i]), items[i].MidScore, 1),
                    new((ScType.None, students[i]), students[i].LowScore, 0),
                });
            }

            var result = KnapsackChoice<(ScType, Student)>.Solve(items, students.Count / 2);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullKnapsackBranch(List<Student> students) {
            List<KnapsackItem<Student>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new(students[i], students[i].Diff, 1));
            }

            var result = KnapsackBranch<Student>.Solve(items, students.Count / 2);

            var included = result.Items.Select(x => x.item).ToList();
            return Tools.GetTestResult(new Result(
                included,
                new(),
                students.Where(x => !included.Contains(x)).ToList()
            ));
        }

        public static TestResult SolveFullKnapsack2D(List<Student> students) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, 1, 0),
                    new((ScType.None, s), s.LowScore, 0, 0)
                });
            }

            var result = Knapsack2D<(ScType, Student)>.Solve(items, students.Count / 2, 0);
            return Tools.GetTestResult(result);
        }

        // -------- Rejection -------- //

        public static TestResult SolveRejectionBrute(List<Student> students, int amount) {
            var result = BasicDistribution.Brute(students, amount);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveRejectionSort(List<Student> students, int amount) {
            var result = MultiSort.Solve(students, amount);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveRejectionKnapsack2D(List<Student> students, int amount) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, 1, 1),
                    new((ScType.None, s), s.LowScore, 0, 1),
                });
            }

            var result = Knapsack2D<(ScType, Student)>.Solve(items, amount / 2, amount);

            return Tools.GetTestResult(result);
        }

        public static TestResult SolveRejectionKnapsack2DBranch(List<Student> students, int amount) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, 1, 1),
                    new((ScType.None, s), s.LowScore, 0, 1),
                });
            }

            var result = KnapsackBranch2D<(ScType, Student)>.Solve(items, amount / 2, amount);

            return Tools.GetTestResult(result);
        }

        // -------- Split -------- //

        public static TestResult SolveSplitBrute(List<Student> students) {
            Result result = HalfDistribution.Brute(students);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveSplitSort(List<Student> students) {
            var result = MultiSortSplit.Solve(students);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveSplitChoiceKnapsack(List<Student> students) {
            List<List<KnapsackItem<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new() {
                    new((ScType.Full, students[i]), students[i].HighScore, 2),
                    new((ScType.Half, students[i]), students[i].MidScore, 1),
                    new((ScType.None, students[i]), students[i].LowScore, 0),
                });
            }

            var result = KnapsackChoice<(ScType, Student)>.Solve(items, students.Count);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveSplitKnapsack2D(List<Student> students) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new() {
                    new((ScType.Full, students[i]), students[i].HighScore, 2, 0),
                    new((ScType.Half, students[i]), students[i].MidScore, 1, 0),
                    new((ScType.None, students[i]), students[i].LowScore, 0, 0),
                });
            }

            int wx = students.Count;
            int wy = 0;

            var result = Knapsack2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveSplitKnapsack2DBranch(List<Student> students) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new() {
                    new((ScType.Full, students[i]), students[i].HighScore, 2, 0),
                    new((ScType.Half, students[i]), students[i].MidScore, 1, 0),
                    new((ScType.None, students[i]), students[i].LowScore, 0, 0),
                });
            }

            int wx = students.Count;
            int wy = 0;

            var result = KnapsackBranch2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        // -------- Split Rejection -------- //

        public static TestResult SolveSplitRejectionSort(List<Student> students, int amount) {
            var result = MultiSortSplitRejection.Solve(students, amount);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveSplitRejectionKnapsack2D(List<Student> students, int amount) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new() {
                    new((ScType.Full, students[i]), students[i].HighScore, 2, 1),
                    new((ScType.Half, students[i]), students[i].MidScore, 1, 1),
                    new((ScType.None, students[i]), students[i].LowScore, 0, 1),
                });
            }

            int wx = amount;
            int wy = amount;

            var result = Knapsack2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        // -------- Approximation -------- //

        public static TestResult SolveApproximation(List<Student> students, int amount, int accuracy) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, 2, (int)Math.Ceiling(s.PHigh * accuracy)),
                    new((ScType.Half, s), s.MidScore, 1, (int)Math.Ceiling(s.PMid * accuracy)),
                    new((ScType.None, s), s.LowScore, 0, (int)Math.Ceiling(s.PLow * accuracy)),
                });
            }

            int wx = amount;
            int wy = amount * accuracy;

            var result = Knapsack2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveApproximationBranch(List<Student> students, int amount, int accuracy) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, 2, (int)Math.Round(s.PHigh * accuracy)),
                    new((ScType.Half, s), s.MidScore, 1, (int)Math.Round(s.PMid * accuracy)),
                    new((ScType.None, s), s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
                });
            }

            int wx = amount;
            int wy = amount * accuracy;

            var result = KnapsackBranch2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        // -------- Double Approximation -------- //

        public static TestResult SolveDoubleApproximation(List<Student> students, int amount, int accuracy) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, (int)Math.Ceiling(s.PHigh * accuracy), (int)Math.Ceiling(s.PHigh * accuracy)),
                    new((ScType.Half, s), s.MidScore, (int)Math.Ceiling(s.PMid * accuracy / 2), (int)Math.Ceiling(s.PMid * accuracy)),
                    new((ScType.None, s), s.LowScore, 0, (int)Math.Ceiling(s.PLow * accuracy)),
                });
            }

            int wx = amount * accuracy / 2;
            int wy = amount * accuracy;

            var result = Knapsack2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveDoubleApproximationBranch(List<Student> students, int amount, int accuracy) {
            List<List<KnapsackItem2D<(ScType, Student)>>> items = new();

            for (int i = 0; i < students.Count; i++) {
                var s = students[i];
                items.Add(new() {
                    new((ScType.Full, s), s.HighScore, (int)Math.Round(s.PHigh * accuracy), (int)Math.Round(s.PHigh * accuracy)),
                    new((ScType.Half, s), s.MidScore, (int)Math.Round(s.PMid * accuracy / 2), (int)Math.Round(s.PMid * accuracy)),
                    new((ScType.None, s), s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
                });
            }

            int wx = amount * accuracy / 2;
            int wy = amount * accuracy;

            var result = KnapsackBranch2D<(ScType, Student)>.Solve(items, wx, wy);
            return Tools.GetTestResult(result);
        }

        // -------- Special -------- //

        public static TestResult SolveFullKnapsackLarge(List<Student> students, int mult) {
            List<KnapsackItem<Student>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new(students[i], students[i].Diff, 1 * mult));
            }

            var result = Knapsack<Student>.Solve(items, students.Count * mult / 2);

            var included = result.Items.Select(x => x.item).ToList();
            return Tools.GetTestResult(new Result(
                included,
                new(),
                students.Where(x => !included.Contains(x)).ToList()
            ));
        }

        public static TestResult SolveFullKnapsackBranchLarge(List<Student> students, int mult) {
            List<KnapsackItem<Student>> items = new();

            for (int i = 0; i < students.Count; i++) {
                items.Add(new(students[i], students[i].Diff, 1 * mult));
            }

            var result = KnapsackBranch<Student>.Solve(items, students.Count * mult / 2);

            var included = result.Items.Select(x => x.item).ToList();
            return Tools.GetTestResult(new Result(
                included,
                new(),
                students.Where(x => !included.Contains(x)).ToList()
            ));
        }
    }
}

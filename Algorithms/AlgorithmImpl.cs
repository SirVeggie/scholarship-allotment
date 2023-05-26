using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class AlgorithmImpl {

        public static TestResult SolveFullBrute(List<Student> items) {
            Result result = BasicDistribution.Brute(items);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullSort(List<Student> items) {
            Result result = BasicDistribution.Sorted(items);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullMedian(List<Student> items) {
            Result result = BasicDistribution.Median(items);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullKnapsack(List<Student> items) {
            List<KnapsackItem<Student>> data = new();

            for (int i = 0; i < items.Count; i++) {
                data.Add(new(items[i], items[i].Diff, 1));
            }

            var result = FastKnapsack<Student>.Solve(data.ToArray(), items.Count / 2);
            return Tools.GetTestResult(result.Items.Select(x => x.item).ToList());
        }

        public static TestResult SolveFullChoiceKnapsack(List<Student> items) {
            List<List<KnapsackItem<(ScType, Student)>>> data = new();

            for (int i = 0; i < items.Count; i++) {
                data.Add(new() {
                    new((ScType.Full, items[i]), items[i].HighScore, 2),
                    //new((ScType.Half, items[i]), items[i].MidScore, 1),
                    new((ScType.None, items[i]), items[i].LowScore, 0),
                });
            }

            var result = FastKnapsackChoice<(ScType, Student)>.Solve(data, items.Count / 2);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveFullBranchKnapsack(List<Student> items) {
            List<KnapsackItem<Student>> data = new();

            for (int i = 0; i < items.Count; i++) {
                data.Add(new(items[i], items[i].Diff, 1));
            }

            var result = FastKnapsackBranch<Student>.Solve(data.ToArray(), items.Count / 2);
            return Tools.GetTestResult(result.Items.Select(x => x.item).ToList());
        }

        public static TestResult SolveHalfBrute(List<Student> items) {
            Result result = HalfDistribution.BruteOld(items);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveHalfChoiceKnapsack(List<Student> items) {
            List<List<KnapsackItem<(ScType, Student)>>> data = new();

            for (int i = 0; i < items.Count; i++) {
                data.Add(new() {
                    new((ScType.Full, items[i]), items[i].HighScore, 2),
                    new((ScType.Half, items[i]), items[i].MidScore, 1),
                    new((ScType.None, items[i]), items[i].LowScore, 0),
                });
            }

            var result = FastKnapsackChoice<(ScType, Student)>.Solve(data, items.Count / 2);
            return Tools.GetTestResult(result);
        }

        public static TestResult SolveStudentKnapsack2D(List<Student> items) {
            return default;
        }

        public static TestResult SolveStudentBranchKnapsack2D(List<Student> items) {
            return default;
        }

        public static TestResult SolveWaiverKnapsack2D(List<Student> items) {
            return default;
        }

        public static TestResult SolveWaiverBranchKnapsack2D(List<Student> items) {
            return default;
        }
    }
}

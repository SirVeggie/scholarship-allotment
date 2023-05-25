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
            //return result.Total();
            return default;
        }

        public static TestResult SolveFullSort(List<Student> items) {
            Result result = BasicDistribution.Sorted(items);
            //return result.Total();
            return default;
        }

        public static TestResult SolveFullMedian(List<Student> items) {
            Result result = BasicDistribution.Median(items);
            //return result.Total();
            return default;
        }

        public static TestResult SolveFullKnapsack(List<Student> items) {
            List<SackItem<Student>> data = new();
            double initialSum = 0;

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                initialSum += s.LowScore;
                data.Add(new(s.HighScore - s.LowScore, 1, s));
            }

            var result = new KnapsackNormal<Student>(data, items.Count / 2).Solve();
            //return result.Sum(x => x.Value) + initialSum;
            return default;
        }

        public static TestResult SolveFullChoiceKnapsack(List<Student> items) {
            List<List<SackItem<Student>>> data = new();

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                List<SackItem<Student>> temp = new();
                temp.Add(new(s.LowScore, 1, s));
                temp.Add(new(s.HighScore, 1, s));
                data.Add(temp);
            }

            var result = new KnapsackChoice<Student>(data, items.Count / 2).Solve();
            //return result.Sum(x => x.Value);
            return default;
        }

        public static TestResult SolveFullBranchKnapsack(List<Student> items) {
            List<SackItem<Student>> data = new();
            double initialSum = 0;

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                initialSum += s.LowScore;
                data.Add(new(s.HighScore - s.LowScore, 1, s));
            }

            var result = new KnapsackBranch<Student>(data, items.Count / 2).Solve();
            //return result.Sum(x => x.Value) + initialSum;
            return default;
        }

        public static TestResult SolveHalfBrute(List<Student> items) {
            Result result = HalfDistribution.Brute(items);
            //return result.Total();
            return default;
        }

        public static TestResult SolveHalfChoiceKnapsack(List<Student> items) {
            List<List<SackItem<string>>> data = new();

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                data.Add(new() {
                    new(s.LowScore, 0, ""),
                    new(s.MidScore, 1, $"{s.Name}-h"),
                    new(s.HighScore, 2, $"{s.Name}-F")
                });
            }

            var result = Knapsack.SolveChoice(data, items.Count, false);
            //return result.Sum(x => x.Value);
            return default;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class AlgorithmImpl {

        public static double SolveFullBrute(List<Student> items) {
            Result result = BasicDistribution.Brute(items);
            return result.Total();
        }

        public static double SolveFullSort(List<Student> items) {
            Result result = BasicDistribution.Sorted(items);
            return result.Total();
        }

        public static double SolveFullMedian(List<Student> items) {
            Result result = BasicDistribution.Median(items);
            return result.Total();
        }

        public static double SolveFullKnapsack(List<Student> items) {
            List<SackItem<Student>> data = new();
            double initialSum = 0;

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                initialSum += s.LowScore;
                data.Add(new(s.HighScore - s.LowScore, 1, s));
            }

            var result = new KnapsackNormal<Student>(data, items.Count / 2).Solve();
            return result.Sum(x => x.Value) + initialSum;
        }

        public static double SolveFullChoiceKnapsack(List<Student> items) {
            List<List<SackItem<Student>>> data = new();

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                List<SackItem<Student>> temp = new();
                temp.Add(new(s.LowScore, 1, s));
                temp.Add(new(s.HighScore, 1, s));
                data.Add(temp);
            }

            var result = new KnapsackChoice<Student>(data, items.Count / 2).Solve();
            return result.Sum(x => x.Value);
        }

        public static double SolveFullBranchKnapsack(List<Student> items) {
            List<SackItem<Student>> data = new();
            double initialSum = 0;

            for (int i = 0; i < items.Count; i++) {
                var s = items[i];
                initialSum += s.LowScore;
                data.Add(new(s.HighScore - s.LowScore, 1, s));
            }

            var result = new KnapsackBranch<Student>(data, items.Count / 2).Solve();
            return result.Sum(x => x.Value) + initialSum;
        }

        public static double SolveHalfBrute(List<Student> items) {
            Result result = HalfDistribution.Brute(items);
            return result.Total();
        }

        public static double SolveHalfChoiceKnapsack(List<Student> items) {
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
            return result.Sum(x => x.Value);
        }

        public static double SolveStudentKnapsack2D(List<Student> items) {
            return 0;
        }

        public static double SolveStudentBranchKnapsack2D(List<Student> items) {
            return 0;
        }

        public static double SolveWaiverKnapsack2D(List<Student> items) {
            return 0;
        }

        public static double SolveWaiverBranchKnapsack2D(List<Student> items) {
            return 0;
        }
    }
}

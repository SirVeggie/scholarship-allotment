using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class BasicDistribution {

        public static Result Median(List<Student> list) {
            Student median = Quickselect.Run(list, (int)Math.Ceiling(list.Count / 2.0), x => x.Diff);

            var full = list.Where(x => x.Diff > median.Diff).ToList();
            var none = list.Where(x => x.Diff < median.Diff).ToList();

            full.AddRange(list.Where(x => x.Diff == median.Diff).Take(list.Count / 2 - full.Count));
            none.AddRange(list.Where(x => x.Diff == median.Diff).Skip(list.Count / 2 - full.Count));

            return new(full, new(), none);
        }

        public static Result Sorted(List<Student> list) {
            List<Student> students = new(list);
            var sorted = students.OrderByDescending(x => x.Diff);

            var full = sorted.Take(list.Count / 2).ToList();
            var none = sorted.Skip(list.Count / 2).ToList();

            return new(full, new(), none);
        }

        public static Result Sorted(List<Student> list, int desiredAmount) {
            List<Student> students = new(list);
            bool odd = desiredAmount % 2 == 1;

            var lowSort = students.OrderByDescending(x => x.LowScore);
            var temp = lowSort.Take(desiredAmount);
            var tempSort = temp.OrderByDescending(x => x.Diff);
            var full = tempSort.Take(desiredAmount / 2).ToList();
            var none = tempSort.Skip(desiredAmount / 2).ToList();
            //var sorted = students.OrderByDescending(x => x.Diff);
            //var full = sorted.Take(desiredAmount / 2).ToList();
            //var remaining = students.Where(x => !full.Contains(x)).ToList();
            //var bottomSorted = remaining.OrderByDescending(x => x.LowScore);
            //var none = bottomSorted.Take(desiredAmount / 2 + (odd ? 1 : 0)).ToList();

            return new(full, new(), none);
        }

        public static Result Brute(List<Student> list) {
            double best = 0;
            int[] res = new int[list.Count / 2];

            foreach (var perm in Tools.Combinations5(list.Count, new int[0])) {
                double total = perm.Aggregate(0.0, (total, x) => total + list[x].Diff);
                //total += list.Where((x, i) => !perm.Contains(i)).Aggregate(0.0, (total, x) => total + x.LowScore);
                if (total > best) {
                    best = total;
                    res = perm;
                }
            }

            var full = list.Where((x, i) => res.Contains(i)).ToList();
            var none = list.Where((x, i) => !res.Contains(i)).ToList();

            return new(full, new(), none);
        }

        public static Result Brute(List<Student> list, int desiredAmount) {
            double best = 0;
            BruteContainer cont = new();

            foreach (var outerPerm in Tools.Combinations5(list.Count, new int[0])) {
                foreach (var innerPerm in Tools.Combinations5(list.Count, outerPerm)) {
                    double full = outerPerm.Aggregate(0.0, (total, x) => total + list[x].Score * list[x].PHigh);
                    double none = innerPerm.Aggregate(0.0, (total, x) => total + list[x].Score * list[x].PLow);
                    double total = full + none;
                    if (total > best) {
                        //Console.WriteLine($"total: {total}");
                        best = total;
                        cont.full = outerPerm;
                        cont.none = innerPerm;
                    }
                }
            }

            var fullStudents = list.Where((x, i) => cont.full.Contains(i)).ToList();
            var noneStudents = list.Where((x, i) => cont.none.Contains(i)).ToList();

            return new(fullStudents, new(), noneStudents);
        }

        private struct BruteContainer {
            public int[] full = new int[0];
            public int[] none = new int[0];

            public BruteContainer(int[] full, int[] none) {
                this.full = full;
                this.none = none;
            }
        }
    }
}

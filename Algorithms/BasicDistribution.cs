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

        public static Result BruteOld(List<Student> list) {
            double best = 0;
            bool[] res = new bool[list.Count];
            var indexed = list.Select((x, i) => new { i, item = x });
            var perms = Tools.BoolPermutations(list.Count / 2, list.Count);
            //Console.WriteLine("perm done");

            foreach (var mask in perms) {
                double total = indexed.Aggregate(0.0, (total, x) => total + x.item.Score * (mask[x.i] ? x.item.PHigh : x.item.PLow));
                if (total > best) {
                    best = total;
                    res = (bool[])mask.Clone();
                }
            }

            var full = list.Where((x, i) => res[i]).ToList();
            var none = list.Where((x, i) => !res[i]).ToList();

            return new(full, new(), none);
        }

        public static Result Brute(List<Student> list) {
            double best = 0;
            int[] res = new int[list.Count / 2];

            foreach (var perm in Tools.Combinations(list.Count, list.Count / 2, new int[0])) {
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

            int fAmount = desiredAmount / 2;

            foreach (var outerPerm in Tools.Combinations(list.Count, fAmount, new int[0])) {
                foreach (var innerPerm in Tools.Combinations(list.Count, desiredAmount - fAmount, outerPerm)) {
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

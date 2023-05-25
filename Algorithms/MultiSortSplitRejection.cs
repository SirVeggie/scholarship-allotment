using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class MultiSortSplitRejection {

        public static Result Solve(List<Student> students, int desiredAmount) {
            double best = 0;
            Result bestRes = null;

            for (int i = 0; i <= desiredAmount / 2; i++) {
                Result res;
                if (i == 0) {
                    res = MultiSort.Solve(students, desiredAmount);
                } else {
                    res = Solve(students, desiredAmount, i);
                }

                if (res.Total() > best) {
                    best = res.Total();
                    bestRes = res;
                }
            }

            return bestRes;
        }

        public static Result Solve(List<Student> students, int desiredAmount, int asHalf) {
            if (desiredAmount <= 0 || desiredAmount > students.Count)
                throw new ArgumentException($"invalid {nameof(desiredAmount)}");
            if (asHalf < 0 || asHalf > desiredAmount / 2)
                throw new ArgumentException($"invalid {nameof(asHalf)}");

            int fcount = desiredAmount / 2 - asHalf;
            int hcount = asHalf * 2 + (desiredAmount % 2 == 1 ? 1 : 0);
            int ncount = desiredAmount - (fcount + hcount);

            var full = students.Take(fcount).ToList();
            var half = students.Skip(fcount).Take(hcount).ToList();
            var none = students.Skip(fcount + hcount).Take(ncount).ToList();
            var rejected = students.Skip(desiredAmount).ToList();
            double best = Calc(full, half, none);

            while (true) {
                bool changed = false;

                // sort full and half
                var sorted = Sort(full, half, x => x.DiffTop);
                double proposition = Calc(sorted.Item1, sorted.Item2, none);
                if (proposition > best) {
                    best = proposition;
                    full = sorted.Item1;
                    half = sorted.Item2;
                    changed = true;
                }

                // sort full and none
                sorted = Sort(full, none, x => x.Diff);
                proposition = Calc(sorted.Item1, half, sorted.Item2);
                if (proposition > best) {
                    best = proposition;
                    full = sorted.Item1;
                    none = sorted.Item2;
                    changed = true;
                }

                // sort half and none
                sorted = Sort(half, none, x => x.DiffMid);
                proposition = Calc(full, sorted.Item1, sorted.Item2);
                if (proposition > best) {
                    best = proposition;
                    half = sorted.Item1;
                    none = sorted.Item2;
                    changed = true;
                }

                // sort none and rejected
                sorted = Sort(none, rejected, x => x.LowScore);
                proposition = Calc(full, half, sorted.Item1);
                if (proposition > best) {
                    best = proposition;
                    none = sorted.Item1;
                    rejected = sorted.Item2;
                    changed = true;
                }

                // sort half and rejected
                sorted = Sort(half, rejected, x => x.MidScore);
                proposition = Calc(full, sorted.Item1, none);
                if (proposition > best) {
                    best = proposition;
                    half = sorted.Item1;
                    rejected = sorted.Item2;
                    changed = true;
                }

                // sort full and rejected
                sorted = Sort(full, rejected, x => x.HighScore);
                proposition = Calc(sorted.Item1, half, none);
                if (proposition > best) {
                    best = proposition;
                    full = sorted.Item1;
                    rejected = sorted.Item2;
                    changed = true;
                }

                if (!changed)
                    break;
            }

            return new(full, half, none);
        }

        private static (List<Student>, List<Student>) Sort<K>(List<Student> a, List<Student> b, Func<Student, K> func) {
            int amount = a.Count;
            var temp = a.Concat(b).OrderByDescending(func);
            return (temp.Take(amount).ToList(), temp.Skip(amount).ToList());
        }

        private static double Calc(List<Student> full, List<Student> half, List<Student> none) {
            return Math.Round(full.Sum(x => x.HighScore) + half.Sum(x => x.MidScore) + none.Sum(x => x.LowScore), 5);
        }
    }
}

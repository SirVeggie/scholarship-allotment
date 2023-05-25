using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class MultiSortSplit {

        public static Result Solve(List<Student> students) {
            double best = 0;
            Result bestRes = null;

            for (int i = 0; i <= students.Count / 2; i++) {
                Result res;
                if (i == 0) {
                    res = BasicDistribution.Median(students);
                } else if (i == students.Count / 2) {
                    res = new(new(), students, new());
                } else {
                    res = Solve(students, i);
                }

                if (res.Total() > best) {
                    best = res.Total();
                    bestRes = res;
                }
            }

            return bestRes;
        }
        public static Result Solve(List<Student> students, int asHalf) {
            if (asHalf < 0 || asHalf > students.Count / 2)
                throw new ArgumentException($"invalid argument {nameof(asHalf)}");

            int fcount = students.Count / 2 - asHalf;
            int hcount = asHalf * 2 + (students.Count % 2 == 1 ? 1 : 0);
            int ncount = students.Count - fcount - hcount;

            var full = students.Take(fcount).ToList();
            var half = students.Skip(fcount).Take(hcount).ToList();
            var none = students.Skip(fcount + hcount).ToList();

            if (full.Count + half.Count + none.Count != students.Count)
                throw new Exception("Internal error: Mismatched amounts");

            double prev = 0;
            while (DoSort()) { }
            while (DoCheck()) { }
            while (DoCheckReverse()) { }
            return new(full, half, none);



            bool DoSort() {
                // sort full and none
                var sorted = Sort(full, half, fcount, x => x.DiffTop);
                full = sorted.Item1;
                half = sorted.Item2;

                // sort half and none
                sorted = Sort(half, none, hcount, x => x.DiffMid);
                half = sorted.Item1;
                none = sorted.Item2;

                // sort full and none
                sorted = Sort(full, none, fcount, x => x.Diff);
                full = sorted.Item1;
                none = sorted.Item2;

                // check for changes
                double value = Calc(full, half, none);
                bool changed = prev != value;
                prev = value;
                return changed;
            }

            bool DoCheck() {
                var lowestFull = full.OrderBy(x => x.DiffTop).First();
                var lowestHalf = half.OrderBy(x => x.DiffMid).First();
                var found = none.Where(x => x.Diff > lowestHalf.DiffMid + lowestFull.DiffTop).ToList();
                if (found.Count > 0) {
                    var bestNone = found.OrderByDescending(x => x.Diff).First();
                    half.Remove(lowestHalf);
                    full.Remove(lowestFull);
                    none.Remove(bestNone);
                    full.Add(bestNone);
                    half.Add(lowestFull);
                    none.Add(lowestHalf);
                    return true;
                } else {
                    return false;
                }
            }

            bool DoCheckReverse() {
                var lowestFull = full.OrderBy(x => x.Diff).First();
                var bestHalf = half.OrderByDescending(x => x.DiffTop).First();
                var found = none.Where(x => x.DiffMid > lowestFull.Diff - bestHalf.DiffTop).ToList();
                if (found.Count > 0) {
                    var bestNone = found.OrderByDescending(x => x.DiffMid).First();
                    half.Remove(bestHalf);
                    full.Remove(lowestFull);
                    none.Remove(bestNone);
                    full.Add(bestHalf);
                    half.Add(bestNone);
                    none.Add(lowestFull);
                    return true;
                } else {
                    return false;
                }
            }
        }

        private static double Calc(List<Student> full, List<Student> half, List<Student> none) {
            return Math.Round(full.Sum(x => x.HighScore) + half.Sum(x => x.MidScore) + none.Sum(x => x.LowScore), 5);
        }

        private static (List<Student>, List<Student>) Sort<K>(List<Student> a, List<Student> b, int aCount, Func<Student, K> func) {
            int amount = aCount;
            var temp = a.Concat(b).OrderByDescending(func);
            return (temp.Take(amount).ToList(), temp.Skip(amount).ToList());
        }
    }
}

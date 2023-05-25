using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class MultiSort {

        //public static Result Solve(List<Student> students, int desiredAmount) {
        //    int fcount = desiredAmount / 2;
        //    int ncount = desiredAmount - desiredAmount / 2;

        //    var full = students.Take(fcount).ToList();
        //    var none = students.Skip(fcount).Take(ncount).ToList();
        //    var rejected = students.Skip(desiredAmount).ToList();
        //    double best = Calc(full, none);

        //    int round = 0;
        //    while (true) {
        //        Console.WriteLine($"Round {++round} | {best}");
        //        bool changed = false;

        //        // sort full and none
        //        var sorted = Sort(full, none, x => x.Diff);
        //        double proposition = Calc(sorted.Item1, sorted.Item2);
        //        if (proposition > best) {
        //            best = proposition;
        //            full = sorted.Item1;
        //            none = sorted.Item2;
        //            changed = true;
        //        }

        //        // sort none and rejected
        //        sorted = Sort(none, rejected, x => x.LowScore);
        //        proposition = Calc(full, sorted.Item1);
        //        if (proposition > best) {
        //            best = proposition;
        //            none = sorted.Item1;
        //            rejected = sorted.Item2;
        //            changed = true;
        //        }

        //        // sort full and rejected
        //        sorted = Sort(full, rejected, x => x.HighScore);
        //        proposition = Calc(sorted.Item1, none);
        //        if (proposition > best) {
        //            best = proposition;
        //            full = sorted.Item1;
        //            rejected = sorted.Item2;
        //            changed = true;
        //        }

        //        if (!changed)
        //            break;
        //    }

        //    return new(full, new(), none);
        //}

        //private static (List<Student>, List<Student>) Sort<K>(List<Student> a, List<Student> b, Func<Student, K> func) {
        //    int amount = a.Count;
        //    var temp = a.Concat(b).OrderByDescending(func);
        //    return (temp.Take(amount).ToList(), temp.Skip(amount).ToList());
        //}

        public static Result Solve(List<Student> students, int desiredAmount) {
            int fcount = desiredAmount / 2;
            int ncount = desiredAmount - desiredAmount / 2;

            var full = students.Take(fcount).ToList();
            var none = students.Skip(fcount).Take(ncount).ToList();
            var rejected = students.Skip(desiredAmount).ToList();
            double prev = 0;

            //int round = 0;
            while (DoSort()) { }
            while (DoCheck()) { }
            while (DoCheckReverse()) { }
            return new(full, new(), none);

            bool DoSort() {
                //Console.WriteLine($"Round {++round} | {prev}");

                // sort full and none
                var sorted = Sort(full, none, fcount, x => x.Diff);
                full = sorted.Item1;
                none = sorted.Item2;

                // sort none and rejected
                sorted = Sort(none, rejected, ncount, x => x.LowScore);
                none = sorted.Item1;
                rejected = sorted.Item2;

                // sort full and rejected
                sorted = Sort(full, rejected, fcount, x => x.HighScore);
                full = sorted.Item1;
                rejected = sorted.Item2;

                // check for changes
                double value = Calc(full, none);
                bool changed = prev != value;
                prev = value;
                return changed;
            }

            bool DoCheck() {
                var lowestNone = none.OrderBy(x => x.LowScore).First();
                var lowestFull = full.OrderBy(x => x.Diff).First();
                var found = rejected.Where(x => x.HighScore > lowestNone.LowScore + lowestFull.Diff).ToList();
                if (found.Count > 0) {
                    //Console.WriteLine($"Found {found.Count} knots");
                    var bestRej = found.OrderByDescending(x => x.HighScore).First();
                    none.Remove(lowestNone);
                    full.Remove(lowestFull);
                    rejected.Remove(bestRej);
                    full.Add(bestRej);
                    none.Add(lowestFull);
                    rejected.Add(lowestNone);
                    return true;
                } else {
                    //Console.WriteLine("Didn't find anything");
                    return false;
                }
            }

            bool DoCheckReverse() {
                var lowestFull = full.OrderBy(x => x.HighScore).First();
                var bestNone = none.OrderByDescending(x => x.Diff).First();
                var found = rejected.Where(x => x.LowScore > lowestFull.HighScore - bestNone.Diff).ToList();
                if (found.Count > 0) {
                    //Console.WriteLine($"Found {found.Count} knots");
                    var bestRej = found.OrderByDescending(x => x.LowScore).First();
                    none.Remove(bestNone);
                    full.Remove(lowestFull);
                    rejected.Remove(bestRej);
                    full.Add(bestNone);
                    none.Add(bestRej);
                    rejected.Add(lowestFull);
                    return true;
                } else {
                    //Console.WriteLine("Didn't find anything");
                    return false;
                }
            }
        }

        private static double Calc(List<Student> full, List<Student> none) {
            return Math.Round(full.Sum(x => x.HighScore) + none.Sum(x => x.LowScore), 5);
        }

        private static (List<Student>, List<Student>) Sort<K>(List<Student> a, List<Student> b, int aCount, Func<Student, K> func) {
            int amount = aCount;
            var temp = a.Concat(b).OrderByDescending(func);
            return (temp.Take(amount).ToList(), temp.Skip(amount).ToList());
        }
    }
}

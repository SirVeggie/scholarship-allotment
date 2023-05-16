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

        public static Result Brute(List<Student> list) {
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
    }
}

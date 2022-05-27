using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution.Algorithms {
    public static class HalfDistribution {

        public static Result Brute(List<Student> list) {
            double best = 0;
            double[] res = new double[list.Count];

            foreach (var perm in Tools.TrioPermutations(list.Count)) {
                double total = 0;
                for (int i = 0; i < list.Count; i++) {
                    var x = list[i];
                    total += x.Score * (perm[i] == 1 ? x.PHigh : perm[i] == 0 ? x.PLow : x.PMid);
                }

                if (total > best) {
                    best = total;
                    res = perm;
                }
            }

            var full = list.Where((x, i) => res[i] == 1).ToList();
            var half = list.Where((x, i) => res[i] == 0.5).ToList();
            var none = list.Where((x, i) => res[i] == 0).ToList();

            return new(full, half, none);
        }

        public static Result Smart(List<Student> list) {
            double balance = list.Count / 2.0;
            var sorted = list.OrderByDescending(x => x.DiffMax).ToList();
            List<Student> full = new();
            List<Student> half = new();

            foreach (Student s in sorted) {
                if (balance == 0.5)
                    Console.WriteLine("half remainder");
                if (balance == 0) {
                    break;
                } else if (s.DiffMax > s.Diff) {
                    half.Add(s);
                    balance -= 0.5;
                } else if (balance >= 1) {
                    full.Add(s);
                    balance -= 1;
                } else if (balance == 0.5) {
                    half.Add(sorted.Where(x => !full.Contains(x) && !half.Contains(x)).OrderByDescending(x => x.Score * x.PMid).First());
                    break;
                }
            }

            List<Student> none = sorted.Where(x => !full.Contains(x) && !half.Contains(x)).OrderByDescending(x => x.Score * x.PLow).ToList();

            return new(full, half, none);
        }

        public static Result Smart2(List<Student> list) {
            throw new NotImplementedException();

            double balance = list.Count / 2;
            var sorted = list.OrderByDescending(x => x.DiffMax).ToList();
            List<Student> full = new();
            List<Student> half = new();

            while (balance > 0) {
                Student s = sorted.First();

                if (s.DiffMax <= s.Diff && balance >= 1) {
                    full.Add(s);
                    balance -= 1;
                    sorted.RemoveAt(0);
                } else if (s.DiffMax <= s.Diff) {
                    var item = sorted.OrderByDescending(x => s.Score * x.PMid).First();
                    half.Add(item);
                    sorted.Remove(item);
                    break;
                } else {

                }
            }
        }
    }
}

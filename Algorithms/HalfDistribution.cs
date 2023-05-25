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
    }
}

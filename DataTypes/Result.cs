using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public class Result {
        public List<Student> Full { get; private set; }
        public List<Student> Half { get; private set; }
        public List<Student> None { get; private set; }

        public string FullNames => $"{string.Join(", ", Full.Select(x => x.Name))}";
        public string HalfNames => $"{string.Join(", ", Half.Select(x => x.Name))}";
        public string NoneNames => $"{string.Join(", ", None.Select(x => x.Name))}";
        public string AllNames => $"Full: {FullNames}\nHalf: {HalfNames}\nNone: {NoneNames}";

        public Result(List<Student> full, List<Student> half, List<Student> none) {
            Full = full ?? new();
            Half = half ?? new();
            None = none ?? new();
            Sort();
        }

        public Result Sort() {
            Full = Full.OrderBy(x => x.Name).ToList();
            Half = Half.OrderBy(x => x.Name).ToList();
            None = None.OrderBy(x => x.Name).ToList();
            return this;
        }

        public double Total() {
            double total = 0;
            total += Full.Aggregate(0.0, (t, x) => t + x.Score * x.PHigh);
            total += Half.Aggregate(0.0, (t, x) => t + x.Score * x.PMid);
            total += None.Aggregate(0.0, (t, x) => t + x.Score * x.PLow);
            return total;
        }
    }
}

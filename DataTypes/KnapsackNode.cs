using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public struct KnapsackNode {
        public double value;
        public bool include;

        public KnapsackNode(double value, bool include) {
            this.value = value;
            this.include = include;
        }
    }
}

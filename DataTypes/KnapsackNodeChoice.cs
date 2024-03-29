﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public struct KnapsackNodeChoice {
        public double value;
        public bool include;
        public int item;

        public KnapsackNodeChoice(double value) {
            this.value = value;
            this.include = false;
            this.item = 0;
        }

        public KnapsackNodeChoice(double value, int item) {
            this.value = value;
            this.include = true;
            this.item = item;
        }
    }
}

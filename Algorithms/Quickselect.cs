using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.Algorithms {
    public static class Quickselect {

        public static T Run<T>(List<T> list, int k, Func<T, double> vFunc) {
            int low = 0;
            int high = list.Count;

            while (true) {
                int p = Partition(list, low, high, vFunc);

                if (p == k - 1) {
                    return list[p];
                } else if (p < k - 1) {
                    low = p + 1;
                } else {
                    high = p;
                }
            }
        }

        private static int Partition<T>(List<T> list, int low, int high, Func<T, double> vFunc) {
            double p = vFunc(list[high - 1]);
            int pLoc = low;

            for (int i = low; i < high; i++) {
                if (vFunc(list[i]) < p) {
                    Tools.Swap(list, i, pLoc);
                    pLoc++;
                }
            }

            Tools.Swap(list, high - 1, pLoc);
            return pLoc;
        }
    }
}

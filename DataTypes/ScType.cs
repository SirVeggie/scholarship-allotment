using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public enum ScType {
        Rejected,
        None,
        Half,
        Full,
    }

    public static class ScTypeExtensions {
        public static bool IsFull(this ScType scType) => scType == ScType.Full;
        public static bool IsHalf(this ScType scType) => scType == ScType.Half;
        public static bool IsNone(this ScType scType) => scType == ScType.None;
        public static bool IsRejected(this ScType scType) => scType == ScType.Rejected;
    }
}

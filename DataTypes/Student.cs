using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public readonly struct Student {
        public string Name { get; init; }
        public double Score { get; init; }
        public double PHigh { get; init; }
        public double PMid { get; init; }
        public double PLow { get; init; }

        public double LowScore => Score * PLow;
        public double MidScore => Score * PMid;
        public double HighScore => Score * PHigh;

        public double Diff => Score * PHigh - Score * PLow;
        public double DiffMid => Score * PMid - Score * PLow;
        public double DiffTop => Score * PHigh - Score * PMid;
        public double DiffMax => Math.Max(Diff, DiffMid * 2);

        public override string ToString() => $"{{{Name} | {Score} | {Math.Round(PLow, 2)}-{Math.Round(PMid, 2)}-{Math.Round(PHigh, 2)}}}";
        public static bool operator ==(Student a, Student b) => a.Name == b.Name && a.Score == b.Score && a.PLow == b.PLow && a.PMid == b.PMid && a.PHigh == b.PHigh;
        public static bool operator !=(Student a, Student b) => !(a == b);
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Student s && s == this;
        public override int GetHashCode() => HashCode.Combine(Name, Score);
    }
}

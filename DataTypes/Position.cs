using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionWaiverDistribution.DataTypes {
    public struct Position {
        public int x;
        public int y;

        public Position(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x}, {y})";
        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is not null && obj is Position p && x == p.x && y == p.y;
        }
    }

    public struct Position3D {
        public int x;
        public int y;
        public int z;

        public Position3D(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override int GetHashCode() => HashCode.Combine(x, y, z);
        public override string ToString() => $"({x}, {y}, {z})";
        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is not null && obj is Position3D p && x == p.x && y == p.y && z == p.z;
        }
    }
}

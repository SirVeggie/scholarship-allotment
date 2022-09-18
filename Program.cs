using System.Diagnostics;
using TuitionWaiverDistribution;
using TuitionWaiverDistribution.Algorithms;
using TuitionWaiverDistribution.DataTypes;

List<Student> students = GenerateData(10);
Console.WriteLine("Solver launched");

//Console.WriteLine(string.Join(", ", students));
//Console.WriteLine();

//CompareBasic(students);
CompareKnapsack(students);
CompareKnapsackChoice(students);
//CompareHalf(students);
//CompareHalfBrute(students);
//CompareKnapsackChoiceHalf(students);
//CompareKnapsackBranch2DX(students);
//CompareKnapsackAdvanced(students);
//CompareKnapsackBranch(students);
//CompareKnapsackBranch2D(students);
//CompareKnapsack2D(students);
//TestKnapsack();


static void TestKnapsack() {
    List<SackItem<string>> items = new() { new(10, 5, "one"), new(40, 4, "two"), new(30, 6, "three"), new(5, 0, "zero"), new(50, 7, "four") };

    var result = Knapsack.Solve(items, 10);
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsack(List<Student> students) {
    List<SackItem<string>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new(s.LowScore, 0, $""));
        items.Add(new(s.HighScore - s.LowScore, 1, $"{s.Name}"));
    }

    var result = Knapsack.Solve(items, students.Count / 2);
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackChoiceBasic(List<Student> students) {
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() { new(s.LowScore, 0, "") });
        items.Add(new() { new(s.HighScore - s.LowScore, 1, $"{s.Name}") });
    }

    var result = Knapsack.Solve(items, students.Count / 2);
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackChoice(List<Student> students) {
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, ""),
            new(s.HighScore, 1, $"{s.Name}")
        });
    }

    var result = Knapsack.Solve(items, students.Count / 2);
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackChoiceHalf(List<Student> students) {
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, ""),
            new(s.MidScore, 1, $"{s.Name}-h"),
            new(s.HighScore, 2, $"{s.Name}-F")
        });
    }

    var result = Knapsack.Solve(items, 20, false);
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsack2D(List<Student> students) {
    List<List<SackItem2D<(int, Student)>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, 1000, (0, s))
            //new(s.MidScore, 1, 1, (1, s)),
            //new(s.HighScore, 2, 1, (2, s))
        });
        items.Add(new() {
            //new(s.LowScore, 0, 1, (0, s)),
            //new(s.MidScore, 1, 1, (1, s)),
            new(s.HighScore, 2, 1000, (2, s))
        });
    }

    int waivers = 50;
    int acceptedStudents = 50000;
    var result = Knapsack.Solve(items, waivers, acceptedStudents, false);

    double totalPercentage = 0;
    foreach (var item in result) {
        if (item.Relation.Item1 == 0) {
            totalPercentage += item.Relation.Item2.PLow;
        } else if (item.Relation.Item1 == 1) {
            totalPercentage += item.Relation.Item2.PMid;
        } else {
            totalPercentage += item.Relation.Item2.PHigh;
        }
    }

    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"Expected number of students: {totalPercentage}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    //CompareKnapsackChoiceHalf(students.Where(x => result.Any(y => y.Relation.Item2.Name == x.Name)).ToList());
    Console.WriteLine();
}

static void CompareKnapsackAdvanced(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<List<SackItem2D<(int, Student)>>> items = new();

    int accuracy = 10;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, (int) Math.Round(s.PLow * accuracy), (int) Math.Round(s.PLow * 0), (0, s)),
            new(s.MidScore, (int) Math.Round(s.PMid * accuracy), (int) Math.Round(s.PMid * accuracy), (1, s)),
            new(s.HighScore, (int) Math.Round(s.PHigh * accuracy), (int) Math.Round(s.PHigh * accuracy * 2), (2, s))
        });
    }

    int studentTarget = 40;
    int waiverTarget = 40;
    var result = Knapsack.Solve(items, studentTarget * accuracy, waiverTarget * accuracy, false);
    watch.Stop();

    double totalPercentage = 0;
    foreach (var item in result) {
        if (item.Relation.Item1 == 0) {
            totalPercentage += item.Relation.Item2.PLow;
        } else if (item.Relation.Item1 == 1) {
            totalPercentage += item.Relation.Item2.PMid;
        } else if (item.Relation.Item1 == 2) {
            totalPercentage += item.Relation.Item2.PHigh;
        }
    }

    double totalWaiver = 0;
    foreach (var item in result) {
        if (item.Relation.Item1 == 1) {
            totalWaiver += item.Relation.Item2.PMid;
        } else if (item.Relation.Item1 == 2) {
            totalWaiver += item.Relation.Item2.PHigh * 2;
        }
    }

    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"Expected number of students: {totalPercentage}");
    Console.WriteLine($"Expected number of waivers: {totalWaiver}");
    Console.WriteLine($"Elapsed time: {watch.Elapsed}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => (x.Relation.Item1, x.Relation.Item2.Name)))}");
    //CompareKnapsackChoiceHalf(students.Where(x => result.Any(y => y.Relation.Item2.Name == x.Name)).ToList());
    Console.WriteLine();
}

static void CompareKnapsackBranch2DX(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<List<SackItem2D<(int, Student)>>> items = new();

    //int accuracy = 10;

    //for (int i = 0; i < students.Count; i++) {
    //    var s = students[i];
    //    items.Add(new() {
    //        new(s.LowScore, (int) Math.Round(s.PLow * accuracy), (int) Math.Round(s.PLow * 0), (0, s)),
    //        new(s.MidScore, (int) Math.Round(s.PMid * accuracy), (int) Math.Round(s.PMid * accuracy), (1, s)),
    //        new(s.HighScore, (int) Math.Round(s.PHigh * accuracy), (int) Math.Round(s.PHigh * accuracy * 2), (2, s))
    //    });
    //}

    //int studentTarget = 40;
    //int waiverTarget = 40;
    //var result = Knapsack.SolveBranch(items, studentTarget * accuracy, waiverTarget * accuracy, false);
    int accuracy = 100;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, (int) Math.Round(s.PLow * 0), (0, s)),
            new(s.MidScore, 1, (int) Math.Round(s.PMid * accuracy), (1, s)),
            new(s.HighScore, 2, (int) Math.Round(s.PHigh * accuracy * 2), (2, s))
        });
    }

    int studentTarget = 50;
    int waiverTarget = 40;
    var result = Knapsack.SolveBranch(items, studentTarget, waiverTarget * accuracy, false);
    watch.Stop();

    double totalPercentage = 0;
    foreach (var item in result) {
        if (item.Relation.Item1 == 0) {
            totalPercentage += item.Relation.Item2.PLow;
        } else if (item.Relation.Item1 == 1) {
            totalPercentage += item.Relation.Item2.PMid;
        } else if (item.Relation.Item1 == 2) {
            totalPercentage += item.Relation.Item2.PHigh;
        }
    }

    double totalWaiver = 0;
    foreach (var item in result) {
        if (item.Relation.Item1 == 1) {
            totalWaiver += item.Relation.Item2.PMid;
        } else if (item.Relation.Item1 == 2) {
            totalWaiver += item.Relation.Item2.PHigh * 2;
        }
    }

    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"Expected number of students: {totalPercentage}");
    Console.WriteLine($"Expected number of waivers: {totalWaiver}");
    Console.WriteLine($"Elapsed time: {watch.Elapsed}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => (x.Relation.Item1, x.Relation.Item2.Name)))}");
    //CompareKnapsackChoiceHalf(students.Where(x => result.Any(y => y.Relation.Item2.Name == x.Name)).ToList());
    Console.WriteLine();
}

static void CompareKnapsackBranch(List<Student> students) {
    List<SackItem<string>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new(s.LowScore, 0, ""));
        items.Add(new(s.HighScore - s.LowScore, 1, $"{s.Name}"));
    }

    var result = Knapsack.SolveBranch(items, students.Count / 2, true);
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackBranch2D(List<Student> students) {
    List<SackItem2D<(int, Student)>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new(s.LowScore, 0, 1000, (0, s)));
        //items.Add(new(s.MidScore, 1, 1, (1, s)));
        items.Add(new(s.HighScore, 2, 1000, (2, s)));
    }

    int waivers = 50;
    int acceptedStudents = 50000;
    var result = Knapsack.SolveBranch(items, waivers, acceptedStudents, false);

    double totalPercentage = 0;
    foreach (var item in result) {
        if (item.Relation.Item1 == 0) {
            totalPercentage += item.Relation.Item2.PLow;
        } else if (item.Relation.Item1 == 1) {
            totalPercentage += item.Relation.Item2.PMid;
        } else {
            totalPercentage += item.Relation.Item2.PHigh;
        }
    }

    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"Expected number of students: {totalPercentage}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    //CompareKnapsackChoiceHalf(students.Where(x => result.Any(y => y.Relation.Item2.Name == x.Name)).ToList());
    Console.WriteLine();
}

static void CompareBasic(List<Student> students) {
    var median = BasicDistribution.Median(students);
    var sorted = BasicDistribution.Sorted(students);
    var brute = BasicDistribution.Brute(students);

    Console.WriteLine($"Median: {median.Total()}");
    Console.WriteLine($"Sorted: {sorted.Total()}");
    Console.WriteLine($"Brute:  {brute.Total()}");
    Console.WriteLine();
}

static void CompareHalf(List<Student> students) {
    var median = BasicDistribution.Median(students);
    var smart = HalfDistribution.Smart(students);

    Console.WriteLine("Median: " + median.Total());
    Console.WriteLine(median.AllNames);
    Console.WriteLine();

    Console.WriteLine("Smart:  " + smart.Total());
    Console.WriteLine(smart.AllNames);
    Console.WriteLine();
}

static void CompareHalfBrute(List<Student> students) {
    var smart = HalfDistribution.Smart(students);
    var brute = HalfDistribution.Brute(students);
    var median = BasicDistribution.Median(students);

    Console.WriteLine("Smart:  " + smart.Total());
    Console.WriteLine(smart.AllNames);
    Console.WriteLine();
    Console.WriteLine("Brute:  " + brute.Total());
    Console.WriteLine(brute.AllNames);
    Console.WriteLine();
    Console.WriteLine("Median: " + median.Total());
    Console.WriteLine(median.AllNames);
    Console.WriteLine();
}


static List<Student> GenerateData(int amount) {
    Random random = new Random();
    List<Student> data = new();

    for (int i = 1; i <= amount; i++) {
        double low = 1, mid = 0, high = 0;

        while (low > mid || mid > high) {
            high = random.NextDouble();
            mid = random.NextDouble();
            low = random.NextDouble();
        }

        data.Add(new() {
            Name = $"s{i}",
            Score = random.Next(1, 101),
            PHigh = high,
            PMid = mid,
            PLow = low,
        });
    }

    return data;
}
﻿using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using TuitionWaiverDistribution;
using TuitionWaiverDistribution.Algorithms;
using TuitionWaiverDistribution.DataTypes;

string[] arguments = args ?? new string[0];

if (arguments.Length == 1 && arguments[0].ToLower() == "test") {
    await Testing.RunTestSuite();
    return;
}

if (arguments.Length == 3) {
    Variation variation = Enum.Parse<Variation>(arguments[0]);
    Algo algo = Enum.Parse<Algo>(arguments[1]);
    int students = int.Parse(arguments[2]);

    Stopwatch watch = Stopwatch.StartNew();
    double result = Testing.ExecuteTest(variation, algo, students);
    watch.Stop();

    //string output = JsonConvert.SerializeObject(new TestResult {
        //success = result != 0,
        //value = result,
        //time = watch.ElapsedMilliseconds
    //});

    Console.WriteLine($" -> {result} | {watch.ElapsedMilliseconds} ms");

    return;
}

List<Student> students1 = Testing.GenerateData(10);
List<Student> students2 = Testing.GenerateData(100);
List<Student> students3 = Testing.GenerateData(1000);
List<Student> students4 = Testing.GenerateData(10000);

//Console.WriteLine(string.Join(", ", students));
//Console.WriteLine();

//CompareBasic(students1);
CompareKnapsack(students1);
CompareKnapsack(students3);
CompareKnapsack(students3);
CompareKnapsack(students1);
CompareKnapsack(students4);
CompareKnapsackStatic(students4, 50);
//CompareKnapsackChoice(students4);
//CompareKnapsackBranch(students4);
//CompareHalf(students2);
//CompareHalfBrute(students1);
//CompareKnapsackChoiceHalf(students2);
//CompareKnapsackBranch2DX(students2);
//CompareKnapsackAdvanced(students1);
CompareKnapsackBranch2D(students1);
CompareKnapsackBranch2D(students2);
CompareKnapsackBranch2D(students3);
//CompareKnapsack2D(students1);
//TestKnapsack();


//static async long ExecuteTest(Action<List<Student>> f, List<Student> students, int timeout) {
//    TaskCompletionSource<long> task = new();
//    Thread t = new Thread(() => {
//        Stopwatch timer = Stopwatch.StartNew();
//        try {
//            f(students);
//            timer.Stop();
//            task.TrySetResult(timer.ElapsedMilliseconds);
//        } catch (ThreadAbortException) {
//            task.TrySetResult(-1);
//        }
//    });

//    Task res = await Task.WhenAny(task.Task, Task.Delay(timeout));
//    if (t.IsAlive) {
//        t.Abort();
//    }
//}


static void TestKnapsack() {
    List<SackItem<string>> items = new() { new(10, 5, "one"), new(40, 4, "two"), new(30, 6, "three"), new(5, 0, "zero"), new(50, 7, "four") };

    var result = Knapsack.Solve(items, 10);
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsack(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<SackItem<string>> items = new();
    double initialSum = 0;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        initialSum += s.LowScore;
        items.Add(new(s.HighScore - s.LowScore, 1, $"{s.Name}"));
    }

    var result = Knapsack.Solve(items, students.Count / 2);
    watch.Stop();

    Console.WriteLine("Basic knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value) + initialSum}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackStatic(List<Student> students, int count) {
    Stopwatch watch = Stopwatch.StartNew();
    List<SackItem<string>> items = new();
    double initialSum = 0;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        initialSum += s.LowScore;
        items.Add(new(s.HighScore - s.LowScore, 1, $"{s.Name}"));
    }

    var result = Knapsack.Solve(items, count);
    watch.Stop();

    Console.WriteLine("Basic knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value) + initialSum}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

//static void CompareKnapsackChoiceBasic(List<Student> students) {
//    Stopwatch watch = Stopwatch.StartNew();
//    List<List<SackItem<string>>> items = new();

//    for (int i = 0; i < students.Count; i++) {
//        var s = students[i];
//        items.Add(new() { new(s.LowScore, 0, "") });
//        items.Add(new() { new(s.HighScore - s.LowScore, 1, $"{s.Name}") });
//    }

//    var result = Knapsack.Solve(items, students.Count / 2);
//    watch.Stop();

//    Console.WriteLine("Choice knapsack");
//    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
//    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
//    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
//    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
//    Console.WriteLine();
//}

static void CompareKnapsackChoice(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, ""),
            new(s.HighScore, 1, $"{s.Name}")
        });
    }

    var result = Knapsack.SolveChoice(items, students.Count / 2);
    watch.Stop();

    Console.WriteLine("Choice knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackChoiceHalf(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, ""),
            new(s.MidScore, 1, $"{s.Name}-h"),
            new(s.HighScore, 2, $"{s.Name}-F")
        });
    }

    var result = Knapsack.SolveChoice(items, students.Count, false);
    watch.Stop();

    Console.WriteLine("Half choice knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsack2D(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
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
    watch.Stop();

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

    Console.WriteLine("2D knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
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

    Console.WriteLine("Advanced knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"Expected number of students: {totalPercentage}");
    Console.WriteLine($"Expected number of waivers: {totalWaiver}");
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

    Console.WriteLine("2DX knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"Expected number of students: {totalPercentage}");
    Console.WriteLine($"Expected number of waivers: {totalWaiver}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => (x.Relation.Item1, x.Relation.Item2.Name)))}");
    //CompareKnapsackChoiceHalf(students.Where(x => result.Any(y => y.Relation.Item2.Name == x.Name)).ToList());
    Console.WriteLine();
}

static void CompareKnapsackBranch(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<SackItem<string>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new(s.LowScore, 0, ""));
        items.Add(new(s.HighScore - s.LowScore, 1, $"{s.Name}"));
    }

    var result = Knapsack.SolveBranch(items, students.Count / 2, true);
    watch.Stop();

    Console.WriteLine("Branch knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    Console.WriteLine();
}

static void CompareKnapsackBranch2D(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<SackItem2D<(int, Student)>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new(s.LowScore, 0, 1000, (0, s)));
        items.Add(new(s.MidScore, 1, 1, (1, s)));
        items.Add(new(s.HighScore, 2, 1000, (2, s)));
    }

    int waivers = 50;
    int acceptedStudents = 50000;
    var result = Knapsack.SolveBranch(items, waivers, acceptedStudents, false);
    watch.Stop();

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

    Console.WriteLine("Branch 2D knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
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
    Stopwatch watch = Stopwatch.StartNew();
    var smart = HalfDistribution.Smart(students);
    watch.Stop();

    Console.WriteLine("Median: " + median.Total());
    Console.WriteLine(median.AllNames);
    Console.WriteLine();

    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine("Smart:  " + smart.Total());
    Console.WriteLine(smart.AllNames);
    Console.WriteLine();
}

static void CompareHalfBrute(List<Student> students) {
    var smart = HalfDistribution.Smart(students);
    Stopwatch watch = Stopwatch.StartNew();
    var brute = HalfDistribution.Brute(students);
    watch.Stop();

    var median = BasicDistribution.Median(students);

    Console.WriteLine("Smart:  " + smart.Total());
    Console.WriteLine(smart.AllNames);
    Console.WriteLine();
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
    Console.WriteLine("Brute:  " + brute.Total());
    Console.WriteLine(brute.AllNames);
    Console.WriteLine();
    Console.WriteLine("Median: " + median.Total());
    Console.WriteLine(median.AllNames);
    Console.WriteLine();
}
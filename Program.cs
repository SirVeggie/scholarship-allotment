using Newtonsoft.Json;
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

if (arguments.Length == 1 && arguments[0].ToLower() == "test_branch") {
    await Testing.RunTestSuiteBranching();
    return;
}

if (arguments.Length == 1 && arguments[0].ToLower() == "test_accuracy") {
    await Testing.RunTestSuiteAccuracy();
    return;
}

if (arguments.Length == 1 && arguments[0].ToLower() == "test_fixed") {
    await Testing.RunTestSuiteFixedRejection();
    return;
}

if (arguments.Length == 1 && arguments[0].ToLower() == "test_fixed_half") {
    await Testing.RunTestSuiteFixedRejectionHalf();
    return;
}

if (arguments.Length == 1 && arguments[0].ToLower() == "test_optimality") {
    await Testing.RunTestSuiteOptimality();
    return;
}

if (arguments.Length == 1 && arguments[0].ToLower() == "test_2023") {
    await Testing.RunTestSuite2023();
    return;
}

if (arguments.Length == 1 && arguments[0].ToLower() == "test1") {
    await Testing.RunSingle();
    return;
}

if (arguments.Length >= 3) {
    Variation variation = Enum.Parse<Variation>(arguments[0]);
    Algo algo = Enum.Parse<Algo>(arguments[1]);
    string studentFile = arguments[2];

    int amount = 0;
    int accuracy = 0;
    bool repeats = true;
    if (arguments.Length > 3)
        amount = int.Parse(arguments[3]);
    if (arguments.Length > 4)
        accuracy = int.Parse(arguments[4]);
    if (arguments.Length > 5)
        repeats = bool.Parse(arguments[5]);

    await Testing.ExecuteTest(variation, algo, studentFile, amount, accuracy, repeats);

    return;
}

List<Student> students1 = Testing.GenerateData(10);
List<Student> students1x = Testing.GenerateData(20);
List<Student> students2 = Testing.GenerateData(100);
List<Student> students2x = Testing.GenerateData(200);
List<Student> students3 = Testing.GenerateData(1000);
List<Student> edge1 = Testing.EdgeCase1(100);
//List<Student> students3x = Testing.GenerateData(2000);
//List<Student> students3xx = Testing.GenerateData(4000);
List<Student> students4 = Testing.GenerateData(10000);
List<Student> approx1 = Testing.GenerateDataApprox(10, true);
//List<Student> students5 = Testing.GenerateData(100000);
//List<Student> students6 = Testing.GenerateData(1000000);

//List<Student> students1x = new();
//int count = 0;
//while (true) {
//    students1x = Testing.GenerateData(50);
//    double a = CompareKnapsack2DXRejection(students1x, 10);
//    double b = CompareSortedRejection(students1x, 10);
//    if (a != b)
//        break;
//    count++;
//    Console.WriteLine($"Attempt {count}");
//}

//var asd = string.Join("\n", approx1.Select(x => x.ToString()));
//Console.WriteLine(asd);
//var asd2 = string.Join("\n", students1.Select(x => x.ToString()));
//Console.WriteLine(asd2);

CompareFastKnapsackAdjustFullHalfNoBranch(Testing.GenerateDataApprox(400, true), 45, 10);
//CompareFastKnapsackAdjustFullHalfNoBranch(Testing.GenerateData(100), 40, 10);

//CompareBruteHalf(Testing.GenerateData(18));

//CompareMultiSortedRejection(students3, 80);
//CompareMultiSortedRejection2(students3, 80);
//CompareFastKnapsackRejection(students3, 80);
//CompareMultiSortedSplit(students3);
//TestKnapsackSimple(students4);
//CompareKnapsack(students4);
//CompareFastKnapsackChoiceHalf(students4);
//CompareKnapsackChoiceHalf(students4);

//CompareFastKnapsackRejectionHalf(students1, 4);
//CompareFastKnapsackRejectionHalf(students2, 40);
//CompareMultiSortedSplit(students4);

//CompareFastKnapsackRejectionHalf(students2, 40);
//CompareFastKnapsackRejectionHalfNoBranch(students2, 40);

//CompareKnapsack2DXSplitRejection(students3, 200);
//CompareKnapsack2DXSplitRejectionNoBranch(students3, 200);

//CompareMultiSortedSplitRejection(students3, 50);
//CompareMultiSortedSplitRejectionBetter(students3, 50);
//CompareKnapsackChoiceHalf(students2);
//CompareFastKnapsackRejectionHalf(students3, 50);
//CompareFastKnapsackAdjust(students2x, 40, 10);
//CompareFastKnapsackAdjustNoBranch(students2x, 40, 10);
//CompareFastKnapsackAdjustBig(students2x, 40, 10);
//CompareFastKnapsackAdjustSpecial(students2x, 40, 10);
//CompareFastKnapsackAdjust(edge1, 10, 100);
//CompareFastKnapsackAdjustHalf(students2x, 100, 10);
//CompareFastKnapsackAdjustHalfNoBranch(students2x, 100, 10);
//CompareFastKnapsackAdjustFull(students2, 40, 10);
//CompareFastKnapsackAdjustFullNoBranch(students2, 40, 10);
//CompareFastKnapsackAdjustFullHalf(students2, 40, 10);
//CompareKnapsackAdvanced(students2);
//CompareKnapsackBranch2DX(students2, 40, 10);
//CompareKnapsackAdjust(students2, 40);
//CompareKnapsackChoice(students2);
//CompareFastKnapsack2DBasic(students2, 100);
//CompareKnapsack2DXBasic(students2, 100);
//CompareMultiSortedRejection(students3, 100);
//CompareFastKnapsackRejection(students3, 100);
//CompareKnapsack2DXRejection(students3, 100);
//CompareMultiSortedRejection(Testing.GenerateData(100000), 10000);
//CompareBruteRejection(students1x, 10);
//CompareBrute(students1);
//CompareBruteOld(students1);
//return;

//Console.WriteLine(string.Join(", ", students));
//Console.WriteLine();

//CompareSorted(students3);
//CompareMedian(students3);
//CompareSorted(students3x);
//CompareMedian(students3x);
//CompareKnapsack2DXBinary(students1);
//return;
//TestKnapsackSimple(students3);
//TestKnapsackSimpleLarge(students3, 100);
//return;
//CompareKnapsack(students2);
//CompareKnapsack(Testing.GenerateData(100 * 2));
//CompareKnapsack(Testing.GenerateData(100 * 2 * 2));
//CompareKnapsack(Testing.GenerateData(100 * 2 * 2 * 2));
//CompareKnapsack(Testing.GenerateData(100 * 2 * 2 * 2 * 2));
//CompareKnapsack(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2));
//CompareKnapsack(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2));
//Console.WriteLine("------------- Fast -------------");
//TestKnapsackSimple(students3);
//TestKnapsackSimple(Testing.GenerateData(100 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2));
//TestKnapsackSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2));
//Console.WriteLine("------------- Fast Large -------------");
//TestKnapsackSimpleLarge(students3, 100);
//Console.WriteLine("------------- Branch -------------");
//TestKnapsackBranchSimple(students3);
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2));
//Console.WriteLine("------------- Branch Large -------------");
//TestKnapsackBranchSimpleLarge(students3, 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2), 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2), 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2), 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2), 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2), 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2), 100);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2), 100);
//Console.WriteLine("------------- Branch Super Large -------------");
//TestKnapsackBranchSimpleLarge(students3, 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2), 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2), 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2), 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2), 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2), 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2), 10000);
//TestKnapsackBranchSimpleLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2), 10000);
//Console.WriteLine("------------- Old Branch -------------");
//CompareKnapsackBranch(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2));
//CompareKnapsackBranchLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2));
//CompareKnapsackBranchLarge(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2));
//TestKnapsackBranchSimple(Testing.GenerateData(100 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2));
//CompareKnapsack(students1);
//CompareKnapsack(students4);
//CompareKnapsackStatic(students4, 50);
//CompareKnapsackChoice(students3);
//CompareKnapsackChoice(students4);
//CompareKnapsack(students3);
//CompareKnapsack(students3x);
//CompareKnapsack(students3xx);
//CompareKnapsackLarge(students3);
//CompareKnapsackLarge(students3);
//CompareKnapsackBranch(students3);
//CompareKnapsackBranch(students3x);
//CompareKnapsackBranch(students3xx);
//CompareKnapsackBranchLarge(students3x);
//CompareKnapsackBranchLarge(students3);
//CompareHalf(students2);
//CompareHalfBrute(students1);
//CompareKnapsackChoiceHalf(students2);
//CompareKnapsackBranch2DX(students2);
//CompareKnapsackAdvanced(students1);
//CompareMultiSortedRejection(students2, 40);
//CompareFastKnapsackRejection(students1, 4);
//CompareFastKnapsackRejection(students2, 40);
//CompareKnapsackBranch2D(students3);
//CompareKnapsack2D(students1);
//TestKnapsack();





static void TestKnapsackSimple(List<Student> students) {
    KnapsackItem<string>[] items = new KnapsackItem<string>[students.Count];
    double initialSum = 0;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        initialSum += s.LowScore;
        items[i] = new(s.Name, s.Diff, 1);
    }

    Stopwatch watch = Stopwatch.StartNew();
    KnapsackResult<string> result = Knapsack<string>.Solve(items, items.Length / 2);
    watch.Stop();

    Console.WriteLine("Simple knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Value: {result.Value + initialSum}");
    Console.WriteLine($"Weight: {result.Weight} / {items.Length / 2}");
    Console.WriteLine();
}

static void TestKnapsackSimpleLarge(List<Student> students, int multiplier) {
    KnapsackItem<string>[] items = new KnapsackItem<string>[students.Count];
    double initialSum = 0;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        initialSum += s.LowScore;
        items[i] = new(s.Name, s.HighScore - s.LowScore, 1 * multiplier);
    }

    Stopwatch watch = Stopwatch.StartNew();
    KnapsackResult<string> result = Knapsack<string>.Solve(items, items.Length * multiplier / 2);
    watch.Stop();

    Console.WriteLine("Simple knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Value: {result.Value + initialSum}");
    Console.WriteLine($"Weight: {result.Weight} / {items.Length * multiplier / 2}");
    Console.WriteLine();
}

static void TestKnapsackBranchSimple(List<Student> students) {
    KnapsackItem<string>[] items = new KnapsackItem<string>[students.Count];
    double initialSum = 0;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        initialSum += s.LowScore;
        items[i] = new(s.Name, s.HighScore - s.LowScore, 1);
    }

    Stopwatch watch = Stopwatch.StartNew();
    KnapsackResult<string> result = KnapsackBranch<string>.Solve(items, items.Length / 2);
    watch.Stop();

    Console.WriteLine("Simple knapsack branch");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Value: {result.Value + initialSum}");
    Console.WriteLine($"Weight: {result.Weight} / {items.Length / 2}");
    Console.WriteLine();
}

static void TestKnapsackBranchSimpleLarge(List<Student> students, int multiplier) {
    KnapsackItem<string>[] items = new KnapsackItem<string>[students.Count];
    double initialSum = 0;

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        initialSum += s.LowScore;
        items[i] = new(s.Name, s.HighScore - s.LowScore, multiplier);
    }

    Stopwatch watch = Stopwatch.StartNew();
    KnapsackResult<string> result = KnapsackBranch<string>.Solve(items, items.Length * multiplier / 2);
    watch.Stop();

    Console.WriteLine($"Simple knapsack branch (large: {multiplier})");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Value: {result.Value + initialSum}");
    Console.WriteLine($"Weight: {result.Weight} / {items.Length * multiplier / 2}");
    Console.WriteLine();
}

static void CompareFastKnapsackChoiceHalf(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    List<List<KnapsackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new($"(0, {s.Name})", s.LowScore, 0),
            new($"(1, {s.Name})", s.MidScore, 1),
            new($"(2, {s.Name})", s.HighScore, 2)
        });
    }

    var result = KnapsackChoice<string>.Solve(items, students.Count);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(2, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Half choice knapsack");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {r.Total()}");
    Console.WriteLine($"Waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}/{students.Count / 2}");
    Console.WriteLine($"Student slots: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}/{students.Count}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareFastKnapsackAdjust(List<Student> students, int desiredAmount, int accuracy) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.HighScore, 1, (int)Math.Round(s.PHigh * accuracy))
        };
    }

    int waivers = desiredAmount / 2;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = KnapsackBranch2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(), new(), students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast branch 2D knapsack (binary + rejection + student)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {r.Total()}");
    Console.WriteLine($"Average score: {r.AverageScore()}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.None.Sum(x => x.PLow)}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareFastKnapsackAdjustNoBranch(List<Student> students, int desiredAmount, int accuracy) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.HighScore, 1, (int)Math.Round(s.PHigh * accuracy))
        };
    }

    int waivers = desiredAmount / 2;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = Knapsack2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(), new(), students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast 2D knapsack (binary + rejection + student)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {r.Total()}");
    Console.WriteLine($"Average score: {r.AverageScore()}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.None.Sum(x => x.PLow)}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareFastKnapsackAdjustHalf(List<Student> students, int desiredAmount, int accuracy) {
    //List<List<SackItem2D<string>>> items = new();
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.MidScore, 1, (int)Math.Round(s.PMid * accuracy)),
            new($"(2, {s.Name})", s.HighScore, 2, (int)Math.Round(s.PHigh * accuracy))
        };
    }

    int waivers = desiredAmount;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = KnapsackBranch2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(2, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast branch 2D knapsack (split + rejection + student)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    //Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Value: {result.Value}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareFastKnapsackAdjustHalfNoBranch(List<Student> students, int desiredAmount, int accuracy) {
    List<List<KnapsackItem2D<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items.Add(new() {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.MidScore, 1, (int)Math.Round(s.PMid * accuracy)),
            new($"(2, {s.Name})", s.HighScore, 2, (int)Math.Round(s.PHigh * accuracy))
        });
    }

    int waivers = desiredAmount;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = Knapsack2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(2, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast 2D knapsack (split + rejection + student)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    //Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Value: {result.Value}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareFastKnapsackAdjustFull(List<Student> students, int desiredAmount, int accuracy) {
    //List<List<SackItem2D<string>>> items = new();
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.HighScore, (int)Math.Round(s.PHigh * accuracy), (int)Math.Round(s.PHigh * accuracy))
        };
    }

    int waivers = desiredAmount * accuracy / 2;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = KnapsackBranch2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        new(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast branch 2D knapsack (binary + rejection + waiver)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Value}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}/{desiredAmount / 2}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}/{desiredAmount}");
    Console.WriteLine();
}

static void CompareFastKnapsackAdjustFullNoBranch(List<Student> students, int desiredAmount, int accuracy) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.HighScore, (int)Math.Round(s.PHigh * accuracy), (int)Math.Round(s.PHigh * accuracy))
        };
    }

    int waivers = desiredAmount * accuracy / 2;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = Knapsack2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        new(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast 2D knapsack (binary + rejection + waiver)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Value}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}/{desiredAmount / 2}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}/{desiredAmount}");
    Console.WriteLine();
}

static void CompareFastKnapsackAdjustFullHalfNoBranch(List<Student> students, int desiredAmount, int accuracy) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, (int)Math.Round(s.PLow * accuracy)),
            new($"(1, {s.Name})", s.MidScore, (int)Math.Round(s.PMid * accuracy), (int)Math.Round(s.PMid * accuracy)),
            new($"(2, {s.Name})", s.HighScore, (int)Math.Round(s.PHigh * accuracy * 2), (int)Math.Round(s.PHigh * accuracy))
        };
    }

    int waivers = desiredAmount * accuracy;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = Knapsack2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(2, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast branch 2D knapsack (split + rejection + waiver)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Value}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}");
    Console.WriteLine();
}

static void CompareFastKnapsack2DBasic(List<Student> students, int desiredAmount) {
    //List<List<SackItem2D<string>>> items = new();
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    int accuracy = 100;

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, 0),
            new($"(1, {s.Name})", s.HighScore, 1, 0)
        };
    }

    int waivers = desiredAmount / 2;
    int acceptedStudents = desiredAmount * accuracy;

    Stopwatch watch = Stopwatch.StartNew();
    var result = KnapsackBranch2D<string>.Solve(items, waivers, acceptedStudents);
    watch.Stop();

    //Result r = new(students.Where(x => result.Any(s => s.Relation == $"(1, {x.Name})")).ToList(), new(), students.Where(x => result.Any(s => s.Relation == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Branch 2D knapsack (test)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    //Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Value: {result.Value}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
    //Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static double CompareMultiSortedRejection(List<Student> students, int desiredAmount) {
    Stopwatch watch = Stopwatch.StartNew();
    var result = MultiSort.Solve(students, desiredAmount);
    watch.Stop();

    Console.WriteLine("Sorted (binary + rejection)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Total()}");
    //Console.WriteLine($"{result.AllNames}");
    Console.WriteLine();
    return result.Total();
}

static double CompareMultiSortedSplit(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    var result = MultiSortSplit.Solve(students);
    watch.Stop();

    Console.WriteLine("Sorted (split)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Total()}");
    //Console.WriteLine($"{result.AllNames}");
    Console.WriteLine();
    return result.Total();
}

static double CompareMultiSortedSplitRejection(List<Student> students, int desiredAmount) {
    Stopwatch watch = Stopwatch.StartNew();
    var result = MultiSortSplitRejection.Solve(students, desiredAmount);
    watch.Stop();

    Console.WriteLine("Sorted better (split + rejection)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Total()}");
    Console.WriteLine($"{result.AllNames}");
    Console.WriteLine();
    return result.Total();
}

static void CompareBruteRejection(List<Student> students, int desiredAmount) {

    //int waivers = desiredAmount / 2;
    //int acceptedStudents = desiredAmount;

    Stopwatch watch = Stopwatch.StartNew();
    var result = BasicDistribution.Brute(students, desiredAmount);
    watch.Stop();

    Console.WriteLine("Brute (binary + rejection)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Total()}");
    //Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}/{waivers}");
    //Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}/{acceptedStudents}");
    Console.WriteLine($"{result.AllNames}");
    Console.WriteLine();
}

static void CompareFastKnapsackRejection(List<Student> students, int desiredAmount) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, 1),
            new($"(1, {s.Name})", s.HighScore, 1, 1)
        };
    }

    int waivers = desiredAmount / 2;

    Stopwatch watch = Stopwatch.StartNew();
    var result = KnapsackBranch2D<string>.Solve(items, waivers, desiredAmount);
    watch.Stop();

    Result r = new(students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(), new(), students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast branch 2D knapsack (binary + rejection)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Value} - {r.Total()}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{desiredAmount}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh)}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.None.Sum(x => x.PLow)}");
    Console.WriteLine();
}

static void CompareFastKnapsackRejectionHalf(List<Student> students, int desiredAmount) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, 1),
            new($"(1, {s.Name})", s.MidScore, 1, 1),
            new($"(2, {s.Name})", s.HighScore, 2, 1)
        };
    }

    int waivers = desiredAmount;

    Stopwatch watch = Stopwatch.StartNew();
    var result = KnapsackBranch2D<string>.Solve(items, waivers, desiredAmount);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(2, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast branch 2D knapsack (split + rejection)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"f-{r.Full.Count} h-{r.Half.Count} n-{r.None.Count}");
    Console.WriteLine($"Value: {result.Value} - {r.Total()}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{desiredAmount}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}/{desiredAmount / 2}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}/{desiredAmount}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareFastKnapsackRejectionHalfNoBranch(List<Student> students, int desiredAmount) {
    KnapsackItem2D<string>[][] items = new KnapsackItem2D<string>[students.Count][];

    for (int i = 0; i < students.Count; i++) {
        Student s = students[i];

        items[i] = new KnapsackItem2D<string>[] {
            new($"(0, {s.Name})", s.LowScore, 0, 1),
            new($"(1, {s.Name})", s.MidScore, 1, 1),
            new($"(2, {s.Name})", s.HighScore, 2, 1)
        };
    }

    int waivers = desiredAmount;

    Stopwatch watch = Stopwatch.StartNew();
    var result = Knapsack2D<string>.Solve(items, waivers, desiredAmount);
    watch.Stop();

    Result r = new(
        students.Where(x => result.Items.Any(s => s.item == $"(2, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(1, {x.Name})")).ToList(),
        students.Where(x => result.Items.Any(s => s.item == $"(0, {x.Name})")).ToList());

    Console.WriteLine("Fast 2D knapsack (split + rejection)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"f-{r.Full.Count} h-{r.Half.Count} n-{r.None.Count}");
    Console.WriteLine($"Value: {result.Value} - {r.Total()}");
    Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
    Console.WriteLine($"Student slots: {result.WeightY}/{desiredAmount}");
    Console.WriteLine($"Approximate waivers used: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid / 2)}/{desiredAmount / 2}");
    Console.WriteLine($"Approximate students joined: {r.Full.Sum(x => x.PHigh) + r.Half.Sum(x => x.PMid) + r.None.Sum(x => x.PLow)}/{desiredAmount}");
    //Console.WriteLine(r.AllNames);
    Console.WriteLine();
}

static void CompareBruteOld(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    var brute = BasicDistribution.BruteOld(students);
    watch.Stop();

    Console.WriteLine($"");
    Console.WriteLine($"Brute old: {brute.Total()} in {watch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}

static void CompareBrute(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    var brute = BasicDistribution.Brute(students);
    watch.Stop();

    Console.WriteLine($"");
    Console.WriteLine($"Brute: {brute.Total()} in {watch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}

static void CompareSorted(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    Result sorted = new(new(), new(), new());
    for (int i = 0; i < 100; i++) {
        sorted = BasicDistribution.Sorted(students);
    }
    watch.Stop();

    Console.WriteLine($"Sorted: {sorted.Total()} in {watch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}

static void CompareMedian(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    Result median = new(new(), new(), new());
    for (int i = 0; i < 100; i++) {
        median = BasicDistribution.Median(students);
    }
    watch.Stop();

    Console.WriteLine($"Median: {median.Total()} in {watch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}

static void CompareBruteHalfOld(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    var brute = HalfDistribution.BruteOld(students);
    watch.Stop();

    Console.WriteLine("Brute old (split)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine("Value:  " + brute.Total());
    Console.WriteLine(brute.AllNames);
    Console.WriteLine();
}

static void CompareBruteHalf(List<Student> students) {
    Stopwatch watch = Stopwatch.StartNew();
    var brute = HalfDistribution.Brute(students);
    watch.Stop();

    Console.WriteLine("Brute (split)");
    Console.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
    Console.WriteLine("Value:  " + brute.Total());
    Console.WriteLine(brute.AllNames);
    Console.WriteLine();
}
using TuitionWaiverDistribution;
using TuitionWaiverDistribution.Algorithms;
using TuitionWaiverDistribution.DataTypes;

List<Student> students = GenerateData(100);

//Console.WriteLine(string.Join(", ", students));
//Console.WriteLine();

//CompareBasic(students);
//CompareKnapsack(students);
//CompareKnapsackChoice(students);
//CompareHalf(students);
//CompareHalfBrute(students);
//CompareKnapsackHalf(students);
//CompareKnapsackHalfBetter(students);
CompareKnapsackChoiceHalf(students);
//TestKnapsack();
//TestKnapsackStack();


static void TestKnapsack() {
    List<SackItem<string>> items = new() { new(10, 5, "one"), new(40, 4, "two"), new(30, 6, "three"), new(5, 0, "zero"), new(50, 7, "four") };

    var result = Knapsack.Solve(items, 10);
    Console.WriteLine();
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
}

//static void TestKnapsackStack() {
//    List<List<SackItem<string>>> items = new() {
//        new() { new(45, 10, "1-1"), new(10, 1, "1-2") },
//        new() { new(10, 10, "2-1"), new(10, 1, "2-2") },
//        new() { new(35, 10, "3-1"), new(10, 1, "3-2") },
//        new() { new(10, 10, "4-1"), new(10, 1, "4-2") }
//    };

//    var result = Knapsack.Solve(items, 30);
//    var unwrap = result.SelectMany(x => x).ToList();
//    Console.WriteLine();
//    Console.WriteLine($"Value: {unwrap.Sum(x => x.Value)}");
//    Console.WriteLine($"Weight: {unwrap.Sum(x => x.Weight)}");
//    Console.WriteLine($"{string.Join(", ", unwrap.Select(x => x.Relation))}");
//}

static void CompareKnapsack(List<Student> students) {
    List<SackItem<string>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new(s.LowScore, 0, $""));
        items.Add(new(s.HighScore - s.LowScore, 1, $"{s.Name}-1"));
    }

    var result = Knapsack.Solve(items, students.Count / 2);
    Console.WriteLine();
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
}

static void CompareKnapsackChoiceBasic(List<Student> students) {
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() { new(s.LowScore, 0, "") });
        items.Add(new() { new(s.HighScore - s.LowScore, 1, $"{s.Name}") });
    }

    var result = Knapsack.Solve(items, students.Count / 2);
    Console.WriteLine();
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
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
    Console.WriteLine();
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
}

static void CompareKnapsackChoiceHalf(List<Student> students) {
    List<List<SackItem<string>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, ""),
            new(s.MidScore, 1, $"{s.Name}-half"),
            new(s.HighScore, 2, $"{s.Name}-full")
        });
    }

    var result = Knapsack.Solve(items, students.Count);
    Console.WriteLine();
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
}

//static void CompareKnapsackHalf(List<Student> students) {
//    List<List<SackItem<string>>> items = new();

//    for (int i = 0; i < students.Count; i++) {
//        //for (int i = 0; i < 1; i++) {
//        var s = students[i];
//        items.Add(new());
//        items[i].Add(new(s.LowScore, 0, $""));
//        items[i].Add(new(s.MidScore - s.LowScore, 1, $"{s.Name}-1"));
//        items[i].Add(new(s.HighScore - (s.MidScore - s.LowScore) - s.LowScore, 1, $"{s.Name}-2"));
//    }

//    var result = Knapsack.Solve(items, 10);
//    var unwrap = result.SelectMany(x => x).OrderBy(x => x.Relation).ToList();
//    Console.WriteLine();
//    Console.WriteLine($"Value: {unwrap.Sum(x => x.Value)}");
//    Console.WriteLine($"Weight: {unwrap.Sum(x => x.Weight)}");
//    Console.WriteLine($"{string.Join(", ", unwrap.Select(x => x.Relation))}");
//}

//static void CompareKnapsackHalfBetter(List<Student> students) {
//    List<List<SackItem<Student>>> items = new();

//    for (int i = 0; i < students.Count; i++) {
//        //for (int i = 0; i < 1; i++) {
//        var s = students[i];
//        items.Add(new());
//        items[i].Add(new(s.LowScore, 0, s));
//        items[i].Add(new(s.MidScore - s.LowScore, 1, s));
//        items[i].Add(new(s.HighScore - (s.MidScore - s.LowScore) - s.LowScore, 1, s));
//    }

//    var result = Knapsack.Solve(items, students.Count);
//    List<Student> full = new();
//    List<Student> half = new();
//    List<Student> none = new();

//    for (int i = 0; i < result.Count; i++) {
//        if (result[i].Count == 2)
//            half.Add(result[i][0].Relation);
//        else if (result[i].Count == 3)
//            full.Add(result[i][0].Relation);
//        else
//            none.Add(result[i][0].Relation);
//    }

//    Console.WriteLine();
//    Console.WriteLine($"Knapsack: {result.SelectMany(x => x).Sum(x => x.Value)}, Confirmation: {full.Sum(x => x.HighScore) + half.Sum(x => x.MidScore) + none.Sum(x => x.LowScore)}");
//    Console.WriteLine($"Full: {Print(full)}");
//    Console.WriteLine($"Half: {Print(half)}");
//    Console.WriteLine($"None: {Print(none)}");

//    string Print(List<Student> s) => string.Join(", ", s.Select(x => x.Name));
//}

static void CompareBasic(List<Student> students) {
    var median = BasicDistribution.Median(students);
    var sorted = BasicDistribution.Sorted(students);
    var brute = BasicDistribution.Brute(students);

    Console.WriteLine();
    Console.WriteLine($"Median: {median.Total()}");
    Console.WriteLine($"Sorted: {sorted.Total()}");
    Console.WriteLine($"Brute:  {brute.Total()}");
}

static void CompareHalf(List<Student> students) {
    var median = BasicDistribution.Median(students);
    var smart = HalfDistribution.Smart(students);

    Console.WriteLine();
    Console.WriteLine("Median: " + median.Total());
    Console.WriteLine(median.AllNames);
    Console.WriteLine();

    Console.WriteLine("Smart:  " + smart.Total());
    Console.WriteLine(smart.AllNames);
}

static void CompareHalfBrute(List<Student> students) {
    var smart = HalfDistribution.Smart(students);
    var brute = HalfDistribution.Brute(students);
    var median = BasicDistribution.Median(students);

    Console.WriteLine();
    Console.WriteLine("Smart:  " + smart.Total());
    Console.WriteLine(smart.AllNames);
    Console.WriteLine();
    Console.WriteLine("Brute:  " + brute.Total());
    Console.WriteLine(brute.AllNames);
    Console.WriteLine();
    Console.WriteLine("Median: " + median.Total());
    Console.WriteLine(median.AllNames);
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
using TuitionWaiverDistribution;
using TuitionWaiverDistribution.Algorithms;
using TuitionWaiverDistribution.DataTypes;

List<Student> students = GenerateData(50);

//Console.WriteLine(string.Join(", ", students));
//Console.WriteLine();

//CompareBasic(students);
//CompareKnapsack(students);
//CompareKnapsackChoice(students);
//CompareHalf(students);
//CompareHalfBrute(students);
//CompareKnapsackChoiceHalf(students);
CompareKnapsack2D(students);
//TestKnapsack();


static void TestKnapsack() {
    List<SackItem<string>> items = new() { new(10, 5, "one"), new(40, 4, "two"), new(30, 6, "three"), new(5, 0, "zero"), new(50, 7, "four") };

    var result = Knapsack.Solve(items, 10);
    Console.WriteLine();
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
}

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
            new(s.MidScore, 1, $"{s.Name}-h"),
            new(s.HighScore, 2, $"{s.Name}-F")
        });
    }

    var result = Knapsack.Solve(items, students.Count, false);
    Console.WriteLine();
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight: {result.Sum(x => x.Weight)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");
}

static void CompareKnapsack2D(List<Student> students) {
    List<List<SackItem2D<Student>>> items = new();

    for (int i = 0; i < students.Count; i++) {
        var s = students[i];
        items.Add(new() {
            new(s.LowScore, 0, 1, s),
            new(s.MidScore, 1, 1, s),
            new(s.HighScore, 2, 1, s)
        });
    }

    int acceptedStudents = 10;
    var result = Knapsack.Solve(items, acceptedStudents, acceptedStudents, false);
    Console.WriteLine();
    Console.WriteLine($"Students: {students.Count}");
    Console.WriteLine($"Value: {result.Sum(x => x.Value)}");
    Console.WriteLine($"Weight X: {result.Sum(x => x.WeightX)}");
    Console.WriteLine($"Weight Y: {result.Sum(x => x.WeightY)}");
    Console.WriteLine($"{string.Join(", ", result.Select(x => x.Relation))}");

    CompareKnapsackChoiceHalf(students.Where(x => result.Any(y => y.Relation.Name == x.Name)).ToList());
}

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
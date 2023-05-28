using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.Algorithms;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution {
    public static class Testing {

        public static Dictionary<Variation, List<Algo>> Valids { get; set; } = new() {
            { Variation.Binary, new() { Algo.Brute, Algo.Sort, Algo.Median, Algo.Knapsack, Algo.BranchKnapsack, Algo.ChoiceKnapsack, Algo.Knapsack2D } },
            { Variation.Rejection, new() { Algo.Brute, Algo.Sort, Algo.Knapsack2D, Algo.BranchKnapsack2D } },
            { Variation.Split, new() { Algo.Brute, Algo.Sort, Algo.ChoiceKnapsack, Algo.Knapsack2D, Algo.BranchKnapsack2D } },
            { Variation.SplitRejection, new() { Algo.Sort, Algo.Knapsack2D } },
            { Variation.Approximate, new() { Algo.Knapsack2D, Algo.BranchKnapsack2D } },
            { Variation.DoubleApproximate, new() { Algo.Knapsack2D, Algo.BranchKnapsack2D } }
        };

        public static async Task RunTestSuite() {
            Dictionary<int, string> data = new() {
                { 10, "s10.json" },
                { 100, "s100.json" },
                { 1000, "s1000.json" },
                { 10000, "s10000.json" },
                { 100000, "s100000.json" },
            };

            SaveData(data[10], GenerateData(10));
            SaveData(data[100], GenerateData(100));
            SaveData(data[1000], GenerateData(1000));
            SaveData(data[10000], GenerateData(10000));
            SaveData(data[100000], GenerateData(100000));

            foreach (Variation variation in Enum.GetValues(typeof(Variation))) {
                Console.WriteLine(variation.ToString());
                int timeout = 60000;
                int accuracy = 10;

                foreach (Algo algo in Enum.GetValues(typeof(Algo))) {
                    if (!Valids[variation].Contains(algo)) continue;
                    bool rejected = variation == Variation.Rejection || variation == Variation.SplitRejection || variation == Variation.Approximate || variation == Variation.DoubleApproximate;
                    Console.WriteLine($"    {algo}");

                    foreach (int dataSize in new int[] { 10, 100, 1000, 10000, 100000 }) {
                        int desired = dataSize / 10 * 4;
                        Console.Write($"        dataset {(rejected ? $"{desired}/" : "")}{dataSize}");
                        Console.Write(" -> ");
                        bool result = await CallTest(variation, algo, data[dataSize], desired, accuracy, timeout);
                        if (!result) {
                            Console.WriteLine("timed out");
                            break;
                        }

                        TestResult test = await LoadResults();
                        if (!test.success)
                            break;
                        Console.WriteLine($"value {test.value} in {test.time} ms | "
                            + $"sch:{test.expectedScholarships}/{(rejected ? desired / 2 : dataSize / 2)}"
                            + $" | std:{test.expectedStudents}/{(rejected ? desired : dataSize)}");
                        SaveResults(default);
                    }
                }

                Console.WriteLine();
            }
        }

        public static async Task RunTestSuiteBranching() {
            string file = "branch.json";
            SaveData(file, GenerateData(1000));

            Console.WriteLine("Branch best case");
            int timeout = 60000;
            int accuracy = 10;

            foreach (Algo algo in new Algo[] { Algo.Knapsack, Algo.BranchKnapsack }) {
                Console.WriteLine($"    {algo}");

                foreach (int size in new int[] { 1, 10, 100, 1000 }) {
                    Console.Write($"        multiplier {size}");
                    Console.Write(" -> ");
                    bool result = await CallTest(Variation.BranchTest, algo, file, size, accuracy, timeout);
                    if (!result) {
                        Console.WriteLine("timed out");
                        break;
                    }

                    TestResult test = await LoadResults();
                    if (!test.success)
                        break;
                    Console.WriteLine($"value {test.value} in {test.time} ms");
                    SaveResults(default);
                }
            }
        }

        public static async Task RunTestSuiteAccuracy() {
            int a1 = 10;
            int a2 = 50;
            int a3 = 100;
            int a4 = 250;

            //Dictionary<int, string> data = new() {
            //    { a1, $"s{a1}.json" },
            //    { a2, $"s{a2}.json" },
            //    { a3, $"s{a3}.json" },
            //    { a4, $"s{a4}.json" },
            //};

            //SaveData(data[a1], GenerateData(a1));
            //SaveData(data[a2], GenerateData(a2));
            //SaveData(data[a3], GenerateData(a3));
            //SaveData(data[a4], GenerateData(a4));

            Dictionary<int, string> data10 = new() {
                { a1, $"s{a1}x.json" },
                { a2, $"s{a2}x.json" },
                { a3, $"s{a3}x.json" },
                { a4, $"s{a4}x.json" },
            };

            SaveData(data10[a1], GenerateDataApprox(a1, true));
            SaveData(data10[a2], GenerateDataApprox(a2, true));
            SaveData(data10[a3], GenerateDataApprox(a3, true));
            SaveData(data10[a4], GenerateDataApprox(a4, true));

            Dictionary<int, string> data = new() {
                { a1, $"s{a1}.json" },
                { a2, $"s{a2}.json" },
                { a3, $"s{a3}.json" },
                { a4, $"s{a4}.json" },
            };

            SaveData(data[a1], GenerateDataApprox(a1, false));
            SaveData(data[a2], GenerateDataApprox(a2, false));
            SaveData(data[a3], GenerateDataApprox(a3, false));
            SaveData(data[a4], GenerateDataApprox(a4, false));


            Console.WriteLine("Accuracy testing");
            foreach (Variation variation in new Variation[] { Variation.Approximate, Variation.DoubleApproximate }) {
                Console.WriteLine(variation);
                int timeout = 60000;

                foreach (int accuracy in new int[] { 10, 100 }) {
                    Console.WriteLine($"    accuracy - {accuracy}");
                    foreach (int dataSize in new int[] { a1, a2, a3, a4 }) {
                        int desired = (int)(dataSize / 10.0 * 4);
                        Console.Write($"        dataset {desired}/{dataSize}");
                        Console.Write(" -> ");
                        bool result = await CallTest(variation, Algo.Knapsack2D, (accuracy == 10 ? data10 : data)[dataSize], desired, accuracy, timeout);
                        if (!result) {
                            Console.WriteLine("timed out");
                            break;
                        }

                        TestResult test = await LoadResults();
                        if (!test.success)
                            break;
                        Console.WriteLine($"value {test.value} in {test.time} ms | "
                            + $"sch:{test.expectedScholarships}/{desired / 2}"
                            + $" | std:{test.expectedStudents}/{desired}");
                        SaveResults(default);
                    }
                }
            }
        }

        public static async Task RunTestSuiteFixedRejection() {
            Dictionary<int, string> data = new() {
                { 10, "s10.json" },
                { 100, "s100.json" },
                { 1000, "s1000.json" },
                { 10000, "s10000.json" },
                { 100000, "s100000.json" },
            };

            //SaveData(data[10], GenerateData(10));
            SaveData(data[10], GenerateDataApprox(10, true));
            //SaveData(data[100], GenerateData(100));
            SaveData(data[100], GenerateDataApprox(100, true));
            //SaveData(data[1000], GenerateData(1000));
            SaveData(data[1000], GenerateDataApprox(1000, true));
            //SaveData(data[10000], GenerateData(10000));
            SaveData(data[10000], GenerateDataApprox(10000, true));
            //SaveData(data[100000], GenerateData(100000));
            SaveData(data[100000], GenerateDataApprox(100000, true));

            foreach (Variation variation in new Variation[] { Variation.Rejection, Variation.SplitRejection, Variation.Approximate, Variation.DoubleApproximate }) {
                Console.WriteLine(variation.ToString());
                int timeout = 60000;
                int accuracy = 10;

                //foreach (Algo algo in Enum.GetValues(typeof(Algo))) {
                //if (!Valids[variation].Contains(algo)) continue;
                //bool rejected = variation == Variation.Rejection || variation == Variation.SplitRejection || variation == Variation.Approximate || variation == Variation.DoubleApproximate;
                bool rejected = true;
                //Console.WriteLine($"    {algo}");

                foreach (int dataSize in new int[] { 10, 100, 1000, 10000, 100000 }) {
                    int desired = 10;
                    Console.Write($"        dataset {(rejected ? $"{desired}/" : "")}{dataSize}");
                    Console.Write(" -> ");
                    bool result = await CallTest(variation, Algo.Knapsack2D, data[dataSize], desired, accuracy, timeout);
                    if (!result) {
                        Console.WriteLine("timed out");
                        break;
                    }

                    TestResult test = await LoadResults();
                    if (!test.success)
                        break;
                    Console.WriteLine($"value {test.value} in {test.time} ms | "
                        + $"sch:{test.expectedScholarships}/{(rejected ? desired / 2 : dataSize / 2)}"
                        + $" | std:{test.expectedStudents}/{(rejected ? desired : dataSize)}");
                    SaveResults(default);
                }
                //}

                Console.WriteLine();
            }
        }

        public static async Task RunTestSuiteFixedRejectionHalf() {
            Dictionary<int, string> data = new() {
                { 10, "s10.json" },
                { 100, "s100.json" },
                { 1000, "s1000.json" },
                { 10000, "s10000.json" },
                { 100000, "s100000.json" },
            };

            //SaveData(data[10], GenerateData(10));
            SaveData(data[10], GenerateDataApproxHalf(10, true));
            //SaveData(data[100], GenerateData(100));
            SaveData(data[100], GenerateDataApproxHalf(100, true));
            //SaveData(data[1000], GenerateData(1000));
            SaveData(data[1000], GenerateDataApproxHalf(1000, true));
            //SaveData(data[10000], GenerateData(10000));
            SaveData(data[10000], GenerateDataApproxHalf(10000, true));
            //SaveData(data[100000], GenerateData(100000));
            SaveData(data[100000], GenerateDataApproxHalf(100000, true));

            foreach (Variation variation in new Variation[] { Variation.Rejection, Variation.SplitRejection, Variation.Approximate, Variation.DoubleApproximate }) {
                Console.WriteLine(variation.ToString());
                int timeout = 60000;
                int accuracy = 10;

                //foreach (Algo algo in Enum.GetValues(typeof(Algo))) {
                //if (!Valids[variation].Contains(algo)) continue;
                //bool rejected = variation == Variation.Rejection || variation == Variation.SplitRejection || variation == Variation.Approximate || variation == Variation.DoubleApproximate;
                bool rejected = true;
                //Console.WriteLine($"    {algo}");

                foreach (int dataSize in new int[] { 10, 100, 1000, 10000, 100000 }) {
                    int desired = 10;
                    Console.Write($"        dataset {(rejected ? $"{desired}/" : "")}{dataSize}");
                    Console.Write(" -> ");
                    bool result = await CallTest(variation, Algo.Knapsack2D, data[dataSize], desired, accuracy, timeout);
                    if (!result) {
                        Console.WriteLine("timed out");
                        break;
                    }

                    TestResult test = await LoadResults();
                    if (!test.success)
                        break;
                    Console.WriteLine($"value {test.value} in {test.time} ms | "
                        + $"sch:{test.expectedScholarships}/{(rejected ? desired / 2 : dataSize / 2)}"
                        + $" | std:{test.expectedStudents}/{(rejected ? desired : dataSize)}");
                    SaveResults(default);
                }
                //}

                Console.WriteLine();
            }
        }

        public static async Task RunTestSuiteOptimality() {
            int size = 1000;
            int desired = 250;
            int repeats = 25;

            List<string> data = new();

            for (int i = 0; i < repeats; i++) {
                data.Add($"opt{i}.json");
                SaveData(data[i], GenerateData(size));
            }

            await Task.Delay(10);

            foreach (Variation variation in new Variation[] { Variation.Binary, Variation.Rejection, Variation.Split, Variation.SplitRejection }) {
                Console.WriteLine(variation.ToString());
                int timeout = 60000;
                int accuracy = 10;

                foreach (Algo algo in new Algo[] { Algo.Sort, Algo.Knapsack2D }) {
                    if (!Valids[variation].Contains(algo)) continue;
                    bool rejected = variation == Variation.Rejection || variation == Variation.SplitRejection || variation == Variation.Approximate || variation == Variation.DoubleApproximate;
                    Console.Write($"    {algo}, dataset {(rejected ? $"{desired}/" : "")}{size}");
                    Console.Write(" -> ");

                    TestResult total = new();
                    for (int i = 0; i < repeats; i++) {
                        bool result = await CallTest(variation, algo, data[i], desired, accuracy, timeout, false);
                        if (!result) {
                            Console.WriteLine("timed out");
                            break;
                        }

                        TestResult test = await LoadResults();
                        if (!test.success)
                            break;
                        SaveResults(default);
                        total.time += test.time;
                        total.value += test.value;
                        total.expectedScholarships += test.expectedScholarships;
                        total.expectedStudents += test.expectedStudents;
                    }

                    total.time /= repeats;
                    total.value /= repeats;
                    total.expectedScholarships /= repeats;
                    total.expectedStudents /= repeats;
                    Console.WriteLine($"value {Math.Round(total.value, 1)} in {Math.Round(total.time, 2)} ms | "
                        + $"sch:{Math.Round(total.expectedScholarships, 2)}/{(rejected ? desired / 2 : size / 2)}"
                        + $" | std:{Math.Round(total.expectedStudents, 2)}/{(rejected ? desired : size)}");
                }
            }
        }

        public static async Task RunTestSuite2023() {
            int size = 400;
            int desired = 45;
            string file = "2023.json";

            SaveData(file, GenerateDataApprox(size, false));

            foreach (Variation variation in new Variation[] { Variation.SplitRejection, Variation.Approximate, Variation.DoubleApproximate }) {
                Console.WriteLine(variation.ToString());
                int timeout = 600000;
                int accuracy = 100;

                bool rejected = variation == Variation.Rejection || variation == Variation.SplitRejection || variation == Variation.Approximate || variation == Variation.DoubleApproximate;
                Console.Write($"    {variation} {(rejected ? $"{desired}/" : "")}{size}");
                Console.Write(" -> ");
                bool result = await CallTest(variation, Algo.Knapsack2D, file, desired, accuracy, timeout);
                if (!result) {
                    Console.WriteLine("timed out");
                    break;
                }

                TestResult test = await LoadResults();
                if (!test.success)
                    break;
                Console.WriteLine($"value {test.value} in {test.time} ms | "
                    + $"sch:{test.expectedScholarships}/{(rejected ? desired / 2 : size / 2)}"
                    + $" | std:{test.expectedStudents}/{(rejected ? desired : size)}");
                SaveResults(default);
            }
        }

        public static async Task RunSingle() {
            List<Student> students = GenerateData(100000);
            string file = "test.json";
            SaveData(file, students);
            if (await CallTest(Variation.Binary, Algo.Sort, file, 0, 0, 10000)) {
                TestResult result = await LoadResults();
                Console.WriteLine("Results");
                Console.WriteLine($"Time: {result.time} ms");
                //Console.WriteLine($"Students: {students.Count}");
                Console.WriteLine($"Value: {result.value}");
                //Console.WriteLine($"Average score: {r.AverageScore()}");
                //Console.WriteLine($"Waivers used: {result.WeightX}/{waivers}");
                //Console.WriteLine($"Student slots: {result.WeightY}/{acceptedStudents}");
                Console.WriteLine($"Approximate scholarships used: {result.expectedScholarships}");
                Console.WriteLine($"Approximate students joined: {result.expectedStudents}");
                Console.WriteLine();
            } else {
                Console.WriteLine("Timeout");
            }
        }

        private static async Task<bool> CallTest(Variation variation, Algo algo, string studentFile, int amount, int accuracy, int timeout, bool repeats = true) {
            if (Environment.ProcessPath == null)
                return false;
            Process p = Start(Environment.ProcessPath, variation.ToString(), algo.ToString(), studentFile, amount.ToString(), accuracy.ToString(), repeats.ToString());

            await Task.WhenAny(p.WaitForExitAsync(), Task.Delay(timeout));
            if (!p.HasExited) {
                p.Kill();
                return false;
            }

            return true;
        }

        public static async Task ExecuteTest(Variation variation, Algo algo, string studentFile, int amount, int accuracy, bool repeats) {
            List<Student> studentList = await LoadData(studentFile);

            Stopwatch sw = Stopwatch.StartNew();
            TestResult result = SelectAndRunImpl(studentList, variation, algo, amount, accuracy);
            sw.Stop();

            result.time = sw.ElapsedMilliseconds;

            if (repeats && sw.ElapsedMilliseconds < 5000) {
                List<Student>[] datasets = new List<Student>[10];
                for (int i = 0; i < 10; i++) {
                    datasets[i] = GenerateData(studentList.Count);
                }

                Stopwatch sw2 = Stopwatch.StartNew();
                for (int i = 0; i < 10; i++) {
                    SelectAndRunImpl(datasets[i], variation, algo, amount, accuracy);
                }
                sw2.Stop();
                result.time = sw2.ElapsedMilliseconds / 10.0;
            }

            SaveResults(result);
        }

        public static TestResult SelectAndRunImpl(List<Student> students, Variation variation, Algo algo, int amount, int accuracy) {
            if (variation == Variation.Binary) {
                switch (algo) {
                case Algo.Brute:
                    return AlgorithmImpl.SolveFullBrute(students);
                case Algo.Sort:
                    return AlgorithmImpl.SolveFullSort(students);
                case Algo.Median:
                    return AlgorithmImpl.SolveFullMedian(students);
                case Algo.Knapsack:
                    return AlgorithmImpl.SolveFullKnapsack(students);
                case Algo.BranchKnapsack:
                    return AlgorithmImpl.SolveFullKnapsackBranch(students);
                case Algo.ChoiceKnapsack:
                    return AlgorithmImpl.SolveFullChoiceKnapsack(students);
                case Algo.Knapsack2D:
                    return AlgorithmImpl.SolveFullKnapsack2D(students);
                }

            } else if (variation == Variation.Rejection) {
                switch (algo) {
                case Algo.Brute:
                    return AlgorithmImpl.SolveRejectionBrute(students, amount);
                case Algo.Sort:
                    return AlgorithmImpl.SolveRejectionSort(students, amount);
                case Algo.Knapsack2D:
                    return AlgorithmImpl.SolveRejectionKnapsack2D(students, amount);
                case Algo.BranchKnapsack2D:
                    return AlgorithmImpl.SolveRejectionKnapsack2DBranch(students, amount);
                }

            } else if (variation == Variation.Split) {
                switch (algo) {
                case Algo.Brute:
                    return AlgorithmImpl.SolveSplitBrute(students);
                case Algo.Sort:
                    return AlgorithmImpl.SolveSplitSort(students);
                case Algo.ChoiceKnapsack:
                    return AlgorithmImpl.SolveSplitChoiceKnapsack(students);
                case Algo.Knapsack2D:
                    return AlgorithmImpl.SolveSplitKnapsack2D(students);
                case Algo.BranchKnapsack2D:
                    return AlgorithmImpl.SolveSplitKnapsack2DBranch(students);
                }

            } else if (variation == Variation.SplitRejection) {
                switch (algo) {
                case Algo.Sort:
                    return AlgorithmImpl.SolveSplitRejectionSort(students, amount);
                case Algo.Knapsack2D:
                    return AlgorithmImpl.SolveSplitRejectionKnapsack2D(students, amount);
                }

            } else if (variation == Variation.Approximate) {
                switch (algo) {
                case Algo.Knapsack2D:
                    return AlgorithmImpl.SolveApproximation(students, amount, accuracy);
                case Algo.BranchKnapsack2D:
                    return AlgorithmImpl.SolveApproximationBranch(students, amount, accuracy);
                }

            } else if (variation == Variation.DoubleApproximate) {
                switch (algo) {
                case Algo.Knapsack2D:
                    return AlgorithmImpl.SolveDoubleApproximation(students, amount, accuracy);
                case Algo.BranchKnapsack2D:
                    return AlgorithmImpl.SolveDoubleApproximationBranch(students, amount, accuracy);
                }

            } else if (variation == Variation.BranchTest) {
                switch (algo) {
                case Algo.Knapsack:
                    return AlgorithmImpl.SolveFullKnapsackLarge(students, amount);
                case Algo.BranchKnapsack:
                    return AlgorithmImpl.SolveFullKnapsackBranchLarge(students, amount);
                }
            }

            return default;
        }

        public static async void SaveData(string file, List<Student> students) {
            string content = string.Join("\n", students
                .Select(x => $"{x.Name}|{x.Score}|{x.PHigh}|{x.PMid}|{x.PLow}"));

            //string content = JsonConvert.SerializeObject(students, new JsonSerializerSettings {
            //TypeNameHandling = TypeNameHandling.All,
            //Formatting = Formatting.Indented
            //});

            using (StreamWriter w = new StreamWriter(file)) {
                await w.WriteAsync(content);
            }
        }

        public static async Task<List<Student>> LoadData(string file) {
            string res;
            using (StreamReader r = new StreamReader(file)) {
                res = await r.ReadToEndAsync();
            }

            //return JsonConvert.DeserializeObject<List<Student>>(res, new JsonSerializerSettings {
            //TypeNameHandling = TypeNameHandling.All,
            //Formatting = Formatting.Indented
            //}) ?? new();

            return res.Split("\n").Select(x => x.Split("|")).Select(x => new Student() {
                Name = x[0],
                Score = double.Parse(x[1]),
                PHigh = double.Parse(x[2]),
                PMid = double.Parse(x[3]),
                PLow = double.Parse(x[4]),
            }).ToList();
        }

        public static async void SaveResults(TestResult result) {
            string content = JsonConvert.SerializeObject(result, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            });

            using (StreamWriter w = new StreamWriter("results.json")) {
                await w.WriteAsync(content);
            }
        }

        public static async Task<TestResult> LoadResults() {
            string res;
            using (StreamReader r = new StreamReader("results.json")) {
                res = await r.ReadToEndAsync();
            }

            TestResult tr = JsonConvert.DeserializeObject<TestResult>(res, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            });

            tr.value = Math.Round(tr.value, 5);
            tr.expectedScholarships = Math.Round(tr.expectedScholarships, 5);
            tr.expectedStudents = Math.Round(tr.expectedStudents, 5);

            return tr;
        }

        public static List<Student> GenerateData(int amount) {
            Random random = new Random();
            List<Student> data = new();

            for (int i = 1; i <= amount; i++) {
                double low = Math.Round(random.NextDouble(), 5);
                double high = Math.Round(random.NextDouble(), 5);
                if (low > high) {
                    double temp = low;
                    low = high;
                    high = temp;
                }
                double mid = Math.Round(random.NextDouble() * (high - low) + low, 5);

                if (low > mid || mid > high || high > 1) {
                    throw new Exception("Badly generated probabilities");
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

        public static List<Student> GenerateDataApprox(int amount, bool tens) {
            Random random = new Random();
            List<Student> data = new();

            for (int i = 1; i <= amount; i++) {
                int low = random.Next(101);
                int high = random.Next(101);
                if (low > high) {
                    int temp = low;
                    low = high;
                    high = temp;
                }
                int mid = (int)(random.NextDouble() * (high - low + 1) + low);

                if (low > mid || mid > high || high > 100) {
                    throw new Exception("Badly generated probabilities");
                }

                if (tens) {
                    high = (int)(Math.Round(high / 10.0) * 10);
                    mid = (int)(Math.Round(mid / 10.0) * 10);
                    low = (int)(Math.Round(low / 10.0) * 10);
                }

                data.Add(new() {
                    Name = $"s{i}",
                    Score = random.Next(1, 101),
                    PHigh = high / 100.0,
                    PMid = mid / 100.0,
                    PLow = low / 100.0,
                });
            }

            return data;
        }

        public static List<Student> GenerateDataApproxHalf(int amount, bool tens) {
            Random random = new Random();
            List<Student> data = new();

            for (int i = 1; i <= amount; i++) {
                int low = random.Next(51);
                int high = random.Next(51);
                if (low > high) {
                    int temp = low;
                    low = high;
                    high = temp;
                }
                int mid = (int)(random.NextDouble() * (high - low + 1) + low);

                if (low > mid || mid > high || high > 100) {
                    throw new Exception("Badly generated probabilities");
                }

                if (tens) {
                    high = (int)(Math.Round(high / 10.0) * 10);
                    mid = (int)(Math.Round(mid / 10.0) * 10);
                    low = (int)(Math.Round(low / 10.0) * 10);
                }

                data.Add(new() {
                    Name = $"s{i}",
                    Score = random.Next(1, 101),
                    PHigh = high / 100.0,
                    PMid = mid / 100.0,
                    PLow = low / 100.0,
                });
            }

            return data;
        }

        public static List<Student> EdgeCase1(int amount) {
            List<Student> data = new();

            for (int i = 0; i < amount; i++) {
                //if (i < amount / 2.0) {
                if (i == 0) {
                    data.Add(new() {
                        Name = $"s{i}",
                        Score = 100,
                        PHigh = 0.5,
                        PMid = 0.5,
                        PLow = 0.5,
                    });
                } else {
                    data.Add(new() {
                        Name = $"s{i}",
                        Score = 50,
                        PHigh = 0.1,
                        PMid = 0.1,
                        PLow = 0.1,
                    });
                }
            }

            return data;
        }

        #region process launch
        /// <summary>Launch a process by specifying a file</summary>
        /// <remarks>Arguments are given as an array of strings. If a string includes spaces, it's wrapped in enclosing quotes: "argument with spaces"</remarks>
        private static Process Start(string file, params string[] arguments) {
            return Base(file, string.Join(" ", arguments.Select(a => a.Contains(" ") && !a.Contains('"') ? $"\"{a}\"" : a)), null, ProcessWindowStyle.Normal);
        }

        /// <summary>Launch files using a single string as arguments</summary>
        //static Process Basic(string file, string arguments, ProcessWindowStyle style = ProcessWindowStyle.Normal) {
        //    return Base(file, arguments, null, style);
        //}

        private static Process Base(string process, string arguments, string workingDir, ProcessWindowStyle style) {
            if (process == null || process == "") {
                return null;
            }

            //if (level == PermissionLevel.Normal && RunningAsAdmin) {
            //SystemUtility.ExecuteProcessUnElevated(process, arguments, workingDir ?? "");
            //return null;
            //}

            Process p = new Process();
            p.StartInfo.FileName = process;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.WindowStyle = style;
            if (workingDir != null && workingDir != "")
                p.StartInfo.WorkingDirectory = workingDir;
            //if (level == PermissionLevel.Admin)
            //p.StartInfo.Verb = "RunAs";
            p.Start();
            return p;
        }
        #endregion
    }

    public struct TestResult {
        public bool success;
        public double value;
        public double expectedStudents;
        public double expectedScholarships;
        public double time;
    }

    public enum Variation {
        Binary,
        Rejection,
        Split,
        SplitRejection,
        Approximate,
        DoubleApproximate,
        BranchTest
    }

    public enum Algo {
        Brute,
        Sort,
        Median,
        Knapsack,
        BranchKnapsack,
        ChoiceKnapsack,
        Knapsack2D,
        BranchKnapsack2D,
    }
}

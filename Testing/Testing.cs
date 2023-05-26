using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionWaiverDistribution.Algorithms;
using TuitionWaiverDistribution.DataTypes;

namespace TuitionWaiverDistribution {
    public static class Testing {

        public static Dictionary<Variation, List<Algo>> Valids { get; set; } = new() {
            { Variation.Full, new() { Algo.Brute, Algo.Sort, Algo.Median, Algo.Knapsack, Algo.BranchKnapsack, Algo.ChoiceKnapsack } },
            { Variation.Reject, new() { Algo.Sort, Algo.Knapsack } },
            { Variation.Split, new() { Algo.Brute, Algo.ChoiceKnapsack } },
            { Variation.Approximate, new() { Algo.Knapsack2D, Algo.BranchKnapsack2D } },
            { Variation.DoubleApproximate, new() { Algo.Knapsack2D, Algo.BranchKnapsack2D } }
        };

        public static async Task RunTestSuite() {
            foreach (Variation variation in Enum.GetValues(typeof(Variation))) {
                Console.WriteLine(variation.ToString());

                foreach (Algo algo in Enum.GetValues(typeof(Algo))) {
                    if (!Valids[variation].Contains(algo)) continue;
                    Console.WriteLine($"    {algo}");

                    foreach (int amount in new int[] { 10, 100, 1000, 10000 }) {
                        Console.Write($"        dataset {amount}");
                        //bool result = await CallTest(variation, algo, amount, 10000);
                        //if (!result) {
                        //Console.WriteLine(" -> timed out");
                        //break;
                        //}

                        //Console.WriteLine($" -> value {result.value} in {result.time}");
                    }
                }
            }
        }

        public static async Task RunSingle() {
            List<Student> students = GenerateData(10000);
            string file = "test.json";
            SaveData(file, students);
            _ = CallTest(Variation.Full, Algo.Sort, file, 10000);
            Console.WriteLine();
        }

        private static async Task<bool> CallTest(Variation variation, Algo algo, string studentFile, int timeout) {
            if (Environment.ProcessPath == null)
                return false;
            Process p = Start(Environment.ProcessPath, variation.ToString(), algo.ToString(), studentFile);
            //StringBuilder sto = new();
            //p.OutputDataReceived += (o, e) => sto.AppendLine(e.Data);

            Task res = await Task.WhenAny(p.WaitForExitAsync(), Task.Delay(timeout));
            if (!p.HasExited) {
                p.Kill();
                return false;
            }

            //string result = sto.ToString();
            //if (string.IsNullOrEmpty(result))
            //return default;
            //return JsonConvert.DeserializeObject<TestResult>(result);
            return true;
        }

        public static async Task ExecuteTest(Variation variation, Algo algo, string studentFile) {
            List<Student> studentList = await LoadData(studentFile);

            Stopwatch sw = Stopwatch.StartNew();
            TestResult result = SelectAndRunImpl(studentList, variation, algo);
            sw.Stop();

            result.time = sw.ElapsedMilliseconds;

            SaveResults(result);
        }

        public static TestResult SelectAndRunImpl(List<Student> students, Variation variation, Algo algo) {
            if (variation == Variation.Full) {
                if (algo == Algo.Brute) {
                    return AlgorithmImpl.SolveFullBrute(students);
                } else if (algo == Algo.Sort) {
                    return AlgorithmImpl.SolveFullSort(students);
                } else if (algo == Algo.Median) {
                    return AlgorithmImpl.SolveFullMedian(students);
                } else if (algo == Algo.Knapsack) {
                    return AlgorithmImpl.SolveFullKnapsack(students);
                } else if (algo == Algo.BranchKnapsack) {
                    return AlgorithmImpl.SolveFullBranchKnapsack(students);
                } else if (algo == Algo.ChoiceKnapsack) {
                    return AlgorithmImpl.SolveFullChoiceKnapsack(students);
                }

            } else if (variation == Variation.Split) {
                if (algo == Algo.Brute) {
                    return AlgorithmImpl.SolveHalfBrute(students);
                } else if (algo == Algo.ChoiceKnapsack) {
                    return AlgorithmImpl.SolveHalfChoiceKnapsack(students);
                }

            } else if (variation == Variation.Approximate) {
                if (algo == Algo.Knapsack2D) {
                    return AlgorithmImpl.SolveStudentKnapsack2D(students);
                } else if (algo == Algo.BranchKnapsack2D) {
                    return AlgorithmImpl.SolveStudentBranchKnapsack2D(students);
                }

            } else if (variation == Variation.DoubleApproximate) {
                if (algo == Algo.Knapsack2D) {
                    return AlgorithmImpl.SolveWaiverKnapsack2D(students);
                } else if (algo == Algo.BranchKnapsack2D) {
                    return AlgorithmImpl.SolveWaiverBranchKnapsack2D(students);
                }
            }

            return default;
        }

        public static List<Student> GenerateData(int amount) {
            Random random = new Random();
            List<Student> data = new();

            for (int i = 1; i <= amount; i++) {
                double low = random.NextDouble();
                double high = random.NextDouble();
                if (low > high) {
                    double temp = low;
                    low = high;
                    high = temp;
                }
                double mid = random.NextDouble() * (high - low) + low;

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
                    high = (int)Math.Round(high / 10.0) * 10;
                    mid = (int)Math.Round(mid / 10.0) * 10;
                    low = (int)Math.Round(low / 10.0) * 10;
                }

                data.Add(new() {
                    Name = $"s{i}",
                    Score = random.Next(1, 101),
                    PHigh = high / 100,
                    PMid = mid / 100,
                    PLow = low / 100,
                });
            }

            return data;
        }

        public static async void SaveData(string file, List<Student> students) {
            string content = JsonConvert.SerializeObject(students, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            });

            using (StreamWriter w = new StreamWriter(file)) {
                await w.WriteAsync(content);
            }
        }

        public static async Task<List<Student>> LoadData(string file) {
            string res;
            using (StreamReader r = new StreamReader(file)) {
                res = await r.ReadToEndAsync();
            }

            return JsonConvert.DeserializeObject<List<Student>>(res, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            }) ?? new();
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

            return JsonConvert.DeserializeObject<TestResult>(res, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            });
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
        public long time;
    }

    public enum Variation {
        Full,
        Reject,
        Split,
        Approximate,
        DoubleApproximate
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

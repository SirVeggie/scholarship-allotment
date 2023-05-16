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
            { Variation.Half, new() { Algo.Brute, Algo.ChoiceKnapsack } },
            { Variation.StudentAdjustment, new() { Algo.Knapsack2D, Algo.BranchKnapsack2D } },
            { Variation.WaiverAdjustment, new() { Algo.Knapsack2D, Algo.BranchKnapsack2D } }
        };

        public static async Task RunTestSuite() {
            foreach (Variation variation in Enum.GetValues(typeof(Variation))) {
                Console.WriteLine(variation.ToString());

                foreach (Algo algo in Enum.GetValues(typeof(Algo))) {
                    if (!Valids[variation].Contains(algo)) continue;
                    Console.WriteLine($"    {algo}");

                    foreach (int amount in new int[] { 10, 100, 1000, 10000 }) {
                        Console.Write($"        dataset {amount}");
                        bool result = await CallTest(variation, algo, amount, 10000);
                        if (!result) {
                            Console.WriteLine(" -> timed out");
                            break;
                        }

                        //Console.WriteLine($" -> value {result.value} in {result.time}");
                    }
                }
            }
        }

        static async Task<bool> CallTest(Variation variation, Algo algo, int students, int timeout) {
            if (Environment.ProcessPath == null)
                return false;
            Process p = Start(Environment.ProcessPath, variation.ToString(), algo.ToString(), students.ToString());
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

        public static double ExecuteTest(Variation variation, Algo algo, int students) {
            List<Student> studentList = GenerateData(students);

            if (variation == Variation.Full) {
                if (algo == Algo.Brute) {
                    return AlgorithmImpl.SolveFullBrute(studentList);
                } else if (algo == Algo.Sort) {
                    return AlgorithmImpl.SolveFullSort(studentList);
                } else if (algo == Algo.Median) {
                    return AlgorithmImpl.SolveFullMedian(studentList);
                } else if (algo == Algo.Knapsack) {
                    return AlgorithmImpl.SolveFullKnapsack(studentList);
                } else if (algo == Algo.BranchKnapsack) {
                    return AlgorithmImpl.SolveFullBranchKnapsack(studentList);
                } else if (algo == Algo.ChoiceKnapsack) {
                    return AlgorithmImpl.SolveFullChoiceKnapsack(studentList);
                } else {
                    return 0;
                }

            } else if (variation == Variation.Half) {
                if (algo == Algo.Brute) {
                    return AlgorithmImpl.SolveHalfBrute(studentList);
                } else if (algo == Algo.ChoiceKnapsack) {
                    return AlgorithmImpl.SolveHalfChoiceKnapsack(studentList);
                }

            } else if (variation == Variation.StudentAdjustment) {
                if (algo == Algo.Knapsack2D) {
                    return AlgorithmImpl.SolveStudentKnapsack2D(studentList);
                } else if (algo == Algo.BranchKnapsack2D) {
                    return AlgorithmImpl.SolveStudentBranchKnapsack2D(studentList);
                }

            } else if (variation == Variation.WaiverAdjustment) {
                if (algo == Algo.Knapsack2D) {
                    return AlgorithmImpl.SolveWaiverKnapsack2D(studentList);
                } else if (algo == Algo.BranchKnapsack2D) {
                    return AlgorithmImpl.SolveWaiverBranchKnapsack2D(studentList);
                }
            }

            return 0;
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
        public long time;
    }

    public enum Variation {
        Full,
        Half,
        //CustomRatio,
        StudentAdjustment,
        WaiverAdjustment
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

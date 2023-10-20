using System;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchMarker;
using CuckooHashTable;

class Program
{
    static void Main(string[] args)
    {
        // BenchMarks Run First
        // var config = BenchmarkDotNet.Configs.DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
        // BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

        // Let's do some analytical testing to check for any empirical correlations between m(Number of Keys) and MaxAttempts
        List<int> keyCounts = new List<int> { 1000, 5000, 10000, 20000, 50000, 100000 };
        Dictionary<int, List<int>> results = new Dictionary<int, List<int>>();

        foreach (var keyCount in keyCounts)
        {
            Container container = new Container(2, keyCount);
            List<int> attempts = new List<int>();

            for (int i = 0; i < keyCount; i++)
            {
                container.Insert(i);
                attempts.Add(container.LastAttemptCount);
            }

            results[keyCount] = attempts;
        }

        // We could use Oxyplot to directly plot it in C#, but we could also use Excel or Python
        string csvFile = "attempts.csv";
        using (StreamWriter writer = new StreamWriter(csvFile))
        {
            writer.WriteLine("KeyCount,Attempts");
            foreach (var entry in results)
            {
                writer.WriteLine($"{entry.Key},{string.Join("|", entry.Value)}");
            }
        }

        List<int> keyCounts2 = new List<int> { 1000, 5000, 10000, 20000, 50000, 100000 };
        Dictionary<int, double> results2 = new Dictionary<int, double>();
        foreach(var keyCount in keyCounts2)
        {
            Container container = new Container(2, keyCount);
            for (int i = 0; i < keyCount; i++)
            {
                container.Insert(i);
            }
            results2[keyCount] = container.AvgLookUpTime();
        }

        // Let's do some analytical testing to check for any empirical correlations between m(Number of Keys) and AverageLookup
        string averageLookupFile = "averageLookup2Tables.csv";
        using (StreamWriter writer = new StreamWriter(averageLookupFile))
        {
            writer.WriteLine("KeyCount,AverageLookup");
            foreach (var entry in results2)
            {
                writer.WriteLine($"{entry.Key},{entry.Value}");
            }
        }

         List<int> keyCounts3 = new List<int> { 1000, 5000, 10000, 20000, 50000, 100000 };
        Dictionary<int, double> results3 = new Dictionary<int, double>();
        foreach(var keyCount in keyCounts3)
        {
            Container container = new Container(3, keyCount);
            for (int i = 0; i < keyCount; i++)
            {
                container.Insert(i);
            }
            results2[keyCount] = container.AvgLookUpTime();
        }

        // Let's do some analytical testing to check for any empirical correlations between m(Number of Keys) and AverageLookup
        string averageLookupFile2 = "averageLookup3Tables.csv";
        using (StreamWriter writer = new StreamWriter(averageLookupFile2))
        {
            writer.WriteLine("KeyCount,AverageLookup");
            foreach (var entry in results2)
            {
                writer.WriteLine($"{entry.Key},{entry.Value}");
            }
        }
    }
}
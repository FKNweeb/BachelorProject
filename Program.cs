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
        // Range of table tests
        int minTables = 2;
        int maxTables = 10;

        // Key List
        List<int> keyCounts = new List<int> { 1000, 5000, 10000, 20000, 50000, 100000 };

        for (int tableCount = minTables; tableCount <= maxTables; tableCount++)
        {
            GenerateAttemptsCSV(tableCount, keyCounts);
            GenerateAverageLookupCSV(tableCount, keyCounts);
        }
    }

    static void SaveToCSV(string directory, string filename, string content)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(Path.Combine(directory, filename), content);
    }

    static void GenerateAttemptsCSV(int tableCount, List<int> keyCounts)
    {
        Dictionary<int, List<int>> results = new Dictionary<int, List<int>>();

        foreach (var keyCount in keyCounts)
        {
            Container container = new Container(tableCount, keyCount);
            List<int> attempts = new List<int>();

            for (int i = 0; i < keyCount; i++)
            {
                container.Insert(i);
                attempts.Add(container.LastAttemptCount);
            }

            results[keyCount] = attempts;
        }

        string csvContent = "KeyCount,Attempts\n";
        foreach (var entry in results)
        {
            csvContent += $"{entry.Key},{string.Join("|", entry.Value)}\n";
        }

        SaveToCSV("CSV_Files", $"attempts_{tableCount}tables.csv", csvContent);
    }

    static void GenerateAverageLookupCSV(int tableCount, List<int> keyCounts)
    {
        Dictionary<int, double> results = new Dictionary<int, double>();

        foreach (var keyCount in keyCounts)
        {
            Container container = new Container(tableCount, keyCount);
            for (int i = 0; i < keyCount; i++)
            {
                container.Insert(i);
            }

            results[keyCount] = container.AvgLookUpTime();
        }

        string csvContent = "KeyCount,AverageLookup\n";
        foreach (var entry in results)
        {
            csvContent += $"{entry.Key},{entry.Value}\n";
        }

        SaveToCSV("CSV_Files", $"averageLookup_{tableCount}tables.csv", csvContent);
    }
}
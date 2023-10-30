using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        /*for (int tableCount = minTables; tableCount <= maxTables; tableCount++)
        {
            GenerateAttemptsCSV(tableCount, keyCounts);
            GenerateAverageLookupCSV(tableCount, keyCounts);
        }*/
        // Generate3TablesAverageLookup(keyCounts);
        Generate3TablesHowFilled(keyCounts);
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

            for (int i = 0; i < keyCount * tableCount; i++)
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

    static void Generate3TablesAverageLookup(List<int> keyCounts) {
        List<List<int>> lists = new List<List<int>>();
        foreach (var keyCount in keyCounts)
        {
            for (int j = 0; j < keyCount/2; j += keyCount / 16)
            {
                lists.Add(new List<int>{keyCount, keyCount - (keyCount/2) + j, keyCount - (keyCount / 2) - j});
            }
        }

        string csvContent = "KeyCount,AverageLookup,TableSize1,TableSize2,TableSize3\n";
        
        foreach (var list in lists)
        {
            GenericContainer container = new GenericContainer(3, list);
            for (int i = 0; i < list[0] * 2; i++)
            {
                container.Insert(i);
            }
            csvContent += $"{list[0]},{container.AvgLookUpTime()},{list[0]},{list[1]},{list[2]}\n";
        }


        SaveToCSV("CSV_Files", $"Generic_3tables.csv", csvContent);
    }

    static void Generate3TablesHowFilled(List<int> keyCounts) {
        List<List<int>> lists = new List<List<int>>();
        foreach (var keyCount in keyCounts)
        {
            for (int j = 0; j < keyCount/2; j += keyCount / 16)
            {
                lists.Add(new List<int>{keyCount, keyCount - (keyCount/2) + j, keyCount - (keyCount / 2) - j});
            }
        }

        string csvContent = "AverageLookup,HowFilled\n";
        
        foreach (var list in lists)
        {
            GenericContainer container = new GenericContainer(3, list);
            for (int i = 0; i < list[0] * 2; i++)
            {
                container.Insert(i);
            }
            csvContent += $"{container.AvgLookUpTime()},{container.HowFilled()}\n";
        }


        SaveToCSV("CSV_Files", $"Generic_3tablesHowFilled.csv", csvContent);
    }
}
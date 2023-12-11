using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchMarker;
using CuckooHashTable;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

class Program
{
    static void Main(string[] args)
    {
        // Key List
        int[] keyCounts = { 1000, 5000, 10000, 20000, 50000, 100000 };

        // foreach (var keyCount in keyCounts)
        // {
        //      for (int i = 2; i <= 10; i++)
        //      {
        //         GenerateAttemptsCSV(i, keyCount);
        //         GenerateAverageLookupCSV(i, keyCount);
        //         GenerateTablesLoadFactor(keyCount, i);
        //     }
        // }
        var selectedHeadersDic = new Dictionary<int, string[]>();
        selectedHeadersDic.Add(2, new string[] {"TableSize1", "TableSize2"});
        selectedHeadersDic.Add(3, new string[] {"TableSize1", "TableSize2", "TableSize3"});
        selectedHeadersDic.Add(4, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4"});
        selectedHeadersDic.Add(5, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4", "TableSize5"});
        selectedHeadersDic.Add(6, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4", "TableSize5", "TableSize6"});
        selectedHeadersDic.Add(7, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4", "TableSize5", "TableSize6", "TableSize7"});
        selectedHeadersDic.Add(8, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4", "TableSize5", "TableSize6", "TableSize7", "TableSize8"});
        selectedHeadersDic.Add(9, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4", "TableSize5", "TableSize6", "TableSize7", "TableSize8", "TableSize9"});
        selectedHeadersDic.Add(10, new string[] {"TableSize1", "TableSize2", "TableSize3", "TableSize4", "TableSize5", "TableSize6", "TableSize7", "TableSize8", "TableSize9", "TableSize10"});

        foreach (var keyCount in keyCounts)
        {
            for (int i = 2; i < 11; i++)
            {
                string csvFilePath = $"CSV_Files/LoadFactor/Pareto/GenericParetoData{keyCount}_{i}tables.csv";
                List<int[]> selectedLines = ReadCsvFile(csvFilePath, selectedHeadersDic[i]);

                GenerateVariance(selectedLines);
                ReduceNoiseLoadFactor(selectedLines);
            }
        }
    }

    static void SaveToCSV(string directory1, string directory2, string filename, string content)
    {
        string FullPath = Path.Combine(directory1, directory2);
        if (!Directory.Exists(FullPath))
        {
            Directory.CreateDirectory(FullPath);
        }

        File.WriteAllText(Path.Combine(FullPath, filename), content);
    }

    static void GenerateAttemptsCSV(int tableCount, int keyCount)
    {
        Dictionary<int, List<int>> results = new Dictionary<int, List<int>>();

        Container container = new Container(tableCount, keyCount);
        List<int> attempts = new List<int>();

        for (int i = 0; i < keyCount * tableCount; i++)
        {
            attempts.Add(container.LastAttemptCount);
            if(!container.Insert(i)) { break; }
            
        }
        results[keyCount] = attempts;

        string csvContent = "KeyCount,Attempts\n";
        foreach (var entry in results)
        {
            csvContent += $"{entry.Key},{string.Join("|", entry.Value)}\n";
        }

        SaveToCSV("CSV_Files", "KeyAttempts", $"attempts{keyCount}_{tableCount}tables.csv", csvContent);
    }

    static void GenerateAverageLookupCSV(int tableCount, int keyCount)
    {
        Dictionary<int, double> results = new Dictionary<int, double>();
        Container container = new Container(tableCount, keyCount);
        for (int i = 0; i < keyCount; i++)
        {
            if(!container.Insert(i)) { break; }
        }

        results[keyCount] = container.AvgLookUpTime();

        string csvContent = "KeyCount,AverageLookup\n";
        foreach (var entry in results)
        {
            csvContent += $"{entry.Key},{entry.Value}\n";
        }

        SaveToCSV("CSV_Files", "AverageLookUp", $"averageLookup{keyCount}_{tableCount}tables.csv", csvContent);
    }

    static void GenerateTablesLoadFactor(int keyCount, int numOfTables) {
        List<List<int>> lists = new List<List<int>>();
        int totalSum = keyCount * 2;
        int numberOfSubLists = 100;
        List<int> firstList = new List<int>();
        for (int i = 0; i < numOfTables; i++)
        {
            firstList.Add(totalSum / numOfTables);
        }
        lists.Add(firstList);
        for (int i = 0; i < numberOfSubLists; i++)
        {
            List<int> subList = new List<int>();
            int remainingSum = totalSum;

            for (int j = 0; j < numOfTables - 1; j++)
            {
                if (remainingSum - numOfTables <= 0) { subList.Clear(); break; }
                int randomValue = new Random().Next(remainingSum - numOfTables);
                if (randomValue == 0) { randomValue = 1; }
                subList.Add(randomValue);
                remainingSum -= randomValue;
            }
            if (remainingSum == 0 || subList.Count() < numOfTables - 1) { subList.Clear(); continue; }
            subList.Add(remainingSum);
            lists.Add(subList);
        }
        List<int> lastList = new List<int>();
        lastList.Add(totalSum - (numOfTables - 1) * 100);
        for (int i = 0; i < numOfTables-1; i++)
        {
            lastList.Add(100);
        }
        lists.Add(lastList);
        

        string csvContent = "AverageLookUp,LoadFactor";
        for (int i = 1; i <= numOfTables; i++)
        {
            csvContent += $",TableSize{i}";
        }
        csvContent += "\n";
        
        foreach (var list in lists)
        {
            list.Sort();
            list.Reverse();
            GenericContainer container = new GenericContainer(numOfTables, list);
            for (int i = 0; i < list[0] * 2; i++)
            {
                if(!container.Insert(i)) { break; }
            }
            csvContent += $"{container.AvgLookUpTime()},{container.LoadFactor()}";
            foreach(var val in list)
            {
                csvContent += $",{val}";
            }
            csvContent += "\n";
        }

        SaveToCSV("CSV_Files", "LoadFactor", $"GenericLoadFactor{keyCount}_{numOfTables}tables.csv", csvContent);
    }


    static void GenerateVariance(List<int[]> lists){

        List<List<int>> outputList = new List<List<int>>();
        var rand = new Random();

        foreach (var item in lists)
        {
            int numberOfSubLists = 10;
            for (int i = 0; i < numberOfSubLists; i++)
            {   
                List<int> point = new List<int>();
                for (int j = 0; j < item.Length; j++)
                {
                    point.Add(rand.Next(item[j] - (int)(item[j] * 0.1), item[j] + (int)(item[j] * 0.1)));
                } 
                outputList.Add(point);
            }
        }

        string csvContent = "AverageLookUp,LoadFactor";
        for (int i = 1; i <= lists[0].Length; i++)
        {
            csvContent += $",TableSize{i}";
        }
        csvContent += "\n";
        
        foreach (var list in outputList)
        {
            list.Sort();
            list.Reverse();
            
            GenericContainer container = new GenericContainer(list.Count, list);
            for (int i = 0; i < list[0] * 2; i++)
            {
                if(!container.Insert(i)) { break; }
            }
            csvContent += $"{container.AvgLookUpTime()},{container.LoadFactor()}";
            foreach(var val in list)
            {
                csvContent += $",{val}";
            }
            csvContent += "\n";
        }

        SaveToCSV("CSV_Files", "WithVariance", $"GenericParetoLoadFactor{lists[0].Sum()/2}_{lists[0].Length}tables.csv", csvContent);
    }

    static List<int[]> ReadCsvFile(string filePath, string[] selectedHeaders)
    {
        List<int[]> selectedLines = new List<int[]>();

        using (TextFieldParser parser = new TextFieldParser(filePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Read the headers
            string[] headers = parser.ReadFields();

            // Find the indices of the selected headers
            Dictionary<string, int> headerIndices = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                headerIndices[headers[i]] = i;
            }

            // Continue reading the file
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();

                // Filter lines based on selected headers
                bool includeLine = true;
                foreach (string selectedHeader in selectedHeaders)
                {
                    if (!headerIndices.ContainsKey(selectedHeader))
                    {
                        Console.WriteLine($"Header '{selectedHeader}' not found in the file.");
                        includeLine = false;
                        break;
                    }
                }

                if (includeLine)
                {
                    int[] intFields = new int[fields.Length - 2];
                    for (int i = 0; i < fields.Length - 2; i++)
                    {
                        intFields[i] = int.Parse(fields[i+2]);
                    }
                    selectedLines.Add(intFields);
                }                      
            }
        }

        return selectedLines;
    }

    // Virker kun når alle lister har samme længde
    static void ReduceNoiseLoadFactor(List<int[]> listOfParameters){
        List<(double avgLookUp, double LoadFactor)> medians = new List<(double avgLookUp, double LoadFactor)>();
        foreach (var parameters in listOfParameters)
        {
            List<(double avgLookUp, double LoadFactor)> results = new List<(double avgLookUp, double LoadFactor)>();
            for (int i = 0; i < 11; i++)
            {
                GenericContainer container = new GenericContainer(parameters.Length, parameters.ToList());
                for (int j = 0; j < parameters[0] * 2; j++)
                {
                    if(!container.Insert(j)) { break; }
                }
                results.Add((container.AvgLookUpTime(), container.LoadFactor()));
            }
            results.OrderBy(x => x.LoadFactor).ToList();
            medians.Add(results[5]);
        }
        string csvContent = "AverageLookUp,LoadFactor";
        for (int i = 1; i <= listOfParameters[0].Length; i++)
        {
            csvContent += $",TableSize{i}";
        }
        csvContent += "\n";

        foreach (var pair in medians)
        {
            csvContent += $"{pair.avgLookUp},{pair.LoadFactor}";
            foreach(var val in listOfParameters[medians.IndexOf(pair)])
            {
                csvContent += $",{val}";
            }
            csvContent += "\n";
        }

        SaveToCSV("CSV_Files", "ReduceNoise", $"GenericLoadFactorReducedNoise{listOfParameters[0].Sum()/2}_{listOfParameters[0].Length}tables.csv", csvContent);
    }
}
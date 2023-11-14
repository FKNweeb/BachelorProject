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
        // GenerateTablesHowFilled(keyCounts, 2);
        // GenerateTablesHowFilled(keyCounts, 3);
        // GenerateTablesHowFilled(keyCounts, 4);
        // GenerateTablesHowFilled(keyCounts, 5);
        // GenerateTablesHowFilled(keyCounts, 6);
        // GenerateTablesHowFilled(keyCounts, 7);
        // GenerateTablesHowFilled(keyCounts, 8);
        // GenerateTablesHowFilled(keyCounts, 9);
        // GenerateTablesHowFilled(keyCounts, 10);
        
        string csvFilePath = "Csv_Files/Pareto/GenericParetoData_2tables.csv";
        string[] selectedHeaders = { "TableSize1", "TableSize2" };
        List<int[]> selectedLines = ReadCsvFile(csvFilePath, selectedHeaders);

        GenerateAroundParetoValue(selectedLines);
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

        // SaveToCSV("CSV_Files", $"attempts_{tableCount}tables.csv", csvContent);
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

        // SaveToCSV("CSV_Files", $"averageLookup_{tableCount}tables.csv", csvContent);
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


        // SaveToCSV("CSV_Files", $"Generic_3tables.csv", csvContent);
    }

    // static void Generate3TablesHowFilled(List<int> keyCounts) {
    //     List<List<int>> lists = new List<List<int>>();
    //     foreach (var keyCount in keyCounts)
    //     {
    //         for (int j = 0; j < keyCount/2; j += keyCount / 16)
    //         {
    //             lists.Add(new List<int>{keyCount, keyCount - (keyCount/2) + j, keyCount - (keyCount / 2) - j});
    //         }
    //     }

    //     string csvContent = "AverageLookup,HowFilled\n";
        
    //     foreach (var list in lists)
    //     {
    //         GenericContainer container = new GenericContainer(3, list);
    //         for (int i = 0; i < list[0] * 2; i++)
    //         {
    //             container.Insert(i);
    //         }
    //         csvContent += $"{container.AvgLookUpTime()},{container.HowFilled()}\n";
    //     }


    //     SaveToCSV("CSV_Files", $"Generic_3tablesHowFilled.csv", csvContent);
    // }

    static void GenerateTablesHowFilled(List<int> keyCounts, int numOfTables) {
        List<List<int>> lists = new List<List<int>>();
        foreach (var keyCount in keyCounts)
        {
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
        }

        string csvContent = "AverageLookUp,HowFilled";
        for (int i = 1; i <= numOfTables; i++)
        {
            csvContent += $",TableSize{i}";
        }
        csvContent += "\n";
        
        int counter = 1;
        foreach (var list in lists)
        {
            list.Sort();
            list.Reverse();
            GenericContainer container = new GenericContainer(numOfTables, list);
            for (int i = 0; i < list[0] * 2; i++)
            {
                if(!container.Insert(i)) { break; }
            }
            csvContent += $"{container.AvgLookUpTime()},{container.HowFilled()}";
            foreach(var val in list)
            {
                csvContent += $",{val}";
            }
            csvContent += "\n";
            Console.WriteLine($"List number: {counter} out of {lists.Count} generating {numOfTables} tables");
            counter++;
        }

        SaveToCSV("CSV_Files", "HowFilled", $"GenericHowFilled_{numOfTables}tables.csv", csvContent);
    }


    static void GenerateAroundParetoValue(List<int[]> lists){

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

        string csvContent = "AverageLookUp,HowFilled";
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
            csvContent += $"{container.AvgLookUpTime()},{container.HowFilled()}";
            foreach(var val in list)
            {
                csvContent += $",{val}";
            }
            csvContent += "\n";
        }

        SaveToCSV("CSV_Files", "Pareto", $"GenericParetoHowFilled_{lists[0].Length}tables.csv", csvContent);
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
}
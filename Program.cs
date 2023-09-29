using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchMarker;
using CuckooHashTable;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

class Program
{
    static void Main(string[] args)
    {
        // BenchMarks Run First
        //var config = BenchmarkDotNet.Configs.DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
        //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

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

        // After collecting data
        var model = new PlotModel { Title = "Attempts vs KeyCount" };
        var series = new LineSeries { MarkerType = MarkerType.Circle };

        foreach (var entry in results)
        {
            series.Points.Add(new DataPoint(entry.Key, entry.Value.Average())); // Assuming you want average attempts for each key count
        }

        model.Series.Add(series);
        var view = new PlotView { Model = model };
        view.Dock = DockStyle.Fill;

        Form form = new Form();
        form.Controls.Add(view);
        Application.Run(form);
    }
}
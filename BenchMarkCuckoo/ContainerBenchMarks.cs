using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace CuckooHashTable.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Throughput)]
    [IterationCount(10)]
    public class ContainerBenchmarks
    {
        private Container _container;

        // Removed LoadFactor parameter.

        [Params(2, 3, 4, 5, 6, 7, 8, 9, 10)]
        public int TableCount;

        [Params(1000, 5000, 10000, 20000, 50000, 100000)]
        public int NumberOfKeys;

        [GlobalSetup]
        public void Setup()
        {
            int tableSize = NumberOfKeys / TableCount;

            _container = new Container(TableCount, tableSize);

            // Try to insert as many keys as possible.
            for (int i = 0; i < NumberOfKeys; i++)
            {
                _container.Insert(i);
            }
        }

        [Benchmark]
        public void InsertBenchmark()
        {
            for (int i = NumberOfKeys; i < NumberOfKeys + 1; i++)
            {
                _container.Insert(i);
            }
        }

        [Benchmark]
        public void ContainsBenchmark()
        {
            for (int i = 0; i < (0.01 * NumberOfKeys); i++)
            {
                _container.Contains(i);
            }
        }
    }
}
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

        [Params(10, 20, 30, 40, 50, 60, 70, 80, 90)]
        public int LoadFactor;

        [Params(2, 3, 4, 5, 6, 7, 8, 9, 10)]
        public int TableCount;

        [Params(1000, 5000, 10000, 20000, 50000, 100000)]
        public int NumberOfKeys;

        public int TotalElements => (LoadFactor * TableCount * NumberOfKeys) / 100;

        [GlobalSetup]
        public void Setup()
        {
            int tableSize = NumberOfKeys / TableCount;

            _container = new Container(TableCount, tableSize);
            for (int i = 0; i < TotalElements; i++)
            {
                _container.Insert(i);
            }
        }

        [Benchmark]
        public void InsertBenchmark()
        {
            for (int i = TotalElements; i < TotalElements + 1; i++)
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
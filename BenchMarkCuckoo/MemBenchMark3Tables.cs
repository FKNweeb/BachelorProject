using CuckooHashTable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


namespace BenchMarker {
    [MemoryDiagnoser]
    public class BenchMark3Tables
    {
        private Container container;
        [IterationSetup]
        public void Setup()
        {
            container = new Container(3, 1000);
        }

        [Benchmark]
        public void BenchmarkInsert_MultipleKeys()
        {
            for (int i = 0; i < 1000; i++)
            {
                container.Insert(i);
            }
        }
    }
}
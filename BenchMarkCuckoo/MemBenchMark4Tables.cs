using CuckooHashTable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


namespace BenchMarker {
    [MemoryDiagnoser]
    public class BenchMark4Tables
    {
        private Container container;
        [IterationSetup]
        public void Setup()
        {
            container = new Container(4, 1000);
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
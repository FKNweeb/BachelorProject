using CuckooHashTable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchMarker {
    [MemoryDiagnoser]
    public class BenchMark2Tables
    {
        private Container container;

        [IterationSetup]
        public void Setup()
        {
            container = new Container(2, 1000);
        }

        [Benchmark]
        public void BenchmarkInsert_MultipleKeys()
        {
            for (int i = 1; i < 1000; i++)
            {
                container.Insert(i);
            }
        }
    }
}

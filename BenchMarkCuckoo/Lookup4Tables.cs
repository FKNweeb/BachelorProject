using CuckooHashTable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchMarker {
    [MemoryDiagnoser]
    public class Lookup4Tables
    {
        private Container container;

        [GlobalSetup]
        public void GlobalSetup()
        {
            container = new Container(4, 1000);
            for (int i = 0; i < 1000; i++)
            {
                container.Insert(i);
            }
        }

        [Benchmark]
        public void BenchmarkLookup_MultipleKeys()
        {
            for (int i = 0; i < 1000; i++)
            {
                container.Contains(i);
            }
        }
    }
}

using CuckooHashTable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


namespace BenchMarker {
    [MemoryDiagnoser]
    public class CuckooBenchMarker
    {
        private Container container;
        [GlobalSetup]
        public void GlobalSetup()
        {
            container = new Container(2, 1000);
        }

        [Benchmark]
        public void BenchmarkInsert_MultipleKeys()
        {
            for (int i = 0; i < 1000; i++)
            {
                container.Insert(i);
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            //Write your cleanup logic here

        }
    }
}
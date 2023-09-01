using HashTable2;

namespace CuckooHashTableTests
{
    [TestFixture]
    public class CuckooHashTableTests
    {
        [Test]
        public void InsertTest()
        {
            // Arrange
            CuckooHashTable<int> hashTable = new CuckooHashTable<int>();
            int[] input = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120 };

            // Act & Assert
            foreach (var item in input)
            {
                Assert.IsTrue(hashTable.Insert(item));
            }
        }

        [Test]
        public void RehashTest()
        {
            // Arrange
            CuckooHashTable<int> hashTable = new CuckooHashTable<int>();
            int[] input = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };

            // Act & Assert
            foreach (var item in input)
            {
                Assert.IsTrue(hashTable.Insert(item));
            }
        }
    }
}
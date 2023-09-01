using System;
using Tables;
namespace tests;

public class CuckooHashTableTests
{
    [Test]
    public void CanAddAndCheckItems()
    {
        // Arrange
        var cuckooTable = new DefaultHashTable<int>();

        // Act
        cuckooTable.PrintTable();
        cuckooTable.Add(42);
        cuckooTable.PrintTable();

        // Assert
        Assert.True(cuckooTable.Contains(42));

    }

    [Test]
    public void CanClearTable()
    {
        // Arrange
        var cuckooTable = new DefaultHashTable<int>();
        cuckooTable.Add(1);
        cuckooTable.Add(2);

        // Act
        cuckooTable.Clear();

        // Assert
        Assert.False(cuckooTable.Contains(1));
        Assert.False(cuckooTable.Contains(2));
    }

    [Test]
    public void HandlesCollisionsAndResizing()
    {
        // Arrange
        var cuckooTable = new DefaultHashTable<int>(5); // Smaller initial capacity for testing

        // Act
        cuckooTable.Add(42);
        cuckooTable.Add(15);
        cuckooTable.Add(7);
        cuckooTable.Add(99); // This will trigger resizing and rehashing

        // Assert
        Assert.True(cuckooTable.Contains(42));
        Assert.True(cuckooTable.Contains(15));
        Assert.True(cuckooTable.Contains(7));
        Assert.True(cuckooTable.Contains(99));
    }
}

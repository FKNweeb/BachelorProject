using System;
using System.ComponentModel.DataAnnotations;
using CuckooHashTable;
namespace tests;

public class TestContainer
{
    /* 
    * -----------------------------------------------------------------------------
    * Tests for Insert
    * -----------------------------------------------------------------------------
    */
    [Test]
    public void TestInsertAndContains_SingleKey()
    {
        // Arrange
        var container = new Container(2, 1000);

        // Act
        container.Insert(42);

        // Assert
        Assert.True(container.Contains(42));
    }
    [Test]
    public void TestInsertAndContains_MultipleKeys()
    {
        // Arrange
        var cuckooTable = new Container(2, 1000);

        // Act
        for (int i = 0; i < 1000; i++)
        {
            cuckooTable.Insert(i);
        }

        // Assert
        Assert.True(cuckooTable.Contains(100));
        Assert.False(cuckooTable.Contains(1001));
    }

    [Test]
    public void TestInsert_MaxAttemptsReached() {
        // Setup
        var container = new Container(1, 1);
        int firstKey = 42;
        int secondKey = 12;

        // Act
        container.Insert(firstKey);
        container.Insert(secondKey);

        Assert.Throws<ArgumentException>(() => container.Insert(secondKey));
    }

}
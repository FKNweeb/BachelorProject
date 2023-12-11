using System;
using System.ComponentModel.DataAnnotations;
using CuckooHashTable;
namespace tests;

public class TestContainer
{
    /* 
    * -----------------------------------------------------------------------------
    * Tests for Insert, m = 100.000
    * -----------------------------------------------------------------------------
    */
    [Test]
    public void TestInsertAndContains_SingleKey()
    {
        // Arrange
        var container = new Container(2, 50000);

        // Act
        container.Insert(42);

        // Assert
        Assert.True(container.Contains(42));
    }

    [Test]
    public void TestInsertAndContains_MultipleKeys()
    {
        // Arrange
        var cuckooTable = new Container(2, 50000);
        

        // Act
        for (int i = 0; i < 50001; i++)
        {
            if (cuckooTable.Insert(i) == false){
                Assert.GreaterOrEqual(i, 25000);
                break;
            }
        }

        // Assert
        Assert.True(cuckooTable.Contains(100));
        Assert.False(cuckooTable.Contains(50001));
        Assert.GreaterOrEqual(cuckooTable.LoadFactor(), 0.48);
    }

    [Test]
    public void TestInsert_MaxAttemptsReached() {
        // Setup
        var container = new Container(1, 1);
        int firstKey = 42;
        int secondKey = 12;

        // Act
        Assert.True(container.Insert(firstKey));
        Assert.False(container.Insert(secondKey));
    }

}
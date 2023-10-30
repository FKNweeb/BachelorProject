using System;
using System.Collections.Generic;

namespace CuckooHashTable
{
    public class HashTable
    {
        public int[] _table;
        
        public HashTable(int size) {
            _table = new int[size];
        }

        public void PrintTable()
        {
            Console.WriteLine("Table 1:");
            for (int i = 0; i < _table.Count(); i++)
            {
                Console.WriteLine($"Position {i}: {_table[i]}");
            }
        }
    }
}
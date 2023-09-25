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
        
        //private int Hash(int key) {
        //    return key % _table.Count();
        //}

        //public void Insert(int key) {
        //    _table[Hash(key)] = key;
        //}

        public void Insert(int key) { 
            _table[key] = key;
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
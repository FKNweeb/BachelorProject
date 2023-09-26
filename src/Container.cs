using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace CuckooHashTable {
    public class Container
    {
        public HashTable[] _tables;
        private int _numTables;
        private int _numElements;
        private int MaxAttempts;
        // Seed
        private Random _random = new Random();
        // Dictionary {1....r1}
        private Dictionary<int, List<int>> _keyDictionary = new Dictionary<int, List<int>>();

        public Container (int numTables, int numElements) {
            this._numTables = numTables;
            this._numElements = numElements;
            this.MaxAttempts = numElements;

            _tables = new HashTable[numTables];
            for (int i = 0; i < numTables; i++)
            {
                _tables[i] = new HashTable(_numElements);
            }
        }

        public void Insert(int key) {
            if (!_keyDictionary.ContainsKey(key)) {
                _keyDictionary.Add(key, ListOfRandomNumbers());
            }

            for (int i = 0; i < MaxAttempts; i++)
            {
                int chosenTable = GetRandomTable();
                var randomTable = _tables[chosenTable];
                var pos = _keyDictionary[key][chosenTable];
                // Stop condition
                if(randomTable._table[pos] == 0) {
                    randomTable._table[pos] = key;
                    return;
                }
                int displacedItem = randomTable._table[pos];
                randomTable._table[pos] = key;
                key = displacedItem;
            }
            RehashElement(key);
        }


/*
        public void Insert(int key) {
            if (!_keyDictionary.ContainsKey(key)) {
                _keyDictionary.Add(key, ListOfRandomNumbers());
            }

            int chosenTable = 0;
            for (int i = 0; i < MaxAttempts; i++)
            {
                int pos = _keyDictionary[key][chosenTable];
                if(_tables[chosenTable]._table[pos] == 0) {
                    _tables[chosenTable]._table[pos] = key;
                    Console.WriteLine("Success");
                    return;
                }
                int displacedItem = _tables[chosenTable]._table[pos];
                _tables[chosenTable]._table[pos] = key;
                key = displacedItem;
                if(chosenTable == 0) { chosenTable++; }
                else { chosenTable--; }
            }
            RehashElement(key);
        }
*/
        private int GetRandomTable() {
            return _random.Next(_numTables);
        }

        private List<int> ListOfRandomNumbers() {
            List<int> result = new List<int>();

            for (int i = 0; i < _numTables; i++)
            {
                result.Add(_random.Next(_numElements));
            }
            return result;
        }

        private void RehashElement(int key) {
            Console.WriteLine($"Couldn't fit {key}");
        }

        public bool Contains(int key)
        { 
            return _keyDictionary.ContainsKey(key);
        }
    }
}
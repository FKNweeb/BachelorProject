using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace CuckooHashTable {
    public class Container
    {
        private HashTable[] _tables;
        private int _numTables;
        private int _numElements;
        private int MaxAttempts;
        // Seed
        private Random _random = new Random(5);
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
            _keyDictionary.Add(key, ListOfRandomNumbers());

            for (int i = 0; i < MaxAttempts; i++)
            {
                int chosenTable = GetRandomTable();
                var randomTable = _tables[chosenTable];
                var pos = _keyDictionary[key][chosenTable];
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
            Console.WriteLine("Couldn't fit key ", key);
        }

        public bool Contains(int key)
        { 
            return _keyDictionary.ContainsKey(key);
        }
    }
}
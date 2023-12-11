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
        public int LastAttemptCount { get ; private set; }

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

        public bool Insert(int key) {
            LastAttemptCount = 0;
            if (!_keyDictionary.ContainsKey(key)) {
                _keyDictionary.Add(key, ListOfRandomNumbers());
            }

            for (int i = 0; i < MaxAttempts; i++)
            {
                LastAttemptCount++;
                int chosenTable = GetRandomTable();
                var randomTable = _tables[chosenTable];
                var pos = _keyDictionary[key][chosenTable];
                // Stop condition
                if(randomTable._table[pos] == 0) {
                    randomTable._table[pos] = key;
                    return true;
                }
                int displacedItem = randomTable._table[pos];
                randomTable._table[pos] = key;
                key = displacedItem;
            }
            return false;
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

        public bool Contains(int key)
        { 
            for (int i = 0; i < _numTables; i++)
            {
                try
                {
                    if (_tables[i]._table[_keyDictionary[key][i]] == key) { return true; } 
                }
                catch (System.Exception) { return false; }
            }
            return false;
        }

        public double LoadFactor() {
            int storage = 0;

            for (int i = 0; i < _numTables; i++)
            {
                for (int j = 0; j < _numElements; j++)
                {
                    if (_tables[i]._table[j] != 0){
                        storage++;
                    }

                }
            }
            return (double)storage/(_numTables * _numElements); 
        }

        public double AvgLookUpTime() {
            double sum = 0.0;
            int n = 0;
            for (int i = 0; i < _numTables; i++)
            {
                int partSum = 0;
                for (int j = 0; j < _numElements; j++)
                {
                    if (_tables[i]._table[j] != 0){
                        partSum++;
                        n++;
                    }
                }
                sum += (i + 1) * partSum;
            }
            return sum / n;
        }
    }
}
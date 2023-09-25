using System;
/* 
namespace CuckooHashTable {
    public class CuckooHashTableContainer 
    {
        private List<HashTable> _tables = new List<HashTable>();
        private int _numTables;
        private int _numElements;

        public CuckooHashTableContainer (int numTables, int numElements) {
            this._numTables = numTables;
            this._numElements = numElements;

            for (int i = numTables; i > 0; i--)
            {
                _tables.Append(new HashTable(_numElements * i));
            }
        }

        public void Insert(int key) {
            if (Lookup(key)) {
                return;
            }
            for (int i = 0; i < _numTables; i++)
            {
                if(!_tables[i].Contains(key)) {
                    _tables[i].Insert(key);
                    return;
                }
                if(NextTableHasSpace(i, key)) { 
                    _tables[i + 1].Insert(key);
                    return;
                }
                int displacedItem = _tables[i]._table[key];
                _tables[i]._table[key] = key;
                key = displacedItem;
                // If true, rehash
                if (i < (_numTables - 1) && _tables[i + 1] != null)
                {
                    RehashElement();
                }
            }
        }

        public bool NextTableHasSpace(int pos, int key) {
            // If the index isn't null and the table contains our key, return false, else true
            if(_tables[pos + 1] != null && _tables[pos + 1].Contains(key)) { return false; }
            return true;
        }

        public bool Lookup(int key) {
            for (int i = 0; i < _numTables; i++) {
                if (_tables[i].Contains(key)) {
                    return true;
                }
            }
            return false;
        }

        private void RehashElement() {
            _numTables *= 2;

            
        }

    }
} */
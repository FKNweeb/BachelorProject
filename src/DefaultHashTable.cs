using System;

namespace Tables {
    public class DefaultHashTable<T> where T : struct
    {
        private const int DefaultCapacity = 10;
        private const int MaxAttempts = 10;

        private object[] table1;
        private object[] table2;
        private int[] hashes1;
        private int[] hashes2;
        private int capacity;

        public DefaultHashTable()
        {
            capacity = DefaultCapacity;
            InitializeTables();
        }

        public DefaultHashTable(int initialCapacity)
        {
            capacity = initialCapacity;
            InitializeTables();
        }

        public void Add(T item)
        {
            if (Contains(item))
            {
                throw new ArgumentException("Item already exists in the hash table.");
            }

            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                int hash1 = ComputeHash1(item);
                if (table1[hash1] is null)
                {
                    table1[hash1] = item;
                    hashes1[hash1] = hash1;
                    return;
                }

                T displacedItem = (T)table1[hash1];
                table1[hash1] = item;
                item = displacedItem;

                int hash2 = ComputeHash2(item);
                if (table2[hash2] is null)
                {
                    table2[hash2] = item;
                    hashes2[hash2] = hash2;
                    return;
                }

                displacedItem = (T)table2[hash2];
                table2[hash2] = item;
                item = displacedItem;
            }

            Rehash();
            Add(item);
        }

        public bool Contains(T item)
        {
            int hash1 = ComputeHash1(item);
            int hash2 = ComputeHash2(item);

            return (table1[hash1]?.Equals(item) ?? false) || 
                   (table2[hash2]?.Equals(item) ?? false);
        }

        public void Clear()
        {
            InitializeTables();
        }

        private void InitializeTables()
        {
        table1 = new object[capacity];
        table2 = new object[capacity];
        hashes1 = new int[capacity];
        hashes2 = new int[capacity];
        }

        private void Rehash()
        {
            capacity *= 2;
            object[] oldTable1 = table1;
            object[] oldTable2 = table2;
            int[] oldHashes1 = hashes1;
            int[] oldHashes2 = hashes2;

            InitializeTables();

            for (int i = 0; i < oldTable1.Length; i++)
            {
                if (oldTable1[i] is not null)
                {
                    Add((T)oldTable1[i]);
                }

                if (oldTable2[i] is not null)
                {
                    Add((T)oldTable2[i]);
                }
            }
        }

        private int ComputeHash1(T item)
        {
            int hashCode = item.GetHashCode();
            int hash1 = hashCode % capacity;
            return Math.Abs(hash1);
        }

        private int ComputeHash2(T item)
        {
            int hashCode = item.GetHashCode();
            int hash2 = (hashCode / capacity) % capacity;
            return Math.Abs(hash2);
        }

        public void PrintTable()
        {
        Console.WriteLine("Table 1:");
        for (int i = 0; i < capacity; i++)
        {
            Console.WriteLine($"Position {i}: {table1[i]}");
        }

        Console.WriteLine("Table 2:");
        for (int i = 0; i < capacity; i++)
        {
            Console.WriteLine($"Position {i}: {table2[i]}");
        }
        }
    }
}

using System;

    namespace HashTable2{
    public class CuckooHashTable<T>
    {
        private T[] table1;
        private T[] table2;
        private int size;
        private const int defaultSize = 11;

        public CuckooHashTable()
        {
            table1 = new T[defaultSize];
            table2 = new T[defaultSize];
            size = defaultSize;
        }

        private int Hash1(T key)
        {
            return key.GetHashCode() % size;
        }

        private int Hash2(T key)
        {
            return (key.GetHashCode() / size) % size;
        }

        public bool Insert(T key)
        {
            for (int cycle = 0; cycle < size; cycle++)
            {
                int pos1 = Hash1(key);
                if (table1[pos1] == null || table1[pos1].Equals(default(T)))
                {
                    table1[pos1] = key;
                    return true;
                }

                // Swap key and table1[pos1]
                T temp = table1[pos1];
                table1[pos1] = key;
                key = temp;

                int pos2 = Hash2(key);
                if (table2[pos2] == null || table2[pos2].Equals(default(T)))
                {
                    table2[pos2] = key;
                    return true;
                }

                // Swap key and table2[pos2]
                temp = table2[pos2];
                table2[pos2] = key;
                key = temp;
            }

            // Rehash or resize if cannot insert
            Rehash();
            return Insert(key);
        }

        private void Rehash()
        {
            Console.WriteLine("Rehashing...");

            T[] oldTable1 = table1;
            T[] oldTable2 = table2;
            
            size = size * 2 + 1;  // Making it an odd number
            table1 = new T[size];
            table2 = new T[size];

            // Insert old values into new tables
            for (int i = 0; i < oldTable1.Length; i++)
            {
                if (oldTable1[i] != null && !oldTable1[i].Equals(default(T)))
                {
                    Insert(oldTable1[i]);
                }
            }

            for (int i = 0; i < oldTable2.Length; i++)
            {
                if (oldTable2[i] != null && !oldTable2[i].Equals(default(T)))
                {
                    Insert(oldTable2[i]);
                }
            }
        }

        public void PrintTable()
        {
            Console.WriteLine("Table 1:");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine($"Position {i}: {table1[i]}");
            }

            Console.WriteLine("Table 2:");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine($"Position {i}: {table2[i]}");
            }
        }
    }
}
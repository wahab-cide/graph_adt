#nullable disable

using System.Diagnostics;

/**
 * A hash table implementation of the IMap<K,V> ADT that uses open addressing
 * with linear probing.
 */
public class HashTable<K, V> : IMap<K, V>
{
    protected static readonly int DEFCAPACITY = 997; // a large prime number
    protected static readonly double LOADFACTOR = 0.7;
    protected Entry[] data;
    protected int elementCount; // # of actually-stored elements
    protected int occupiedCount; // # of occupied entries


    /**
     * Initializes a HashTable with the given initial capacity.
     */
    public HashTable(int initialCapacity)
    {
        Debug.Assert(initialCapacity > 0, "HashTable capacity must be a positive number.");
        data = new Entry[initialCapacity];
    }

    /**
     * Initializes a hash table with a default initial capabity.
     */
    public HashTable() : this(DEFCAPACITY) { }

    /**
     * Returns the number of entries in the map.
     *
     * @return An integer.
     */
    public int Size()
    {
        return elementCount;
    }

    /**
     * Returns true if and only if this map does not contain any entries.
     *
     * @return True if empty, false otherwise.
     */
    public bool IsEmpty
    {
        get
        {
            return elementCount == 0;
        }
    }

    /**
     * Locates the index of a key.  Behavior differs depending on
     * the value of `is_insertion`.
     *
     * When `is_insertion` is true, returns the location to insert an
     * entry with the given `key`, even if an entry for that key
     * already exists.
     *
     * When `is_insertion` is false, returns the location of the location
     * with the given `key`.  If the entry with the given `key` is not
     * present, returns -1.
     *
     * @return The index for the given key.
     */
    private static int Locate<T>(Entry[] data, T key, bool is_insertion)
    {
        int idx = (key.GetHashCode() & 0x7FFFFFFF) % data.Length;
        int firstRemoved = -1;

        while (true)
        {
            var e = data[idx];
            if (e == null)
            {
                // this only happens when the key is not present.
                // on insert, reuse a removed cell if we've encountered one, otherwise
                // return the current index;
                // on non-insert, this means that the key is not present, so return -1
                return is_insertion ? (firstRemoved != -1 ? firstRemoved : idx) : -1;
            }
            if (e.IsRemoved)
            {
                // when we encounter a removed entry, we remember it, because we'd
                // prefer to use this one; but we first need to ensure that the
                // table does not already contain the key later in our linear probe,
                // so keep scanning until we are convinced that it is not here.
                if (firstRemoved == -1)
                {
                    firstRemoved = idx;
                }
            }
            else if (e.Key.Equals(key))
            {
                // on insert, if we find the key, then we want to overwise it, so return idx
                // on non-insert, if we find the key, then we want to access it, so return idx
                return idx;
            }

            // probe linearly
            idx = (idx + 1) % data.Length;
        }
    }

    /**
     * Returns true if the map contains an entry for the given `key`.
     *
     * @return True if found, false otherwise.
     */
    public bool ContainsKey(K key)
    {
        return Locate(data, key, false) != -1;
    }

    /**
     * Returns the value of type V if the given `key` is in the map,
     * otherwise throws `MapDoesNotContainKey`.
     *
     * @return A value.
     */
    public V Get(K key)
    {
        int idx = Locate(data, key, false);
        if (idx == -1)
        {
            throw new MapDoesNotContainKey();
        }
        else
        {
            return data[idx].Value;
        }
    }

    /**
     * Inserts an entry for the given `key` and `value`.  If the given
     * `key` is already present in the map, the old entry is overwritten
     * with the new one.
     */
    public void Put(K key, V value)
    {
        int idx = Locate(data, key, true);
        Entry e = data[idx];
        if (e == null)
        {
            elementCount++;
            occupiedCount++;
        }
        else if (e.IsRemoved)
        {
            elementCount++;
        }
        data[idx] = new Entry(key, value);

        // if we've exceeded the load factor, expand
        if ((double)occupiedCount / data.Length >= LOADFACTOR)
        {
            Expand();
        }
    }

    /**
     * Expand the hash table, rehashing elements and discarding
     * deleted element placeholders.
     */
    public void Expand()
    {
        Entry[] data2 = new Entry[data.Length * 2];
        int count = 0;
        for (int i = 0; i < data.Length; i++)
        {
            // re-hash old entries before copying
            Entry e = data[i];
            if (e != null && !e.IsRemoved)
            {
                int idx = Locate<K>(data2, e.Key, true);
                data2[idx] = e;
                count++;
            }
        }
        // update the counts since we no longer have placeholders
        elementCount = count;
        occupiedCount = count;
        data = data2;
    }

    /**
     * Removes the entry for the given `key`.  If the `key` is not
     * present, throws `MapDoesNotContainKey`.
     */
    public void Remove(K key)
    {
        int idx = Locate(data, key, false);
        if (idx == -1)
        {
            throw new MapDoesNotContainKey();
        }
        data[idx].Remove();
        // we decrease the element count, but not the
        // occupied count since we did not actually
        // remove anything; removal happens on expansion
        elementCount--;
    }

    /**
     * Removes all entries in the map.
     */
    public void Clear()
    {
        data = new Entry[DEFCAPACITY];
        elementCount = 0;
        occupiedCount = 0;
    }

    /**
     * Returns all of the keys in the map.  Keys may be returned in
     * any order.
     *
     * @return a vector of keys.
     */
    public IList<K> Keys {
        get
        {
            Vector<K> keys = new Vector<K>();
            for (int i = 0; i < data.Length; i++)
            {
                var e = data[i];
                if (e != null && !e.IsRemoved)
                {
                    keys.Append(e.Key);
                }
            }
            Debug.Assert(keys.Size() == elementCount, "Vector should contain elementCount keys.");
            return keys;
        }
    }

    /**
     * Returns all of the values in the map.  Values may be returned in
     * any order.
     *
     * @return a vector of values.
     */
    public IList<V> Values
    {
        get
        {
            Vector<V> values = new Vector<V>();
            for (int i = 0; i < data.Length; i++)
            {
                var e = data[i];
                if (e != null && !e.IsRemoved)
                {
                    values.Append(e.Value);
                }
            }
            Debug.Assert(values.Size() == elementCount, "Vector should contain elementCount values.");
            return values;
        }
    }

    /**
     * A variation on the `Pair<K,V>` class that also allows us to represent
     * removed entries.
     */
    public class Entry
    {
        protected readonly K key;
        protected readonly V value;
        protected bool isRemoved = false;

        public Entry(K key, V value)
        {
            this.key = key;
            this.value = value;
        }

        public K Key
        {
            get
            {
                return key;
            }
        }

        public V Value
        {
            get
            {
                return value;
            }
        }

        public bool IsRemoved
        {
            get
            {
                return isRemoved;
            }
        }

        public V Remove()
        {
            Debug.Assert(isRemoved == false, $"Cannot remove entry for key '{key}' more than once.");
            isRemoved = true;
            return value;
        }

        public override bool Equals(object other)
        {
            if (other is not Entry s)
            {
                return false;
            }
            else
            {
                return key.Equals(s.Key);
            }
        }

        public override int GetHashCode()
        {
            return key.GetHashCode();
        }
    }
}


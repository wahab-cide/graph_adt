/**
 * An interface for the map (aka dictionary) abstract data type.
 * A map stores a value of type V for a unique key of type K.
 */
public interface IMap<K, V>
{
    /**
     * Returns the number of entries in the map.
     *
     * @return An integer.
     */
    public int Size();

    /**
     * Returns true if and only if this map does not contain any entries.
     *
     * @return True if empty, false otherwise.
     */
    public bool IsEmpty { get; }

    /**
     * Returns true if the map contains an entry for the given `key`.
     *
     * @return True if found, false otherwise.
     */
    public bool ContainsKey(K key);

    /**
     * Returns the value of type V if the given `key` is in the map,
     * otherwise throws `MapDoesNotContainKey`.
     *
     * @return A value.
     */
    public V Get(K key);

    /**
     * Inserts an entry for the given `key` and `value`.  If the given
     * `key` is already present in the map, the old entry is overwritten
     * with the new one.
     */
    public void Put(K key, V value);

    /**
     * Removes the entry for the given `key`.  If the `key` is not
     * present, throws `MapDoesNotContainKey`.
     */
    public void Remove(K key);

    /**
     * Removes all entries in the map.
     */
    public void Clear();

    /**
     * Returns all of the keys in the map.  Keys may be returned in
     * any order.
     *
     * @return a vector of keys.
     */
    public IList<K> Keys { get; }

    /**
     * Returns all of the values in the map.  Values may be returned in
     * any order.
     *
     * @return a vector of values.
     */
    public IList<V> Values { get; }
}

public class MapDoesNotContainKey : Exception
{
    public MapDoesNotContainKey() : base() { }
}

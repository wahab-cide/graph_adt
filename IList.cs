/**
 * Interface describing a list ADT. A list is a sequence
 * of data with a head. A value may be added or removed
 * from either end, as well as by-value from the middle.
 */
public interface IList<E> : IEnumerable<E> {
    /**
     * Determine size of list.
     *
     * @return The number of elements in list.
     */
    public int Size();

    /**
     * Insert value at location.
     * Throws ListIndexOutOfBounds when `i` is invalid.
     *
     * @param i index of this new value
     * @param value value to be stored
     */
    public void Insert(int i, E value);

    /**
     * Add a value to the head of the list.
     *
     * @param value The value to be added to the head of the list.
     */
    public void Prepend(E value);

    /**
     * Add a value to tail of list.
     *
     * @param value The value to be added to tail of list.
     */
    public void Append(E value);

    /**
     * Remove all elements of list.
     */
    public void Clear();

    /**
     * Fetch first element of list.
     * Throws `ListDoesNotContainValue` if the list is empty.
     *
     * @return A reference to first element of list.
     */
    public E Head();

    /**
     * Fetch last element of list.
     * Throws `ListDoesNotContainValue` if the list is empty.
     *
     * @return A reference to last element of list.
     */
    public E Last();

    /**
     * Determine first location of a value in list.
     * Throws `ListDoesNotContainValue` when the list does not
     * contain `value`.
     *
     * @param value The value sought.
     * @return index (0 is first element) of value.
     */
    public int IndexOf(E value);

    /**
     * Check to see if a value is in list.
     *
     * @param value value sought.
     * @return `true` if value is within list, otherwise `false`
     */
    public bool Contains(E value);

    /**
     * Determine if list is empty.
     *
     * @return True if list has no elements.
     */
    public bool IsEmpty();

    /**
     * Get value at location i.
     * Throws `ListIndexOutOfBounds` when `i` is invalid.
     *
     * @param i position of value to be retrieved.
     * @return value retrieved from location i.
     */
    public E Get(int i);

    /**
     * Set value stored at location i to object o, returning old value.
     * Throws `ListIndexOutOfBounds` when `i` is invalid.
     *
     * @param i location of entry to be changed.
     * @param value new value
     * @return former value of ith entry of list.
     */
    public E Set(int i, E value);

    /**
     * Remove and return value at location i.
     * Throws `ListIndexOutOfBounds` when `i` is invalid.
     *
     * @param i position of value to be retrieved.
     * @return value retrieved from location i.
     */
    public E RemoveAt(int i);

    /**
     * Remove a value from first element of list.
     * Throws `ListDoesNotContainValue` when the list is empty.
     *
     * @return The value actually removed.
     */
    public E RemoveFirst();

    /**
     * Remove last value from list.
     * Throws `ListDoesNotContainValue` when the list is empty.
     *
     * @return The value actually removed.
     */
    public E RemoveLast();

    /**
     * Remove a value from list. At most one of value
     * will be removed.
     * Throws `ListDoesNotContainValue` when the list does not
     * contain `value`.
     *
     * @param value The value to be removed.
     * @return The actual value removed.
     */
    public E Remove(E value);
}

public class ListDoesNotContainValue : Exception
{
    public ListDoesNotContainValue() : base() { }
}

public class ListIndexOutOfBounds : Exception
{
    public ListIndexOutOfBounds() : base() { }
}

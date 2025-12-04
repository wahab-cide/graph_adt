/**
 * An implementation of extensible arrays.
 * Inspired by Duane Bailey's structure.Vector class for Java,
 * but with numerous additional modifications (e.g., type safety),
 * as well as some small optimizations.
 */

#nullable disable

public class Vector<E> : IList<E>
{
    /*
     * Invariant:
     * [0..count) is filled with data.
     */
    protected E[] data;
    protected int count;
    protected readonly int capacityIncrement = 0; // 0 means "double"
    protected readonly static int DEFAULTCAPACITY = 10;

    /**
     * Initializes a Vector with a default capacity (of 10) which
     * doubles when more space is needed.
     */
    public Vector() : this(DEFAULTCAPACITY) { }

    /**
     * Initializes a Vector with the given `initialCapacity` which
     * doubles when more space is needed.
     */
    public Vector(int initialCapacity) : this(initialCapacity, 0) { }

    /**
     * Initializes a Vector with the given `initialCapacity` which
     * grows by `capacityIncr` when more space is needed.
     */
    public Vector(int initialCapacity, int capacityIncr)
    {
        data = new E[initialCapacity];
        capacityIncrement = capacityIncr;
    }


    /**
     * Makes a copy of the given `list`, setting the initial capacity
     * to the size of the given list.  This list doubles when more
     * space is needed.
     */
    public Vector(IList<E> list)
    {
        int sz = list.Size();
        data = new E[sz];
        for (int i = 0; i < sz; i++)
        {
            data[i] = list.Get(i);
        }
    }

    /**
     * A generic utility method that returns a new array of the
     * given length, where the first `arr.Length` values are
     * copied from the given `arr`.
     */
    protected static T[] Copy<T>(T[] arr, int newLength)
    {
        T[] arr2 = new T[newLength];
        for (int i = 0; i < arr.Length; i++)
        {
            arr2[i] = arr[i];
        }
        return arr2;
    }

    /**
     * Find the minimum array length that is at least
     * `minCapacity` using the user's stated `capacityIncrement`.
     *
     * @param minCapacity The minimum length.
     */
    protected int Grow(int minCapacity)
    {
        int newLength = data.Length; // initial guess
        if (capacityIncrement == 0)
        {
            // double until capacity is at least minCapacity
            if (newLength == 0)
            {
                newLength = 1;
            }
            while (newLength < minCapacity)
            {
                newLength *= 2;
            }
        }
        else
        {
            // increment using user's pameter until capacity
            // is at least minCapacity
            while (newLength < minCapacity)
            {
                newLength += capacityIncrement;
            }
        }
        return newLength;
    }

    /**
     * Ensure that the vector is capable of holding at least
     * minCapacity values without expansion.
     *
     * @param minCapacity The minimum size of array before expansion.
     */
    protected void EnsureCapacity(int minCapacity)
    {
        if (data.Length < minCapacity)
        {
            int newLength = Grow(minCapacity);
            data = Copy(data, newLength);
        }
    }

    /**
     * Explicitly set the size of the array, growing
     * if needed.
     *
     * @param newSize The ultimate size of the vector.
     */
    protected void SetSize(int newSize)
    {
        // this method does not erase any elements; it
        // just changes the count so that array locations
        // can be reused
        if (newSize >= count)
        {
            EnsureCapacity(newSize);
        }
        count = newSize;
    }

    /**
     * Determine size of the Vector.
     *
     * @return The number of elements in the Vector.
     */
    public int Size()
    {
        return count;
    }

    /**
     * Insert value at location.
     * Throws ListIndexOutOfBounds when `i` is invalid.
     *
     * @param i index of this new value
     * @param value value to be stored
     */
    public void Insert(int i, E value)
    {
        if (i < 0 || i > count)
        {
            throw new ListIndexOutOfBounds();
        }

        EnsureCapacity(count + 1);
        // copy from right to left to avoid destroying data
        for (int j = count; j > i; j--)
        {
            data[j] = data[j - 1];
        }
        data[i] = value;
        count++;
    }

    /**
     * Add a value to the front of the Vector.
     *
     * @param value The value to be added.
     */
    public void Prepend(E value)
    {
        Insert(0, value);
    }

    /**
    * Add a value to the end of the Vector, possibly expanding
    * to make space.
    *
    * @param value The value to be added.
    */
    public void Append(E value)
    {
        EnsureCapacity(count + 1);
        data[count] = value;
        count++;
    }

    /**
     * Remove all elements of list.
     */
    public void Clear()
    {
        SetSize(0);
    }

    /**
     * Fetch first element.
     * Throws `ListDoesNotContainValue` if the list is empty.
     *
     * @return The first element.
     */
    public E Head()
    {
        if (count == 0)
        {
            throw new ListDoesNotContainValue();
        }
        return data[0];
    }

    /**
     * Fetch last element.
     * Throws `ListDoesNotContainValue` if the list is empty.
     *
     * @return The last element.
     */
    public E Last()
    {
        if (count == 0)
        {
            throw new ListDoesNotContainValue();
        }
        return data[count - 1];
    }

    /**
     * Determine first location of the given value in the Vector.
     * Throws `ListDoesNotContainValue` when the Vector does not
     * contain `value`.
     *
     * @param value The value sought.
     * @return index of value.
     */
    public int IndexOf(E value)
    {
        for (int i = 0; i < count; i++)
        {
            if (data[i].Equals(value))
            {
                return i;
            }
        }
        throw new ListDoesNotContainValue();
    }

    /**
     * Check to see if a value is in the Vector.
     *
     * @param value The value sought.
     * @return `true` if value is within list, otherwise `false`
     */
    public bool Contains(E value)
    {
        try
        {
            IndexOf(value);
            return true;
        }
        catch (ListDoesNotContainValue)
        {
            return false;
        }
    }

    /**
     * Determine if list is empty.
     *
     * @return true if list has no elements.
     */
    public bool IsEmpty()
    {
        return count == 0;
    }

    /**
     * Get value at location i.
     * Throws `ListIndexOutOfBounds` when `i` is invalid.
     *
     * @param i position of value to be retrieved.
     * @return value retrieved from location i.
     */
    public E Get(int i)
    {
        if (i < 0 || i >= count)
        {
            throw new ListIndexOutOfBounds();
        }

        return data[i];
    }

    /**
     * Set value stored at location i to value, returning old value.
     * Throws `ListIndexOutOfBounds` when `i` is invalid.
     *
     * @param i location of entry to be changed.
     * @param value new value
     * @return former value of ith entry of list.
     */
    public E Set(int i, E value)
    {
        if (i < 0 || i >= count)
        {
            throw new ListIndexOutOfBounds();
        }

        E oldValue = data[i];
        data[i] = value;
        return oldValue;
    }

    /**
     * Remove and return value at location i.
     * Throws `ListIndexOutOfBounds` when `i` is invalid.
     *
     * @param i position of value to be retrieved.
     * @return value retrieved from location i.
     */
    public E RemoveAt(int i)
    {
        // fetch old element
        E result = data[i];

        // decrease count
        count--;

        // copy all of the elements to the right of i
        // one location to the left
        while (i < count)
        {
            data[i] = data[i + 1];
            i++;
        }

        return result;
    }

    /**
     * Remove the first element.
     * Throws `ListDoesNotContainValue` when the list is empty.
     *
     * @return The value actually removed.
     */
    public E RemoveFirst()
    {
        if (count == 0)
        {
            throw new ListDoesNotContainValue();
        }

        return RemoveAt(0);
    }

    /**
     * Remove last element.
     * Throws `ListDoesNotContainValue` when the list is empty.
     *
     * @return The value actually removed.
     */
    public E RemoveLast()
    {
        if (count == 0)
        {
            throw new ListDoesNotContainValue();
        }

        return RemoveAt(count - 1);
    }

    /**
     * Remove a value from list. At most one of value
     * will be removed.
     * Throws `ListDoesNotContainValue` when the list does not
     * contain `value`.
     *
     * @param value The value to be removed.
     * @return The actual value removed.
     */
    public E Remove(E value)
    {
        return RemoveAt(IndexOf(value));
    }

    /**
     * Trim the underlying array so that there is no
     * wasted space.
     */
    public void Trim()
    {
        E[] arr2 = new E[count];
        for (int i = 0; i < count; i++)
        {
            arr2[i] = data[i];
        }
        data = arr2;
    }

    /**
     * Returns a string representation of the Vector.
     *
     * @return A string.
     */
    public override String ToString()
    {
        var sb = new System.Text.StringBuilder();

        sb.Append("Vector[");
        var sz = Size();
        for (int i = 0; i < sz; i++)
        {
            sb.Append(Get(i));
            if (i != sz - 1)
            {
                sb.Append(", ");
            }
        }
        sb.Append("]");

        return sb.ToString();
    }

    public E[] Export()
    {
        return data;
    }

    public override bool Equals(object other)
    {
        if (other is not Vector<E> v)
        {
            return false;
        }

        int sz = Size();

        if (sz != v.Size())
        {
            return false;
        }
        // now compare element by element
        for (int i = 0; i < sz; i++)
        {
            if (!data[i].Equals(v.data[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return data.GetHashCode();
    }

    /**
     * Exposes the enumerator, which supports a simple iteration over a
     * collection of a specified type.
     *
     * @tparam T The type of element yielded by the enumerator.
     * @return An implementation of `IEnumerator<T>`.
     */
    public IEnumerator<E> GetEnumerator()
    {
        return new VectorEnumerator(this);
    }

    /**
     * Exposes a non-generic enumerator. Required by the interface, but only
     * useful for legacy code. Returns the generic enumerator, as it can be
     * used in a non-generic context.
     *
     * @return An implementation of `IEnumerator`.
     */
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public class VectorEnumerator : IEnumerator<E>
    {
        protected Vector<E> v;
        protected int i;

        public VectorEnumerator(Vector<E> v)
        {
            this.v = v;
            i = -1;
        }

        /**
         * Advances the enumerator to the next element of the collection.
         * An extending method must use the `override` keyword.
         *
         * @return true if and only if the enumerator returns a valid
         *         element.
         */
        public bool MoveNext()
        {
            i++;
            return i < v.Size();
        }

        /**
         * Sets the enumerator to its initial position, before the first
         * element in the collection.
         * An extending method must use the `override` keyword.
         */
        public void Reset()
        {
            i = -1;
        }

        /**
         * Gets the element in the collection at the current position of
         * the enumerator.
         * An extending property must use the `override` keyword.
         *
         * @return an element.
         */
        public E Current
        {
            get
            {
                return v.Get(i);
            }
        }

        /**
         * A non-generic getter that gets the element in the collection at
         * the current position of the enumerator. Required by the interface,
         * but only useful for legacy code.  This implementation simply
         * calls the generic `Current` getter, which can be used in a non-
         * generic context.
         * An extending method must use the `override` keyword.
         *
         * @return an element.
         */
        object System.Collections.IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /**
        * Performs application-defined tasks associated with freeing, releasing,
        * or resetting unmanaged resources. Required by the `IEnumerator<T>`
        * interface, which implements `IDisposable<T>`.  `AbstractEnumerator<T>`
        * provides a default implementation that does nothing.
        * An extending method must use the `override` keyword.
        */
        public void Dispose() { }
    }
}

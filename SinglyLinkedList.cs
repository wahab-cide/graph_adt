#nullable disable

public class SinglyLinkedList<E> : IList<E>
{
    private ListNode<E> start;

    public SinglyLinkedList()
    {
        start = null;
    }

    /**
     * Determine size of list.
     *
     * @return The number of elements in list.
     */
    public int Size()
    {
        int count = 0;
        ListNode<E> cur = start;
        while (cur != null)
        {
            cur = cur.tail;
            count++;
        }
        return count;
    }

    /**
     * Returns the ListNode of the element at the given index.
     * Throws `ListIndexOutOfBounds` when `index` is invalid.
     *
     * @param index
     * @return The node.
     */
    private ListNode<E> NodeAt(int index)
    {
        if (index < 0)
        {
            throw new ListIndexOutOfBounds();
        }
        ListNode<E> cur = start;
        for (int i = 0; i < index; i++)
        {
            if (cur == null)
            {
                throw new ListIndexOutOfBounds();
            }
            cur = cur.tail;
        }
        if (index != 0 && cur == null)
        {
            throw new ListIndexOutOfBounds();
        }
        return cur;
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
        // three cases:
        // 1. list is empty or i is the start of the list
        // 2. element is somewhere inside the list
        // 3. i is invalid
        if (i == 0)
        {
            ListNode<E> oldHead = start;
            start = new ListNode<E>(value);
            start.tail = oldHead;
        }
        else
        {
            // insert after the i-1th node
            ListNode<E> parent = NodeAt(i - 1);  // throws ListOutOfBounds if i does not make sense
            ListNode<E> node = new ListNode<E>(value);
            node.tail = parent.tail;
            parent.tail = node;
        }
    }

    /**
     * Add a value to the head of the list.
     *
     * @param value The value to be added to the head of the list.
     */
    public void Prepend(E value)
    {
        Insert(0, value);
    }

    /**
     * Add a value to tail of list.
     *
     * @param value The value to be added to tail of list.
     */
    public void Append(E value)
    {
        Insert(Size(), value);
    }

    /**
     * Remove all elements of list.
     */
    public void Clear()
    {
        start = null;
    }

    /**
     * Fetch first element of list.
     * Throws `ListDoesNotContainValue` if the list is empty.
     *
     * @return A reference to first element of list.
     */
    public E Head()
    {
        if (start == null)
        {
            throw new ListDoesNotContainValue();
        }
        else
        {
            return start.value;
        }
    }

    /**
     * Fetch last element of list.
     * Throws `ListDoesNotContainValue` if the list is empty.
     *
     * @return A reference to last element of list.
     */
    public E Last()
    {
        if (start == null)
        {
            throw new ListDoesNotContainValue();
        }
        else
        {
            return NodeAt(Size() - 1).value;
        }
    }

    /**
     * Determine first location of a value in list.
     * Throws `ListDoesNotContainValue` when the list does not
     * contain `value`.
     *
     * @param value The value sought.
     * @return index (0 is first element) of value.
     */
    public int IndexOf(E value)
    {
        int count = 0;
        ListNode<E> cur = start;
        while (cur != null)
        {
            if (cur.value.Equals(value))
            {
                return count;
            }
            cur = cur.tail;
            count++;
        }
        throw new ListDoesNotContainValue();
    }

    /**
     * Check to see if a value is in list.
     *
     * @param value value sought.
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
     * @return True if list has no elements.
     */
    public bool IsEmpty()
    {
        return start == null;
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
        return NodeAt(i).value;
    }

    /**
     * Set value stored at location i to object o, returning old value.
     * Throws ListIndexOutOfBounds when `i` is invalid.
     *
     * @param i location of entry to be changed.
     * @param value new value
     * @return former value of ith entry of list.
     */
    public E Set(int i, E value)
    {
        ListNode<E> node = NodeAt(i);
        E oldValue = node.value;
        node.value = value;
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
        if (i == 0 && start != null)
        {
            E value = start.value;
            start = start.tail;
            return value;
        }
        else
        {
            ListNode<E> parent = NodeAt(i - 1);
            if (parent.tail == null)
            {
                throw new ListIndexOutOfBounds();
            }
            E value = parent.tail.value;
            parent.tail = parent.tail.tail;
            return value;
        }
        throw new ListIndexOutOfBounds();
    }

    /**
     * Remove a value from first element of list.
     * Throws `ListDoesNotContainValue` when the list is empty.
     *
     * @return The value actually removed.
     */
    public E RemoveFirst()
    {
        return RemoveAt(0);
    }

    /**
     * Remove last value from list.
     * Throws ListDoesNotContainValue when the list is empty.
     *
     * @return The value actually removed.
     */
    public E RemoveLast()
    {
        return RemoveAt(Size() - 1);
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
        int loc = IndexOf(value);
        return RemoveAt(loc);
    }

    /**
     * Pretty-prints the linked list.
     *
     * @return a string representation of the list.
     */
    public override string ToString()
    {
        E[] xs = new E[Size()];
        ListNode<E> cur = start;
        int i = 0;
        while (cur != null)
        {
            xs[i] = cur.value;
            cur = cur.tail;
            i++;
        }
        return "[" + System.String.Join(", ", xs) + "]";
    }

    /**
     * Returns an enumerator so that a SinglyLinkedList<E>
     * is enumerable.
     *
     * @return An enumerator.
     */
    public IEnumerator<E> GetEnumerator()
    {
        return new SinglyLinkedListEnumerator<E>(start);
    }

    /**
     * Returns a non-generic enumerator so that a SinglyLinkedList<E>
     * is enumerable.
     *
     * @return An enumerator.
     */
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

/**
 * A class representing a single element in a linked list.
 *
 * Written 2023 Dan Barowy
 */
public class ListNode<E>
{
    public E value; // the value of the element
    public ListNode<E> tail; // a reference to the next node

    /**
     * Constructs a node with the given value.
     *
     * @param val An element value of type E.
     */
    public ListNode(E value)
    {
        this.value = value;
    }
}

public class SinglyLinkedListEnumerator<E> : IEnumerator<E>
{
    private readonly ListNode<E> start;
    private ListNode<E> cur;

    /**
     * Initialize this enumerator with the start of a
     * SinglyLinkedList<E>.
     */
    public SinglyLinkedListEnumerator(ListNode<E> start)
    {
        this.start = start;
        cur = null; // before-first position
    }

    /**
     * Gets the element in the collection at the current position of
     * the enumerator.
     *
     * @return The element in the collection at the current position of
     * the enumerator.
     */
    public E Current
    {
        get
        {
            if (cur == null)
                throw new InvalidOperationException("Enumeration has not started or has already finished.");
            return cur.value;
        }
    }

    /**
     * Gets the element in the collection at the current position of
     * the enumerator. This is to allow an `IEnumerator<E>` to be
     * used in a non-generic context.
     *
     * @return The element in the collection at the current position of
     * the enumerator.
     */
    object System.Collections.IEnumerator.Current
    {
        get { return Current; }
    }

    /**
     * Advances the enumerator to the next element of the collection.
     *
     * @return `true` if the enumerator was successfully advanced to
     * the next element; `false` if the enumerator has passed the
     * end of the collection.
     */
    public bool MoveNext()
    {
        if (cur == null)
        {
            cur = start;
        }
        else
        {
            cur = cur.tail;
        }

        return cur != null;
    }

    /**
     * Sets the enumerator to its initial position, which is before the
     * first element in the collection.
     *
     * Throws `InvalidOperationException` if the collection is modified
     * after the enumerator is created.
     * Throws `NotSupportedException` if the enumerator does not support
     * being reset.
     *
     */
    public void Reset()
    {
        cur = null;
    }

    /**
     * Performs application-defined tasks associated with freeing, releasing,
     * or resetting unmanaged resources.
     *
     * Note: here because `IEnumerator<E>` is disposable. We don't need to
     * do anything for SinglyLinkedList<E>.
     */
    public void Dispose() { }
}

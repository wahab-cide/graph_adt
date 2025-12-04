#nullable disable

using System.Text;

/**
 * Represents a directed edge.
 */
public class Edge<V, E>
{
    protected V source;
    protected V destination;
    protected E label;
    protected bool isDirected;
    protected bool isVisited;

    /**
     * An Edge constructor.
     */
    public Edge(V source, V destination, E label, bool isDirected)
    {
        this.source = source;
        this.destination = destination;
        this.label = label;
        this.isDirected = isDirected;
        isVisited = false;
    }

    /**
     * An Edge constructor used only for Edge lookups (does not store
     * an edge label).
     */
    public Edge(V source, V destination, bool isDirected) : this(source, destination, default, isDirected) { }

    public V Source
    {
        get
        {
            return source;
        }
    }

    public V Destination
    {
        get
        {
            return destination;
        }
    }

    public E Label
    {
        get
        {
            return label;
        }
    }

    public bool IsVisited
    {
        get
        {
            return isVisited;
        }

        set
        {
            isVisited = value;
        }
    }

    public override int GetHashCode()
    {
        // XOR is commutative, which is important because
        // when isDirected == false, we don't know which
        // vertex the user will designate as the source and
        // which the destination; with this hashcode, it
        // does not matter.
        return source.GetHashCode() ^ destination.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Edge<V, E> e)
        {
            return false;
        }

        bool same = source.Equals(e.source) && destination.Equals(e.destination);
        bool converse = source.Equals(e.destination) && destination.Equals(e.source);
        return same || (!isDirected && converse);
    }

    /**
     * Construct a string representation of edge.
     *
     * @return The edge as a string.
     */
    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        s.Append("<Edge:");
        if (isVisited) s.Append(" visited");
        s.Append(" " + Source);
        if (isDirected) s.Append(" ->");
        else s.Append(" <->");
        s.Append(" " + Destination + ">");
        return s.ToString();
    }
}

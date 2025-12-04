#nullable disable

/**
 * Represents a vertex.
 */
public class Vertex<V>
{
    protected V label;
    protected bool isVisited;

    public Vertex(V label)
    {
        this.label = label;
        isVisited = false;
    }

    public V Label
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
        return label.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Vertex<V> v)
        {
            return false;
        }
        return label.Equals(v.label);
    }
}

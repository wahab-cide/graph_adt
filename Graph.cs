#nullable disable

using System.Text;

/**
 * Students: put your graph implementation here.
 * You may implement your graph however you wish.
 * A list-based graph is the easiest to implement;
 * we provide suggested fields for the list approach
 * below.  However, you need not accept our suggestion
 * and you will get full credit as long as your
 * implementation faithfully satisfies the IGraph<V, E>
 * interface.
 */
public class Graph<V, E> : IGraph<V, E>
{
    /**
     * Suggested fields for a list-based graph.
     */
    protected Vector<Vertex<V>> vertices;  // store a set of vertices
    protected Vector<Edge<V, E>> edges;    // store a set of edges
    protected bool isDirected;             // remember whether graph is directed or undirected
    protected HashTable<V, Vector<V>> adjacencyList;  // adjacency list for fast neighbor lookup

    public Graph(bool isDirected)
    {
        /**
         * Your graph constructor MUST allow the user to select
         * between directed and undirected graphs.
         */
        this.isDirected = isDirected;
        vertices = new Vector<Vertex<V>>();
        edges = new Vector<Edge<V, E>>();
        adjacencyList = new HashTable<V, Vector<V>>();
    }

    // find vertex by label or throw exception
    private Vertex<V> VertexFor(V label)
    {
        for (int i = 0; i < vertices.Size(); i++)
        {
            Vertex<V> v = vertices.Get(i);
            if (v.Label.Equals(label))
            {
                return v;
            }
        }
        throw new GraphDoesNotContainVertex();
    }

    // find edge by labels or throw exception
    private Edge<V, E> EdgeFor(V source, V destination)
    {
        Edge<V, E> searchEdge = new Edge<V, E>(source, destination, isDirected);
        for (int i = 0; i < edges.Size(); i++)
        {
            Edge<V, E> e = edges.Get(i);
            if (e.Equals(searchEdge))
            {
                return e;
            }
        }
        throw new GraphDoesNotContainEdge();
    }

    /**
     * Adds a vertex to the graph.  Adding a duplicate vertex does
     * nothing.
     *
     * @param label Label for the vertex.
     */
    public void AddVertex(V label)
    {
        if (ContainsVertex(label))
        {
            return;
        }

        Vertex<V> v = new Vertex<V>(label);
        vertices.Append(v);
        adjacencyList.Put(label, new Vector<V>());
    }

    /**
     * Adds an edge between two vertices within the graph.  Both
     * vertices must already exist in the graph or a
     * GraphDoesNotContainVertex is thrown.  The edge is directed
     * if and only if the graph is directed.  Adding a
     * duplicate edge replaces the original edge. Vertices are
     * identified by their labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     * @param label Label associated with the removed edge.
     */
    public void AddEdge(V source, V destination, E label)
    {
        if (!ContainsVertex(source) || !ContainsVertex(destination))
        {
            throw new GraphDoesNotContainVertex();
        }

        if (ContainsEdge(source, destination))
        {
            RemoveEdge(source, destination);
        }

        Edge<V, E> edge = new Edge<V, E>(source, destination, label, isDirected);
        edges.Append(edge);

        Vector<V> neighbors = adjacencyList.Get(source);
        neighbors.Append(destination);

        if (!isDirected)
        {
            Vector<V> reverseNeighbors = adjacencyList.Get(destination);
            reverseNeighbors.Append(source);
        }
    }

    /**
     * Removes a vertex from the graph.  Associated edges are also
     * removed. Throws GraphDoesNotContainVertex when the graph
     * does not contain a vertex with the given label.
     *
     * @param label The label of the vertex within the graph.
     * @return The label associated with the vertex.
     */
    public V RemoveVertex(V label)
    {
        Vertex<V> v = VertexFor(label);
        V removed = v.Label;

        for (int i = edges.Size() - 1; i >= 0; i--)
        {
            Edge<V, E> e = edges.Get(i);
            if (e.Source.Equals(removed) || e.Destination.Equals(removed))
            {
                edges.RemoveAt(i);
            }
        }

        vertices.Remove(v);
        adjacencyList.Remove(removed);

        IList<V> allVertices = adjacencyList.Keys;
        for (int i = 0; i < allVertices.Size(); i++)
        {
            V vertexLabel = allVertices.Get(i);
            Vector<V> neighbors = adjacencyList.Get(vertexLabel);
            if (neighbors.Contains(removed))
            {
                neighbors.Remove(removed);
            }
        }

        return removed;
    }

    /**
     * Remove possible edge between vertices labeled vLabel1 and vLabel2.
     * Throws GraphDoesNotContainEdge when the graph does not contain an
     * edge with the given labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     * @return The label associated with the removed edge.
     */
    public E RemoveEdge(V label1, V label2)
    {
        Edge<V, E> edge = EdgeFor(label1, label2);
        E edgeLabel = edge.Label;

        edges.Remove(edge);

        Vector<V> neighbors = adjacencyList.Get(label1);
        neighbors.Remove(label2);

        if (!isDirected)
        {
            Vector<V> reverseNeighbors = adjacencyList.Get(label2);
            reverseNeighbors.Remove(label1);
        }

        return edgeLabel;
    }

    /**
     * Gets the label data of vertex.  Vertex labels are located using their
     * .Equals method.  Does not modify the graph. Throws GraphDoesNotContainVertex
     * when the graph does not contain a vertex with the given label.
     *
     * @param label The label of the vertex sought.
     * @return The label.
     */
    public V GetVertex(V label)
    {
        Vertex<V> v = VertexFor(label);
        return v.Label;
    }

    /**
     * Returns the edge label for the edge with the given source
     * and destination vertices. Throws GraphDoesNotContainEdge if
     * edge is not in graph.
     *
     * @param source The first (or source, if directed) vertex.
     * @param destination The second (or destination, if directed) vertex.
     * @return The edge label.
     */
    public E GetEdge(V source, V destination)
    {
        Edge<V, E> edge = EdgeFor(source, destination);
        return edge.Label;
    }

    /**
     * Returns true if and only if the graph contains a vertex
     * with the given label.
     *
     * @param label The label of the vertex sought.
     */
    public bool ContainsVertex(V label)
    {
        try
        {
            VertexFor(label);
            return true;
        }
        catch (GraphDoesNotContainVertex)
        {
            return false;
        }
    }

    /**
     * Returns true if and only if the graph contains an edge
     * with the given labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     * @return True if found, false otherwise.
     */
    public bool ContainsEdge(V label1, V label2)
    {
        try
        {
            EdgeFor(label1, label2);
            return true;
        }
        catch (GraphDoesNotContainEdge)
        {
            return false;
        }
    }

    /**
     * Sets visited flag on vertex, returning the previous value.
     * Throws GraphDoesNotContainVertex when the graph does not
     * contain a vertex with the given label.
     *
     * @param label Label of vertex to be visited.
     */
    public bool VisitVertex(V label)
    {
        Vertex<V> v = VertexFor(label);
        bool oldValue = v.IsVisited;
        v.IsVisited = true;
        return oldValue;
    }

    /**
     * Sets visited flag on edge, returning the previous value.
     * Throws GraphDoesNotContainEdge when the graph does not
     * contain an edge for the given vertex labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     */
    public bool VisitEdge(V label1, V label2)
    {
        Edge<V, E> edge = EdgeFor(label1, label2);
        bool oldValue = edge.IsVisited;
        edge.IsVisited = true;
        return oldValue;
    }

    /**
     * Returns the visited flag of the vertex with the given label.
     * Throws GraphDoesNotContainVertex when the graph does not
     * contain a vertex with the given label.
     *
     * @param label Label of vertex.
     */
    public bool VertexIsVisited(V label)
    {
        Vertex<V> v = VertexFor(label);
        return v.IsVisited;
    }

    /**
     * Returns the visited flag of the edge with the given
     * vertex labels.  Throws GraphDoesNotContainEdge when the
     * graph does not contain an edge with the given vertex labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     */
    public bool EdgeIsVisited(V source, V destination)
    {
        Edge<V, E> edge = EdgeFor(source, destination);
        return edge.IsVisited;
    }

    /**
     * Returns a list of all of the vertex labels in the graph. No
     * ordering of labels should be assumed.
     *
     * @return A list of vertices.
     */
    public IList<V> Vertices
    {
        get
        {
            Vector<V> vertexLabels = new Vector<V>();
            for (int i = 0; i < vertices.Size(); i++)
            {
                vertexLabels.Append(vertices.Get(i).Label);
            }
            return vertexLabels;
        }
    }

    /**
     * Returns a list of all of the edges in the graph, as pairs
     * of vertex labels. No ordering of edges should be assumed.
     *
     * @return A list of edges.
     */
    public IList<Tuple<V, V>> Edges
    {
        get
        {
            Vector<Tuple<V, V>> edgePairs = new Vector<Tuple<V, V>>();
            for (int i = 0; i < edges.Size(); i++)
            {
                Edge<V, E> e = edges.Get(i);
                edgePairs.Append(new Tuple<V, V>(e.Source, e.Destination));
            }
            return edgePairs;
        }
    }

    /**
     * Clears the visited flags of edges and vertices.
     */
    public void Reset()
    {
        for (int i = 0; i < vertices.Size(); i++)
        {
            vertices.Get(i).IsVisited = false;
        }
        for (int i = 0; i < edges.Size(); i++)
        {
            edges.Get(i).IsVisited = false;
        }
    }

    /**
      * Returns the number of vertices within graph.
      *
      * @return A count.
      */
    public int Size
    {
        get
        {
            return vertices.Size();
        }
    }

    /**
     * Returns a list of all of label's neighbors. If the graph is directed
     * then only the neighbors at the end of a directed edge with the
     * given label as the source are returned.  If the graph is undirected
     * then all vertices that share an edge with the given vertex are
     * returned. Throws GraphDoesNotContainVertex when the graph does not
     * contain a vertex with the given label. No ordering of labels should
     * be assumed.
     *
     * @param label First (or source, if directed) vertex.
     */
    public IList<V> Neighbors(V source)
    {
        if (!ContainsVertex(source))
        {
            throw new GraphDoesNotContainVertex();
        }

        return adjacencyList.Get(source);
    }

    /**
     * For a directed graph, determines out-degree of the vertex
     * with the given vertex label as an edge source. For an undirected
     * graph, just computes the degree of the vertex with the given
     * label. Throws GraphDoesNotContainVertex when the graph does
     * not contain a vertex with the given label.
     *
     * @param label Label associated with vertex.
     * @return The number of edges with this vertex as source.
     */
    public int OutDegree(V label)
    {
        if (!ContainsVertex(label))
        {
            throw new GraphDoesNotContainVertex();
        }

        return adjacencyList.Get(label).Size();
    }

    /**
     * For a directed graph, determines in-degree of the vertex
     * with the given vertex label as an edge source. For an undirected
     * graph, just computes the degree of the vertex with the given
     * label. Throws GraphDoesNotContainVertex when the graph does
     * not contain a vertex with the given label.
     *
     * @param label Label associated with vertex.
     * @return The number of edges with this vertex as destination.
     */
    public int InDegree(V label)
    {
        if (!ContainsVertex(label))
        {
            throw new GraphDoesNotContainVertex();
        }

        if (!isDirected)
        {
            return OutDegree(label);
        }

        int count = 0;
        for (int i = 0; i < edges.Size(); i++)
        {
            Edge<V, E> e = edges.Get(i);
            if (e.Destination.Equals(label))
            {
                count++;
            }
        }
        return count;
    }

    /**
     * Returns true if and only if the graph contains no vertices.
     */
    public bool IsEmpty
    {
        get
        {
            return vertices.IsEmpty();
        }
    }

    /**
     * Returns true if and only if the graph is directed.
     */
    public bool IsDirected
    {
        get
        {
            return isDirected;
        }
    }

    /** 
     * Prints the graph in GraphViz format.
     * To visualize, call this method and save the output in a file, 
     * e.g., "graph.dot".
     *
     * If GraphViz is installed locally, run
     * $ dot -Tpng graph.dot -o graph.png
     * and open the file graph.png.
     *
     * If GraphViz is not installed, paste the contents of "graph.dot"
     * into https://dreampuf.github.io/GraphvizOnline
     */
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        var gname = isDirected ? "digraph" : "graph";
        sb.Append("strict " + gname + " {\n");
        foreach (var edge in Edges)
        {
            var label = GetEdge(edge.Item1, edge.Item2);
            var arrow = isDirected ? "->" : "--";
            sb.Append($" \"{edge.Item1}\" {arrow} \"{edge.Item2}\" [label=\"{label}\"];\n");
        }
        sb.Append("}\n");
        return sb.ToString();
    }
}

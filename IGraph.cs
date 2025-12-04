#nullable disable

/**
 * An interface for the graph abstract data type. A graph is comprised
 * of a set of vertices and a set of edges.  Edges may or may not be
 * directed.  Vertex and edge lookups are done by checking for equality
 * on labels.  Vertex (V) and Edge (E) label types MUST implement
 * GetHashCode() and Equals() for this data structure to work correctly.
 */
public interface IGraph<V, E>
{
    /**
     * Adds a vertex to the graph.  Adding a duplicate vertex does
     * nothing.
     *
     * @param label Label for the vertex.
     */
    public void AddVertex(V label);

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
    public void AddEdge(V source, V destination, E label);

    /**
     * Removes a vertex from the graph.  Associated edges are also
     * removed. Throws GraphDoesNotContainVertex when the graph
     * does not contain a vertex with the given label.
     *
     * @param label The label of the vertex within the graph.
     * @return The label associated with the vertex.
     */
    public V RemoveVertex(V label);

    /**
     * Remove possible edge between vertices labeled vLabel1 and vLabel2.
     * Throws GraphDoesNotContainEdge when the graph does not contain an
     * edge with the given labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     * @return The label associated with the removed edge.
     */
    public E RemoveEdge(V label1, V label2);

    /**
     * Gets the label data of vertex.  Vertex labels are located using their
     * .Equals method.  Does not modify the graph. Throws GraphDoesNotContainVertex
     * when the graph does not contain a vertex with the given label.
     *
     * @param label The label of the vertex sought.
     * @return The label.
     */
    public V GetVertex(V label);

    /**
     * Returns the edge label for the edge with the given source
     * and destination vertices. Throws GraphDoesNotContainEdge if
     * edge is not in graph.
     *
     * @param source The first (or source, if directed) vertex.
     * @param destination The second (or destination, if directed) vertex.
     * @return The edge label.
     */
    public E GetEdge(V source, V destination);

    /**
     * Returns true if and only if the graph contains a vertex
     * with the given label.
     *
     * @param label The label of the vertex sought.
     */
    public bool ContainsVertex(V label);

    /**
     * Returns true if and only if the graph contains an edge
     * with the given labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     * @return True if found, false otherwise.
     */
    public bool ContainsEdge(V label1, V label2);

    /**
     * Sets visited flag on vertex, returning the previous value.
     * Throws GraphDoesNotContainVertex when the graph does not
     * contain a vertex with the given label.
     *
     * @param label Label of vertex to be visited.
     */
    public bool VisitVertex(V label);

    /**
     * Sets visited flag on edge, returning the previous value.
     * Throws GraphDoesNotContainEdge when the graph does not
     * contain an edge for the given vertex labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     */
    public bool VisitEdge(V label1, V label2);

    /**
     * Returns the visited flag of the vertex with the given label.
     * Throws GraphDoesNotContainVertex when the graph does not
     * contain a vertex with the given label.
     *
     * @param label Label of vertex.
     */
    public bool VertexIsVisited(V label);

    /**
     * Returns the visited flag of the edge with the given
     * vertex labels.  Throws GraphDoesNotContainEdge when the
     * graph does not contain an edge with the given vertex labels.
     *
     * @param source First (or source, if directed) vertex.
     * @param destination Second (or destination, if directed) vertex.
     */
    public bool EdgeIsVisited(V source, V destination);

    /**
     * Returns a list of all of the vertex labels in the graph. No
     * ordering of labels should be assumed.
     *
     * @return A list of vertices.
     */
    public IList<V> Vertices { get; }

    /**
     * Returns a list of all of the edges in the graph, as pairs
     * of vertex labels. No ordering of edges should be assumed.
     *
     * @return A list of edges.
     */
    public IList<Tuple<V,V>> Edges { get; }

    /**
     * Clears the visited flags of edges and vertices.
     */
    public void Reset();

    /**
      * Returns the number of vertices within graph.
      *
      * @return A count.
      */
    public int Size { get; }

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
    public IList<V> Neighbors(V source);

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
    public int OutDegree(V label);

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
    public int InDegree(V label);

    /**
     * Returns true if and only if the graph contains no vertices.
     */
    public bool IsEmpty { get; }

    /**
     * Returns true if and only if the graph is directed.
     */
    public bool IsDirected { get; }
}

public class GraphDoesNotContainVertex : Exception
{
    public GraphDoesNotContainVertex() : base() { }
}

public class GraphDoesNotContainEdge : Exception
{
    public GraphDoesNotContainEdge() : base() { }
}

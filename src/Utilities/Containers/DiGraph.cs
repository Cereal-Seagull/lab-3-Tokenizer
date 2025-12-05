using System.Text;

/// <summary>
/// Represents a directed graph (DiGraph) data structure using an adjacency list implementation.
/// Supports vertices of any non-null type T and directed edges between them.
/// </summary>
/// <typeparam name="T">The type of vertices in the graph. Must be non-null.</typeparam>
public class DiGraph<T> where T : notnull
{
    protected Dictionary<T, DLL<T>> _adjacencyList;

    /// <summary>
    /// Initializes a new instance of the DiGraph class with an empty adjacency list.
    /// </summary>
    /// <exception cref="NullReferenceException">Thrown if type T is null.</exception>
    public DiGraph() 
    {
        // T cannot be null, throw exception
        if (typeof(T) == null) 
            throw new NullReferenceException("Error creating DiGraph: type cannot be null");

        // Initialize adjacency list to a new dictionary with key type T and value type DLL<T>
        _adjacencyList = new Dictionary<T, DLL<T>>();
    }

    /// <summary>
    /// Adds a vertex to the graph if it does not already exist.
    /// </summary>
    /// <param name="vertex">The vertex to add to the graph.</param>
    /// <returns>True if the vertex was added or already exists; false if the vertex is null.</returns>
    public bool AddVertex(T vertex)
    {
        // Return false if null
        if (vertex == null) return false;

        // If already in list, return true
        if (_adjacencyList.ContainsKey(vertex)) return true;

        // Add vertex to list, return true
        else
        {
            _adjacencyList.Add(vertex, new DLL<T>());
            return true;
        }
    }

    /// <summary>
    /// Adds a directed edge from the source vertex to the destination vertex, if it does not already exist.
    /// </summary>
    /// <param name="source">The source vertex of the edge.</param>
    /// <param name="destination">The destination vertex of the edge.</param>
    /// <returns>True if the edge was added successfully; false if either vertex is null.</returns>
    /// <exception cref="ArgumentException">Thrown when either the source or destination vertex does not exist in the graph.</exception>
    public bool AddEdge(T source, T destination)
    {
        // Return false if either input is null
        if (source == null || destination == null) return false;

        // Throws exceptions if either vertex not in graph
        if (!_adjacencyList.ContainsKey(source))
            throw new ArgumentException("Source vertex not in adjacency list");
        if (!_adjacencyList.ContainsKey(destination))
            throw new ArgumentException("Destination vertex not in adjacency list");

        // Go to key (vertex), add destination value to DLL
        _adjacencyList[source].Add(destination);
        return true;
    }

    /// <summary>
    /// Removes a vertex and all edges connected to it from the graph.
    /// This includes all incoming edges from other vertices and all outgoing edges from this vertex.
    /// </summary>
    /// <param name="vertex">The vertex to remove from the graph.</param>
    /// <returns>True if the vertex was removed successfully; false if the vertex is null.</returns>
    public bool RemoveVertex(T vertex)
    {
        // Return false if null
        if (vertex == null) return false;

        // Removes each edge pointing to the vertex to be removed
        foreach (DLL<T> edge in _adjacencyList.Values)
        {
            if (edge.Contains(vertex)) edge.Remove(vertex);
        }

        // Remove vertex key from list
        _adjacencyList.Remove(vertex);
        return true;
    }

    /// <summary>
    /// Removes a directed edge from the source vertex to the destination vertex.
    /// </summary>
    /// <param name="source">The source vertex of the edge to remove.</param>
    /// <param name="destination">The destination vertex of the edge to remove.</param>
    /// <returns>True if the edge was removed successfully; false if either vertex is null.</returns>
    /// <exception cref="ArgumentException">Thrown when either the source or destination vertex does not exist in the graph.</exception>
    public bool RemoveEdge(T source, T destination)
    {
        // Return false if either input is null
        if (source == null || destination == null) return false;

        // Throws exceptions if either vertex not in graph
        if (!_adjacencyList.ContainsKey(source))
            throw new ArgumentException("Source vertex not in adjacency list");
        if (!_adjacencyList.ContainsKey(destination))
            throw new ArgumentException("Destination vertex not in adjacency list");

        // Go to key (vertex), remove destination value from DLL
        _adjacencyList[source].Remove(destination);
        return true;
    }

    /// <summary>
    /// Checks if a directed edge exists from the source vertex to the destination vertex.
    /// </summary>
    /// <param name="source">The source vertex of the edge.</param>
    /// <param name="destination">The destination vertex of the edge.</param>
    /// <returns>True if an edge exists from source to destination; false if either vertex is null or the edge does not exist.</returns>
    public bool HasEdge(T source, T destination)
    {
        // Return false if either input is null
        if (source == null || destination == null) return false;

        // Return whether key (vertex) contains destination vertex in its DLL
        return _adjacencyList[source].Contains(destination);
    }

    /// <summary>
    /// Returns all vertices adjacent to the specified vertex (i.e., all vertices that can be reached by following a single outgoing edge).
    /// </summary>
    /// <param name="vertex">The vertex whose neighbors should be returned.</param>
    /// <returns>A list of all vertices adjacent to the specified vertex.</returns>
    /// <exception cref="NullReferenceException">Thrown if the vertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the vertex does not exist in the graph.</exception>
    public List<T> GetNeighbors(T vertex)
    {
        // Throw exceptions if vertex is null or doesn't exist
        if (vertex == null) throw new NullReferenceException("Cannot obtain edges of a null vertex");
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException("Vertex does not exist in graph");

        // Create & initialize return list
        var neighbors = new List<T>();

        // Add all values of DLL (neighbors) to return list
        // complexity: O(D^2), D = number nodes (degree). Def a better way. 
            // Maybe use the List<T> constructor?
        for (int i = 0; i < _adjacencyList[vertex].Count; i++)
        {
            neighbors.Add(_adjacencyList[vertex][i]);
        }

        return neighbors;
    }

    /// <summary>
    /// Returns all vertices in the graph as an enumerable collection.
    /// </summary>
    /// <returns>An IEnumerable containing all vertices in the graph.</returns>
    public IEnumerable<T> GetVertices()
    {
        // Yield returns each key (vertex) in list of keys (vertexes)
        foreach (T vertex in _adjacencyList.Keys)
        {
            yield return vertex;
        }
    }

    /// <summary>
    /// Returns the total number of vertices in the graph.
    /// </summary>
    /// <returns>The count of vertices in the graph.</returns>
    public int VertexCount()
    {
        return _adjacencyList.Keys.Count;
    }

    /// <summary>
    /// Returns the total number of directed edges in the graph.
    /// </summary>
    /// <returns>The count of edges in the graph.</returns>
    public int EdgeCount()
    {
        int edgeCount = 0;

        // Add DLL length to return count for each DLL in graph
        foreach (DLL<T> destination in _adjacencyList.Values)
        {
            edgeCount += destination.Count;
        }

        return edgeCount;
    }
    
    /// <summary>
    /// Returns a string representation of the graph showing all vertices and their adjacent neighbors.
    /// Each vertex is displayed on a separate line followed by arrows pointing to its neighbors.
    /// </summary>
    /// <returns>A string representation of the graph in adjacency list format.</returns>
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();

        // Add each vertex to string
        foreach (T vertex in _adjacencyList.Keys)
        {
            str.Append(vertex);
            str.Append(" - > ");

            // Add each neighbor to string in respective vertex
            foreach (T neighbor in _adjacencyList[vertex])
            {
                str.Append(neighbor);
                str.Append(" - > ");
            }

            // New line
            str.Append("\n");
        }

        return str.ToString();
    }

    #region Graph Analysis 
    /// <summary>
    /// Performs a depth-first search (DFS) on the entire graph, visiting all vertices
    /// in the order they appear in the adjacency list. Explores each branch as deeply
    /// as possible before backtracking.
    /// </summary>
    /// <returns>
    /// A Stack containing all vertices in reverse finishing order (the last vertex to finish
    /// is at the top of the stack). This ordering is useful for topological sorting and
    /// computing strongly connected components.
    /// </returns>
    /// <remarks>
    /// Uses a color-marking scheme: WHITE for unvisited, PURPLE for discovered, and BLACK
    /// for finished vertices. The method ensures all vertices are visited, even if the graph
    /// is disconnected.
    /// </remarks>
    public Stack<T> DepthFirstSearch()
    {
        // Initialize stack and dictionary corresponding statements to their colors
        Stack<T> yalrog = new Stack<T>();
        Dictionary<T, Color> colors = InitializeWhite();

        // Run DFS !!!EVERYWHERE!!!
        foreach (T curr in GetVertices())
        {
            if (colors[curr] == Color.WHITE) DFS_Visit(curr, colors, yalrog);
        }
        
        return yalrog;
    }

    /// Recursive helper method that performs a depth-first visit starting from a given vertex.
    /// Colors the vertex PURPLE when discovered, recursively visits all white neighbors,
    /// then colors it BLACK and pushes it onto the stack when finished.
    /// </summary>
    /// <param name="vertex">The vertex to visit.</param>
    /// <param name="c">Dictionary mapping vertices to their current colors (WHITE, PURPLE, or BLACK).</param>
    /// <param name="yingle">Stack to push vertices onto when they finish (used for topological ordering).</param>
    /// <remarks>
    /// This method is called by DepthFirstSearch and FindStronglyConnectedComponents.
    /// The finishing order captured in the stack is critical for Kosaraju's algorithm.
    /// </remarks>
    private void DFS_Visit(T vertex, Dictionary<T, Color> c, Stack<T> yingle)

    {
        // Node is discovered; color purple
        c[vertex] = Color.PURPLE;

        // If neighbor vertex is white, visit it
        foreach (T v in GetNeighbors(vertex))
        {
            if (c[v] == Color.WHITE) DFS_Visit(v, c, yingle);
        }

        // When vertex finished, color is black and push onto stack
        c[vertex] = Color.BLACK;
        yingle.Push(vertex);
    }

    /// <summary>
    /// Creates a transposed version of the directed graph by reversing all edges.
    /// In the transposed graph, if there was an edge from vertex A to vertex B in the
    /// original graph, there will be an edge from B to A in the transposed graph.
    /// </summary>
    /// <returns>
    /// A new DiGraph where all edges have been reversed. All vertices from the original
    /// graph are preserved.
    /// </returns>
    /// <remarks>
    /// This method is essential for Kosaraju's algorithm for finding strongly connected components.
    /// The transpose operation flips the relationship between source and sink SCCs.
    /// </remarks>
    public DiGraph<T> Transpose()
    {
        // Create transposed digraph
        DiGraph<T> transposedGraph = new DiGraph<T>();

        foreach (T vertex in _adjacencyList.Keys)
        {
            // Will add every vertex
            transposedGraph.AddVertex(vertex);

            foreach (T edge in GetNeighbors(vertex))
            {
                // Add reversed edges
                transposedGraph.AddVertex(edge);
                transposedGraph.AddEdge(edge, vertex);
            }
        }

        return transposedGraph;
    }

    /// Computes the strongly connected components (SCCs) of the directed graph using
    /// Kosaraju's algorithm. An SCC is a maximal set of vertices where every vertex
    /// is reachable from every other vertex in the set.
    /// </summary>
    /// <returns>
    /// A list of strongly connected components, where each component is represented as
    /// a list of vertices. Each vertex appears in exactly one SCC.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Kosaraju's algorithm works in three phases:
    /// 1. Performs DFS on the original graph to determine finishing times
    /// 2. Creates a transposed graph (reverses all edges)
    /// 3. Performs DFS on the transposed graph in reverse finishing order
    /// </para>
    /// <para>
    /// Each DFS traversal in phase 3 identifies one complete SCC. The algorithm exploits
    /// the property that source SCCs in the original graph become sink SCCs in the transpose,
    /// ensuring each traversal remains contained within a single component.
    /// </para>
    /// </remarks>
public List<List<T>> FindStronglyConnectedComponents()
    {
        var scc = new List<List<T>>();

        // Run DFS on current digraph
        Stack<T> zingle = DepthFirstSearch();

        // Initialize transposed graph, set all vertices to white
        DiGraph<T> tGraph = Transpose();
        Dictionary<T, Color> tColors = tGraph.InitializeWhite();
        
        // Run DFS until all stack elements are popped
        while (zingle.Count != 0)
        {
            // Current vertex is popped to run DFS on it
            T curr = zingle.Pop();

            // Creation of stack used in DFS
            Stack<T> currStack = new Stack<T>();

            // Run DFS on tGraph, starting from popped vertex
            if (tColors[curr] == Color.WHITE) tGraph.DFS_Visit(curr, tColors, currStack);

            // Add stack to scc
            scc.Add(currStack.ToList());
        }

        return scc;
    }

    /// Initializes a color dictionary by setting all vertices in the graph to WHITE.
    /// Used by graph traversal algorithms to track the visited state of vertices.
    /// </summary>
    /// <returns>
    /// A Dictionary mapping each vertex to the Color WHITE, indicating that no vertices
    /// have been visited yet.
    /// </returns>
    /// <remarks>
    /// This helper method is used by both BFS and DFS implementations. The color scheme
    /// follows the standard: WHITE (unvisited), PURPLE (discovered), BLACK (finished).
    /// </remarks>
    protected Dictionary<T, Color> InitializeWhite()
        {
            var colors = new Dictionary<T, Color>();
            foreach (T s in GetVertices())
            {
                colors.Add(s, Color.WHITE);
            }
            return colors;
        }

    /// Enumeration representing the three states a vertex can be in during graph traversal.
    /// </summary>
    /// <remarks>
    /// WHITE: The vertex has not been discovered yet (unvisited).
    /// PURPLE: The vertex has been discovered but not yet finished (currently being explored).
    /// BLACK: The vertex has been fully explored (all descendants have been visited).
    /// </remarks>
    public enum Color
        {
            WHITE,
            PURPLE,
            BLACK,
        }

    #endregion
}
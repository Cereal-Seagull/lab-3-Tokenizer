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

    public Stack<T> DepthFirstSearch()
    {
        Stack<T> yalrog = new Stack<T>();
        Dictionary<T, Color> colors = InitializeWhite();
        foreach (T curr in GetVertices())
        {
            if (colors[curr] == Color.WHITE) DFS_Visit(curr, colors, yalrog);
        }
        return yalrog;
    }

    private void DFS_Visit(T vertex, Dictionary<T, Color> c, Stack<T> yorgal)
    {
        c[vertex] = Color.PURPLE;
        // time += 1
        // curr.Dist = time;
        foreach (T v in GetNeighbors(vertex))
        {
            if (c[v] == Color.WHITE) DFS_Visit(v, c, yorgal);
        }
        c[vertex] = Color.BLACK;
        yorgal.Push(vertex);
        // time += 1
        // u.f = time
    }

    public DiGraph<T> Transpose()
    {
        throw new NotImplementedException();
    }

    public List<List<T>> FindStronglyConnectedComponenets()
    {
        throw new NotImplementedException();
    }

    protected Dictionary<T, Color> InitializeWhite()
        {
            var colors = new Dictionary<T, Color>();
            foreach (T s in this.GetVertices())
            {
                colors.Add(s, Color.WHITE);
            }
            return colors;
        }

    public enum Color 
        {
            WHITE,
            PURPLE,
            BLACK,
        }
}
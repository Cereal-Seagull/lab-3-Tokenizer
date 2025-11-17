using System.Text;

public class DiGraph<T> where T : notnull
{
    protected Dictionary<T, DLL<T>> _adjacencyList;

    // Constructor initializing _adjacencyList
    public DiGraph() 
    {
        // T cannot be null, throw exception
        if (typeof(T) == null) 
            throw new NullReferenceException("Error creating DiGraph: type cannot be null");

        _adjacencyList = new Dictionary<T, DLL<T>>();
    }

    // Adds a vertex to the graph if it does not already exist.
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

    // Adds a directed edge from source to destination, if it does not already exist.
    // Throws ArgumentException when either vertex does not exist in the graph
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

    // Removes a vertex and all edges connected to it.
    public bool RemoveVertex(T vertex)
    {
        // Return false if null
        if (vertex == null) return false;

        // Remove vertex key from list
        _adjacencyList.Remove(vertex);
        return true;
    }

    // Removes a directed edge from source to destination.
    // ArgumentException when either vertex does not exist in the graph
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

    // Checks if an edge exists from source to destination.
    public bool HasEdge(T source, T destination)
    {
        // Return false if either input is null
        if (source == null || destination == null) return false;

        // Return whether key (vertex) contains destination vertex in its DLL
        return _adjacencyList[source].Contains(destination);
    }

    // Returns all vertices adjacent to the specified vertex.
    // ArgumentException when the vertex does not exist in the graph
    public List<T> GetNeighbors(T vertex)
    {
        // Throw exceptions if vertex is null or doesn't exist
        if (vertex == null) throw new NullReferenceException("Cannot obtain edges of a null vertex");
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException("Vertex does not exist in graph");

        // Create & initialize return list
        var neighbors = new List<T>();

        // Add all values of DLL (neighbors) to return list
        for (int i = 0; i < _adjacencyList[vertex].Count; i++)
        {
            neighbors.Add(_adjacencyList[vertex][i]);
        }

        return neighbors;
    }

    // Returns all vertices in the graph as an iterable container.
    public IEnumerable<T> GetVertices()
    {
        // Yield returns each key (vertex) in list of keys (vertexes)
        foreach (T vertex in _adjacencyList.Keys)
        {
            yield return vertex;
        }
    }

    // Returns the number of vertices in the graph.
    public int VertexCount()
    {
        return _adjacencyList.Keys.Count;
    }

    // Returns the number of edges in the graph.
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
    
    // Returns a string representation of the graph.
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

}
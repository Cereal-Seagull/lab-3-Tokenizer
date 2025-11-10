public class DiGraph<T> where T : notnull
{
    protected Dictionary<T, DLL<T>> _adjacencyList;

    // Constructor initializing _adjacencyList
    public DiGraph() 
    {
        _adjacencyList = new Dictionary<T, DLL<T>>();
    }

    // Adds a vertex to the graph if it does not already exist.
    public bool AddVertex(T vertex)
    {
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
        // Throws exceptions if either vertex not in graph
        if (!_adjacencyList.ContainsKey(source))
            throw new ArgumentException("Source vertex not in adjacency list");
        if (!_adjacencyList.ContainsKey(destination))
            throw new ArgumentException("Destination vertex not in adjacency list");
        throw new NotImplementedException();


    }

    // Removes a vertex and all edges connected to it.
    public bool RemoveVertex(T vertex)
    {
        _adjacencyList.Remove(vertex);
        return true;
    }

    // Removes a directed edge from source to destination.
    // ArgumentException when either vertex does not exist in the graph
    public bool RemoveEdge(T source, T destination)
    {
        throw new NotImplementedException();
    }

    // Checks if an edge exists from source to destination.
    public bool HasEdge(T source, T destination)
    {
        throw new NotImplementedException();
    }

    // Returns all vertices adjacent to the specified vertex.
    // ArgumentException when the vertex does not exist in the graph
    public List<T> GetNeighbors(T vertex)
    {
        throw new NotImplementedException();
    }

    // Returns all vertices in the graph as an iterable container.
    public IEnumerable<T> GetVertices()
    {
        throw new NotImplementedException();
    }

    // Returns the number of vertices in the graph.
    public int VertexCount()
    {
        throw new NotImplementedException();
    }

    // Returns the number of edges in the graph.
    public int EdgeCount()
    {
        throw new NotImplementedException();
    }
    
    // Returns a string representation of the graph.
    public override string ToString()
    {
        throw new NotImplementedException();
    }

}
using AST;

namespace Optimizer
{
    /// <summary>
    /// Represents a Control Flow Graph (CFG) for a DEC program.
    /// Extends DiGraph to provide a directed graph structure where vertices are Statement nodes
    /// and edges represent the flow of control between statements.
    /// </summary>
    public class CFG : DiGraph<Statement>
    {
        /// <summary>
        /// Gets or sets the starting statement of the DEC program.
        /// This represents the entry point of the control flow graph.
        /// </summary>
        public Statement? Start { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the CFG class.
        /// Constructs a new CFG based on the generic DiGraph implementation.
        /// </summary>
        public CFG() : base() { } // I think all your base are belong to us

        #region BFS
        /// <summary>
        /// Performs a breadth-first search (BFS) starting from the Start statement to identify
        /// reachable and unreachable statements in the control flow graph.
        /// Explores the graph level by level, visiting all neighbors before moving to the next level.
        /// </summary>
        /// <returns>
        /// A tuple containing two lists:
        /// - reachable: List of Statement nodes that can be reached from the Start statement
        /// - unreachable: List of Statement nodes that cannot be reached from the Start statement
        /// </returns>
        /// <remarks>
        /// Uses a color-marking scheme where WHITE indicates unvisited, PURPLE indicates discovered,
        /// and BLACK indicates fully explored. All statements begin in the unreachable list and are
        /// moved to reachable as they are discovered during the traversal.
        /// </remarks>
        public (List<Statement> reachable, List<Statement> unreachable) BreadthFirstSearch()
        {
            // Create and initalize return tuple and queue
            (List<Statement> reachable, List<Statement> unreachable) = 
                (new List<Statement>(), new List<Statement>());
            InitializeUnreachableList(unreachable);
            Queue<Statement> q = new Queue<Statement>();

            // Create dictionary corresponding statements to their colors
            Dictionary<Statement, Color> colors = InitializeWhite();
            
            // Put starting statement into queue
            q.Enqueue(Start);

            while (q.Count != 0)
            {
                // Current stmt is dequeued stmt
                Statement curr = q.Dequeue();

                // Set current color to purple; not unreachable
                colors[curr] = Color.PURPLE;
                unreachable.Remove(curr);

                /// Set adjacent node colors to purple if unexplored & enqueue them
                foreach (Statement adj in GetNeighbors(curr))
                {
                    if (colors[adj] == Color.WHITE)
                        {
                            colors[adj] = Color.PURPLE;
                            q.Enqueue(adj);
                        }   
                }

                // Current is fully explored; is reachable and set to black
                colors[curr] = Color.BLACK;
                reachable.Add(curr);
            }

            return (reachable, unreachable);
        }

        /// <summary>
        /// Initializes the unreachable list by adding all vertices from the adjacency list.
        /// This helper method is used by BreadthFirstSearch to establish the initial set of
        /// potentially unreachable statements before the BFS traversal begins.
        /// </summary>
        /// <param name="unreachable">The list to populate with all statements in the graph.</param>
        /// <returns>The populated unreachable list containing all vertices.</returns>
        private List<Statement> InitializeUnreachableList(List<Statement> unreachable)
        {
            // Add every vertex to unreachable list
            foreach (Statement vertex in _adjacencyList.Keys)
            {
                unreachable.Add(vertex);
            }
            return unreachable;
        }
        #endregion
    }
}
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

        // Helper method initializing the unreachable list in return tuple for BFS
        private List<Statement> InitializeUnreachableList(List<Statement> unreachable)
        {
            // Add every vertex to unreachable list
            foreach (Statement vertex in _adjacencyList.Keys)
            {
                unreachable.Add(vertex);
            }
            return unreachable;
        }

        
    }
}
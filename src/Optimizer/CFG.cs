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
            // Create and initalize return tuple
            (List<Statement> reachable, List<Statement> unreachable) = 
                (new List<Statement>(), new List<Statement>());
            // int dist = 0;
            // Statement? p = null;

            Dictionary<Statement, Color> colors = InitializeWhite();
            Queue<Statement> q = new Queue<Statement>();
            if (Start != null) q.Enqueue(Start);

            while (q.Count != 0)
            {
                Statement curr = q.Dequeue();
                colors[curr] = Color.PURPLE;
                foreach (Statement adj in this.GetVertices())
                {
                 if (colors[adj] == Color.WHITE)
                    {
                        colors[adj] = Color.PURPLE;
                        // adj.Dist = dist + 1;
                        // adj.p = curr;
                        q.Enqueue(adj);
                    }   
                }
                colors[curr] = Color.BLACK;
            }


            
            return (reachable, unreachable);
        }

        private Dictionary<Statement, Color> InitializeWhite()
        {
            var colors = new Dictionary<Statement, Color>();
            foreach (Statement s in this.GetVertices())
            {
                colors.Add(s, Color.WHITE);
            }
            return colors;
        }
    }

    public enum Color 
    {
        WHITE,
        PURPLE,
        BLACK,
    }
}
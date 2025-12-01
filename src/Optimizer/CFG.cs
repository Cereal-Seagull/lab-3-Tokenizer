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
            (List<Statement> reachable, List<Statement> unreachable) = (new List<Statement>(), new List<Statement>());
            const bool WHITE = false;
            const bool BLACK = true;

            

            return (reachable, unreachable);
        }
    }
}
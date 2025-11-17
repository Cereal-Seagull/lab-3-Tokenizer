using AST;

namespace Optimizer
{
    public class CFG : DiGraph<Statement>
    {
        // Starting statement of DEC program, stored as property
        public Statement? Start { get; set; }

        // I think all your base are belong to us
        // Constructs new CFG based on generic DiGraph implementation
        public CFG() : base() { }
    }
}

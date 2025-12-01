using Optimizer;

namespace AST
{
    /// <summary>
    /// Visitor implementation that traverses an Abstract Syntax Tree (AST) and generates a Control Flow Graph (CFG).
    /// Implements the visitor pattern for Statement nodes, building a graph representation of program control flow.
    /// </summary>
    public class ControlFlowGraphGeneratorVisitor : IVisitor<Statement, Statement>
    {
        public CFG _cfg;

        public bool IsStart { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ControlFlowGraphGeneratorVisitor class with an empty CFG.
        /// </summary>
        public ControlFlowGraphGeneratorVisitor()
        {
            _cfg = new CFG();
        }

        /// <summary>
        /// Generates a Control Flow Graph from the provided Abstract Syntax Tree.
        /// Resets internal state to allow multiple CFG generations with the same visitor instance.
        /// </summary>
        /// <param name="ast">The root statement node of the AST to traverse.</param>
        /// <returns>A CFG representing the control flow of the provided AST.</returns>
        public CFG GenerateCFG(Statement ast)
        {
            // Generates new CFG in case of several calls
            _cfg = new CFG();
            // Start statement is not set
            IsStart = false;
            // Begin scanning AST empty CFG (no statements exist yet) 
            ast.Accept(this, null);

            return _cfg;
        }

        #region Binary Operator nodes

        /// <summary>
        /// Visits a plus operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The plus operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(PlusNode node, Statement prev)
        {
            // All node Visit methods don't do anything
            return prev;
        }

        /// <summary>
        /// Visits a minus operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The minus operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(MinusNode node, Statement prev)
        {
            return prev;
        }

        /// <summary>
        /// Visits a multiplication operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The multiplication operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(TimesNode node, Statement prev)
        {
            return prev;
        }

        /// <summary>
        /// Visits a floating-point division operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The float division operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(FloatDivNode node, Statement prev)
        {
            return prev;
        }

        /// <summary>
        /// Visits an integer division operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The integer division operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(IntDivNode node, Statement prev)
        {
            return prev;
        }

        /// <summary>
        /// Visits a modulus operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The modulus operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(ModulusNode node, Statement prev)
        {
            return prev;
        }

        /// <summary>
        /// Visits an exponentiation operator node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The exponentiation operator node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(ExponentiationNode node, Statement prev)
        {
            return prev;
        }

        #endregion

        #region Singleton Expression nodes

        /// <summary>
        /// Visits a literal value node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The literal node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(LiteralNode node, Statement prev)
        {
            return prev;
        }

        /// <summary>
        /// Visits a variable reference node. Expression nodes do not affect control flow.
        /// </summary>
        /// <param name="node">The variable node being visited.</param>
        /// <param name="prev">The previous statement in the control flow.</param>
        /// <returns>The previous statement, unchanged.</returns>
        public Statement Visit(VariableNode node, Statement prev)
        {
            // maybe return null? These shouldn't be visited
            // maybe an exception?
            return prev;
        }

        #endregion

        #region Statement nodes

        /// <summary>
        /// Visits an assignment statement, adding it as a vertex in the CFG and creating an edge from the previous statement.
        /// </summary>
        /// <param name="stmt">The assignment statement being visited.</param>
        /// <param name="prev">The previous statement in the control flow, or null if this is the first statement.</param>
        /// <returns>The current assignment statement, to be used as the previous statement for subsequent nodes.</returns>
        public Statement Visit(AssignmentStmt stmt, Statement prev)
        {
            // Add current statement as vertex
            _cfg.AddVertex(stmt);

            // If previous stmt exists and is not a return, add edge
            if (prev != null) _cfg.AddEdge(prev, stmt);

            return stmt;
        }

        /// <summary>
        /// Visits a return statement, adding it as a vertex in the CFG and creating an edge from the previous statement.
        /// Returns null to indicate that control flow terminates at this point.
        /// </summary>
        /// <param name="stmt">The return statement being visited.</param>
        /// <param name="prev">The previous statement in the control flow, or null if this is the first statement.</param>
        /// <returns>Null, indicating that no statements follow a return statement in the control flow.</returns>
        public Statement Visit(ReturnStmt stmt, Statement prev)
        {
            // Add current statement as vertex
            _cfg.AddVertex(stmt);

            // If previous stmt exists and is not a return, add edge
            if (prev != null) _cfg.AddEdge(prev, stmt);

            return null;
        }

        /// <summary>
        /// Visits a block statement, recursively processing all statements within the block.
        /// Sets the CFG start node to the first non-block statement encountered.
        /// </summary>
        /// <param name="node">The block statement containing a list of statements to process.</param>
        /// <param name="prev">The previous statement in the control flow, or null if this is the first statement.</param>
        /// <returns>The last statement processed in the block, or null if the block ends with a return statement.</returns>
        public Statement Visit(BlockStmt node, Statement prev)
        {
            // Loop through all statements in block
            for (int i = 0; i < node.Statements.Count; i++)
            {
                // If not start and not a block stmt, set Start to current stmt
                if (!IsStart && node.Statements[i].GetType() != typeof(BlockStmt)) 
                {
                    _cfg.Start = node.Statements[i];
                    // Start is set; cannot be any other stmt
                    IsStart = true;
                }
                // Visit current node, then set prev to current node
                prev = node.Statements[i].Accept(this, prev);
            }

            return prev;
        }

        #endregion
    }
}
using Optimizer;

namespace AST
{
    public class ControlFlowGraphGeneratorVisitor : IVisitor<Statement, Statement>
    {
        public CFG _cfg;
        public bool isStart;

        public ControlFlowGraphGeneratorVisitor()
        {
            _cfg = new CFG();
        }

        public CFG GenerateCFG(Statement ast)
        {
            // Generates new CFG in case of several calls
            _cfg = new CFG();

            isStart = false;
            // Begin scanning AST empty CFG (no statements exist yet) 
            ast.Accept(this, null);

            return _cfg;
        }

        #region Binary Operator nodes

        public Statement Visit(PlusNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(MinusNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(TimesNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(FloatDivNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(IntDivNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(ModulusNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(ExponentiationNode node, Statement prev)
        {
            return prev;
        }

        #endregion

        #region Singleton Expression nodes

        public Statement Visit(LiteralNode node, Statement prev)
        {
            return prev;
        }

        public Statement Visit(VariableNode node, Statement prev)
        {
            return prev;
        }

        #endregion

        #region Statement nodes

        public Statement Visit(AssignmentStmt stmt, Statement prev)
        {
            // Add current statement as vertex
            _cfg.AddVertex(stmt);

            // If prev is not null or a return, add edge
            if (prev != null && prev.GetType() != typeof(ReturnStmt)) _cfg.AddEdge(prev, stmt);

            return stmt;
        }

        public Statement Visit(ReturnStmt stmt, Statement prev)
        {
            // Add current statement as vertex (or node)
            _cfg.AddVertex(stmt);

            // If prev is not null or a return, add edge
            if (prev != null && prev.GetType() != typeof(ReturnStmt)) _cfg.AddEdge(prev, stmt);

            return null;
        }

        public Statement Visit(BlockStmt node, Statement prev)
        {
            // find a way to track what Start is
            for (int i = 0; i < node.Statements.Count; i++)
            {
                // If not start and not a block stmt, set start to current stmt
                if (!isStart && node.Statements[i].GetType() != typeof(BlockStmt)) 
                {
                    _cfg.Start = node.Statements[i];
                    isStart = true;
                }

                prev = node.Statements[i].Accept(this, prev);
            }

            return prev;
        }

        #endregion
    }
}
/*
{
    x := (2)
    {
        y := (2 * 4)
        return (x+y)
    }
    z := (3)
    return x
}

*/
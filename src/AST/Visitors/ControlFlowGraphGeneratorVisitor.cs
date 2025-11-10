namespace AST
{
    /// <summary>
    /// Exception thrown when an evaluation error occurs
    /// </summary>
    // public class EvaluationException : Exception
    // {
    //     public EvaluationException(string message) : base(message)
    //     {
    //     }
    // }

    public class ControlFlowGraphGeneratorVisitor : IVisitor<Statement, Statement>
    {
        public CFG _cfg;

        public ControlFlowGraphGeneratorVisitor() { }

        public CFG GenerateCFG(Statement ast)
        {
            // Begin scanning AST empty CFG (no statements exist yet) 
            ast.Accept(this, null);

            return _cfg;
        }

        #region Binary Operator nodes

        public Statement Visit(PlusNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        public Statement Visit(MinusNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        public Statement Visit(TimesNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        public Statement Visit(FloatDivNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        public Statement Visit(IntDivNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        public Statement Visit(ModulusNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        public Statement Visit(ExponentiationNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Singleton Expression nodes

        public Statement Visit(LiteralNode node, Statement prev)
        {
            // return 
            throw new NotImplementedException();
        }

        public Statement Visit(VariableNode node, Statement prev)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Statement nodes

        public Statement Visit(AssignmentStmt stmt, Statement prev)
        {
            _cfg.AddVertex(stmt);
            // something like this idk
            _cfg.AddVertex(stmt);
            if (_cfg.AddVertex(prev)) _cfg.AddEdge(prev, stmt);
            throw new NotImplementedException();
        }

        public Statement Visit(ReturnStmt stmt, Statement prev)
        {
            // bro this is absolute caca. Somone should fix this
            Statement left = stmt.Expression.Accept(this, prev);
            // something like this idk
            _cfg.AddVertex(stmt.Accept(this, prev));
            if (_cfg.AddVertex(prev)) _cfg.AddEdge(prev, stmt);

            Statement empty = null ; 
            _cfg.AddVertex(empty);
            _cfg.AddEdge(stmt, empty);

            return stmt;

            throw new NotImplementedException();
        }

        public Statement Visit(BlockStmt node, Statement prev)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
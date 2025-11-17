using Optimizer;

namespace AST
{
    public class ControlFlowGraphGeneratorVisitor : IVisitor<Statement, Statement>
    {
        public CFG _cfg;

        public ControlFlowGraphGeneratorVisitor()
        {
            _cfg = new CFG();
        }

        public CFG GenerateCFG(Statement ast)
        {
            // Begin scanning AST empty CFG (no statements exist yet) 
            ast.Accept(this, ast);

            return _cfg;
        }

        #region Binary Operator nodes

        public Statement Visit(PlusNode node, Statement prev)
        {
            // implement constant propogation in these methods?
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
            // Add current statement as vertex
            _cfg.AddVertex(stmt);

            // If prev is not a return, add edge
            if (prev.GetType() != typeof(ReturnStmt)) _cfg.AddEdge(prev, stmt);

            return stmt; // ? What do we return
        }

        public Statement Visit(ReturnStmt stmt, Statement prev)
        {
            // Add current statement as vertex (or node)
            _cfg.AddVertex(stmt);

            // If prev is not a return, add edge
            if (prev.GetType() != typeof(ReturnStmt)) _cfg.AddEdge(prev, stmt);

            return stmt; // ? What do we return
        }

        public Statement Visit(BlockStmt node, Statement prev)
        {
            for (int i = 0; i < node.Statements.Count; i++)
            {
                // Pass prev as null (to avoid index range problems)
                if (i == 0) node.Statements[i].Accept(this, null);
                // Visit current statement, pass prev stmt as parameter
                else node.Statements[i].Accept(this, node.Statements[i-1]);
            }

            return node; // ? What do we return
        }

        #endregion
    }
}   
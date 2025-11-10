using AST;

public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
{   

    public void Analyze()
    {
        List<string> errorList = new List<string>();
    }

    #region Binary Operator nodes

    public bool Visit(PlusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }

    public bool Visit(MinusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }

    public bool Visit(TimesNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }

    public bool Visit(FloatDivNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }

    public bool Visit(IntDivNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }

    public bool Visit(ExponentiationNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }
    
    public bool Visit(ModulusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);
        return left && right;
    }

    #endregion

    #region Singleton Expression nodes

    public bool Visit(LiteralNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        return true;
    }

    public bool Visit(VariableNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        return p.Item1.ContainsKey(n.Name);
    }

    #endregion

    #region Statement nodes

    public bool Visit(AssignmentStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        return n.Variable.Accept(this, p) && n.Expression.Accept(this, p);
    }

    public bool Visit(ReturnStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        return n.Expression.Accept(this, p);
    }
    
    public bool Visit(BlockStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        foreach (Statement stmt in n.Statements)
        {
            bool curr = stmt.Accept(this, p);

            if (curr == false) Console.WriteLine($"ERROR: undefined variable in {stmt}");
        }
        return true;
    }
    
    #endregion
}
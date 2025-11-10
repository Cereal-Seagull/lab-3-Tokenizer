using AST;

public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
{

    private List<string> errorList = new List<string>();

    public void Analyze(Statement stmt)
    {
        var st = new SymbolTable<string, object>();
        var tup = new Tuple<SymbolTable<string, object>, Statement>(st, stmt);
        stmt.Accept(this, tup);
        Console.WriteLine(errorList);
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
        var exp = n.Expression.Accept(this, p);
        p.Item1[n.Variable.Name] = exp;
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
            // if (stmt.GetType() == typeof(BlockStmt))
            // {
            //     var childTup = new Tuple<SymbolTable<string, object>, Statement>(new SymbolTable<string, object>(p.Item1), stmt);
                
            //     stmt.Accept(this, childTup);
            // }
            
            bool curr = stmt.Accept(this, p);

            if (curr == false)
            {
                errorList.Add($"ERROR: undefined variable in {stmt}");
            }
        }
        if (errorList.Count > 0) return false;
        else return true;
    }
    
    #endregion
}
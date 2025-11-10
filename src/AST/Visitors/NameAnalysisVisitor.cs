using AST;

public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
{
    // List of encountered errors that can be accessed by any method in class
    private List<string> errorList = new List<string>();

    public void Analyze(Statement ast)
    {
        // New symbol table and tuple for parent block stmt
        var st = new SymbolTable<string, object>();
        var tup = new Tuple<SymbolTable<string, object>, Statement>(st, ast);

        // Visits entire block stmt
        ast.Accept(this, tup);

        // Prints all errors encountered
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
        // Literals are not variables; vacuously true
        return true;
    }

    public bool Visit(VariableNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Returns whether variable exists in symbol table
        return p.Item1.ContainsKey(n.Name);
    }

    #endregion

    #region Statement nodes

    public bool Visit(AssignmentStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Passes in current symbol table; Statement becomes current assignment stmt
        p = new Tuple<SymbolTable<string, object>, Statement>(p.Item1, n);

        // Checks right expression
        bool right = n.Expression.Accept(this, p);

        // Only add left side to symboltable if exp is valid
        // Example: (y := y + 1) only works if y already previously defined
        if (right) p.Item1[n.Variable.Name] = right;

        // Check left side
        bool left = n.Variable.Accept(this, p);

        // Return validity of both expressions
        return left && right;
    }

    public bool Visit(ReturnStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Passes in current symbol table; Statement becomes current return stmt
        p = new Tuple<SymbolTable<string, object>, Statement>(p.Item1, n);

        return n.Expression.Accept(this, p);
    }

    public bool Visit(BlockStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        var st = new SymbolTable<string, object>();
        var s = new BlockStmt(new SymbolTable<string, object>());


        foreach (Statement stmt in n.Statements)
        {
            // if (stmt.GetType() == typeof(BlockStmt))
            // {
            //     var childTup = new Tuple<SymbolTable<string, object>, Statement>(new SymbolTable<string, object>(p.Item1), stmt);

            //     stmt.Accept(this, childTup);
            // }

            // Visits and analyzes current statement
            bool curr = stmt.Accept(this, p);

            // If variable undefined, add error to error list
            if (curr == false) errorList.Add($"ERROR: undefined variable in {stmt}");
        }

        // If error list is not empty, return false
        return errorList.Count == 0;

    }
    
    #endregion
}
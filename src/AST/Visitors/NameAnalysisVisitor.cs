using AST;

public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
{
    // List of encountered errors that can be accessed by any method in class
    private List<string> errorList = new List<string>();

    /// <summary>
    /// Analyzes the given AST for name resolution errors and reports any issues found
    /// </summary>
    /// <param name="ast">The AST statement to analyze</param>
    public void Analyze(Statement ast)
    {
        // New symbol table and tuple for parent block stmt
        var st = new SymbolTable<string, object>();
        var tup = new Tuple<SymbolTable<string, object>, Statement>(st, ast);

        // Visits entire block stmt
        bool analyze = ast.Accept(this, tup);

        if (analyze) Console.WriteLine("0 errors encountered!");
        // Prints all errors encountered
        else
        {
            foreach (string err in errorList)
            {
                Console.WriteLine(err);
            }
        }
        
    }

    #region Binary Operator nodes

    /// <summary>
    /// Visits a PlusNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The PlusNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(PlusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }

    /// <summary>
    /// Visits a MinusNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The MinusNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(MinusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }

    /// <summary>
    /// Visits a TimesNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The TimesNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(TimesNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }

    /// <summary>
    /// Visits a FloatDivNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The FloatDivNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(FloatDivNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }

    /// <summary>
    /// Visits an IntDivNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The IntDivNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(IntDivNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }

    /// <summary>
    /// Visits an ExponentiationNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The ExponentiationNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(ExponentiationNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }
    
    /// <summary>
    /// Visits a ModulusNode and checks both operands for name resolution errors
    /// </summary>
    /// <param name="n">The ModulusNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both operands are valid, false otherwise</returns>
    public bool Visit(ModulusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Check left and right expressions
        bool left = n.Left.Accept(this, p);
        bool right = n.Right.Accept(this, p);

        return left && right;
    }

    #endregion

    #region Singleton Expression nodes

    /// <summary>
    /// Visits a LiteralNode, which is always valid as literals are not variables
    /// </summary>
    /// <param name="n">The LiteralNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>Always returns true since literals are vacuously valid</returns>
    public bool Visit(LiteralNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Literals are not variables; vacuously true
        return true;
    }

    /// <summary>
    /// Visits a VariableNode and checks if the variable exists in the symbol table
    /// </summary>
    /// <param name="n">The VariableNode to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if the variable exists in the symbol table, false otherwise</returns>
    public bool Visit(VariableNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Returns whether variable exists in symbol table
        return p.Item1.ContainsKey(n.Name);
    }

    #endregion

    #region Statement nodes

    /// <summary>
    /// Visits an AssignmentStmt and validates the assignment expression, adding the variable to the symbol table if valid
    /// </summary>
    /// <param name="n">The AssignmentStmt to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if both the expression and variable are valid, false otherwise</returns>
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

    /// <summary>
    /// Visits a ReturnStmt and validates the return expression
    /// </summary>
    /// <param name="n">The ReturnStmt to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if the return expression is valid, false otherwise</returns>
    public bool Visit(ReturnStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Passes in current symbol table; Statement becomes current return stmt
        p = new Tuple<SymbolTable<string, object>, Statement>(p.Item1, n);

        return n.Expression.Accept(this, p);
    }

    /// <summary>
    /// Visits a BlockStmt and analyzes all statements within the block, collecting any errors
    /// </summary>
    /// <param name="n">The BlockStmt to analyze</param>
    /// <param name="p">Tuple containing the symbol table and parent statement</param>
    /// <returns>True if no errors were encountered in any statement, false otherwise</returns>
    public bool Visit(BlockStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        // Iterates through each statement in the block
        foreach (Statement stmt in n.Statements)
        {
            // Visits and analyzes current statement
            bool curr = stmt.Accept(this, p);

            // If variable undefined, add error to error list
            if (curr == false) errorList.Add($"Undefined variable in {stmt.GetType()} {stmt}" + Environment.NewLine);
        }

        // If error list is not empty, return false
        return errorList.Count == 0;
    }
    
    #endregion
}
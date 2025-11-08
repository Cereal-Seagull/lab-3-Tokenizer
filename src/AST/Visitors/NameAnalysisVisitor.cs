using AST;

public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
{

    // WILL USE EXCEPTIONS!!!!
    // Analyzes ENTIRE tree regardless of errors or returns
    
    #region Binary Operator nodes

    public bool Visit(PlusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(MinusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(TimesNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(FloatDivNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(IntDivNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(ExponentiationNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }
    
    public bool Visit(ModulusNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Singleton Expression nodes

    public bool Visit(LiteralNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(VariableNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Statement nodes

    public bool Visit(AssignmentStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(BlockStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }

    public bool Visit(ReturnStmt n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }
    
    #endregion
}
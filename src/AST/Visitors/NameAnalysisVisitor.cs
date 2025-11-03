using AST;
public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
{
    #region Basic Nodes
    public bool Visit(LiteralNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }
    public bool Visit(VariableNode n, Tuple<SymbolTable<string, object>, Statement> p)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Operator Visits
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
    #region Statement Visits
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
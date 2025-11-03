using AST;

public class EvaluateVisitor : IVisitor<SymbolTable<string,object>, object>
{
    #region Binary Operator nodes
    
    public object Visit(PlusNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(MinusNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(TimesNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(FloatDivNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(IntDivNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(ModulusNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(ExponentiationNode node, SymbolTable<string, object> st)
    {
        throw new NotImplementedException();
    }

    #endregion
    
    #region Singleton Expression nodes

    public object Visit(LiteralNode node, SymbolTable<string, object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(VariableNode node, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Statement nodes

    public object Visit(AssignmentStmt stmt, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(ReturnStmt stmt, SymbolTable<string,object> st)
    {
        throw new NotImplementedException();
    }

    public object Visit(BlockStmt node, SymbolTable<string, object> st)
    {
        throw new NotImplementedException();
    }
    #endregion
}
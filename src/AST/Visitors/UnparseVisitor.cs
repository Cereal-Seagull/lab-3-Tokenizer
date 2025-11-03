using AST;

public class UnparseVisitor : IVisitor<int, string>
{
    #region Binary Operator nodes
    
    public string Visit(PlusNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(MinusNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(TimesNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(FloatDivNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(IntDivNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(ModulusNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(ExponentiationNode node, int level)
    {
        throw new NotImplementedException();
    }

    #endregion
    
    #region Singleton Expression nodes

    public string Visit(LiteralNode node, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(VariableNode node, int level)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Statement nodes

    public string Visit(AssignmentStmt stmt, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(ReturnStmt stmt, int level)
    {
        throw new NotImplementedException();
    }

    public string Visit(BlockStmt node, int level)
    {
        throw new NotImplementedException();
    }
    
    #endregion
}
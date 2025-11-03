using System.Xml;
using AST;

public class EvaluateVisitor : IVisitor<SymbolTable<string,object>, object>
{
    #region Binary Operator nodes

    private void NumberCaster(object node)
    {
        node = (int)node;        
    }
    
    public object Visit(PlusNode node, SymbolTable<string,object> st)
    {
        return (int)node.Left.Accept(this, st) - (int)node.Right.Accept(this, st);
    }

    public object Visit(MinusNode node, SymbolTable<string,object> st)
    {
        return (int)node.Left.Accept(this, st) - (int)node.Right.Accept(this, st);
    }

    public object Visit(TimesNode node, SymbolTable<string,object> st)
    {
        return (int)node.Left.Accept(this, st) * (int)node.Right.Accept(this, st);
    }

    public object Visit(FloatDivNode node, SymbolTable<string,object> st)
    {
        var right = (float)node.Right.Accept(this, st);
        if (right.Equals(0)) throw new DivideByZeroException();
        float exp = (float)node.Left.Accept(this, st) / right;
        return exp;
    }

    public object Visit(IntDivNode node, SymbolTable<string,object> st)
    {
        var right = (int)node.Right.Accept(this, st);
        if (right.Equals(0)) throw new DivideByZeroException();
        int exp = (int)node.Left.Accept(this, st) / right;
        return exp;
    }

    public object Visit(ModulusNode node, SymbolTable<string,object> st)
    {
        return (int)node.Left.Accept(this, st) % (int)node.Right.Accept(this, st);
    }

    public object Visit(ExponentiationNode node, SymbolTable<string, object> st)
    {
        return Math.Pow((int)node.Left.Accept(this, st), (int)node.Right.Accept(this, st));
    }

    #endregion
    
    #region Singleton Expression nodes

    public object Visit(LiteralNode node, SymbolTable<string, object> st)
    {
        return node.Value;
    }

    public object Visit(VariableNode node, SymbolTable<string,object> st)
    {
        return st[node.Name];
    }

    #endregion

    #region Statement nodes

    public object Visit(AssignmentStmt stmt, SymbolTable<string,object> st)
    {
        st[stmt.Variable.Name] = stmt.Expression.Accept(this, st);
        return st[stmt.Variable.Name];
    }

    public object Visit(ReturnStmt stmt, SymbolTable<string,object> st)
    {
        return stmt.Expression.Accept(this, st);
    }

    public object Visit(BlockStmt node, SymbolTable<string, object> st)
    {
        int i = 0;
        while (i < node.Statements.Count)
        {
            Statement stmt = node.Statements[i];
            if (stmt is ReturnStmt) break;
            stmt.Accept(this, st);
            i++;
        }
        // otherwise, return last known value
        return node.Statements[i].Accept(this, st);
    }
    #endregion
}
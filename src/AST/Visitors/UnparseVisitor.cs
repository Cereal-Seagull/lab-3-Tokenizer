using System.Text;
using AST;
using Tokenizer;

public class UnparseVisitor : IVisitor<int, string>
{
    #region Binary Operator nodes
    
    public string Visit(PlusNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.PLUS + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    public string Visit(MinusNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.SUBTRACTION + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    public string Visit(TimesNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.TIMES + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    public string Visit(FloatDivNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.FLOAT_DIVISION + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    public string Visit(IntDivNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.INT_DIVISION + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    public string Visit(ModulusNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.MODULUS + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    public string Visit(ExponentiationNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.LEFT_PAREN + node.Left.Accept(this, 0) 
            + " " + TokenConstants.EXPONENTIATE + " " + node.Right.Accept(this, 0) + TokenConstants.RIGHT_PAREN;
    }

    #endregion
    
    #region Singleton Expression nodes

    public string Visit(LiteralNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + node.Value.ToString();
    }

    public string Visit(VariableNode node, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + node.Name;
    }

    #endregion

    #region Statement nodes

    public string Visit(AssignmentStmt stmt, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + stmt.Variable.Accept(this, 0) +
            TokenConstants.ASSIGNMENT + stmt.Expression.Accept(this, 0);
    }

    public string Visit(ReturnStmt stmt, int level = 0)
    {
        return GeneralUtils.GetIndentation(level) + TokenConstants.RETURN + " " +
            stmt.Expression.Accept(this, 0);
    }

    public string Visit(BlockStmt node, int level = 0)
    {
        StringBuilder str = new StringBuilder();

        // Add left curly brace
        str.Append(GeneralUtils.GetIndentation(level));
        str.AppendLine(TokenConstants.LEFT_CURLY);
        
        // Increment indentation
        level++;

        // Append all statements in block stmt (including block stmts)
        for (int i = 0; i < node.Statements.Count; i++)
        {
            str.AppendLine(node.Statements[i].Accept(this, level));
        }

        // Add right curly brace
        str.Append(GeneralUtils.GetIndentation(level - 1));
        str.AppendLine(TokenConstants.RIGHT_CURLY);

        return str.ToString();
    }
    
    #endregion
}
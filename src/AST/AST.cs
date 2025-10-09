using Tokenizer;

namespace AST
{

    #region Nodes
    public abstract class ExpressionNode { }
    public class LiteralNode : ExpressionNode
    {
        protected object Value { get; private set; }
        public LiteralNode(object val) { Value = val; }  
    }
    public class VariableNode : ExpressionNode
    {
        protected string Name;
        public VariableNode(string str) { Name = str; }
    }

    #endregion
    #region Operators
    public abstract class Operator : ExpressionNode { }
    public abstract class BinaryOperator : Operator
    {
        protected ExpressionNode Left;
        protected string SIGN;
        protected ExpressionNode Right;

        public BinaryOperator(ExpressionNode l, string s, ExpressionNode r)
        {
            Left = l;
            SIGN = s;
            Right = r;
        }

        public void SetLeft(ExpressionNode l) { Left = l; }
        public void SetRight(ExpressionNode r) { Right = r; }
        public void SetChildren(ExpressionNode l, ExpressionNode r) { SetLeft(l); SetRight(r); }
    }
    // null likely is placeholder for null node to be implemented in NullBuilder
    public class PlusNode : BinaryOperator
    {
        public PlusNode() : base(null, TokenConstants.PLUS, null) { }
    }
    public class MinusNode : BinaryOperator
    {
        public MinusNode() : base(null, TokenConstants.SUBTRACTION, null) { }
    }
    public class TimesNode : BinaryOperator
    {
        public TimesNode() : base(null, TokenConstants.TIMES, null) { }
    }
    public class FloatDivNode : BinaryOperator
    {
        public FloatDivNode() : base(null, TokenConstants.FLOAT_DIVISION, null) { }
    }
    public class IntDivNode : BinaryOperator
    {
        public IntDivNode() : base(null, TokenConstants.INT_DIVISION, null) { }
    }
    public class ModulusNode : BinaryOperator
    {
        public ModulusNode() : base(null, TokenConstants.MODULUS, null) { }
    }
    public class ExponentiationNode : BinaryOperator
    {
        public ExponentiationNode() : base(null, TokenConstants.EXPONENTIATE, null) { }
    }
    #endregion
    #region Statements
    public abstract class Statement { }
    public class BlockStmt : Statement
    {
        SymbolTable<string, object> Block;
        public BlockStmt(SymbolTable<string, object> st)
        {
            Block = st;
        }
    }
    public class AssignmentStmt : Statement
    {
        protected VariableNode Var;
        protected const string SIGN = TokenConstants.ASSIGNMENT;
        protected ExpressionNode Exp;
        public AssignmentStmt(VariableNode v, ExpressionNode x)
        {
            Var = v;
            Exp = x;   
        }        
    }
    public class ReturnStmt : Statement
    {
        protected ExpressionNode Exp;
        protected const string SIGN = TokenConstants.RETURN;
        public ReturnStmt(ExpressionNode x)
        {
            Exp = x;
        }
    }
    #endregion    
}
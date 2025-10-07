using Tokenizer;

namespace AST
{ 

    public abstract class ExpressionNode { }
    public class LiteralNode : ExpressionNode
    {
        protected object Value;
        public LiteralNode(object val) { Value = val; }    
    }
    public class VariableNode : ExpressionNode
    {
        protected string Name;
        public VariableNode(string str) { Name = str; }
    }


    public abstract class Operator : ExpressionNode { }
    public abstract class BinaryOperator : Operator
    {
        protected ExpressionNode Left;
        protected object Data;
        protected ExpressionNode Right;

        public BinaryOperator(ExpressionNode l, object d, ExpressionNode r)
        {
            Left = l;
            Data = d;
            Right = r;
        }
    }
    // null likely is placeholder for null node to be implemented in NullBuilder
    public class PlusNode : BinaryOperator { public PlusNode() : base(null, TokenConstants.PLUS, null) { } }
    public class MinusNode : BinaryOperator { public MinusNode() : base(null, TokenConstants.SUBTRACTION, null) { } }
    public class TimesNode : BinaryOperator { public TimesNode() : base(null, TokenConstants.TIMES, null) { } }
    public class FloatDivNode : BinaryOperator { public FloatDivNode() : base(null, TokenConstants.FLOAT_DIVISION, null) { } }
    public class IntDivNode : BinaryOperator { public IntDivNode() : base(null, TokenConstants.INT_DIVISION, null) { } }
    public class ModulusNode : BinaryOperator { public ModulusNode() : base(null, TokenConstants.MODULUS, null) { } }
    public class ExponentiationNode : BinaryOperator { public ExponentiationNode() : base(null, TokenConstants.EXPONENTIATE, null) { } }

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
        public ReturnStmt(ExpressionNode x)
        {
            Exp = x;
        } 
    }
    
}
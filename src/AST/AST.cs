using System.Text;
using Tokenizer;

namespace AST
{
    #region Nodes


    public abstract class ExpressionNode
    {
        public abstract string Unparse(int level = 0);
    }

    public class LiteralNode : ExpressionNode
    {
        protected object Value { get; private set; }
        public LiteralNode(object val) { Value = val; } // do we want Value to be object or int? 
                                                        //need ints and floats

        public override string Unparse(int level)
        {
            return GeneralUtils.GetIndentation(level) + Value.ToString();
        }
    }
    
    public class VariableNode : ExpressionNode
    {
        protected string Name { get; private set; }

        public VariableNode(string str) { Name = str; }

        public override string Unparse(int level)
        {
            return GeneralUtils.GetIndentation(level) + Name;
        }
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

        public override string Unparse(int level)
        {
            StringBuilder str = new StringBuilder();

            str.Append(Left.Unparse(level));
            str.Append(" ");
            str.Append(SIGN);
            str.Append(" ");
            str.Append(Right.Unparse(0));

            return str.ToString();
        }

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


    public abstract class Statement
    {
        public abstract string Unparse(int level = 0);
    }

    public class BlockStmt : Statement
    {
        private List<Statement> Statements;

        public BlockStmt(List<Statement> lst)
        {
            Statements = lst;
        }

        // Adds a statement to Block
        public void AddStmt(Statement obj)
        {
            Statements.Add(obj);
        }

        public override string Unparse(int level)
        {
            StringBuilder str = new StringBuilder();

            str.Append(TokenConstants.LEFT_CURLY);
            str.Append("\n");

            // Call Unparse() on child nodes
            for (int i = 0; i < Statements.Count(); i++)
            {
                str.Append(Statements[i].Unparse(level));
                str.Append("\n");
            }
            
            str.Append(TokenConstants.RIGHT_CURLY);

            return str.ToString();
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

        public override string Unparse(int level)
        {
            return GeneralUtils.GetIndentation(level) + Var.Unparse(0) + " " + SIGN 
                                                      + " " + Exp.Unparse(0);
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

        public override string Unparse(int level)
        {
            return GeneralUtils.GetIndentation(level) + SIGN + " " + Exp.Unparse(0);
        }
    }
    
    #endregion    
}
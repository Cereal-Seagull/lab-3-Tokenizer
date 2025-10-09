namespace AST
{
    public abstract class ExpressionNode { }
    public abstract class Operator : ExpressionNode { }
    public abstract class BinaryOperator : Operator
    {
        public ExpressionNode? Left { get; set; }
        public ExpressionNode? Right { get; set; }
        public object Data { get; set; }

        public BinaryOperator()
        {
            Left = null;
            Right = null;
        }
    }

    public class PlusNode : BinaryOperator
    {
        public PlusNode() : base()
        {
            Data = "+";
        }
    }
    public class MinusNode : BinaryOperator
    {
        public MinusNode() : base()
        {
            Data = "-";
        }
    }
    public class TimesNode : BinaryOperator
    { 
        public TimesNode() : base()
        {
            Data = "*";
        }
    }
    public class FloatDivNode : BinaryOperator { }
    public class IntDivNode : BinaryOperator { }
    public class ModulusNode : BinaryOperator { }
    public class ExponentiationNode : BinaryOperator { }

    public class LiteralNode : ExpressionNode
    {
        public object Value { get; set; }
        public LiteralNode(object val)
        {
            Value = val;
        }
    }

    public class VariableNode : ExpressionNode
    {
        public string Name { get; set; }
        public VariableNode(string str)
        {
            Name = str;
        }
    }

    public abstract class Statement { }
    public class BlockStmt : Statement
    {
        SymbolTable<string, object> symbolTable = new SymbolTable<string, object>();
    }
    public class AssignmentStmt : Statement { }
    public class ReturnStmt : Statement { }
    
}
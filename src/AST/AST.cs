namespace AST
{

    public class LiteralNode { }
    public class VariableNode { }

    public abstract class ExpressionNode { }
    public abstract class Operator : ExpressionNode { }
    public abstract class BinaryOperator : Operator { }
    public class PlusNode : BinaryOperator { }
    public class MinusNode : BinaryOperator { }
    public class TimesNode : BinaryOperator { }
    public class FloatDivNode : BinaryOperator { }
    public class IntDivNode : BinaryOperator { }
    public class ModulusNode : BinaryOperator { }
    public class ExponentiationNode : BinaryOperator { }

    public abstract class Statement { }
    public class BlockStmt : Statement { }
    public class AssignmentStmt : Statement { }
    public class ReturnStmt : Statement { }
    
}
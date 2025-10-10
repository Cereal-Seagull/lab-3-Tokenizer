namespace AST
{
    /// <summary>
    /// DebugBuilder that does not create any objects; useful for assessing parsing problems
    /// while avoiding object creation
    /// </summary>
    public class DebugBuilder : DefaultBuilder
    {
        // Override all creation methods to return null
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Plus node created");
            return base.CreatePlusNode(left, right);
        }

        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Minus node created");
            return base.CreateMinusNode(left, right);
        }

        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Times node created");
            return base.CreateTimesNode(left, right);
        }

        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Float Division node created");
            return base.CreateFloatDivNode(left, right);
        }

        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Integer Division Node created.");
            return base.CreateIntDivNode(left, right);
        }

        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Modulus node created.");
            return base.CreateModulusNode(left, right);
        }

        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Exponentiation Node created.");
            return base.CreateExponentiationNode(left, right);
        }

        public override LiteralNode CreateLiteralNode(object value)
        {
            Console.WriteLine("Literal Node created.");
            return base.CreateLiteralNode(value);
        }

        public override VariableNode CreateVariableNode(string name)
        {
            Console.WriteLine("Variable Node created.");
            return base.CreateVariableNode(name);
        }

        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Console.WriteLine("Assignment Node created.");
            return base.CreateAssignmentStmt(variable, expression);
        }

        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            Console.WriteLine("Return Statement Created.");
            return base.CreateReturnStmt(expression);
        }

        public override BlockStmt CreateBlockStmt(List<Statement> lst)
        {
            Console.WriteLine("Block Statement Created.");
            return base.CreateBlockStmt(lst);
        }
    }
}
namespace AST
{
    /// <summary>
    /// DebugBuilder that does not create any objects; useful for assessing parsing problems
    /// while avoiding object creation
    /// </summary>
    public class DebugBuilder : DefaultBuilder
    {
        /// <summary>
        /// Creates a PlusNode for addition operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The left operand of the addition operation.</param>
        /// <param name="right">The right operand of the addition operation.</param>
        /// <returns>A <see cref="PlusNode"/> with the specified operands.</returns>
        // Override all creation methods to return null
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Plus node created");
            return base.CreatePlusNode(left, right);
        }

        /// <summary>
        /// Creates a MinusNode for subtraction operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The left operand of the subtraction operation.</param>
        /// <param name="right">The right operand of the subtraction operation.</param>
        /// <returns>A <see cref="MinusNode"/> with the specified operands.</returns>
        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Minus node created");
            return base.CreateMinusNode(left, right);
        }

        /// <summary>
        /// Creates a TimesNode for multiplication operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The left operand of the multiplication operation.</param>
        /// <param name="right">The right operand of the multiplication operation.</param>
        /// <returns>A <see cref="TimesNode"/> with the specified operands.</returns>
        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Times node created");
            return base.CreateTimesNode(left, right);
        }

        /// <summary>
        /// Creates a FloatDivNode for floating-point division operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The left operand of the division operation.</param>
        /// <param name="right">The right operand of the division operation.</param>
        /// <returns>A <see cref="FloatDivNode"/> with the specified operands.</returns>
        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Float Division node created");
            return base.CreateFloatDivNode(left, right);
        }

        /// <summary>
        /// Creates an IntDivNode for integer division operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The left operand of the integer division operation.</param>
        /// <param name="right">The right operand of the integer division operation.</param>
        /// <returns>An <see cref="IntDivNode"/> with the specified operands.</returns>
        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Integer Division Node created.");
            return base.CreateIntDivNode(left, right);
        }

        /// <summary>
        /// Creates a ModulusNode for modulus operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The left operand of the modulus operation.</param>
        /// <param name="right">The right operand of the modulus operation.</param>
        /// <returns>A <see cref="ModulusNode"/> with the specified operands.</returns>
        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Modulus node created.");
            return base.CreateModulusNode(left, right);
        }

        /// <summary>
        /// Creates an ExponentiationNode for exponentiation operations and outputs debug information to the console.
        /// </summary>
        /// <param name="left">The base operand of the exponentiation operation.</param>
        /// <param name="right">The exponent operand of the exponentiation operation.</param>
        /// <returns>An <see cref="ExponentiationNode"/> with the specified operands.</returns>
        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Exponentiation Node created.");
            return base.CreateExponentiationNode(left, right);
        }

        /// <summary>
        /// Creates a LiteralNode for constant values and outputs debug information to the console.
        /// </summary>
        /// <param name="value">The literal value to store in the node (e.g., integer, float, string, boolean).</param>
        /// <returns>A <see cref="LiteralNode"/> containing the specified value.</returns>
        public override LiteralNode CreateLiteralNode(object value)
        {
            Console.WriteLine("Literal Node created.");
            return base.CreateLiteralNode(value);
        }

        /// <summary>
        /// Creates a VariableNode for variable references and outputs debug information to the console.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>A <see cref="VariableNode"/> with the specified name.</returns>
        public override VariableNode CreateVariableNode(string name)
        {
            Console.WriteLine("Variable Node created.");
            return base.CreateVariableNode(name);
        }

        /// <summary>
        /// Creates an AssignmentStmt for variable assignment operations and outputs debug information to the console.
        /// </summary>
        /// <param name="variable">The variable node representing the target of the assignment.</param>
        /// <param name="expression">The expression node representing the value to assign.</param>
        /// <returns>An <see cref="AssignmentStmt"/> with the specified variable and expression.</returns>
        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Console.WriteLine("Assignment Node created.");
            return base.CreateAssignmentStmt(variable, expression);
        }

        /// <summary>
        /// Creates a ReturnStmt for return statements and outputs debug information to the console.
        /// </summary>
        /// <param name="expression">The expression node representing the value to return.</param>
        /// <returns>A <see cref="ReturnStmt"/> with the specified expression.</returns>
        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            Console.WriteLine("Return Statement Created.");
            return base.CreateReturnStmt(expression);
        }

        /// <summary>
        /// Creates a BlockStmt for code blocks and outputs debug information to the console.
        /// </summary>
        /// <param name="lst">The list of statements to include in the block.</param>
        /// <returns>A <see cref="BlockStmt"/> containing the specified statements.</returns>
        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> st)
        {
            Console.WriteLine("Block Statement Created.");
            return base.CreateBlockStmt(st);
        }
    }
}
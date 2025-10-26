namespace AST
{
    /// <summary>
    /// DefaultBuilder that creates AST node objects using the builder pattern.
    /// Provides virtual factory methods for creating expression nodes, operator nodes, and statement nodes.
    /// Can be inherited to customize node creation behavior.
    /// </summary>
    public class DefaultBuilder
    {
        #region Operator Node Creation

        /// <summary>
        /// Creates a PlusNode for addition operations.
        /// </summary>
        /// <param name="left">The left operand of the addition operation.</param>
        /// <param name="right">The right operand of the addition operation.</param>
        /// <returns>A <see cref="PlusNode"/> with the specified left and right operands.</returns>
        public virtual PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            PlusNode pn = new PlusNode(left, right);
            return pn;
        }

        /// <summary>
        /// Creates a MinusNode for subtraction operations.
        /// </summary>
        /// <param name="left">The left operand of the subtraction operation.</param>
        /// <param name="right">The right operand of the subtraction operation.</param>
        /// <returns>A <see cref="MinusNode"/> with the specified left and right operands.</returns>
        public virtual MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            MinusNode mn = new MinusNode(left, right);
            return mn;
        }

        /// <summary>
        /// Creates a TimesNode for multiplication operations.
        /// </summary>
        /// <param name="left">The left operand of the multiplication operation.</param>
        /// <param name="right">The right operand of the multiplication operation.</param>
        /// <returns>A <see cref="TimesNode"/> with the specified left and right operands.</returns>
        public virtual TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            TimesNode tn = new TimesNode(left, right);
            return tn;
        }

        /// <summary>
        /// Creates a FloatDivNode for floating-point division operations.
        /// </summary>
        /// <param name="left">The left operand of the division operation (dividend).</param>
        /// <param name="right">The right operand of the division operation (divisor).</param>
        /// <returns>A <see cref="FloatDivNode"/> with the specified left and right operands.</returns>
        public virtual FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            FloatDivNode fdn = new FloatDivNode(left, right);
            return fdn;
        }

        /// <summary>
        /// Creates an IntDivNode for integer division operations.
        /// </summary>
        /// <param name="left">The left operand of the integer division operation (dividend).</param>
        /// <param name="right">The right operand of the integer division operation (divisor).</param>
        /// <returns>An <see cref="IntDivNode"/> with the specified left and right operands.</returns>
        public virtual IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            IntDivNode idn = new IntDivNode(left, right);
            return idn;
        }

        /// <summary>
        /// Creates a ModulusNode for modulus operations.
        /// </summary>
        /// <param name="left">The left operand of the modulus operation (dividend).</param>
        /// <param name="right">The right operand of the modulus operation (divisor).</param>
        /// <returns>A <see cref="ModulusNode"/> with the specified left and right operands.</returns>
        public virtual ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            ModulusNode mdn = new ModulusNode(left, right);
            return mdn;
        }

        /// <summary>
        /// Creates an ExponentiationNode for exponentiation operations.
        /// </summary>
        /// <param name="left">The base operand of the exponentiation operation.</param>
        /// <param name="right">The exponent operand of the exponentiation operation.</param>
        /// <returns>An <see cref="ExponentiationNode"/> with the specified base and exponent operands.</returns>
        public virtual ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            ExponentiationNode expn = new ExponentiationNode(left, right);
            return expn;
        }

        #endregion
        #region Expression Creation

        /// <summary>
        /// Creates a LiteralNode for constant values.
        /// </summary>
        /// <param name="value">The literal value to store in the node. Can be any type such as int, float, string, or boolean.</param>
        /// <returns>A <see cref="LiteralNode"/> containing the specified value.</returns>
        public virtual LiteralNode CreateLiteralNode(object value)
        {
            return new LiteralNode(value);
        }

        /// <summary>
        /// Creates a VariableNode for variable references.
        /// </summary>
        /// <param name="name">The name of the variable to reference.</param>
        /// <returns>A <see cref="VariableNode"/> with the specified variable name.</returns>
        public virtual VariableNode CreateVariableNode(string name)
        {
            return new VariableNode(name);
        }

        #endregion
        #region Statement Creation

        /// <summary>
        /// Creates an AssignmentStmt for variable assignment operations.
        /// </summary>
        /// <param name="variable">The variable node representing the target of the assignment.</param>
        /// <param name="expression">The expression node representing the value to assign to the variable.</param>
        /// <returns>An <see cref="AssignmentStmt"/> that assigns the expression value to the variable.</returns>
        public virtual AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            return new AssignmentStmt(variable, expression);
        }

        /// <summary>
        /// Creates a ReturnStmt for return statements.
        /// </summary>
        /// <param name="expression">The expression node representing the value to return.</param>
        /// <returns>A <see cref="ReturnStmt"/> that returns the value of the specified expression.</returns>
        public virtual ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            return new ReturnStmt(expression);
        }

        /// <summary>
        /// Creates a BlockStmt for code blocks containing multiple statements.
        /// </summary>
        /// <param name="lst">The list of statements to include in the block.</param>
        /// <returns>A <see cref="BlockStmt"/> containing the specified list of statements.</returns>
        public virtual BlockStmt CreateBlockStmt(SymbolTable<string, object> st)
        {
            return new BlockStmt(st);
        }

        #endregion
    }
}
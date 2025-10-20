using System;
using System.Collections.Generic;

namespace AST
{
    /// <summary>
    /// NullBuilder that does not create any objects; useful for assessing parsing problems
    /// while avoiding object creation
    /// </summary>
    public class NullBuilder : DefaultBuilder
    {
        /// <summary>
        /// Override that returns null instead of creating a PlusNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The left operand of the addition operation (ignored).</param>
        /// <param name="right">The right operand of the addition operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        // Override all creation methods to return null
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a MinusNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The left operand of the subtraction operation (ignored).</param>
        /// <param name="right">The right operand of the subtraction operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a TimesNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The left operand of the multiplication operation (ignored).</param>
        /// <param name="right">The right operand of the multiplication operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a FloatDivNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The left operand of the division operation (ignored).</param>
        /// <param name="right">The right operand of the division operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating an IntDivNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The left operand of the integer division operation (ignored).</param>
        /// <param name="right">The right operand of the integer division operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a ModulusNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The left operand of the modulus operation (ignored).</param>
        /// <param name="right">The right operand of the modulus operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating an ExponentiationNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="left">The base operand of the exponentiation operation (ignored).</param>
        /// <param name="right">The exponent operand of the exponentiation operation (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a LiteralNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="value">The literal value (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override LiteralNode CreateLiteralNode(object value)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a VariableNode.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="name">The variable name (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override VariableNode CreateVariableNode(string name)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating an AssignmentStmt.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="variable">The variable node representing the target of the assignment (ignored).</param>
        /// <param name="expression">The expression node representing the value to assign (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a ReturnStmt.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="expression">The expression node representing the value to return (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            return null;
        }

        /// <summary>
        /// Override that returns null instead of creating a BlockStmt.
        /// Used for testing parsing logic without the overhead of object creation.
        /// </summary>
        /// <param name="lst">The list of statements to include in the block (ignored).</param>
        /// <returns>Always returns null.</returns>
        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> st)
        {
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AST
{
    /// <summary>
    /// DefaultBuilder that does not create any objects; useful for assessing parsing problems
    /// </summary>
    public class DefaultBuilder
    {
        // Override all creation methods to return null
        public PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public LiteralNode CreateLiteralNode(object value)
        {
            throw new NotImplementedException();
        }

        public VariableNode CreateVariableNode(string name)
        {
            throw new NotImplementedException();
        }

        public AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            throw new NotImplementedException();
        }

        public ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            throw new NotImplementedException();
        }

        public BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            throw new NotImplementedException();
        }
    }
}
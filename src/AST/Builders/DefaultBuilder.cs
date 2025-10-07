namespace AST
{
    /// <summary>
    /// DefaultBuilder that does not create any objects; useful for assessing parsing problems
    /// </summary>
    public class DefaultBuilder
    {
        // Override all creation methods to return null
        public virtual PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public virtual LiteralNode CreateLiteralNode(object value)
        {
            throw new NotImplementedException();
        }

        public virtual VariableNode CreateVariableNode(string name)
        {
            throw new NotImplementedException();
        }

        public virtual AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            throw new NotImplementedException();
        }

        public virtual ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            throw new NotImplementedException();
        }

        public virtual BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            throw new NotImplementedException();
        }
    }
}
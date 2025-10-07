namespace AST
{
    /// <summary>
    /// NullBuilder that does not create any objects; useful for assessing parsing problems
    /// while avoiding object creation
    /// </summary>
    public class DebugBuilder : DefaultBuilder
    {
        // Override all creation methods to return null
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            throw new NotImplementedException();
        }

        public override LiteralNode CreateLiteralNode(object value)
        {
            throw new NotImplementedException();
        }

        public override VariableNode CreateVariableNode(string name)
        {
            throw new NotImplementedException();
        }

        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            throw new NotImplementedException();
        }

        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            throw new NotImplementedException();
        }

        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            throw new NotImplementedException();
        }
    }
}
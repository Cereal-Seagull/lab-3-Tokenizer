namespace AST
{
    /// <summary>
    /// DefaultBuilder that does not create any objects; useful for assessing parsing problems
    /// </summary>
    public class DefaultBuilder
    {
        #region Operator Node Creation
        public virtual PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            PlusNode pn = new PlusNode();
            pn.SetChildren(left, right);
            return pn;
        }

        public virtual MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            MinusNode mn = new MinusNode();
            mn.SetChildren(left, right);
            return mn;
        }

        public virtual TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            TimesNode tn = new TimesNode();
            tn.SetChildren(left, right);
            return tn;
        }

        public virtual FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            FloatDivNode fdn = new FloatDivNode();
            fdn.SetChildren(left, right);
            return fdn;
        }

        public virtual IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            IntDivNode idn = new IntDivNode();
            idn.SetChildren(left, right);
            return idn;
        }

        public virtual ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            ModulusNode mdn = new ModulusNode();
            mdn.SetChildren(left, right);
            return mdn;
        }

        public virtual ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            ExponentiationNode expn = new ExponentiationNode();
            expn.SetChildren(left, right);
            return expn;
        }
        #endregion
        #region Expression Creation
        public virtual LiteralNode CreateLiteralNode(object value)
        {
            return new LiteralNode(value);
        }

        public virtual VariableNode CreateVariableNode(string name)
        {
            return new VariableNode(name);
        }
        #endregion
        #region Statement Creation
        public virtual AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            return new AssignmentStmt(variable, expression);
        }

        public virtual ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            return new ReturnStmt(expression);
        }

        public virtual BlockStmt CreateBlockStmt(List<Statement> lst)
        {
            return new BlockStmt(lst);
        }
        #endregion
    }
}
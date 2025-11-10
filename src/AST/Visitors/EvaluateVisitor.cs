namespace AST
{
    /// <summary>
    /// Exception thrown when an evaluation error occurs
    /// </summary>
    public class EvaluationException : Exception
    {
        public EvaluationException(string message) : base(message)
        {
        }
    }

    public class EvaluateVisitor : IVisitor<SymbolTable<string, object>, object>
    {
        // Flag to indicate if a return statement has been encountered
        private bool _returnEncountered;

        // Value from the return statement
        private object _returnValue;

        /// <summary>
        /// Initializes a new instance of the EvaluateVisitor class
        /// </summary>
        public EvaluateVisitor()
        {
            _returnEncountered = false;
            _returnValue = null;
        }

        /// <summary>
        /// Evaluates the given AST and returns the result
        /// </summary>
        /// <param name="ast">The AST to evaluate</param>
        /// <returns>The result of the evaluation (typically from a return statement)</returns>
        public object Evaluate(Statement ast)
        {
            _returnEncountered = false;
            _returnValue = null;

            // Execute the AST with a null initial scope
            // (the BlockStmt will use its own symbol table)
            ast.Accept(this, null);

            return _returnValue;
        }

        #region Binary Operator nodes

        private dynamic Convert(object node)
        {
            if (node.GetType() == typeof(float)) return (float)node;
            else if (node.GetType() == typeof(int)) return (int)node;

            else throw new EvaluationException($"Node is an unknown type (not float or int): {node.GetType()}");
        }

        public object Visit(PlusNode node, SymbolTable<string, object> st)
        {
            return Convert(node.Left.Accept(this, st)) + Convert(node.Right.Accept(this, st));
        }

        public object Visit(MinusNode node, SymbolTable<string, object> st)
        {
            return Convert(node.Left.Accept(this, st)) - Convert(node.Right.Accept(this, st));
        }

        public object Visit(TimesNode node, SymbolTable<string, object> st)
        {
            return Convert(node.Left.Accept(this, st)) * Convert(node.Right.Accept(this, st));
        }

        // Always a float? don't know
        public object Visit(FloatDivNode node, SymbolTable<string, object> st)
        {
            var right = (float)node.Right.Accept(this, st);
            if (right.Equals(0)) throw new EvaluationException("Float div cannot divide by 0");

            float exp = (float)node.Left.Accept(this, st) / right;
            return exp;
        }

        public object Visit(IntDivNode node, SymbolTable<string, object> st)
        {
            var right = (int)node.Right.Accept(this, st);
            if (right.Equals(0)) throw new EvaluationException("Int div cannot divide by 0");

            int exp = (int)node.Left.Accept(this, st) / right;
            return exp;
        }

        public object Visit(ModulusNode node, SymbolTable<string, object> st)
        {
            return Convert(node.Left.Accept(this, st)) % Convert(node.Right.Accept(this, st));
        }

        public object Visit(ExponentiationNode node, SymbolTable<string, object> st)
        {
            return Math.Pow(Convert(node.Left.Accept(this, st)), Convert(node.Right.Accept(this, st)));
        }

        #endregion

        #region Singleton Expression nodes

        public object Visit(LiteralNode node, SymbolTable<string, object> st)
        {
            return node.Value;
        }

        public object Visit(VariableNode node, SymbolTable<string, object> st)
        {
            // Variables return their value from the symbol table
            return GetVariableValue(node.Name, st);
        }

        private object GetVariableValue(string n, SymbolTable<string, object> st)
        {
            object? val;
            st.TryGetValue(n, out val);
            return val;
        }

        #endregion

        #region Statement nodes

        public object Visit(AssignmentStmt stmt, SymbolTable<string, object> st)
        {
            // Adds the expression value to variable in symbol table
            st[stmt.Variable.Name] = stmt.Expression.Accept(this, st);

            return st[stmt.Variable.Name];
        }

        public object Visit(ReturnStmt stmt, SymbolTable<string, object> st)
        {
            _returnEncountered = true;
            _returnValue = stmt.Expression.Accept(this, st);

            return _returnValue;
        }

        public object Visit(BlockStmt node, SymbolTable<string, object> st)
        {
            // Use this block's symbol table, which is already linked to its parent
            SymbolTable<string, object> currentScope = node.SymbolTable;

            for (int i = 0; i < node.Statements.Count; i++)
            {
                node.Statements[i].Accept(this, currentScope);

                // If return stmt encountered, stop evaluating and return
                if (_returnEncountered) return _returnValue;
            }

            // otherwise, return last known value
            return node.Statements[node.Statements.Count - 1].Accept(this, currentScope);
        }
        #endregion
    }
}
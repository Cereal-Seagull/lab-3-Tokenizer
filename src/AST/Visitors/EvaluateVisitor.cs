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
            // If float or double, return type float
            if (node.GetType() == typeof(float)) return (float)node;
            // If int, return int type
            else if (node.GetType() == typeof(int)) return (int)node;

            // Unknown type (not float or int);
            else throw new EvaluationException($"Node is an unknown type (not float or int): {node.GetType()}");
        }

        public object Visit(PlusNode node, SymbolTable<string, object> st)
        {
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return left + right;
        }

        public object Visit(MinusNode node, SymbolTable<string, object> st)
        {
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return left - right;
        }

        public object Visit(TimesNode node, SymbolTable<string, object> st)
        {
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return left * right;
        }

        public object Visit(FloatDivNode node, SymbolTable<string, object> st)
        {
            float right = Convert(node.Right.Accept(this, st));
            if (right.Equals(0)) throw new EvaluationException("Float div cannot divide by 0");

            float left = Convert(node.Left.Accept(this, st));

            float exp = left / right;
            return exp;
        }

        public object Visit(IntDivNode node, SymbolTable<string, object> st)
        {
            int right = System.Convert.ToInt32(node.Right.Accept(this, st));
            if (right.Equals(0)) throw new EvaluationException("Int div cannot divide by 0");

            int left = System.Convert.ToInt32(node.Left.Accept(this, st));

            var exp = left / right;
            return System.Convert.ToInt32(exp);
        }

        public object Visit(ModulusNode node, SymbolTable<string, object> st)
        {
            // Convert left & right expressions to ints/floats
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            // Throw exception when % 0
            if (right.Equals(0)) throw new EvaluationException("Cannot mod by 0");

            return left % right;
        }

        public object Visit(ExponentiationNode node, SymbolTable<string, object> st)
        {
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return Math.Pow(left, right);
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
            // Check right 
            object right = stmt.Expression.Accept(this, st);

            if (right == null) throw new EvaluationException("Undefined variable in assignment statement");
            st[stmt.Variable.Name] = right;

            _returnValue = right;
            return right;
        }

        public object Visit(ReturnStmt stmt, SymbolTable<string, object> st)
        {
            _returnEncountered = true;
            _returnValue = stmt.Expression.Accept(this, st);

            return _returnValue;
        }

        public object Visit(BlockStmt node, SymbolTable<string, object> st)
        {
            // If BlockStmt is empty, return nothing
            if (node.Statements.Count == 0) return null;

            // Use this block's symbol table, which is already linked to its parent
            SymbolTable<string, object> currentScope = node.SymbolTable;

            for (int i = 0; i < node.Statements.Count; i++)
            {
                node.Statements[i].Accept(this, currentScope);

                // If return stmt encountered, stop evaluating and return
                if (_returnEncountered) return _returnValue;
            }

            // otherwise, return last known value'
            // _returnValue = node.Statements[node.Statements.Count - 1].Accept(this, currentScope);
            return _returnValue;
        }
        #endregion
    }
}
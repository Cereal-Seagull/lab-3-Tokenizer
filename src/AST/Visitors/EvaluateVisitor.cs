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

        /// <summary>
        /// Converts a node value to a dynamic type (float or int)
        /// </summary>
        /// <param name="node">The node value to convert</param>
        /// <returns>The converted value as dynamic</returns>
        /// <exception cref="EvaluationException">Thrown when node type is neither float nor int</exception>
        private dynamic Convert(object node)
        {
            // If float or double, return type float
            if (node.GetType() == typeof(float)) return (float)node;
            // If int, return int type
            else if (node.GetType() == typeof(int)) return (int)node;

            // Unknown type (not float or int);
            else throw new EvaluationException($"Node is an unknown type (not float or int): {node.GetType()}");
        }

        /// <summary>
        /// Visits a PlusNode and performs addition on its operands
        /// </summary>
        /// <param name="node">The PlusNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The sum of left and right operands</returns>
        public object Visit(PlusNode node, SymbolTable<string, object> st)
        {
            // Visit left and right expressions
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return left + right;
        }

        /// <summary>
        /// Visits a MinusNode and performs subtraction on its operands
        /// </summary>
        /// <param name="node">The MinusNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The difference of left and right operands</returns>
        public object Visit(MinusNode node, SymbolTable<string, object> st)
        {
            // Visit left and right expressions
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return left - right;
        }

        /// <summary>
        /// Visits a TimesNode and performs multiplication on its operands
        /// </summary>
        /// <param name="node">The TimesNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The product of left and right operands</returns>
        public object Visit(TimesNode node, SymbolTable<string, object> st)
        {
            // Visit left and right expressions
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return left * right;
        }

        /// <summary>
        /// Visits a FloatDivNode and performs floating-point division on its operands
        /// </summary>
        /// <param name="node">The FloatDivNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The quotient of left and right operands as a float</returns>
        /// <exception cref="EvaluationException">Thrown when attempting to divide by zero</exception>
        public object Visit(FloatDivNode node, SymbolTable<string, object> st)
        {
            // Visit right expression
            float right = Convert(node.Right.Accept(this, st));
            // Throw exception if divide by 0
            if (right.Equals(0)) throw new EvaluationException("Float div cannot divide by 0");

            // Visit left expression & divide both sides
            float left = Convert(node.Left.Accept(this, st));
            float exp = left / right;

            return exp;
        }

        /// <summary>
        /// Visits an IntDivNode and performs integer division on its operands
        /// </summary>
        /// <param name="node">The IntDivNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The quotient of left and right operands as an integer</returns>
        /// <exception cref="EvaluationException">Thrown when attempting to divide by zero</exception>
        public object Visit(IntDivNode node, SymbolTable<string, object> st)
        {
            // Visit right expression
            int right = Convert(node.Right.Accept(this, st));
            // Throw exception if divide by 0
            if (right.Equals(0)) throw new EvaluationException("Int div cannot divide by 0");

            // Visit left expression & divide both sides
            int left = Convert(node.Left.Accept(this, st));
            int exp = left / right;

            return exp;
        }

        /// <summary>
        /// Visits a ModulusNode and performs modulo operation on its operands
        /// </summary>
        /// <param name="node">The ModulusNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The remainder of left divided by right</returns>
        /// <exception cref="EvaluationException">Thrown when attempting to mod by zero</exception>
        public object Visit(ModulusNode node, SymbolTable<string, object> st)
        {
            // Visit left and right expressions
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            // Throw exception when % 0
            if (right.Equals(0)) throw new EvaluationException("Cannot mod by 0");

            return left % right;
        }

        /// <summary>
        /// Visits an ExponentiationNode and raises left operand to the power of right operand
        /// </summary>
        /// <param name="node">The ExponentiationNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The result of left raised to the power of right</returns>
        public object Visit(ExponentiationNode node, SymbolTable<string, object> st)
        {
            // Visit left and right expressions
            var left = Convert(node.Left.Accept(this, st));
            var right = Convert(node.Right.Accept(this, st));

            return Math.Pow(left, right);
        }

        #endregion

        #region Singleton Expression nodes

        /// <summary>
        /// Visits a LiteralNode and returns its value
        /// </summary>
        /// <param name="node">The LiteralNode to evaluate</param>
        /// <param name="st">The symbol table (unused for literals)</param>
        /// <returns>The literal value</returns>
        public object Visit(LiteralNode node, SymbolTable<string, object> st)
        {
            // Return the literal value
            return node.Value;
        }

        /// <summary>
        /// Visits a VariableNode and retrieves its value from the symbol table
        /// </summary>
        /// <param name="node">The VariableNode to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The value of the variable from the symbol table</returns>
        public object Visit(VariableNode node, SymbolTable<string, object> st)
        {
            // Variables return their value from the symbol table
            return GetVariableValue(node.Name, st);
        }

        /// <summary>
        /// Retrieves the value of a variable from the symbol table
        /// </summary>
        /// <param name="n">The name of the variable</param>
        /// <param name="st">The symbol table to search</param>
        /// <returns>The value of the variable, or null if not found</returns>
        private object GetVariableValue(string n, SymbolTable<string, object> st)
        {
            // Get the specified key's value from symbol table
            // Default value is null (if not found)
            st.TryGetValue(n, out object? val);
            return val;
        }

        #endregion

        #region Statement nodes

        /// <summary>
        /// Visits an AssignmentStmt and assigns the expression value to the variable
        /// </summary>
        /// <param name="stmt">The AssignmentStmt to evaluate</param>
        /// <param name="st">The symbol table to update with the assignment</param>
        /// <returns>The value that was assigned</returns>
        /// <exception cref="EvaluationException">Thrown when the expression evaluates to null</exception>
        public object Visit(AssignmentStmt stmt, SymbolTable<string, object> st)
        {
            // Check right 
            object right = stmt.Expression.Accept(this, st);

            // Throw exception if variable's value not found
            if (right == null) throw new EvaluationException("Undefined variable in assignment statement");

            // Set variable's value to expression
            st[stmt.Variable.Name] = right;

            _returnValue = right;
            return right;
        }

        /// <summary>
        /// Visits a ReturnStmt and sets the return value for the evaluation
        /// </summary>
        /// <param name="stmt">The ReturnStmt to evaluate</param>
        /// <param name="st">The symbol table containing variable values</param>
        /// <returns>The value of the return expression</returns>
        public object Visit(ReturnStmt stmt, SymbolTable<string, object> st)
        {
            // Update variables tracking return states
            _returnEncountered = true;

            // Set return value to return stmt expression
            _returnValue = stmt.Expression.Accept(this, st);

            return _returnValue;
        }

        /// <summary>
        /// Visits a BlockStmt and evaluates all statements within the block
        /// </summary>
        /// <param name="node">The BlockStmt to evaluate</param>
        /// <param name="st">The parent symbol table (unused, as block has its own)</param>
        /// <returns>The return value if a return statement was encountered, otherwise the last known value</returns>
        public object Visit(BlockStmt node, SymbolTable<string, object> st)
        {
            // If BlockStmt is empty, return nothing
            if (node.Statements.Count == 0) return null;

            // Use this block's symbol table, which is already linked to its parent
            SymbolTable<string, object> currentScope = node.SymbolTable;

            // Iterate through list of statements in block
            for (int i = 0; i < node.Statements.Count; i++)
            {
                // Visit statement
                node.Statements[i].Accept(this, currentScope);

                // If return stmt encountered, stop evaluating and return
                if (_returnEncountered) return _returnValue;
            }

            // otherwise, return last known value
            return _returnValue;
        }
        #endregion
    }
}
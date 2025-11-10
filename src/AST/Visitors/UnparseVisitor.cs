using System.Text;
using Tokenizer;

namespace AST
{
    public class UnparseVisitor : IVisitor<int, string>
    {

        /// <summary>
        /// Unparses the given AST node with the specified indentation level
        /// </summary>
        /// <param name="node">The AST node to unparse</param>
        /// <param name="level">The indentation level</param>
        /// <returns>String representation of the node</returns>
        public string Unparse(ExpressionNode node, int level = 0)
        {
            return node.Accept(this, level);
        }

        #region Binary Operator nodes

        public string Visit(PlusNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);

            return $"({left} {TokenConstants.PLUS} {right})";
        }

        public string Visit(MinusNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);

            return $"({left} {TokenConstants.SUBTRACTION} {right})";
        }

        public string Visit(TimesNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);

            return $"({left} {TokenConstants.TIMES} {right})";
        }

        public string Visit(FloatDivNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);

            return $"({left} {TokenConstants.FLOAT_DIVISION} {right})";
        }

        public string Visit(IntDivNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);

            return $"({left} {TokenConstants.INT_DIVISION} {right})";
        }

        public string Visit(ModulusNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);

            return $"({left} {TokenConstants.MODULUS} {right})";
        }

        public string Visit(ExponentiationNode node, int level = 0)
        {
            // Visit left and right expressions
            string left = node.Left.Accept(this, level);
            string right = node.Right.Accept(this, level);
            
            return $"({left} {TokenConstants.EXPONENTIATE} {right})";
        }

        #endregion

        #region Singleton Expression nodes

        public string Visit(LiteralNode node, int level = 0)
        {
            // Return literal value as string
            return node.Value.ToString();
        }

        public string Visit(VariableNode node, int level = 0)
        {
            // Return variable name
            return node.Name;
        }

        #endregion

        #region Statement nodes

        /// <summary>
        /// Unparses the given statement with the specified indentation level
        /// </summary>
        /// <param name="stmt">The statement to unparse</param>
        /// <param name="level">The indentation level</param>
        /// <returns>String representation of the statement</returns>
        public string Unparse(Statement stmt, int level = 0)
        {
            return stmt.Accept(this, level);
        }

        public string Visit(AssignmentStmt stmt, int level = 0)
        {
            // Visit both sides of statement
            string v = stmt.Variable.Accept(this, 0);
            string exp = stmt.Expression.Accept(this, 0);

            return $"{GeneralUtils.GetIndentation(level)}{v} {TokenConstants.ASSIGNMENT} {exp}";
        }

        public string Visit(ReturnStmt stmt, int level = 0)
        {
            // Visit right side of return stmt
            string exp = stmt.Expression.Accept(this, 0);
            
            return $"{GeneralUtils.GetIndentation(level)}{TokenConstants.RETURN} {exp}";
        }

        public string Visit(BlockStmt node, int level = 0)
        {
            StringBuilder str = new StringBuilder();

            // Add left curly brace
            str.Append(GeneralUtils.GetIndentation(level));
            str.AppendLine(TokenConstants.LEFT_CURLY);

            // Increment indentation
            level++;

            // Append all statements in block stmt (including block stmts)
            for (int i = 0; i < node.Statements.Count; i++)
            {
                str.AppendLine(node.Statements[i].Accept(this, level));
            }

            // Add right curly brace
            str.Append(GeneralUtils.GetIndentation(level - 1));
            str.AppendLine(TokenConstants.RIGHT_CURLY);

            return str.ToString();
        }

        #endregion
    }
}
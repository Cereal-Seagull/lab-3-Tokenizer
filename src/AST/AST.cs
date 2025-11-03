using System.Text;
using Tokenizer;

namespace AST
{
    #region Nodes

    /// <summary>
    /// Abstract base class for all expression nodes in the abstract syntax tree.
    /// Expression nodes represent values that can be evaluated.
    /// </summary>
    public abstract class ExpressionNode
    {
        /// <summary>
        /// Converts the expression node back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting. Default is 0.</param>
        /// <returns>A string representation of the expression node.</returns>
        public abstract string Unparse(int level = 0);
    }

    /// <summary>
    /// Represents a literal value in the abstract syntax tree.
    /// Literal values are constant values such as numbers, strings, or booleans.
    /// </summary>
    public class LiteralNode : ExpressionNode
    {
        /// <summary>
        /// Gets the literal value stored in this node.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralNode"/> class.
        /// </summary>
        /// <param name="val">The literal value to store in this node.</param>
        public LiteralNode(object val) { Value = val; } // do we want Value to be object or int? 
                                                        //need ints and floats

        /// <summary>
        /// Converts the literal node back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting.</param>
        /// <returns>A string representation of the literal value with appropriate indentation.</returns>
        public override string Unparse(int level = 0)
        {
            return GeneralUtils.GetIndentation(level) + Value.ToString();
        }
    }
    
    /// <summary>
    /// Represents a variable reference in the abstract syntax tree.
    /// Variable nodes store the name of a variable being referenced.
    /// </summary>
    public class VariableNode : ExpressionNode
    {
        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="str">The name of the variable.</param>
        public VariableNode(string str) { Name = str; }

        /// <summary>
        /// Converts the variable node back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting.</param>
        /// <returns>A string representation of the variable name with appropriate indentation.</returns>
        public override string Unparse(int level = 0)
        {
            return GeneralUtils.GetIndentation(level) + Name;
        }
    }

    #endregion
    #region Operators

    /// <summary>
    /// Abstract base class for all operator nodes in the abstract syntax tree.
    /// Operators are expressions that perform operations on other expressions.
    /// </summary>
    public abstract class Operator : ExpressionNode { }

    /// <summary>
    /// Abstract base class for binary operators in the abstract syntax tree.
    /// Binary operators take two operands (left and right) and produce a result.
    /// </summary>
    public abstract class BinaryOperator : Operator
    {
        /// <summary>
        /// The left operand of the binary operation.
        /// </summary>
        public ExpressionNode Left;

        /// <summary>
        /// The operator symbol (e.g., "+", "-", "*").
        /// </summary>
        public string SIGN;

        /// <summary>
        /// The right operand of the binary operation.
        /// </summary>
        public ExpressionNode Right;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperator"/> class.
        /// </summary>
        /// <param name="l">The left operand expression node.</param>
        /// <param name="s">The operator symbol.</param>
        /// <param name="r">The right operand expression node.</param>
        public BinaryOperator(ExpressionNode l, string s, ExpressionNode r)
        {
            Left = l;
            SIGN = s;
            Right = r;
        }

        /// <summary>
        /// Converts the binary operator node back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting.</param>
        /// <returns>A string representation of the binary operation in the format "left SIGN right".</returns>
        public override string Unparse(int level = 0)
        {
            StringBuilder str = new StringBuilder();

            str.Append(TokenConstants.LEFT_PAREN);
            str.Append(Left.Unparse(level));
            str.Append(" ");
            str.Append(SIGN);
            str.Append(" ");
            str.Append(Right.Unparse(0));
            str.Append(TokenConstants.RIGHT_PAREN);

            return str.ToString();
        }

    }

    /// <summary>
    /// Represents an addition operation in the abstract syntax tree.
    /// Performs addition of two operands using the "+" operator.
    /// </summary>
    public class PlusNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlusNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public PlusNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.PLUS, r) { }
    }

    /// <summary>
    /// Represents a subtraction operation in the abstract syntax tree.
    /// Performs subtraction of two operands using the "-" operator.
    /// </summary>
    public class MinusNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinusNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public MinusNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.SUBTRACTION, r) { }
    }

    /// <summary>
    /// Represents a multiplication operation in the abstract syntax tree.
    /// Performs multiplication of two operands using the "*" operator.
    /// </summary>
    public class TimesNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimesNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public TimesNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.TIMES, r) { }
    }

    /// <summary>
    /// Represents a floating-point division operation in the abstract syntax tree.
    /// Performs division of two operands using the "/" operator, resulting in a floating-point value.
    /// </summary>
    public class FloatDivNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatDivNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public FloatDivNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.FLOAT_DIVISION, r) { }
    }

    /// <summary>
    /// Represents an integer division operation in the abstract syntax tree.
    /// Performs division of two operands using the "//" operator, resulting in an integer value.
    /// </summary>
    public class IntDivNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntDivNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public IntDivNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.INT_DIVISION, r) { }
    }

    /// <summary>
    /// Represents a modulus operation in the abstract syntax tree.
    /// Computes the remainder of division using the "%" operator.
    /// </summary>
    public class ModulusNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModulusNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public ModulusNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.MODULUS, r) { }
    }

    /// <summary>
    /// Represents an exponentiation operation in the abstract syntax tree.
    /// Raises the left operand to the power of the right operand using the "**" operator.
    /// </summary>
    public class ExponentiationNode : BinaryOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentiationNode"/> class.
        /// Operands initialized to given left and right expression nodes.
        /// </summary>
        public ExponentiationNode(ExpressionNode l, ExpressionNode r) : base(l, TokenConstants.EXPONENTIATE, r) { }
    }

    #endregion
    #region Statements

    /// <summary>
    /// Abstract base class for all statement nodes in the abstract syntax tree.
    /// Statements represent executable actions or declarations in the program.
    /// </summary>
    public abstract class Statement
    {
        /// <summary>
        /// Converts the statement node back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting. Default is 0.</param>
        /// <returns>A string representation of the statement.</returns>
        public abstract string Unparse(int level = 0);
    }

    /// <summary>
    /// Represents a block of statements in the abstract syntax tree.
    /// A block contains zero or more statements enclosed in curly braces.
    /// </summary>
    public class BlockStmt : Statement
    {
        /// <summary>
        /// The list of statements contained within this block.
        /// </summary>
        public List<Statement> Statements;

        public SymbolTable<string, object> SymbolTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockStmt"/> class.
        /// </summary>
        /// <param name="lst">The list of statements to include in the block.</param>
        public BlockStmt(SymbolTable<string, object> st)
        {
            SymbolTable = st;
            Statements = new List<Statement>();
        }

        /// <summary>
        /// Adds a statement to the end of this block.
        /// </summary>
        /// <param name="obj">The statement to add to the block.</param>
        // Adds a statement to Block
        public void AddStatement(Statement obj)
        {
            Statements.Add(obj);
        }

        /// <summary>
        /// Converts the block statement back into its source code representation.
        /// The block is formatted with opening and closing curly braces and each statement on its own line.
        /// </summary>
        /// <param name="level">The indentation level for formatting.</param>
        /// <returns>A string representation of the block with all contained statements.</returns>
        public override string Unparse(int level = 0)
        {
            StringBuilder str = new StringBuilder();

            // Add curly brace if block is nested
            if (level != 0)
            {
                str.Append(GeneralUtils.GetIndentation(level - 1));
                str.Append(TokenConstants.LEFT_CURLY);
                str.Append("\n");
            }

            // Call Unparse() on child nodes
            for (int i = 0; i < Statements.Count; i++)
            {
                // If block statement, increase their statement indent by 1
                if (Statements[i].GetType().Equals(typeof(BlockStmt))) 
                    str.Append(Statements[i].Unparse(level + 1));
                else str.Append(Statements[i].Unparse(level));

                // New line unless it is last statement
                if (i != Statements.Count - 1) str.Append("\n");

            }

            // Add curly brace if block is nested
            if (level != 0)
            {
                str.Append("\n");
                str.Append(GeneralUtils.GetIndentation(level - 1));
                str.Append(TokenConstants.RIGHT_CURLY);
            }

            return str.ToString();
        }
    }

    /// <summary>
    /// Represents an assignment statement in the abstract syntax tree.
    /// Assignment statements assign the value of an expression to a variable.
    /// </summary>
    public class AssignmentStmt : Statement
    {
        /// <summary>
        /// The variable being assigned to.
        /// </summary>
        public VariableNode Variable;

        /// <summary>
        /// The assignment operator symbol ("=").
        /// </summary>
        public const string SIGN = TokenConstants.ASSIGNMENT;

        /// <summary>
        /// The expression whose value is being assigned.
        /// </summary>
        public ExpressionNode Expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentStmt"/> class.
        /// </summary>
        /// <param name="v">The variable node representing the target of the assignment.</param>
        /// <param name="x">The expression node representing the value to assign.</param>
        public AssignmentStmt(VariableNode v, ExpressionNode x)
        {
            Variable = v;
            Expression = x;
        }

        /// <summary>
        /// Converts the assignment statement back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting.</param>
        /// <returns>A string representation of the assignment in the format "variable = expression".</returns>
        public override string Unparse(int level = 0)
        {
            return GeneralUtils.GetIndentation(level) + Variable.Unparse(0) + " " + SIGN 
                                                      + " " + Expression.Unparse(0);
        }
    }

    /// <summary>
    /// Represents a return statement in the abstract syntax tree.
    /// Return statements specify the value to return from a function or method.
    /// </summary>
    public class ReturnStmt : Statement
    {
        /// <summary>
        /// The expression whose value is being returned.
        /// </summary>
        public ExpressionNode Expression;

        /// <summary>
        /// The return keyword.
        /// </summary>
        public const string SIGN = TokenConstants.RETURN;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnStmt"/> class.
        /// </summary>
        /// <param name="x">The expression node representing the value to return.</param>
        public ReturnStmt(ExpressionNode x)
        {
            Expression = x;
        }

        /// <summary>
        /// Converts the return statement back into its source code representation.
        /// </summary>
        /// <param name="level">The indentation level for formatting.</param>
        /// <returns>A string representation of the return statement in the format "return expression".</returns>
        public override string Unparse(int level = 0)
        {
            return GeneralUtils.GetIndentation(level) + SIGN + " " + Expression.Unparse(0);
        }
    }
    
    #endregion    
}
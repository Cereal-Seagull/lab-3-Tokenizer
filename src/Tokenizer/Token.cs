/**
* Token class that is to be used in the Tokenizer implementation. In practice,
* converts a string value (int, float, operator, etc.) into a token with properties 
* _value (the string itself) and _type (variable, int, float, operator, assignment, keyword).
* Also defines constants for operators, keywords, and group characters.
*
* Bugs: None known
*
* @author Reza Naqvi
* @author Will Zoeller
* @date 9/28/25
*/
using System.Text;

namespace Tokenizer
{
    /// <summary>
    /// Contains constant string values for operators, keywords, and grouping
    /// symbols used in the DEC language. Centralizes definitions for consistency
    /// and easier maintenance.
    /// </summary>
    static class TokenConstants
    {
        #region Single-character operators
        public const string PLUS = "+";
        public const string SUBTRACTION = "-";
        public const string TIMES = "*";
        public const string MODULUS = "%";
        public const string FLOAT_DIVISION = "/";
        public const string EQUALS = "=";
        #endregion

        #region Keywords
        public const string RETURN = "return";
        #endregion

        #region Multi-character operators
        public const string INT_DIVISION = "//";
        public const string EXPONENTIATE = "**";
        public const string ASSIGNMENT = ":=";
        #endregion

        #region Float support
        public const string DECIMAL_POINT = ".";
        #endregion

        #region Grouping characters
        public const string LEFT_PAREN = "(";
        public const string RIGHT_PAREN = ")";
        public const string LEFT_CURLY = "{";
        public const string RIGHT_CURLY = "}";
        #endregion
    }

    /// <summary>
    /// Represents a single token with its string value and type.
    /// Encapsulates the lexical content and semantic meaning
    /// of input code during tokenization.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The raw string value of the token (e.g., "42", "+", "return").
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// The classification of the token (e.g., VARIABLE, INTEGER, OPERATOR).
        /// </summary>
        public readonly TokenType Type;

        // Potential extension fields for tracking token location in source code.
        // private int _line;
        // private int _col;
        // private int _len;

        /// <summary>
        /// Initializes a new Token with a given value and type.
        /// </summary>
        /// <param name="val">The string value of the token.</param>
        /// <param name="T">The token type, as defined by <see cref="TokenType"/>.</param>
        public Token(string val, TokenType T)
        {
            Value = val;
            Type = T;
        }

        /// <summary>
        /// Returns a string representation of the token in the format:
        /// [value, type]
        /// </summary>
        /// <returns>A formatted string describing the token.</returns>
        public override string ToString()
        {
            return new StringBuilder().Append($"[{Value}, {Type}]").ToString();
        }

        /// <summary>
        /// Compares this token with another token for equality.
        /// </summary>
        /// <param name="other">The token to compare with this instance.</param>
        /// <returns>True if both tokens share the same value and type, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is null.</exception>
        public bool Equals(Token other)
        {
            if (other == null) throw new ArgumentNullException();
            return Value == other.Value && Type == other.Type;
        }
    }

    /// <summary>
    /// Enumeration of possible token types in the DEC language.
    /// Defines categories such as variables, literals, operators, and grouping symbols.
    /// </summary>
    public enum TokenType
    {
        VARIABLE,
        RETURN,
        INTEGER,
        FLOAT,
        OPERATOR,
        ASSIGNMENT,
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_CURLY,
        RIGHT_CURLY
    }
}


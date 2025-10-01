/**
<<<<<<< Updated upstream
* Summary: 
=======
* Represents tokens and constants used in a tokenizer implementation.
* Provides constants for operators, keywords, and delimiters, as well as a
* Token class to encapsulate string values and their associated token types.
>>>>>>> Stashed changes
*
* Bugs: None known
*
* @author Reza Naqvi
* @author Will Zoeller
* @date 9/28/25
*/
<<<<<<< Updated upstream
using System;
using System.Runtime.CompilerServices;
=======

>>>>>>> Stashed changes
using System.Text;

namespace Tokenizer
{
<<<<<<< Updated upstream
    // enum and class definitions
    /// <summary>
    /// </summary>
    /// <typeparam name="">The type of keys in the symbol table</typeparam>
    /// <typeparam name="">The type of values in the symbol table</typeparam>
    // public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
    // {

    // }
    // TokenType enumeration
    // TokenConstants static class
    static class TokenConstants
    {
        const string PLUS = "+";
        const string ASSIGNMENT = ":=";

        const string LEFT_PAREN = "(";

        const string RIGHT_PAREN = ")";

        const string LEFT_CURLY = "{";

        const string DECIMAL_POINT = ".";
        const string TIMES = "*";
        const string SUBTRACTION = "-";
        const string FLOAT_DIVISION = "/";
        const string INT_DIVISION = "//";
        const string MODULUS = "%";
        const string EXPONENTIATE = "**";

    }

    // Token class - should not be complex
    class Token
=======
    /// <summary>
    /// Contains string constants for operators, keywords, delimiters, and special
    /// symbols used when tokenizing input code.
    /// </summary>
    static class TokenConstants
    {
        #region Operator Constants
        // Single-character operators
        public const string PLUS = "+";
        public const string SUBTRACTION = "-";
        public const string TIMES = "*";
        public const string MODULUS = "%";
        public const string FLOAT_DIVISION = "/";
        public const string EQUALS = "=";

        // Multi-character operators
        public const string INT_DIVISION = "//";
        public const string EXPONENTIATE = "**";
        public const string ASSIGNMENT = ":=";
        #endregion

        #region Keyword Constants
        // Reserved keywords
        public const string RETURN = "return";
        #endregion

        #region Literal/Grouping Constants
        // For float values
        public const string DECIMAL_POINT = ".";

        // Grouping characters
        public const string LEFT_PAREN = "(";
        public const string RIGHT_PAREN = ")";
        public const string LEFT_CURLY = "{";
        public const string RIGHT_CURLY = "}";
        #endregion
    }

    /// <summary>
    /// Represents a token in the tokenizer, storing both the string value
    /// (e.g., identifier, operator, keyword) and its type classification.
    /// </summary>
    public class Token
>>>>>>> Stashed changes
    {
        /// <summary>
        /// The raw string value of the token (e.g., "+", "return", "42").
        /// </summary>
        protected string _value;
<<<<<<< Updated upstream
        protected string _type;
=======

        /// <summary>
        /// The type of token, as defined by the <see cref="TokenType"/> enum.
        /// </summary>
        protected TokenType _type;
>>>>>>> Stashed changes

        // Potential extension fields for source code location (line, column, length).
        // Not currently in use, but kept for possible expansion.
        // private int _line;
        // private int _col;
        // private int _len;

<<<<<<< Updated upstream
        Token()
        {
            _value = null;
            _type = null;
        }
        Token(string val, string T)
=======
        /// <summary>
        /// Constructs a new token with the given value and type.
        /// </summary>
        /// <param name="val">The string representation of the token.</param>
        /// <param name="T">The classification of the token.</param>
        public Token(string val, TokenType T)
>>>>>>> Stashed changes
        {
            _value = val;
            _type = T;
        }

<<<<<<< Updated upstream
        

        // ToString
=======
        /// <summary>
        /// Returns a string representation of the token in the format:
        /// [value, type]
        /// </summary>
        /// <returns>A formatted string describing the token.</returns>
>>>>>>> Stashed changes
        public override string ToString()
        {
            // Build string representation using StringBuilder for efficiency
            return new StringBuilder().Append($"[{_value}, {_type}]").ToString();
        }

        /// <summary>
        /// Compares the current token with another token for equality.
        /// </summary>
        /// <param name="other">The token to compare against.</param>
        /// <returns>
        /// True if both tokens share the same value and type; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="other"/> is null.
        /// </exception>
        public bool Equals(Token other)
        {
            if (other == null) throw new ArgumentNullException();
<<<<<<< Updated upstream
            return _value == other._value;
        }



        // etc.
    }

    enum TokenType
=======
            return _value == other._value && _type == other._type;
        }
    }

    /// <summary>
    /// Enumeration of all possible token types recognized by the tokenizer.
    /// </summary>
    public enum TokenType
>>>>>>> Stashed changes
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


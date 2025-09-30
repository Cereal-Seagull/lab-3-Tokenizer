/**
* Summary: 
*
* Bugs: 
*
* @author Reza Naqvi and Will Zoeller
* @date 9/28/25
*/
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Tokenizer
{
    static class TokenConstants
    {
        // single ops
        public const string PLUS = "+";
        public const string SUBTRACTION = "-";
        public const string TIMES = "*";
        public const string MODULUS = "%";
        public const string FLOAT_DIVISION = "/";
        public const string EQUALS = "=";

        // multiops
        public const string INT_DIVISION = "//";
        public const string EXPONENTIATE = "**";
        public const string ASSIGNMENT = ":=";

        // for floats
        public const string DECIMAL_POINT = ".";

        // group chars
        public const string LEFT_PAREN = "(";
        public const string RIGHT_PAREN = ")";
        public const string LEFT_CURLY = "{";
        public const string RIGHT_CURLY = "}";

    }

    // Token class - should not be complex
    public class Token
    {
        protected string _value;
        protected TokenType _type;

        // Should these be properties?
        // private int _line;
        // private int _col;
        // private int _len;

        public Token(string val, TokenType T)
        {
            _value = val;
            _type = T;
        }

        // ToString
        public override string ToString()
        {
            // var sb = new StringBuilder();
            // sb.Append($"[{_value}, {_type}]");
            // return sb.ToString();
            return new StringBuilder().Append($"[{_value}, {_type}]").ToString();
        }

        // Equals
        public bool Equals(Token other)
        {
            if (other == null) throw new ArgumentNullException();
            return _value == other._value;
        }

        
    }

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

/**
* Tokenizer implementation that converts a string into a list of tokens
* for the DEC language. Uses private helper methods to handle different
* categories of tokens such as variables, numbers, operators, keywords,
* and grouping symbols.
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
    /// Implements lexical analysis for the DEC language.
    /// The tokenizer scans character by character and produces
    /// a list of tokens, delegating parsing to helper methods
    /// for assignment, operators, numbers, keywords, and grouping.
    /// </summary>
    public class TokenizerImpl
    {
        /// <summary>
        /// Converts the input string into a list of tokens by checking each
        /// character and calling the appropriate handler based on its type.
        /// Whitespace is ignored.
        /// </summary>
        /// <param name="str">The input string to tokenize.</param>
        /// <returns>A list of tokens extracted from the input string.</returns>
        public List<Token> Tokenize(string str)
        {
            var lst = new List<Token>();
            int idx = 0;

            // return keyword may be differnt, make all char in str lowercase for tests (assuming not case-sensitive)

            // Main loop scans each character until the end of the string
            while (idx < str.Length)
            {
                char e = str[idx];

                // Skip whitespace, handle meaningful characters
                if (!IsWhiteSpace(str[idx]))
                {
                    // Handle assignment operator (":=")
                    if (e == ':') lst.Add(HandleAssignment(str, ref idx));

                    // Handle "return" keyword (or fallback to variable)
                    else if (e == 'r') lst.Add(HandleReturn(str, ref idx));

                    // Handle variable (sequence of letters)
                    else if (IsLetter(e)) lst.Add(HandleVariable(str, ref idx));

                    // Handle numbers (integer or float)
                    else if (IsDigit(e)) lst.Add(HandleNumber(str, ref idx));

                    // Handle grouping symbols: (), {}
                    else if (IsGrouper(e)) lst.Add(HandleGrouping(str, ref idx));

                    // Handle single-character operators: +, -, %, =
                    else if (e == '+' || e == '-' || e == '%' || e == '=')
                    {
                        lst.Add(HandleSingleOp(str, ref idx));
                    }

                    // Handle multi-character operators: **, //
                    else if (e == '/' || e == '*')
                    {
                        // Delegates to HandleSingleOp if only one symbol
                        lst.Add(HandleMultiOp(str, ref idx));
                    }

                    // Unrecognized characters are skipped or would raise exceptions
                }
                // If whitespace, handle functions all increment index
                else idx += 1;
            }

            return lst;
        }

        #region Handlers
        // Methods that extract specific token types from the input

        /// <summary>
        /// Handles assignment operator ":=".
        /// </summary>
        /// <param name="s">The input string being tokenized.</param>
        /// <param name="idx">Reference to the current index in the string.</param>
        /// <returns>A token of type ASSIGNMENT.</returns>
        /// <exception cref="ArgumentException">Thrown if the operator is incomplete or invalid.</exception>
        private Token HandleAssignment(string s, ref int idx)
        {
            if (idx == s.Length - 1) throw new ArgumentException($"Invalid Assignment Operator: {idx}");
            string code = String.Concat(s[idx], s[idx + 1]);
            if (code != TokenConstants.ASSIGNMENT) throw new ArgumentException($"Invalid Assignment Operator: {code}");

            idx += 2;
            return new Token(code, TokenType.ASSIGNMENT);
        }

        /// <summary>
        /// Handles detection of the "return" keyword.
        /// Falls back to variable handling if characters do not exactly match.
        /// </summary>
        private Token HandleReturn(string s, ref int idx)
        {
            // If remaining string is too short, treat as variable
            if (s.Length - idx < TokenConstants.RETURN.Length) return HandleVariable(s, ref idx);

            // use string constant
            // List<char> letters = new List<char> { 'r', 'e', 't', 'u', 'r', 'n' };
            int lIdx = 0;
            // string expected = "";
            string actual = "";

            // Build actual vs. expected letter by letter
            while (lIdx < 6)
            {
                actual += s[idx];
                // expected += letters[lIdx];

                // If mismatch occurs, reset and treat as variable
                if (s[idx] != TokenConstants.RETURN[lIdx])
                {
                    idx -= lIdx;
                    return HandleVariable(s, ref idx);
                }
                idx += 1;
                lIdx += 1;
            }

            // Confirm keyword if at end, or followed by whitespace/grouper
            if (idx == s.Length || IsWhiteSpace(s[idx]) || IsGrouper(s[idx]))
            {
                return new Token(actual, TokenType.RETURN);
            }
            else
            {
                // Otherwise revert and treat as variable
                idx -= lIdx;
                return HandleVariable(s, ref idx);
            }
        }

        /// <summary>
        /// Handles single-character operators such as +, -, *, /, %, =.
        /// </summary>
        private Token HandleSingleOp(string s, ref int idx)
        {
            // Search for operator in predefined list
            List<string> ops = new List<string> { TokenConstants.PLUS, TokenConstants.SUBTRACTION, TokenConstants.TIMES, TokenConstants.FLOAT_DIVISION, TokenConstants.MODULUS, TokenConstants.EQUALS };
            int index = ops.IndexOf(s[idx].ToString());
            idx += 1;
            // condense down
            // if (index != -1)
            // {
            //     string singleOp = ops[index];
            //     var token = new Token(singleOp, TokenType.OPERATOR);
            //     idx += 1;
            //     return token;
            // }
            if (index != -1) return new Token(ops[index], TokenType.OPERATOR);
            else throw new ArgumentException($"Invalid single operator {s[idx]}");
        }

        /// <summary>
        /// Handles multi-character operators (** for exponentiation, // for integer division).
        /// Falls back to HandleSingleOp if only one symbol.
        /// </summary>
        private Token HandleMultiOp(string s, ref int idx)
        {
            // add comments plz
            if (idx == s.Length - 1) return HandleSingleOp(s, ref idx);

            string code = String.Concat(s[idx], s[idx + 1]);

            if (code == TokenConstants.EXPONENTIATE || code == TokenConstants.INT_DIVISION)
            {
                var token = new Token(code, TokenType.OPERATOR);
                idx += 2;
                return token;
            }
            else if (s[idx].ToString() == TokenConstants.TIMES || s[idx].ToString() == TokenConstants.FLOAT_DIVISION)
            {
                return HandleSingleOp(s, ref idx);
            }
            else throw new ArgumentException($"Invalid multi-operator: {code}");
        }

        /// <summary>
        /// Handles variables (identifiers), which are sequences of letters.
        /// </summary>
        private Token HandleVariable(string s, ref int idx)
        {
            string code = "";
            while (idx < s.Length && IsLetter(s[idx]))
            {
                code += s[idx];
                idx += 1;
            }
            return new Token(code, TokenType.VARIABLE);
        }

        /// <summary>
        /// Handles numbers and determines whether they are integers or floats. If float, run HandleDigit again after decimal point.
        /// </summary>
        private Token HandleNumber(string s, ref int idx)
        {
            string numbers = "";

            // Collect digits before decimal point
            numbers += HandleDigits(s, ref idx);

            // Check for decimal point to classify as float
            if (idx < s.Length && s[idx].ToString() == TokenConstants.DECIMAL_POINT)
            {
                numbers += s[idx];
                idx += 1;
                numbers += HandleDigits(s, ref idx);
                return new Token(numbers, TokenType.FLOAT);
            }

            return new Token(numbers, TokenType.INTEGER);
        }

        /// <summary>
        /// Handles digits.
        /// </summary>
        private string HandleDigits(string s, ref int idx)
        {
            // redundant - do handleInteger
            string decNums = "";
            while (idx < s.Length && IsDigit(s[idx]))
            {
                decNums += s[idx];
                idx += 1;
            }
            return decNums;
        }

        /// <summary>
        /// Handles grouping symbols: ( ), { }.
        /// </summary>
        private Token HandleGrouping(string s, ref int idx)
        {
            TokenType tkType;
            string code = s[idx].ToString();

            if (code == TokenConstants.LEFT_PAREN) tkType = TokenType.LEFT_PAREN;
            else if (code == TokenConstants.RIGHT_PAREN) tkType = TokenType.RIGHT_PAREN;
            else if (code == TokenConstants.LEFT_CURLY) tkType = TokenType.LEFT_CURLY;
            else if (code == TokenConstants.RIGHT_CURLY) tkType = TokenType.RIGHT_CURLY;
            else throw new ArgumentException($"Unrecognized Grouping character: {code}");

            idx += 1;
            return new Token(code, tkType);
        }

        #endregion

        #region Identifiers
        // Helper methods to classify characters

        /// <summary>
        /// Determines if a character is whitespace.
        /// </summary>
        private bool IsWhiteSpace(char c)
        {
            return String.IsNullOrWhiteSpace(c.ToString());
        }

        /// <summary>
        /// Determines if a character is a digit (0–9).
        /// </summary>
        private bool IsDigit(char c)
        {
            return Char.IsDigit(c);
        }

        /// <summary>
        /// Determines if a character could be part of a float literal
        /// (either digit or decimal point).
        /// </summary>
        private bool IsFloat(char c)
        {
            return Char.IsDigit(c) || c == '.';
        }

        /// <summary>
        /// Determines if a character is an alphabetic letter.
        /// </summary>
        private bool IsLetter(char c)
        {
            return Char.IsLetter(c);
        }

        /// <summary>
        /// Determines if a character is a grouping symbol (() or {}).
        /// </summary>
        private bool IsGrouper(char c)
        {
            string s = c.ToString();
            return s == "(" || s == ")" || s == "{" || s == "}";
        }

        #endregion
    }
}

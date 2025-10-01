


using System.Text;


/**
<<<<<<< Updated upstream
* Summary: 
=======
* Tokenizer implementation that converts a string into a list of tokens,
* using helper functions to identify operators, numbers, keywords, variables,
* and grouping symbols. Supports handling of multi-character operators and
* differentiation between integers and floats.
*
* Bugs: None known
>>>>>>> Stashed changes
*
* @author Reza Naqvi
* @author Will Zoeller
* @date 9/28/25
*/
<<<<<<< Updated upstream
=======
using System.Text;

>>>>>>> Stashed changes
namespace Tokenizer
{
    /// <summary>
    /// Provides functionality to convert a string of code into a sequence
    /// of tokens that can be further analyzed by later components (e.g., parser).
    /// </summary>
    public class TokenizerImpl
<<<<<<< Updated upstream
    { 
        private List<Token> Tokenize(string str)
        {
            var lst = new List<Token>();
            // watch out for multiple-char ops
            string multChar = "";
            foreach (char e in str)
            {
                if (!IsWhiteSpace(e))
                {
                    if (multChar.Length != 0)
                    {
                        // if (multChar == ":=") HandleAssignment();
                        // else if (multChar == "//") HandleIntDiv();
                        // else if (multChar == "**") HandleExponent();


                        lst.Append(HandleMultiOp(multChar += e));

                        multChar = "";
                    }

                    // if a letter, handle variable
                    // if (IsLetter(e)) lst.Append(HandleVariable());

                    // if number, handle int/float
                    // else if (IsDigit(e)) lst.Append(HandleNumber());

                    // if keyword, handle return???

                    // if ()/{}, handle grouping
                    // else if (IsGrouper(e)) lst.Append(HandleGrouping());

                    // if :=
                    else if (e == ':' | e == '/' | e == '*')
=======
    {
        /// <summary>
        /// Converts the input string into a list of tokens by scanning each
        /// character and delegating to specialized handlers depending on
        /// whether the character is a letter, digit, operator, keyword, or grouping symbol.
        /// </summary>
        /// <param name="str">The input string to tokenize.</param>
        /// <returns>A list of tokens representing the input string.</returns>
        public List<Token> Tokenize(string str)
        {
            var lst = new List<Token>();
            int idx = 0;

            // Main loop: iterate through each character of the string
            while (idx < str.Length)
            {
                char e = str[idx];

                if (!IsWhiteSpace(str[idx]))
                {
                    // Handle assignment operator (:=)
                    if (e == ':') lst.Add(HandleAssignment(str, ref idx));

                    // Handle the "return" keyword, or fallback to variable
                    else if (e == 'r') lst.Add(HandleReturn(str, ref idx));

                    // Handle general variable (identifier)
                    else if (IsLetter(e)) lst.Add(HandleVariable(str, ref idx));

                    // Handle number literal (integer or float)
                    else if (IsDigit(e)) lst.Add(HandleNumber(str, ref idx));

                    // Handle grouping symbols: (), {}
                    else if (IsGrouper(e)) lst.Add(HandleGrouping(str, ref idx));

                    // Handle single-character operators: +, -, %, =
                    else if (e == '+' || e == '-' || e == '%' || e == '=')
>>>>>>> Stashed changes
                    {
                        // int idx = str.IndexOf(e);
                        // if (str[idx + 1] == '=') HandleAssignment();
                        // else throw new Exception();
                        multChar += e;
                    }

<<<<<<< Updated upstream

=======
                    // Handle multi-character operators: **, //
                    else if (e == '/' || e == '*')
                    {
                        // Delegates to HandleSingleOp if only one symbol
                        lst.Add(HandleMultiOp(str, ref idx));
                    }
>>>>>>> Stashed changes

                    // If character does not match known categories, it is ignored for now
                }
            }


            return lst;
        }
<<<<<<< Updated upstream
      
        #region Handlers 
        private Token HandleAssignment(string s)
=======

        #region Handlers

        /// <summary>
        /// Handles assignment operator ":=".
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <param name="idx">Reference to the current index in the string.</param>
        /// <returns>A Token representing the assignment operator.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the operator is incomplete or invalid.
        /// </exception>
        private Token HandleAssignment(string s, ref int idx)
>>>>>>> Stashed changes
        {
            if (s.Length != 2) throw new ArgumentException($"Invalid Assignment Operator: {s}");

<<<<<<< Updated upstream
            throw new NotImplementedException();
            // return;

        }

        private Token HandleSingleOp(char c)
        {
            List<string> ops = new List<string> { "+", "-", "*", "/", "%" };
            //Create a new token based on if input contains one of the operators
            int idx = ops.IndexOf(s);
            if (idx != -1)
            {
                var singleOp = ops[idx];
            }
            else
            {
                throw new ArgumentException($"Invalid single operator{s}");
            }
        }

        private Token HandleMultiOp(string s)
        {
            throw new NotImplementedException();
            if (s.StartsWith("*") | s.StartsWith("/"))
=======
            idx += 2;
            return new Token(code, TokenType.ASSIGNMENT);
        }

        /// <summary>
        /// Handles detection of the keyword "return".
        /// Falls back to variable handling if the characters
        /// do not exactly match the keyword.
        /// </summary>
        private Token HandleReturn(string s, ref int idx)
        {
            // If string is too short, treat as variable
            if (s.Length - idx < 6) return HandleVariable(s, ref idx);

            List<char> letters = new List<char> { 'r', 'e', 't', 'u', 'r', 'n' };
            int lIdx = 0;
            string expected = "";
            string actual = "";

            // Compare each character to expected "return"
            while (lIdx < 6)
            {
                actual += s[idx];
                expected += letters[lIdx];

                if (actual != expected)
                {
                    // Reset index if mismatch, fallback to variable
                    idx -= lIdx;
                    return HandleVariable(s, ref idx);
                }
                idx += 1;
                lIdx += 1;
            }

            // If followed by whitespace or grouper, it's a valid keyword
            if (idx == s.Length || IsWhiteSpace(s[idx]) || IsGrouper(s[idx]))
            {
                return new Token(actual, TokenType.RETURN);
            }
            else
            {
                // Otherwise treat as variable
                idx -= lIdx;
                return HandleVariable(s, ref idx);
            }
        }

        /// <summary>
        /// Handles single-character operators such as +, -, *, /, %, =.
        /// </summary>
        private Token HandleSingleOp(string s, ref int idx)
        {
            List<string> ops = new List<string> { "+", "-", "*", "/", "%", "=" };
            int index = ops.IndexOf(s[idx].ToString());

            if (index != -1)
>>>>>>> Stashed changes
            {
                if (s == "**")
                {
                    // create exponentiation token
                    // return;
                }
                if (s == "//")
                {
                    // create exponentiation token
                    // return;
                }
                // create multiplication token
                else return HandleSingleOp(s[0]);
            }
            else
            {
                return HandleAssignment(s);
            }
        }

<<<<<<< Updated upstream
        private Token HandleVariable(string s)
        {
            throw new NotImplementedException();
        }

        private Token HandleNumber(string s)
        {
            // determines float or long string of ints
            throw new NotImplementedException();
        }

        private Token HandleInt(string s)
        {
            throw new NotImplementedException();
        }

        private Token HandleFloat(string s)
        {
            throw new NotImplementedException();
        }

        private Token HandleGrouping(char c)
        {
            throw new NotImplementedException();
=======
        /// <summary>
        /// Handles multi-character operators (** for exponentiation, // for integer division).
        /// Falls back to HandleSingleOp if only one symbol exists.
        /// </summary>
        private Token HandleMultiOp(string s, ref int idx)
        {
            if (idx == s.Length - 1) return HandleSingleOp(s, ref idx);

            string code = String.Concat(s[idx], s[idx + 1]);
            Token token;

            if (code == TokenConstants.EXPONENTIATE || code == TokenConstants.INT_DIVISION)
            {
                token = new Token(code, TokenType.OPERATOR);
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
        /// Handles variables (identifiers), which consist of sequences of letters.
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
        /// Handles numbers, determining whether they are integers or floats.
        /// </summary>
        private Token HandleNumber(string s, ref int idx)
        {
            string numbers = "";

            // Collect digits before decimal point
            while (idx < s.Length && IsDigit(s[idx]))
            {
                numbers += s[idx];
                idx += 1;
            }

            // If decimal point exists, parse as float
            if (idx < s.Length && s[idx].ToString() == TokenConstants.DECIMAL_POINT)
            {
                numbers += s[idx];
                idx += 1;
                numbers += HandleAfterDecimalPoint(s, ref idx);
                return new Token(numbers, TokenType.FLOAT);
            }

            return new Token(numbers, TokenType.INTEGER);
        }

        /// <summary>
        /// Handles digits appearing after the decimal point in a float literal.
        /// </summary>
        private string HandleAfterDecimalPoint(string s, ref int idx)
        {
            string decNums = "";
            while (idx < s.Length && IsDigit(s[idx]))
            {
                decNums += s[idx];
                idx += 1;
            }
            return decNums;
        }

        /// <summary>
        /// Handles grouping symbols: (), {}.
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
>>>>>>> Stashed changes
        }

        #endregion

        #region Identifiers

        /// <summary>
        /// Determines if a character is whitespace.
        /// </summary>
        private bool IsWhiteSpace(char c)
        {
            return c.Equals(" ");
            

        }

        /// <summary>
        /// Determines if a character is a digit.
        /// </summary>
        private bool IsDigit(char c)
        {
<<<<<<< Updated upstream
            return IsDigit(c);
=======
            return Char.IsDigit(c);
        }

        /// <summary>
        /// Determines if a character could be part of a float literal (digit or '.').
        /// </summary>
        private bool IsFloat(char c)
        {
            return Char.IsDigit(c) || c == '.';
>>>>>>> Stashed changes
        }

        /// <summary>
        /// Determines if a character is a letter (A–Z, a–z).
        /// </summary>
        private bool IsLetter(char c)
        {
            return IsLetter(c);
        }

        /// <summary>
        /// Determines if a character is a grouping symbol ((), {}).
        /// </summary>
        private bool IsGrouper(char c)
        {
            string s = c.ToString();
            return s == "(" | s == ")" | s == "{" | s == "}";
        }
        
        #endregion
    }
}

using System.Text;

/**
* Summary: 
*
* Bugs: 
*
* @author Reza Naqvi and Will Zoeller
* @date 9/28/25
*/
namespace Tokenizer
{
    public class TokenizerImpl
    { 
        public List<Token> Tokenize(string str)
        {
            var lst = new List<Token>();
            // watch out for multiple-char ops
            // string multChar = "";
            int idx = 0;
            while (idx < str.Length)
            {
                if (!IsWhiteSpace(str[idx]))
                {
                    // if assignment
                    if (str[idx] == ':') lst.Add(HandleAssignment(str, ref idx));

                    // if a letter, handle variable
                    else if (IsLetter(str[idx])) lst.Add(HandleVariable(str, ref idx));

                    // if number, handle int/float
                    else if (IsDigit(str[idx])) lst.Add(HandleNumber(str, ref idx));

                    // if keyword, handle return???

                    // if ()/{}, handle grouping
                    else if (IsGrouper(str[idx])) lst.Add(HandleGrouping(str, ref idx));

                    // if +, -, %
                    else if (str[idx] == '+' | str[idx] == '-' | str[idx] == '%' | str[idx] == '=')
                    {
                        lst.Add(HandleSingleOp(str, ref idx));
                    }

                    // if *, **, /, //
                    else if (str[idx] == '/' | str[idx] == '*')
                    {
                        // MultiOp calls Single Op if there's only 1 * or 1 /
                        lst.Add(HandleMultiOp(str, ref idx));
                    }

                    // if neither, handle operator

                }
                idx += 1;
            }

            return lst;
        }
      
        #region Handlers 
        private Token HandleAssignment(string s, ref int idx)
        {
            string code = String.Concat(s[idx], s[idx + 1]);
            if (code != ":=") throw new ArgumentException($"Invalid Assignment Operator: {code}");

            idx += 1;
            return new Token(code, TokenType.ASSIGNMENT);

        }

        private Token HandleSingleOp(string s, ref int idx)
        {
            // This is O(n), should be an O(1) soln
            List<string> ops = new List<string> { "+", "-", "*", "/", "%" , "="};
            //Create a new token based on if input contains one of the operators
            int index = ops.IndexOf(s);
            if (index != -1)
            {
                string singleOp = ops[index];
                var token = new Token(singleOp, TokenType.OPERATOR);
                return token;
            }
            else throw new ArgumentException($"Invalid single operator{s[idx]}");
        }
        
        private Token HandleMultiOp(string s, ref int idx)
        {
            string code = String.Concat(s[idx], s[idx + 1]);
            Token token;
            if (code == TokenConstants.EXPONENTIATE | code == TokenConstants.INT_DIVISION)
            {
                // create exponentiation token
                token = new Token(code, TokenType.OPERATOR);
                idx += 1;
                return token;
            }
            else if (s[idx].ToString() == TokenConstants.TIMES | s[idx].ToString() == TokenConstants.FLOAT_DIVISION)
            {
                return HandleSingleOp(s, ref idx);
            }
            // create multiplication token
            // else return HandleSingleOp(s[0]);
            else throw new ArgumentException($"Invalid multi-operator: {code}");

        }

        private Token HandleVariable(string s, ref int idx)
        {
            string code = "";
            while (idx < s.Length & IsLetter(s[idx]))
            {
                code += s[idx];
                idx += 1;
            }
            return new Token(code, TokenType.VARIABLE);
        }

        // TODO
        private Token HandleNumber(string s, ref int idx)
        {
            // determines float or long string of ints
            throw new NotImplementedException();

        }

        // TODO
        private Token HandleInt(string s, ref int idx)
        {
            throw new NotImplementedException();
        }

        // TODO
        private Token HandleFloat(string s, ref int idx)
        {
            throw new NotImplementedException();
        }

        private Token HandleGrouping(string s, ref int idx)
        {
            TokenType tkType;
            string code = s[idx].ToString();
            if (code == TokenConstants.LEFT_PAREN) tkType = TokenType.LEFT_PAREN;
            else if (code == TokenConstants.RIGHT_PAREN) tkType = TokenType.RIGHT_PAREN;
            else if (code == TokenConstants.LEFT_CURLY) tkType = TokenType.LEFT_CURLY;
            else if (code == TokenConstants.RIGHT_CURLY) tkType = TokenType.RIGHT_CURLY;

            else throw new ArgumentException($"Unrecognized Grouping character: {code}");

            return new Token(code, tkType);
        }

        #endregion

        #region Identifiers

        private bool IsWhiteSpace(char c)
        {
            return String.IsNullOrWhiteSpace(c.ToString());
        }

        private bool IsDigit(char c)
        {
            return Char.IsDigit(c);
        }

        private bool IsLetter(char c)
        {
            return Char.IsLetter(c);
        }

        private bool IsGrouper(char c)
        {
            string s = c.ToString();
            return s == "(" | s == ")" | s == "{" | s == "}";
        }
        
        #endregion
    }
}
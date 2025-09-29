
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
                    {
                        // int idx = str.IndexOf(e);
                        // if (str[idx + 1] == '=') HandleAssignment();
                        // else throw new Exception();
                        multChar += e;
                    }



                    // if neither, handle operator

                }
            }


            return lst;
        }

        #region Handlers 
        private Token HandleAssignment(string s)
        {
            if (s.Length != 2) throw new ArgumentException($"Invalid Assignment Operator: {s}");

            throw new NotImplementedException();
            // return;

        }

        private Token HandleSingleOp(char c)
        {
            throw new NotImplementedException();
        }

        private Token HandleMultiOp(string s)
        {
            throw new NotImplementedException();
            if (s.StartsWith("*") | s.StartsWith("/"))
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
        }

        #endregion

        #region Identifiers

        private bool IsWhiteSpace(char c)
        {
            return c.Equals(" ");
        }

        private bool IsDigit(char c)
        {
            return IsDigit(c);
        }

        private bool IsLetter(char c)
        {
            return IsLetter(c);
        }

        private bool IsGrouper(char c)
        {
            string s = c.ToString();
            return s == "(" | s == ")" | s == "{" | s == "}";
        }
        
        #endregion
    }
}
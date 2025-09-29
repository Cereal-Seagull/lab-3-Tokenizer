
using Microsoft.VisualBasic;


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
            foreach (char e in str)
            {
                if (!IsWhiteSpace(e))
                {
                    
                }
            }


            return lst;
        }

        private void HandleAssignment(string s)
        {
            //var token = new Token();

        }


        //TODO: Return a token.
        private void HandleSingleOp(string s)
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

        private void HandleMultiOp()
        {
            throw new NotImplementedException();
        }

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
    }
}
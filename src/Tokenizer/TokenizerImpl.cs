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

        private void HandleAssignment()
        {
            throw new NotImplementedException();
        }

        private void HandleSingleOp()
        {
            throw new NotImplementedException();
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
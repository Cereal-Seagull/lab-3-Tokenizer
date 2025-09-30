/**
* Summary: 
*
* Bugs: 
*
* @author Reza Naqvi and Will Zoeller
* @date 9/28/25
*/
using System;
using Tokenizer;
using Xunit;

namespace TokenizerTests
{
    public class TokenizerTests
    {
        [Theory]
        [InlineData("x := (2 + 11) * 3", """[["x", VARIABLE], [":=", ASSIGNMENT], ["(", LEFT_PAREN], ["2", INTEGER], ["+", ADD], ["11", INTEGER], [")", RIGHT_PAREN], ["*", TIMES], ["3", INTEGER]]""")]
        public void TestFor_TokenizeFunc(string str, string expectedStr)
        {
            var tkimp = new TokenizerImpl();
            var lst = tkimp.Tokenize(str);
            Assert.Equal(lst.ToString(), expectedStr);
        }
    }
}
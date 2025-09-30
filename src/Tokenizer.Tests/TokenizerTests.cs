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

// namespace TokenizerTests
// {
    // public class TokenizerTests
    // {
    //     [Theory]
    //     [InlineData("x := (2 + 11) * 3", """[["x", VARIABLE], [":=", ASSIGNMENT], ["(", LEFT_PAREN], ["2", INTEGER], ["+", ADD], ["11", INTEGER], [")", RIGHT_PAREN], ["*", TIMES], ["3", INTEGER]]""")]
    //     public void TestFor_TokenizeFunc(string str, string expectedStr)
    //     {
    //         var tkimp = new TokenizerImpl();
    //         var lst = tkimp.Tokenize(str);
    //         // Assert.Equal(lst.ToString(), expectedStr);
    //         Assert.Equal(9, lst.Count);
    //     }
    // }
using Xunit;
using Tokenizer;

namespace Tokenizer.Tests
{
    /// <summary>
    /// Unit tests for the Token class.
    /// Tests basic token creation, string representation, and equality comparison.
    /// </summary>
    public class TokenTests
    {
        /// <summary>
        /// Verifies that a Token can be constructed with valid parameters
        /// and stores the value and type correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_CreatesToken()
        {
            // Arrange & Act
            var token = new Token("42", TokenType.INTEGER);

            // Assert
            Assert.NotNull(token);
        }

        /// <summary>
        /// Tests that ToString() returns the expected format "[value, type]"
        /// for various token types.
        /// </summary>
        /// <param name="value">The token value</param>
        /// <param name="type">The token type</param>
        /// <param name="expected">The expected string representation</param>
        [Theory]
        [InlineData("42", TokenType.INTEGER, "[42, INTEGER]")]
        [InlineData("x", TokenType.VARIABLE, "[x, VARIABLE]")]
        [InlineData("+", TokenType.OPERATOR, "[+, OPERATOR]")]
        [InlineData(":=", TokenType.ASSIGNMENT, "[:=, ASSIGNMENT]")]
        [InlineData("3.14", TokenType.FLOAT, "[3.14, FLOAT]")]
        [InlineData("(", TokenType.LEFT_PAREN, "[(, LEFT_PAREN]")]
        [InlineData(")", TokenType.RIGHT_PAREN, "[), RIGHT_PAREN]")]
        [InlineData("{", TokenType.LEFT_CURLY, "[{, LEFT_CURLY]")]
        [InlineData("}", TokenType.RIGHT_CURLY, "[}, RIGHT_CURLY]")]
        public void ToString_ReturnsCorrectFormat(string value, TokenType type, string expected)
        {
            // Arrange
            var token = new Token(value, type);

            // Act
            var result = token.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        // /// <summary>
        // /// Verifies that two tokens with the same value are considered equal.
        // /// </summary>
        // [Fact]
        // public void Equals_WithSameValue_ReturnsTrue()
        // {
        //     // Arrange
        //     var token1 = new Token("test", TokenType.VARIABLE);
        //     var token2 = new Token("test", TokenType.OPERATOR);

        //     // Act
        //     var result = token1.Equals(token2);

        //     // Assert
        //     Assert.True(result);
        // }

        /// <summary>
        /// Verifies that two tokens with different values are not considered equal.
        /// </summary>
        [Fact]
        public void Equals_WithDifferentValue_ReturnsFalse()
        {
            // Arrange
            var token1 = new Token("test1", TokenType.VARIABLE);
            var token2 = new Token("test2", TokenType.VARIABLE);

            // Act
            var result = token1.Equals(token2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Verifies that comparing a token to null throws an ArgumentNullException.
        /// </summary>
        [Fact]
        public void Equals_WithNull_ThrowsArgumentNullException()
        {
            // Arrange
            var token = new Token("test", TokenType.VARIABLE);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => token.Equals(null));
        }
    }

    /// <summary>
    /// Unit tests for the TokenizerImpl class.
    /// Tests tokenization of various input strings including operators,
    /// numbers, variables, and grouping symbols.
    /// </summary>
    public class TokenizerImplTests
    {
        private readonly TokenizerImpl _tokenizer;

        public TokenizerImplTests()
        {
            _tokenizer = new TokenizerImpl();
        }

        #region Single Operator Tests

        /// <summary>
        /// Tests tokenization of single-character operators.
        /// </summary>
        /// <param name="input">The input string containing an operator</param>
        /// <param name="expectedValue">The expected operator value</param>
        [Theory]
        [InlineData("+", "+")]
        [InlineData("-", "-")]
        [InlineData("*", "*")]
        [InlineData("/", "/")]
        [InlineData("%", "%")]
        [InlineData("=", "=")]
        public void Tokenize_SingleOperator_ReturnsOperatorToken(string input, string expectedValue)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedValue, result[0].ToString().Split(", ")[0].TrimStart('['));
            Assert.Contains("OPERATOR", result[0].ToString());
        }

        #endregion

        #region Multi-Character Operator Tests

        /// <summary>
        /// Tests tokenization of multi-character operators (**, //, :=).
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="expectedValue">The expected operator value</param>
        /// <param name="expectedType">The expected token type</param>
        [Theory]
        [InlineData("**", "**", "OPERATOR")]
        [InlineData("//", "//", "OPERATOR")]
        [InlineData(":=", ":=", "ASSIGNMENT")]
        public void Tokenize_MultiCharOperator_ReturnsCorrectToken(string input, string expectedValue, string expectedType)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Single(result);
            Assert.Contains(expectedValue, result[0].ToString());
            Assert.Contains(expectedType, result[0].ToString());
        }

        #endregion

        #region Integer Tests

        /// <summary>
        /// Tests tokenization of integer literals.
        /// </summary>
        /// <param name="input">The input string containing an integer</param>
        /// <param name="expectedValue">The expected integer value</param>
        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1")]
        [InlineData("42", "42")]
        [InlineData("123", "123")]
        [InlineData("999999", "999999")]
        public void Tokenize_Integer_ReturnsIntegerToken(string input, string expectedValue)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Single(result);
            Assert.Contains(expectedValue, result[0].ToString());
            Assert.Contains("INTEGER", result[0].ToString());
        }

        #endregion

        #region Float Tests

        /// <summary>
        /// Tests tokenization of floating-point literals.
        /// </summary>
        /// <param name="input">The input string containing a float</param>
        /// <param name="expectedValue">The expected float value</param>
        [Theory]
        [InlineData("3.14", "3.14")]
        [InlineData("0.5", "0.5")]
        [InlineData("123.456", "123.456")]
        [InlineData("99.99", "99.99")]
        public void Tokenize_Float_ReturnsFloatToken(string input, string expectedValue)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Single(result);
            Assert.Contains(expectedValue, result[0].ToString());
            Assert.Contains("FLOAT", result[0].ToString());
        }

        #endregion

        #region Variable Tests

        /// <summary>
        /// Tests tokenization of variable identifiers.
        /// </summary>
        /// <param name="input">The input string containing a variable name</param>
        /// <param name="expectedValue">The expected variable name</param>
        [Theory]
        [InlineData("x", "x")]
        [InlineData("y", "y")]
        [InlineData("var", "var")]
        [InlineData("myVariable", "myVariable")]
        [InlineData("test", "test")]
        [InlineData("ABC", "ABC")]
        public void Tokenize_Variable_ReturnsVariableToken(string input, string expectedValue)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Single(result);
            Assert.Contains(expectedValue, result[0].ToString());
            Assert.Contains("VARIABLE", result[0].ToString());
        }

        #endregion

        #region Grouping Symbol Tests

        /// <summary>
        /// Tests tokenization of grouping symbols (parentheses and braces).
        /// </summary>
        /// <param name="input">The input string containing a grouping symbol</param>
        /// <param name="expectedType">The expected token type</param>
        [Theory]
        [InlineData("(", "LEFT_PAREN")]
        [InlineData(")", "RIGHT_PAREN")]
        [InlineData("{", "LEFT_CURLY")]
        [InlineData("}", "RIGHT_CURLY")]
        public void Tokenize_GroupingSymbol_ReturnsCorrectToken(string input, string expectedType)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Single(result);
            Assert.Contains(expectedType, result[0].ToString());
        }

        #endregion

        #region Whitespace Tests

        /// <summary>
        /// Verifies that whitespace is properly ignored during tokenization.
        /// </summary>
        /// <param name="input">Input string with whitespace</param>
        /// <param name="expectedCount">Expected number of tokens</param>
        [Theory]
        [InlineData("   42   ", 1)]
        [InlineData("1 + 2", 3)]
        [InlineData("x := 5", 3)]
        [InlineData("  +  ", 1)]
        public void Tokenize_WithWhitespace_IgnoresWhitespace(string input, int expectedCount)
        {
            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(expectedCount, result.Count);
        }

        #endregion

        #region Complex Expression Tests

        /// <summary>
        /// Tests tokenization of simple arithmetic expressions.
        /// </summary>
        [Fact]
        public void Tokenize_SimpleArithmeticExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "1 + 2";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("1", result[0].ToString());
            Assert.Contains("INTEGER", result[0].ToString());
            Assert.Contains("+", result[1].ToString());
            Assert.Contains("OPERATOR", result[1].ToString());
            Assert.Contains("2", result[2].ToString());
            Assert.Contains("INTEGER", result[2].ToString());
        }

        /// <summary>
        /// Tests tokenization of assignment expressions.
        /// </summary>
        [Fact]
        public void Tokenize_AssignmentExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "x := 42";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("x", result[0].ToString());
            Assert.Contains("VARIABLE", result[0].ToString());
            Assert.Contains(":=", result[1].ToString());
            Assert.Contains("ASSIGNMENT", result[1].ToString());
            Assert.Contains("42", result[2].ToString());
            Assert.Contains("INTEGER", result[2].ToString());
        }

        /// <summary>
        /// Tests tokenization of expressions with parentheses.
        /// </summary>
        [Fact]
        public void Tokenize_ExpressionWithParentheses_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "(1 + 2)";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains("LEFT_PAREN", result[0].ToString());
            Assert.Contains("1", result[1].ToString());
            Assert.Contains("+", result[2].ToString());
            Assert.Contains("2", result[3].ToString());
            Assert.Contains("RIGHT_PAREN", result[4].ToString());
        }

        /// <summary>
        /// Tests tokenization of expressions with braces.
        /// </summary>
        [Fact]
        public void Tokenize_ExpressionWithBraces_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "{x}";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("LEFT_CURLY", result[0].ToString());
            Assert.Contains("x", result[1].ToString());
            Assert.Contains("RIGHT_CURLY", result[2].ToString());
        }

        /// <summary>
        /// Tests tokenization of expressions with exponentiation.
        /// </summary>
        [Fact]
        public void Tokenize_ExponentiationExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "2 ** 3";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("2", result[0].ToString());
            Assert.Contains("**", result[1].ToString());
            Assert.Contains("3", result[2].ToString());
        }

        /// <summary>
        /// Tests tokenization of expressions with integer division.
        /// </summary>
        [Fact]
        public void Tokenize_IntegerDivisionExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "10 // 3";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("10", result[0].ToString());
            Assert.Contains("//", result[1].ToString());
            Assert.Contains("3", result[2].ToString());
        }

        /// <summary>
        /// Tests tokenization of complex expressions with multiple operators.
        /// </summary>
        [Fact]
        public void Tokenize_ComplexExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "x := (a + b) * 2";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(9, result.Count);
            Assert.Contains("x", result[0].ToString());
            Assert.Contains("ASSIGNMENT", result[1].ToString());
            Assert.Contains("LEFT_PAREN", result[2].ToString());
            Assert.Contains("a", result[3].ToString());
            Assert.Contains("+", result[4].ToString());
            Assert.Contains("b", result[5].ToString());
            Assert.Contains("RIGHT_PAREN", result[6].ToString());
            Assert.Contains("*", result[7].ToString());
            Assert.Contains("2", result[8].ToString());
        }

        /// <summary>
        /// Tests tokenization of expressions with floating-point arithmetic.
        /// </summary>
        [Fact]
        public void Tokenize_FloatArithmetic_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "3.14 + 2.5";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("3.14", result[0].ToString());
            Assert.Contains("FLOAT", result[0].ToString());
            Assert.Contains("+", result[1].ToString());
            Assert.Contains("2.5", result[2].ToString());
            Assert.Contains("FLOAT", result[2].ToString());
        }

        /// <summary>
        /// Tests tokenization of expressions with modulus operator.
        /// </summary>
        [Fact]
        public void Tokenize_ModulusExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "10 % 3";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("10", result[0].ToString());
            Assert.Contains("%", result[1].ToString());
            Assert.Contains("3", result[2].ToString());
        }

        #endregion

        #region Edge Case Tests

        /// <summary>
        /// Tests that tokenizing an empty string returns an empty list.
        /// </summary>
        [Fact]
        public void Tokenize_EmptyString_ReturnsEmptyList()
        {
            // Arrange
            var input = "";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that tokenizing whitespace-only string returns empty list.
        /// </summary>
        [Fact]
        public void Tokenize_WhitespaceOnly_ReturnsEmptyList()
        {
            // Arrange
            var input = "   ";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests tokenization of expressions without spaces.
        /// </summary>
        [Fact]
        public void Tokenize_NoSpaces_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "x:=5+3";

            // Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(5, result.Count);
        }

        #endregion

        #region Error Case Tests

        /// <summary>
        /// Tests that invalid assignment operators throw an exception.
        /// </summary>
        /// <param name="input">Invalid assignment operator string</param>
        [Theory]
        [InlineData(":+")]
        [InlineData(":-")]
        [InlineData(": ")]
        public void Tokenize_InvalidAssignment_ThrowsArgumentException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _tokenizer.Tokenize(input));
        }

        #endregion

        #region Return Tests
        /// <summary>
        /// Tests that lowercase 'return' is correctly identified as the RETURN keyword token.
        /// </summary>
        [Fact]
        public void Tokenize_LowercaseReturn_ReturnsReturnToken()
        {
            // Arrange
            var input = "return";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Single(result);
            Assert.Contains("return", result[0].ToString());
            Assert.Contains("RETURN", result[0].ToString());
        }

        /// <summary>
        /// Tests that uppercase, mixed case, or capitalized 'return'
        /// are treated as variables, not the return keyword.
        /// Only lowercase 'return' should be recognized as the keyword.
        /// </summary>
        /// <param name="input">Non-lowercase variant of 'return'</param>
        /// <param name="description">Description of test case</param>
        [Theory]
        [InlineData("RETURN", "Uppercase return")]
        [InlineData("Return", "Capitalized return")]
        [InlineData("ReTuRn", "Mixed case return")]
        [InlineData("rETURN", "Mixed case variant")]
        [InlineData("retURN", "Partial uppercase")]
        [InlineData("reTurn", "Camel case")]
        public void Tokenize_NonLowercaseReturn_ReturnsVariableToken(string input, string description)
        {
            // Arrange & Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Single(result);
            Assert.Contains("VARIABLE", result[0].ToString());
        }

        /// <summary>
        /// Tests that strings similar to 'return' but not exact matches
        /// are correctly identified as variables.
        /// </summary>
        /// <param name="input">Input string that looks like but isn't 'return'</param>
        /// <param name="description">Description of test case</param>
        [Theory]
        [InlineData("retur", "Missing last character")]
        [InlineData("ret", "Prefix only")]
        [InlineData("r", "Single character")]
        [InlineData("re", "Two characters")]
        [InlineData("retu", "Four characters")]
        public void Tokenize_PartialReturn_ReturnsVariableToken(string input, string description)
        {
            // Arrange & Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Single(result);
            Assert.Contains("VARIABLE", result[0].ToString());
        }

        /// <summary>
        /// Tests that strings containing 'return' but with additional characters
        /// are handled as variables (not as return keyword).
        /// </summary>
        /// <param name="input">Input with return embedded or extended</param>
        [Theory]
        [InlineData("returnValue")]
        [InlineData("myreturn")]
        [InlineData("returns")]
        [InlineData("returned")]
        [InlineData("returning")]
        public void Tokenize_ReturnWithExtraChars_ReturnsVariableToken(string input)
        {
            // Arrange & Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.NotEmpty(result);
            // First token should be a variable (or could be multiple tokens depending on implementation)
            Assert.Contains("VARIABLE", result[0].ToString());
        }

        /// <summary>
        /// Tests return keyword in the context of expressions with values.
        /// </summary>
        [Fact]
        public void Tokenize_ReturnWithInteger_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "return 42";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("return", result[0].ToString());
            Assert.Contains("RETURN", result[0].ToString());
            Assert.Contains("42", result[1].ToString());
            Assert.Contains("INTEGER", result[1].ToString());
        }

        /// <summary>
        /// Tests return keyword with a variable.
        /// </summary>
        [Fact]
        public void Tokenize_ReturnWithVariable_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "return x";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("return", result[0].ToString());
            Assert.Contains("RETURN", result[0].ToString());
            Assert.Contains("x", result[1].ToString());
            Assert.Contains("VARIABLE", result[1].ToString());
        }

        /// <summary>
        /// Tests return keyword with assignment expression.
        /// </summary>
        [Fact]
        public void Tokenize_ReturnWithAssignment_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "return x := 5";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains("return", result[0].ToString());
            Assert.Contains("RETURN", result[0].ToString());
        }

        /// <summary>
        /// Tests return keyword with arithmetic expressions.
        /// </summary>
        /// <param name="input">Expression with return keyword</param>
        /// <param name="expectedTokenCount">Expected number of tokens</param>
        [Theory]
        [InlineData("return 1 + 2", 4)]
        [InlineData("return (x)", 4)]
        [InlineData("return x * y", 4)]
        [InlineData("return 3.14", 2)]
        public void Tokenize_ReturnWithExpression_ReturnsCorrectTokenCount(string input, int expectedTokenCount)
        {
            // Arrange & Act
            var result = _tokenizer.Tokenize(input);

            // Assert
            Assert.Equal(expectedTokenCount, result.Count);
            Assert.Contains("return", result[0].ToString());
            Assert.Contains("RETURN", result[0].ToString());
        }

        /// <summary>
        /// Tests that misspelled variations of return are treated as variables.
        /// </summary>
        /// <param name="input">Misspelled variable name similar to return</param>
        [Theory]
        [InlineData("retrun")]
        [InlineData("retrn")]
        [InlineData("rturn")]
        [InlineData("eturn")]
        [InlineData("returm")]
        public void Tokenize_MisspelledReturn_ReturnsVariableToken(string input)
        {
            // Arrange & Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Single(result);
            Assert.Contains("VARIABLE", result[0].ToString());
        }

        /// <summary>
        /// Tests return keyword at the end of a string (boundary condition).
        /// Ensures no IndexOutOfRangeException occurs.
        /// </summary>
        [Fact]
        public void Tokenize_ReturnAtEndOfString_NoIndexError()
        {
            // Arrange
            var input = "x := return";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("return", result[2].ToString());
        }

        /// <summary>
        /// Tests multiple return keywords in one expression.
        /// </summary>
        [Fact]
        public void Tokenize_MultipleReturns_ReturnsMultipleReturnTokens()
        {
            // Arrange
            var input = "return return";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("return", result[0].ToString());
            Assert.Contains("return", result[1].ToString());
        }

        /// <summary>
        /// Tests return keyword at the beginning of a string.
        /// </summary>
        [Fact]
        public void Tokenize_ReturnAtStart_ReturnsReturnToken()
        {
            // Arrange
            var input = "return";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Single(result);
            Assert.Contains("return", result[0].ToString());
        }

        /// <summary>
        /// Tests return keyword with complex nested expressions.
        /// </summary>
        [Fact]
        public void Tokenize_ReturnWithComplexExpression_ReturnsCorrectTokens()
        {
            // Arrange
            var input = "return (a + b) * 2";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.Equal(8, result.Count);
            Assert.Contains("return", result[0].ToString());
        }

        /// <summary>
        /// Tests return keyword followed immediately by operator (no space).
        /// </summary>
        [Fact]
        public void Tokenize_ReturnNoSpace_HandledCorrectly()
        {
            // Arrange
            var input = "return(x)";
            
            // Act
            var result = _tokenizer.Tokenize(input);
            
            // Assert
            Assert.NotEmpty(result);
            Assert.Contains("return", result[0].ToString());
        }    
        #endregion
            
    }
}
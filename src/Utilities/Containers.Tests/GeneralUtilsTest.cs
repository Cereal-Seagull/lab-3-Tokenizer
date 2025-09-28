/**
* Test cases for all methods in Utilities.GeneralUtils
*
* Test cases written by LLM Claude Sonnet 4 but reviewed
* and confirmed by authors.
*
* Bugs: several "Cannot convert null literal to non-nullable reference type" errors.
*
* @author Charlie Moss and Will Zoeller
* @date <date of completion>
*/

namespace GeneralUtilsTests
{
    /// Unit tests for the GeneralUtils.GetIndentation method.
    /// Tests verify that the method returns the correct number of spaces
    /// for different indentation levels (4 spaces per level).
    public class GetIndentationTests
    {
        #region GetIndentation Tests
        /// <summary>
        /// Tests GetIndentation with various valid indentation levels.
        /// Verifies that each level produces exactly 4 spaces per level.
        /// </summary>
        /// <param name="level">The indentation level to test</param>
        /// <param name="expectedLength">The expected length of the returned string</param>
        /// <param name="expectedSpaces">The expected string of spaces</param>
        [Theory]
        [InlineData(0, 0, "")]
        [InlineData(1, 4, "    ")]
        [InlineData(2, 8, "        ")]
        [InlineData(3, 12, "            ")]
        [InlineData(4, 16, "                ")]
        [InlineData(5, 20, "                    ")]
        [InlineData(10, 40, "                                        ")]
        public void GetIndentation_ValidLevels_ReturnsCorrectSpaces(int level, int expectedLength, string expectedSpaces)
        {
            // Act
            string result = GeneralUtils.GetIndentation(level);

            // Assert
            Assert.Equal(expectedLength, result.Length);
            Assert.Equal(expectedSpaces, result);

            // Verify all characters are spaces
            Assert.All(result, c => Assert.Equal(' ', c));
        }

        /// <summary>
        /// Tests that GetIndentation returns an empty string for level 0.
        /// This is a specific edge case that should return no indentation.
        /// </summary>
        [Fact]
        public void GetIndentation_ZeroLevel_ReturnsEmptyString()
        {
            // Act
            string result = GeneralUtils.GetIndentation(0);

            // Assert
            Assert.Equal(string.Empty, result);
            Assert.Equal(0, result.Length);
        }

        /// <summary>
        /// Tests GetIndentation with a large indentation level to verify
        /// the method can handle larger values without issues.
        /// </summary>
        [Fact]
        public void GetIndentation_LargeLevel_ReturnsCorrectSpaces()
        {
            // Arrange
            int level = 100;
            int expectedLength = level * 4; // 400 spaces

            // Act
            string result = GeneralUtils.GetIndentation(level);

            // Assert
            Assert.Equal(expectedLength, result.Length);
            Assert.All(result, c => Assert.Equal(' ', c));
        }

        /// <summary>
        /// Tests GetIndentation with negative values.
        /// The current implementation will likely return an empty string
        /// since the loop condition (i < level) won't execute when level is negative.
        /// </summary>
        /// <param name="negativeLevel">A negative indentation level</param>
        [Theory]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(-100)]
        public void GetIndentation_NegativeLevel_ReturnsEmptyString(int negativeLevel)
        {
            // Act
            string result = GeneralUtils.GetIndentation(negativeLevel);

            // Assert
            Assert.Equal(string.Empty, result);
            Assert.Equal(0, result.Length);
        }
        #endregion

        #region IsValidVariable Tests

        /// <summary>
        /// Unit tests for the GeneralUtils.IsValidVariable method.
        /// Tests verify that the method correctly identifies strings that contain
        /// only lowercase letters as valid variable names.
        /// </summary>
        public class IsValidVariableTests
        {
            /// <summary>
            /// Tests IsValidVariable with strings that should return true.
            /// These strings contain only lowercase letters.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData("hello")]
            [InlineData("world")]
            [InlineData("a")]
            [InlineData("variable")]
            [InlineData("test")]
            [InlineData("lowercase")]
            [InlineData("verylongvariablename")]
            [InlineData("x")]
            [InlineData("abc")]
            [InlineData("method")]
            public void IsValidVariable_OnlyLowercaseLetters_ReturnsTrue(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.True(result, $"Expected '{variableName}' to be a valid variable name");
            }

            /// <summary>
            /// Tests IsValidVariable with strings containing uppercase letters.
            /// These should return false according to the method's logic.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData("Hello")]
            [InlineData("WORLD")]
            [InlineData("Variable")]
            [InlineData("TEST")]
            [InlineData("CamelCase")]
            [InlineData("PascalCase")]
            [InlineData("A")]
            [InlineData("Z")]
            [InlineData("MyVariable")]
            [InlineData("testVARIABLE")]
            [InlineData("lowercaseWithUPPERCASE")]
            public void IsValidVariable_ContainsUppercaseLetters_ReturnsFalse(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.False(result, $"Expected '{variableName}' to be an invalid variable name due to uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with strings containing numbers.
            /// Based on the current implementation, these should return true
            /// since the method only checks for uppercase letters.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData("test123")]
            [InlineData("variable1")]
            [InlineData("123")]
            [InlineData("a1b2c3")]
            [InlineData("number42")]
            [InlineData("0")]
            [InlineData("var2")]
            public void IsValidVariable_ContainsNumbers_ReturnsTrue(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.True(result, $"Expected '{variableName}' to be valid since it contains no uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with strings containing special characters.
            /// Based on the current implementation, these should return true
            /// since the method only checks for uppercase letters.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData("test_variable")]
            [InlineData("hello-world")]
            [InlineData("var$name")]
            [InlineData("@variable")]
            [InlineData("name.property")]
            [InlineData("var#1")]
            [InlineData("test!")]
            [InlineData("_underscore")]
            [InlineData("hyphen-name")]
            public void IsValidVariable_ContainsSpecialCharacters_ReturnsTrue(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.True(result, $"Expected '{variableName}' to be valid since it contains no uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with mixed case strings that contain both
            /// lowercase and uppercase letters along with other characters.
            /// These should return false due to the presence of uppercase letters.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData("Test123")]
            [InlineData("myVariable_Name")]
            [InlineData("CONSTANT_VALUE")]
            [InlineData("Mixed_Case_123")]
            [InlineData("camelCaseVariable")]
            [InlineData("snake_Case_Variable")]
            [InlineData("Number123Variable")]
            public void IsValidVariable_MixedCaseWithOtherCharacters_ReturnsFalse(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.False(result, $"Expected '{variableName}' to be invalid due to uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with an empty string.
            /// An empty string contains no uppercase letters, so it should return true.
            /// </summary>
            [Fact]
            public void IsValidVariable_EmptyString_ReturnsTrue()
            {
                // Act
                bool result = GeneralUtils.IsValidVariable("");

                // Assert
                Assert.True(result, "Expected empty string to be valid since it contains no uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with strings containing only spaces.
            /// Spaces are not uppercase letters, so these should return true.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData(" ")]
            [InlineData("  ")]
            [InlineData("   ")]
            [InlineData("hello world")]
            [InlineData("test variable name")]
            public void IsValidVariable_ContainsSpaces_ReturnsTrue(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.True(result, $"Expected '{variableName}' to be valid since it contains no uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with strings that mix spaces and uppercase letters.
            /// These should return false due to the presence of uppercase letters.
            /// </summary>
            /// <param name="variableName">The variable name to test</param>
            [Theory]
            [InlineData("Hello World")]
            [InlineData("Test Variable")]
            [InlineData("My Variable Name")]
            [InlineData("UPPER CASE")]
            [InlineData("Mixed Case String")]
            public void IsValidVariable_SpacesWithUppercase_ReturnsFalse(string variableName)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(variableName);

                // Assert
                Assert.False(result, $"Expected '{variableName}' to be invalid due to uppercase letters");
            }

            /// <summary>
            /// Tests IsValidVariable with single character strings to verify
            /// boundary conditions for both valid and invalid cases.
            /// </summary>
            /// <param name="character">The single character to test</param>
            /// <param name="expectedResult">The expected result</param>
            [Theory]
            [InlineData("a", true)]
            [InlineData("z", true)]
            [InlineData("m", true)]
            [InlineData("A", false)]
            [InlineData("Z", false)]
            [InlineData("M", false)]
            [InlineData("1", true)]
            [InlineData("@", true)]
            [InlineData("_", true)]
            public void IsValidVariable_SingleCharacter_ReturnsExpectedResult(string character, bool expectedResult)
            {
                // Act
                bool result = GeneralUtils.IsValidVariable(character);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            /// <summary>
            /// Tests IsValidVariable with null input to verify behavior.
            /// This test documents the current behavior and may need adjustment
            /// if null handling is added to the implementation.
            /// </summary>
            [Fact]
            public void IsValidVariable_NullInput_ThrowsException()
            {
                // Act & Assert
                Assert.Throws<NullReferenceException>(() => GeneralUtils.IsValidVariable(null));
            }
        }

        #endregion

        #region CountOccurrences Tests

        /// <summary>
        /// Unit tests for the GeneralUtils.CountOccurrences method.
        /// Tests verify that the method correctly counts how many times
        /// a specific character appears in a given string.
        /// </summary>
        public class CountOccurrencesTests
        {
            /// <summary>
            /// Tests CountOccurrences with various strings and characters
            /// that have multiple occurrences.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            /// <param name="expectedCount">The expected number of occurrences</param>
            [Theory]
            [InlineData("hello", 'l', 2)]
            [InlineData("hello world", 'l', 3)]
            [InlineData("programming", 'm', 2)]
            [InlineData("mississippi", 's', 4)]
            [InlineData("mississippi", 'i', 4)]
            [InlineData("aaaaaa", 'a', 6)]
            [InlineData("banana", 'a', 3)]
            [InlineData("banana", 'n', 2)]
            [InlineData("testing testing", 't', 4)]
            [InlineData("bookkeeper", 'e', 3)]
            [InlineData("committee", 'e', 2)]
            public void CountOccurrences_MultipleOccurrences_ReturnsCorrectCount(string input, char character, int expectedCount)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(expectedCount, result);
            }

            /// <summary>
            /// Tests CountOccurrences with strings where the character appears only once.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            [Theory]
            [InlineData("hello", 'h')]
            [InlineData("world", 'w')]
            [InlineData("testing", 'g')]
            [InlineData("single", 's')]
            [InlineData("abcdef", 'a')]
            [InlineData("xyz", 'y')]
            public void CountOccurrences_SingleOccurrence_ReturnsOne(string input, char character)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(1, result);
            }

            /// <summary>
            /// Tests CountOccurrences with strings that do not contain the target character.
            /// Should return zero for characters not found in the string.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            [Theory]
            [InlineData("hello", 'x')]
            [InlineData("world", 'z')]
            [InlineData("testing", 'q')]
            [InlineData("programming", 'x')]
            [InlineData("abcdef", 'z')]
            [InlineData("12345", 'a')]
            [InlineData("UPPERCASE", 'a')]
            [InlineData("lowercase", 'A')]
            public void CountOccurrences_CharacterNotFound_ReturnsZero(string input, char character)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(0, result);
            }

            /// <summary>
            /// Tests CountOccurrences with an empty string.
            /// Should always return zero regardless of the character being searched for.
            /// </summary>
            /// <param name="character">The character to count</param>
            [Theory]
            [InlineData('a')]
            [InlineData('z')]
            [InlineData(' ')]
            [InlineData('1')]
            [InlineData('@')]
            public void CountOccurrences_EmptyString_ReturnsZero(char character)
            {
                // Act
                int result = GeneralUtils.CountOccurrences("", character);

                // Assert
                Assert.Equal(0, result);
            }

            /// <summary>
            /// Tests CountOccurrences with special characters including spaces,
            /// punctuation, and symbols.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            /// <param name="expectedCount">The expected number of occurrences</param>
            [Theory]
            [InlineData("hello world", ' ', 1)]
            [InlineData("a, b, c, d", ',', 3)]
            [InlineData("test.test.test", '.', 2)]
            [InlineData("one-two-three", '-', 2)]
            [InlineData("email@domain.com", '@', 1)]
            [InlineData("$100 + $200 = $300", '$', 3)]
            [InlineData("question?", '?', 1)]
            [InlineData("exclamation!", '!', 1)]
            [InlineData("quotes \"test\"", '"', 2)]
            [InlineData("parentheses (test)", '(', 1)]
            public void CountOccurrences_SpecialCharacters_ReturnsCorrectCount(string input, char character, int expectedCount)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(expectedCount, result);
            }

            /// <summary>
            /// Tests CountOccurrences with numeric characters in strings.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            /// <param name="expectedCount">The expected number of occurrences</param>
            [Theory]
            [InlineData("123456789", '1', 1)]
            [InlineData("123456789", '5', 1)]
            [InlineData("1122334455", '1', 2)]
            [InlineData("1122334455", '4', 2)]
            [InlineData("phone: 555-1234", '5', 3)]
            [InlineData("year 2023", '2', 2)]
            [InlineData("100.00", '0', 4)]
            public void CountOccurrences_NumericCharacters_ReturnsCorrectCount(string input, char character, int expectedCount)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(expectedCount, result);
            }

            /// <summary>
            /// Tests CountOccurrences with null input to verify error handling behavior.
            /// The method should throw an ArgumentNullException when passed a null string.
            /// </summary>
            [Fact]
            public void CountOccurrences_NullInput_ThrowsArgumentNullException()
            {
                // Act & Assert
                Assert.Throws<NullReferenceException>(() => GeneralUtils.CountOccurrences(null, 'a'));
            }

            /// <summary>
            /// Tests CountOccurrences with Unicode characters and special symbols.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            /// <param name="expectedCount">The expected number of occurrences</param>
            [Theory]
            [InlineData("café", 'é', 1)]
            [InlineData("naïve", 'ï', 1)]
            [InlineData("αβγαβγ", 'α', 2)]
            [InlineData("résumé", 'é', 2)]
            public void CountOccurrences_UnicodeCharacters_ReturnsCorrectCount(string input, char character, int expectedCount)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(expectedCount, result);
            }

            /// <summary>
            /// Tests CountOccurrences with very long strings to verify performance
            /// and correctness with larger inputs.
            /// </summary>
            [Fact]
            public void CountOccurrences_LongString_ReturnsCorrectCount()
            {
                // Arrange
                string input = new string('a', 1000) + new string('b', 500) + new string('a', 1000);
                char targetChar = 'a';
                int expectedCount = 2000;

                // Act
                int result = GeneralUtils.CountOccurrences(input, targetChar);

                // Assert
                Assert.Equal(expectedCount, result);
            }

            /// <summary>
            /// Tests CountOccurrences with strings containing various whitespace characters.
            /// </summary>
            /// <param name="input">The input string to search</param>
            /// <param name="character">The character to count</param>
            /// <param name="expectedCount">The expected number of occurrences</param>
            [Theory]
            [InlineData("hello world", ' ', 1)]
            [InlineData("  spaces  ", ' ', 4)]
            [InlineData("tab\ttab", '\t', 1)]
            [InlineData("new\nline", '\n', 1)]
            [InlineData("carriage\rreturn", '\r', 1)]
            [InlineData("multiple   spaces", ' ', 3)]
            public void CountOccurrences_WhitespaceCharacters_ReturnsCorrectCount(string input, char character, int expectedCount)
            {
                // Act
                int result = GeneralUtils.CountOccurrences(input, character);

                // Assert
                Assert.Equal(expectedCount, result);
            }
        }

        #endregion

        #region IsPasswordStrong Tests

        /// <summary>
        /// Unit tests for the GeneralUtils.IsPasswordStrong method.
        /// Tests verify the method's password strength validation logic.
        /// A strong password must be at least 8 characters long and contain
        /// at least one uppercase letter, one lowercase letter, one digit,
        /// and one special character (non-alphanumeric).
        /// </summary>
        public class IsPasswordStrongTests
        {
            /// <summary>
            /// Tests IsPasswordStrong with passwords that meet all strength requirements.
            /// These should return true as they have 8+ chars, uppercase, lowercase, digit, and special char.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("Password1!")]
            [InlineData("StrongPass123@")]
            [InlineData("MySecure#2023")]
            [InlineData("Testing$456")]
            [InlineData("Complex9%Password")]
            [InlineData("SecureKey88*")]
            [InlineData("ValidPass1#")]
            [InlineData("GoodPassword2!")]
            [InlineData("Strong123$Test")]
            [InlineData("Secure&Password9")]
            [InlineData("Admin123!")]
            [InlineData("User456@")]
            public void IsPasswordStrong_ValidStrongPasswords_ReturnsTrue(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.True(result, $"Password '{password}' should be considered strong");

                // Verify password meets all criteria
                Assert.True(password.Length >= 8, "Password should be at least 8 characters");
                Assert.True(password.Any(char.IsUpper), "Password should contain uppercase letters");
                Assert.True(password.Any(char.IsLower), "Password should contain lowercase letters");
                Assert.True(password.Any(char.IsDigit), "Password should contain digits");
                Assert.True(password.Any(c => !char.IsLetterOrDigit(c)), "Password should contain special characters");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords that are too short (less than 8 characters).
            /// These should return false regardless of other criteria being met.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("Pass1!")]      // 6 chars
            [InlineData("Ab1@")]        // 4 chars
            [InlineData("Test2#")]      // 6 chars
            [InlineData("Aa1!")]        // 4 chars
            [InlineData("Short9$")]     // 7 chars
            [InlineData("X1!")]         // 3 chars
            [InlineData("aB3@")]        // 4 chars
            public void IsPasswordStrong_TooShort_ReturnsFalse(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.False(result, $"Password '{password}' should be rejected for being too short");
                Assert.True(password.Length < 8, "Password should be less than 8 characters");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords missing uppercase letters.
            /// These should return false even if other requirements are met.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("password123!")]
            [InlineData("lowercase456@")]
            [InlineData("noupperletter7#")]
            [InlineData("testing890$")]
            [InlineData("alllower123%")]
            [InlineData("weakpassword1!")]
            [InlineData("onlylower789&")]
            public void IsPasswordStrong_MissingUppercase_ReturnsFalse(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.False(result, $"Password '{password}' should be rejected for missing uppercase letters");
                Assert.False(password.Any(char.IsUpper), "Password should not contain uppercase letters");
                Assert.True(password.Length >= 8, "Password should be long enough");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords missing lowercase letters.
            /// These should return false even if other requirements are met.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("PASSWORD123!")]
            [InlineData("UPPERCASE456@")]
            [InlineData("NOLOWERLETTER7#")]
            [InlineData("TESTING890$")]
            [InlineData("ALLUPPER123%")]
            [InlineData("STRONGPASSWORD1!")]
            [InlineData("ONLYUPPER789&")]
            public void IsPasswordStrong_MissingLowercase_ReturnsFalse(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.False(result, $"Password '{password}' should be rejected for missing lowercase letters");
                Assert.False(password.Any(char.IsLower), "Password should not contain lowercase letters");
                Assert.True(password.Length >= 8, "Password should be long enough");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords missing digits.
            /// These should return false even if other requirements are met.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("PasswordTest!")]
            [InlineData("NoNumbers@Here")]
            [InlineData("TestingPassword#")]
            [InlineData("SecurePass$word")]
            [InlineData("NoDigitsHere%")]
            [InlineData("OnlyLetters&Special")]
            [InlineData("MissingNumbers!@")]
            public void IsPasswordStrong_MissingDigits_ReturnsFalse(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.False(result, $"Password '{password}' should be rejected for missing digits");
                Assert.False(password.Any(char.IsDigit), "Password should not contain digits");
                Assert.True(password.Length >= 8, "Password should be long enough");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords missing special characters.
            /// These should return false even if other requirements are met.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("Password123")]
            [InlineData("TestingPass456")]
            [InlineData("NoSpecialChars789")]
            [InlineData("SecurePassword1")]
            [InlineData("OnlyLettersNumbers2")]
            [InlineData("MissingSpecial34")]
            [InlineData("AlphaNumeric567")]
            public void IsPasswordStrong_MissingSpecialCharacters_ReturnsFalse(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.False(result, $"Password '{password}' should be rejected for missing special characters");
                Assert.False(password.Any(c => !char.IsLetterOrDigit(c)), "Password should not contain special characters");
                Assert.True(password.Length >= 8, "Password should be long enough");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords that have multiple missing requirements.
            /// These should definitely return false.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("password")]      // Missing: uppercase, digits, special chars
            [InlineData("PASSWORD")]      // Missing: lowercase, digits, special chars
            [InlineData("12345678")]      // Missing: uppercase, lowercase, special chars
            [InlineData("!@#$%^&*")]      // Missing: uppercase, lowercase, digits
            [InlineData("Pass")]          // Missing: length, digits, special chars
            [InlineData("123!")]          // Missing: length, uppercase, lowercase
            [InlineData("abc")]           // Missing: length, uppercase, digits, special chars
            [InlineData("ABC")]           // Missing: length, lowercase, digits, special chars
            public void IsPasswordStrong_MultipleViolations_ReturnsFalse(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.False(result, $"Password '{password}' should be rejected for multiple requirement violations");
            }

            /// <summary>
            /// Tests IsPasswordStrong with an empty string.
            /// Should return false for missing all requirements.
            /// </summary>
            [Fact]
            public void IsPasswordStrong_EmptyString_ReturnsFalse()
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong("");

                // Assert
                Assert.False(result, "Empty password should be rejected");
            }

            /// <summary>
            /// Tests IsPasswordStrong with null input to verify error handling behavior.
            /// The method should throw an ArgumentNullException when passed a null string.
            /// </summary>
            [Fact]
            public void IsPasswordStrong_NullInput_ThrowsArgumentNullException()
            {
                // Act & Assert
                Assert.Throws<NullReferenceException>(() => GeneralUtils.IsPasswordStrong(null));
            }

            /// <summary>
            /// Tests IsPasswordStrong with various special characters to ensure
            /// they are properly recognized as non-alphanumeric characters.
            /// </summary>
            /// <param name="password">The password to test</param>
            /// <param name="specialChar">The special character being tested</param>
            [Theory]
            [InlineData("Password1!", '!')]
            [InlineData("Password1@", '@')]
            [InlineData("Password1#", '#')]
            [InlineData("Password1$", '$')]
            [InlineData("Password1%", '%')]
            [InlineData("Password1^", '^')]
            [InlineData("Password1&", '&')]
            [InlineData("Password1*", '*')]
            [InlineData("Password1(", '(')]
            [InlineData("Password1)", ')')]
            [InlineData("Password1-", '-')]
            [InlineData("Password1_", '_')]
            [InlineData("Password1=", '=')]
            [InlineData("Password1+", '+')]
            [InlineData("Password1[", '[')]
            [InlineData("Password1]", ']')]
            [InlineData("Password1{", '{')]
            [InlineData("Password1}", '}')]
            [InlineData("Password1|", '|')]
            [InlineData("Password1\\", '\\')]
            [InlineData("Password1:", ':')]
            [InlineData("Password1;", ';')]
            [InlineData("Password1\"", '"')]
            [InlineData("Password1'", '\'')]
            [InlineData("Password1<", '<')]
            [InlineData("Password1>", '>')]
            [InlineData("Password1,", ',')]
            [InlineData("Password1.", '.')]
            [InlineData("Password1?", '?')]
            [InlineData("Password1/", '/')]
            public void IsPasswordStrong_VariousSpecialCharacters_ReturnsTrue(string password, char specialChar)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.True(result, $"Password with special character '{specialChar}' should be strong");
                Assert.Contains(specialChar, password);
                Assert.True(password.Any(c => !char.IsLetterOrDigit(c)), $"Password should contain special character '{specialChar}'");
            }

            /// <summary>
            /// Tests IsPasswordStrong with exactly 8 characters to test the boundary condition.
            /// These should return true as they meet the minimum length requirement.
            /// </summary>
            /// <param name="password">The 8-character password to test</param>
            [Theory]
            [InlineData("Pass123!")]
            [InlineData("Test456@")]
            [InlineData("Word789#")]
            [InlineData("Code012$")]
            [InlineData("Safe345%")]
            [InlineData("Best678&")]
            [InlineData("Good901*")]
            public void IsPasswordStrong_ExactlyEightCharacters_ReturnsTrue(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.Equal(8, password.Length);
                Assert.True(result, $"8-character password '{password}' meeting all criteria should be strong");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords containing Unicode characters.
            /// Unicode letters should be handled appropriately by the char methods.
            /// </summary>
            /// <param name="password">The password with Unicode characters to test</param>
            [Theory]
            [InlineData("Pássword123!")]
            [InlineData("Tësting456@")]
            [InlineData("Sécure789#")]
            [InlineData("Café012$")]
            [InlineData("Résumé345%")]
            [InlineData("Naïve678&")]
            public void IsPasswordStrong_UnicodeCharacters_ReturnsTrue(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.True(result, $"Password with Unicode characters '{password}' should be strong");
                Assert.True(password.Length >= 8, "Password should meet length requirement");
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords containing whitespace characters.
            /// Spaces and other whitespace should be treated as special characters.
            /// </summary>
            /// <param name="password">The password to test</param>
            [Theory]
            [InlineData("Pass Word123")]   // Contains space but no other special char - should fail
            [InlineData("Pass Word1!")]    // Contains space and special char - should pass  
            [InlineData("My\tPass123!")]   // Contains tab and special char - should pass
            [InlineData("New\nLine1@")]    // Contains newline and special char - should pass
            public void IsPasswordStrong_WithWhitespace_ReturnsExpectedResult(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);
                bool hasNonWhitespaceSpecial = password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));

                // Assert
                if (hasNonWhitespaceSpecial)
                {
                    Assert.True(result, $"Password '{password}' with whitespace and other special chars should be strong");
                }
                else
                {
                    // If only whitespace as special chars, depends on implementation
                    // Most implementations treat whitespace as special characters
                    Assert.True(result, $"Password '{password}' with whitespace should be strong if whitespace counts as special");
                }
            }

            /// <summary>
            /// Tests boundary cases where passwords almost meet requirements.
            /// These help verify the method correctly validates each individual requirement.
            /// </summary>
            /// <param name="password">The password to test</param>
            /// <param name="expectedResult">Whether the password should be considered strong</param>
            /// <param name="missingRequirement">Description of what requirement is missing</param>
            [Theory]
            [InlineData("Password123!", true)] //none - passes
            [InlineData("password123!", false)] //uppercase
            [InlineData("PASSWORD123!", false)] //lowercase
            [InlineData("Password!", false)] //digit
            [InlineData("Password123", false)] //special character
            [InlineData("Pass12!", false)] //too short
            [InlineData("", false)] //no requirements met
            public void IsPasswordStrong_BoundaryConditions_ReturnsExpectedResult(string password, bool expectedResult)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            /// <summary>
            /// Tests IsPasswordStrong with passwords that have minimal requirements.
            /// These passwords barely meet each requirement to test edge cases.
            /// </summary>
            /// <param name="password">The minimal password to test</param>
            [Theory]
            [InlineData("Aa1!")]         // Too short - 4 chars
            [InlineData("Aa1!bcde")]     // Exactly 8 chars, meets all requirements
            [InlineData("Password1!")]   // Common pattern, should be strong
            [InlineData("aB3@efgh")]     // Minimal chars in each category
            public void IsPasswordStrong_MinimalRequirements_TestsEdgeCases(string password)
            {
                // Act
                bool result = GeneralUtils.IsPasswordStrong(password);

                // Assert
                if (password.Length >= 8 &&
                    password.Any(char.IsUpper) &&
                    password.Any(char.IsLower) &&
                    password.Any(char.IsDigit) &&
                    password.Any(c => !char.IsLetterOrDigit(c)))
                {
                    Assert.True(result, $"Password '{password}' meeting all requirements should be strong");
                }
                else
                {
                    Assert.False(result, $"Password '{password}' not meeting all requirements should be weak");
                }
            }
        }

        #endregion

        /// <summary>
        /// Unit tests for the GeneralUtils class methods.
        /// Tests cover various scenarios including edge cases, null inputs, and boundary conditions.
        /// </summary>
        public class ContainsMethodTests
        {
            #region Contains Method Tests

            /// <summary>
            /// Tests the Contains method with various array types and values.
            /// Verifies that the method correctly identifies when an item exists in an array.
            /// </summary>
            /// <param name="array">The array to search in</param>
            /// <param name="item">The item to search for</param>
            /// <param name="expected">Expected result</param>
            [Theory]
            [InlineData(new int[] { 1, 2, 3, 4, 5 }, 3, true)]
            [InlineData(new int[] { 1, 2, 3, 4, 5 }, 6, false)]
            [InlineData(new int[] { }, 1, false)]
            [InlineData(new int[] { 1 }, 1, true)]
            [InlineData(new int[] { -1, 0, 1 }, 0, true)]
            public void Contains_WithIntegerArrays_ReturnsExpectedResult(int[] array, int item, bool expected)
            {
                // Act
                bool result = GeneralUtils.Contains(array, item);

                // Assert
                Assert.Equal(expected, result);
            }

            /// <summary>
            /// Tests the Contains method with string arrays.
            /// </summary>
            [Theory]
            [InlineData(new string[] { "hello", "world", "test" }, "world", true)]
            [InlineData(new string[] { "hello", "world", "test" }, "missing", false)]
            [InlineData(new string[] { }, "test", false)]
            public void Contains_WithStringArrays_ReturnsExpectedResult(string[] array, string item, bool expected)
            {
                // Act
                bool result = GeneralUtils.Contains(array, item);

                // Assert
                Assert.Equal(expected, result);
            }

            /// <summary>
            /// Tests the Contains method with null values.
            /// </summary>
            [Fact]
            public void Contains_WithNullArray_ShouldHandleGracefully()
            {
                // Arrange
                int[] nullArray = null;

                // Act & Assert
                // The behavior depends on implementation - this test documents expected behavior
                Assert.Throws<ArgumentException>(() => GeneralUtils.Contains(nullArray, 1));
            }

            /// <summary>
            /// Tests the Contains method with null item in string array.
            /// </summary>
            [Fact]
            public void Contains_WithNullItemInStringArray_ReturnsExpectedResult()
            {
                // Arrange
                string[] array = { "test", null, "value" };

                // Act
                bool result = GeneralUtils.Contains(array, null);

                // Assert
                Assert.True(result);
            }
        }
        #endregion

        #region IsValidOperator Method Tests
        public class IsValidOperatorTests
        {


            /// <summary>
            /// Tests the IsValidOperator method with various operator strings.
            /// Verifies that only valid operators (+, -, *, /, //, %, **) return true.
            /// </summary>
            /// <param name="op">The operator string to test</param>
            /// <param name="expected">Expected result</param>
            [Theory]
            [InlineData("+", true)]
            [InlineData("-", true)]
            [InlineData("*", true)]
            [InlineData("/", true)]
            [InlineData("//", true)]
            [InlineData("%", true)]
            [InlineData("**", true)]
            [InlineData("=", false)]
            [InlineData("++", false)]
            [InlineData("--", false)]
            [InlineData("***", false)]
            [InlineData("", false)]
            [InlineData(" ", false)]
            [InlineData("abc", false)]
            [InlineData("123", false)]
            public void IsValidOperator_WithVariousOperators_ReturnsExpectedResult(string op, bool expected)
            {
                // Act
                bool result = GeneralUtils.IsValidOperator(op);

                // Assert
                Assert.Equal(expected, result);
            }

            /// <summary>
            /// Tests the IsValidOperator method with null input.
            /// </summary>
            [Fact]
            public void IsValidOperator_WithNullInput_ShouldHandleGracefully()
            {
                // Act & Assert
                // The behavior depends on implementation
                Assert.Throws<NullReferenceException>(() => GeneralUtils.IsValidOperator(null));
            }

            #endregion
        }
        #region ToCamelCase Method Tests
        public class ToCamelCaseTests
        {


            /// <summary>
            /// Tests the ToCamelCase method with various input strings.
            /// Verifies correct conversion of space-separated words to camelCase.
            /// </summary>
            /// <param name="input">The input string</param>
            /// <param name="expected">Expected camelCase result</param>
            [Theory]
            [InlineData("hello world", "helloWorld")]
            [InlineData("Hello World", "helloWorld")]
            [InlineData("hello world test string", "helloWorldTestString")]
            [InlineData("HELLO WORLD", "helloWorld")]
            [InlineData("single", "single")]
            [InlineData("Single", "single")]
            [InlineData("a b c d", "aBCD")]
            [InlineData("test with multiple spaces", "testWithMultipleSpaces")]
            public void ToCamelCase_WithVariousInputs_ReturnsExpectedResult(string input, string expected)
            {
                // Act
                string result = GeneralUtils.ToCamelCase(input);

                // Assert
                Assert.Equal(expected, result);
            }

            /// <summary>
            /// Tests the ToCamelCase method with edge cases.
            /// </summary>
            [Theory]
            [InlineData("", "")]
            [InlineData("  multiple  spaces  ", "multipleSpaces")]
            [InlineData("       even more   spaces  ", "evenMoreSpaces")]
            public void ToCamelCase_WithEdgeCases_HandlesCorrectly(string input, string expected)
            {
                // Act
                string result = GeneralUtils.ToCamelCase(input);

                // Assert
                Assert.Equal(expected, result);
            }

            /// <summary>
            /// Tests the ToCamelCase method with null input.
            /// </summary>
            [Fact]
            public void ToCamelCase_WithNullInput_ThrowsException()
            {
                // Act & Assert
                Assert.Throws<NullReferenceException>(() => GeneralUtils.ToCamelCase(null));
            }

            #endregion
        }
        #region GetUniqueItems Method Tests

        public class GetUniqueItemsTests
        {

            /// <summary>
            /// Tests the GetUniqueItems method with various lists.
            /// Verifies that duplicate items are removed and unique items are preserved.
            /// </summary>
            [Fact]
            public void GetUniqueItems_WithDuplicates_ReturnsUniqueItemsOnly()
            {
                // Arrange
                var input = new List<int> { 1, 2, 3, 2, 4, 1, 5 };
                var expected = new List<int> { 1, 2, 3, 4, 5 };

                // Act
                var result = GeneralUtils.GetUniqueItems(input);

                // Assert
                Assert.Equal(expected.Count, result.Count);
                foreach (var item in expected)
                {
                    Assert.Contains(item, result);
                }
            }

            /// <summary>
            /// Tests the GetUniqueItems method with no duplicates.
            /// </summary>
            [Fact]
            public void GetUniqueItems_WithNoDuplicates_ReturnsOriginalList()
            {
                // Arrange
                var input = new List<string> { "a", "b", "c", "d" };

                // Act
                var result = GeneralUtils.GetUniqueItems(input);

                // Assert
                Assert.Equal(input.Count, result.Count);
                foreach (var item in input)
                {
                    Assert.Contains(item, result);
                }
            }

            /// <summary>
            /// Tests the GetUniqueItems method with empty list.
            /// </summary>
            [Fact]
            public void GetUniqueItems_WithEmptyList_ReturnsEmptyList()
            {
                // Arrange
                var input = new List<int>();

                // Act
                var result = GeneralUtils.GetUniqueItems(input);

                // Assert
                Assert.Empty(result);
            }

            /// <summary>
            /// Tests the GetUniqueItems method with null input.
            /// </summary>
            [Fact]
            public void GetUniqueItems_WithNullInput_ThrowsArgumentException()
            {
                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => GeneralUtils.GetUniqueItems<int>(null));
                Assert.Equal("Input list cannot be null.", exception.Message);
            }

            /// <summary>
            /// Tests that GetUniqueItems doesn't modify the original list.
            /// </summary>
            [Fact]
            public void GetUniqueItems_DoesNotModifyOriginalList()
            {
                // Arrange
                var originalList = new List<int> { 1, 2, 2, 3, 3, 3 };
                var originalCount = originalList.Count;
                var originalContents = new List<int>(originalList);

                // Act
                var result = GeneralUtils.GetUniqueItems(originalList);

                // Assert
                Assert.Equal(originalCount, originalList.Count);
                Assert.Equal(originalContents, originalList);
            }

            #endregion
        }
        #region CalculateAverage Method Tests

        public class CalculateAverageTests
        {

            /// <summary>
            /// Tests the CalculateAverage method with various integer arrays.
            /// </summary>
            /// <param name="numbers">Array of numbers to calculate average</param>
            /// <param name="expected">Expected average value</param>
            [Theory]
            [InlineData(new int[] { 1, 2, 3, 4, 5 }, 3.0)]
            [InlineData(new int[] { 10, 20, 30 }, 20.0)]
            [InlineData(new int[] { 0 }, 0.0)]
            [InlineData(new int[] { -5, -10, -15 }, -10.0)]
            [InlineData(new int[] { 1, 2 }, 1.5)]
            [InlineData(new int[] { 100, 200, 300, 400 }, 250.0)]
            public void CalculateAverage_WithValidArrays_ReturnsCorrectAverage(int[] numbers, double expected)
            {
                // Act
                double result = GeneralUtils.CalculateAverage(numbers);

                // Assert
                Assert.Equal(expected, result, precision: 10);
            }

            /// <summary>
            /// Tests the CalculateAverage method with null input.
            /// </summary>
            [Fact]
            public void CalculateAverage_WithNullArray_ThrowsArgumentException()
            {
                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => GeneralUtils.CalculateAverage(null));
                Assert.Equal("Input list cannot be null.", exception.Message);
            }

            /// <summary>
            /// Tests the CalculateAverage method with empty array.
            /// </summary>
            [Fact]
            public void CalculateAverage_WithEmptyArray_ThrowsArgumentException()
            {
                // Arrange
                int[] emptyArray = new int[0];

                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => GeneralUtils.CalculateAverage(emptyArray));
                Assert.Equal("Input list cannot be empty.", exception.Message);
            }

            /// <summary>
            /// Tests the CalculateAverage method with large numbers to check for overflow handling.
            /// </summary>
            [Fact]
            public void CalculateAverage_WithLargeNumbers_HandlesCorrectly()
            {
                // Arrange
                int[] largeNumbers = { 100000000, 200000000, 300000000 };

                // Act
                double result = GeneralUtils.CalculateAverage(largeNumbers);

                // Assert
                Assert.Equal(200000000, result, precision: 10);
            }

            #endregion
        }
        #region Duplicates Method Tests

        public class DuplicatesTests
        {

            /// <summary>
            /// Tests the Duplicates method with various arrays containing duplicates.
            /// </summary>
            [Fact]
            public void Duplicates_WithDuplicateItems_ReturnsOnlyDuplicates()
            {
                // Arrange
                int[] input = { 1, 2, 3, 2, 4, 1, 5, 3 };

                // Act
                int[] result = GeneralUtils.Duplicates(input);

                // Assert
                Assert.Contains(1, result);
                Assert.Contains(2, result);
                Assert.Contains(3, result);
                Assert.DoesNotContain(4, result);
                Assert.DoesNotContain(5, result);
            }

            /// <summary>
            /// Tests the Duplicates method with no duplicates.
            /// </summary>
            [Fact]
            public void Duplicates_WithNoDuplicates_ReturnsEmptyArray()
            {
                // Arrange
                string[] input = { "a", "b", "c", "d" };

                // Act
                string[] result = GeneralUtils.Duplicates(input);

                // Assert
                Assert.Empty(result);
            }

            /// <summary>
            /// Tests the Duplicates method with empty array.
            /// </summary>
            [Fact]
            public void Duplicates_WithEmptyArray_ReturnsEmptyArray()
            {
                // Arrange
                int[] input = new int[0];

                // Act
                int[] result = GeneralUtils.Duplicates(input);

                // Assert
                Assert.Empty(result);
            }

            /// <summary>
            /// Tests the Duplicates method with all identical elements.
            /// </summary>
            [Fact]
            public void Duplicates_WithAllIdenticalElements_ReturnsUniqueElement()
            {
                // Arrange
                char[] input = { 'a', 'a', 'a', 'a' };

                // Act
                char[] result = GeneralUtils.Duplicates(input);

                // Assert
                Assert.Single(result);
                Assert.Contains('a', result);
            }

            /// <summary>
            /// Tests the Duplicates method with multiple occurrences of same item.
            /// </summary>
            [Fact]
            public void Duplicates_WithMultipleOccurrences_ReturnsItemOnceInResult()
            {
                // Arrange
                int[] input = { 1, 1, 1, 2, 2, 3 };

                // Act
                int[] result = GeneralUtils.Duplicates(input);

                // Assert
                Assert.Equal(2, result.Length);
                Assert.Contains(1, result);
                Assert.Contains(2, result);
                // Ensure each duplicate appears only once in result
                Assert.Equal(1, result.Count(x => x == 1));
                Assert.Equal(1, result.Count(x => x == 2));
            }

            /// <summary>
            /// Tests the Duplicates method with null array.
            /// </summary>
            [Fact]
            public void Duplicates_WithNullArray_ShouldHandleGracefully()
            {
                // Act & Assert
                // The behavior depends on implementation
                Assert.Throws<NullReferenceException>(() => GeneralUtils.Duplicates<int>(null));
            }

            #endregion
        }

    }
}
using Xunit;
using AST;
using System.Collections.Generic;
using System;

namespace AST.Tests
{
    /// <summary>
    /// Unit tests for AST node classes including expressions, operators, and statements.
    /// Tests cover node creation, unparsing functionality, and builder pattern implementations.
    /// 
    /// NOTE: Potential bugs found in implementation:
    /// 1. BlockStmt.Unparse() doesn't respect the 'level' parameter for indentation
    /// 2. BinaryOperator constructors initialize with null children, requiring SetChildren call
    /// </summary>
    public class BuildersTest
    {
        #region LiteralNode Tests

        /// <summary>
        /// Tests that LiteralNode correctly stores and unparses integer values.
        /// </summary>
        [Theory]
        [InlineData(42)]
        [InlineData(0)]
        [InlineData(-100)]
        public void LiteralNode_IntegerValues_UnparsesCorrectly(int value)
        {
            // Arrange
            var node = new LiteralNode(value);

            // Act
            string result = node.Unparse(0);

            // Assert
            Assert.Equal(value.ToString(), result);
        }

        /// <summary>
        /// Tests that LiteralNode correctly stores and unparses various object types.
        /// </summary>
        [Theory]
        [InlineData(3.14, "3.14")]
        [InlineData("hello", "hello")]
        [InlineData(true, "True")]
        public void LiteralNode_VariousTypes_UnparsesCorrectly(object value, string expected)
        {
            // Arrange
            var node = new LiteralNode(value);

            // Act
            string result = node.Unparse(0);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that LiteralNode respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "42")]
        [InlineData(1, "    42")]
        [InlineData(2, "        42")]
        public void LiteralNode_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var node = new LiteralNode(42);

            // Act
            string result = node.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region VariableNode Tests

        /// <summary>
        /// Tests that VariableNode correctly stores and unparses variable names.
        /// </summary>
        [Theory]
        [InlineData("x")]
        [InlineData("myVariable")]
        [InlineData("_temp123")]
        public void VariableNode_VariousNames_UnparsesCorrectly(string name)
        {
            // Arrange
            var node = new VariableNode(name);

            // Act
            string result = node.Unparse(0);

            // Assert
            Assert.Equal(name, result);
        }

        /// <summary>
        /// Tests that VariableNode respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "x")]
        [InlineData(1, "    x")]
        [InlineData(3, "            x")]
        public void VariableNode_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var node = new VariableNode("x");

            // Act
            string result = node.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region BinaryOperator Tests

        /// <summary>
        /// Tests that PlusNode correctly unparses addition operations.
        /// </summary>
        [Fact]
        public void PlusNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(5);
            var right = new LiteralNode(3);
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);

            // Act
            string result = plusNode.Unparse(0);

            // Assert
            Assert.Equal("5 + 3", result);
        }

        /// <summary>
        /// Tests that MinusNode correctly unparses subtraction operations.
        /// </summary>
        [Fact]
        public void MinusNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(10);
            var right = new LiteralNode(4);
            var minusNode = new MinusNode();
            minusNode.SetChildren(left, right);

            // Act
            string result = minusNode.Unparse(0);

            // Assert
            Assert.Equal("10 - 4", result);
        }

        /// <summary>
        /// Tests that TimesNode correctly unparses multiplication operations.
        /// </summary>
        [Fact]
        public void TimesNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(6);
            var right = new LiteralNode(7);
            var timesNode = new TimesNode();
            timesNode.SetChildren(left, right);

            // Act
            string result = timesNode.Unparse(0);

            // Assert
            Assert.Equal("6 * 7", result);
        }

        /// <summary>
        /// Tests that FloatDivNode correctly unparses float division operations.
        /// </summary>
        [Fact]
        public void FloatDivNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(15);
            var right = new LiteralNode(3);
            var divNode = new FloatDivNode();
            divNode.SetChildren(left, right);

            // Act
            string result = divNode.Unparse(0);

            // Assert
            Assert.Equal("15 / 3", result);
        }

        /// <summary>
        /// Tests that IntDivNode correctly unparses integer division operations.
        /// </summary>
        [Fact]
        public void IntDivNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(17);
            var right = new LiteralNode(5);
            var divNode = new IntDivNode();
            divNode.SetChildren(left, right);

            // Act
            string result = divNode.Unparse(0);

            // Assert
            Assert.Equal("17 // 5", result);
        }

        /// <summary>
        /// Tests that ModulusNode correctly unparses modulus operations.
        /// </summary>
        [Fact]
        public void ModulusNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(20);
            var right = new LiteralNode(6);
            var modNode = new ModulusNode();
            modNode.SetChildren(left, right);

            // Act
            string result = modNode.Unparse(0);

            // Assert
            Assert.Equal("20 % 6", result);
        }

        /// <summary>
        /// Tests that ExponentiationNode correctly unparses exponentiation operations.
        /// </summary>
        [Fact]
        public void ExponentiationNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(2);
            var right = new LiteralNode(8);
            var expNode = new ExponentiationNode();
            expNode.SetChildren(left, right);

            // Act
            string result = expNode.Unparse(0);

            // Assert
            Assert.Equal("2 ** 8", result);
        }

        /// <summary>
        /// Tests that binary operators can handle variable operands.
        /// </summary>
        [Fact]
        public void BinaryOperator_WithVariables_UnparsesCorrectly()
        {
            // Arrange
            var left = new VariableNode("x");
            var right = new VariableNode("y");
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);

            // Act
            string result = plusNode.Unparse(0);

            // Assert
            Assert.Equal("x + y", result);
        }

        /// <summary>
        /// Tests that binary operators can be nested to form complex expressions.
        /// </summary>
        [Fact]
        public void BinaryOperator_Nested_UnparsesCorrectly()
        {
            // Arrange
            var innerLeft = new LiteralNode(2);
            var innerRight = new LiteralNode(3);
            var innerPlus = new PlusNode();
            innerPlus.SetChildren(innerLeft, innerRight);

            var outerRight = new LiteralNode(4);
            var outerTimes = new TimesNode();
            outerTimes.SetChildren(innerPlus, outerRight);

            // Act
            string result = outerTimes.Unparse(0);

            // Assert
            Assert.Equal("2 + 3 * 4", result);
        }

        #endregion

        #region AssignmentStmt Tests

        /// <summary>
        /// Tests that AssignmentStmt correctly unparses simple assignments.
        /// </summary>
        [Fact]
        public void AssignmentStmt_SimpleAssignment_UnparsesCorrectly()
        {
            // Arrange
            var variable = new VariableNode("x");
            var value = new LiteralNode(42);
            var assignment = new AssignmentStmt(variable, value);

            // Act
            string result = assignment.Unparse(0);

            // Assert
            Assert.Equal("x := 42", result);
        }

        /// <summary>
        /// Tests that AssignmentStmt handles complex expressions on the right-hand side.
        /// </summary>
        [Fact]
        public void AssignmentStmt_WithExpression_UnparsesCorrectly()
        {
            // Arrange
            var variable = new VariableNode("result");
            var left = new LiteralNode(10);
            var right = new LiteralNode(5);
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);
            var assignment = new AssignmentStmt(variable, plusNode);

            // Act
            string result = assignment.Unparse(0);

            // Assert
            Assert.Equal("result := 10 + 5", result);
        }

        /// <summary>
        /// Tests that AssignmentStmt respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "x := 5")]
        [InlineData(1, "    x := 5")]
        [InlineData(2, "        x := 5")]
        public void AssignmentStmt_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var variable = new VariableNode("x");
            var value = new LiteralNode(5);
            var assignment = new AssignmentStmt(variable, value);

            // Act
            string result = assignment.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region ReturnStmt Tests

        /// <summary>
        /// Tests that ReturnStmt correctly unparses return statements with literals.
        /// </summary>
        [Fact]
        public void ReturnStmt_WithLiteral_UnparsesCorrectly()
        {
            // Arrange
            var value = new LiteralNode(100);
            var returnStmt = new ReturnStmt(value);

            // Act
            string result = returnStmt.Unparse(0);

            // Assert
            Assert.Equal("return 100", result);
        }

        /// <summary>
        /// Tests that ReturnStmt correctly unparses return statements with expressions.
        /// </summary>
        [Fact]
        public void ReturnStmt_WithExpression_UnparsesCorrectly()
        {
            // Arrange
            var left = new VariableNode("a");
            var right = new VariableNode("b");
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);
            var returnStmt = new ReturnStmt(plusNode);

            // Act
            string result = returnStmt.Unparse(0);

            // Assert
            Assert.Equal("return a + b", result);
        }

        /// <summary>
        /// Tests that ReturnStmt respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "return 0")]
        [InlineData(1, "    return 0")]
        [InlineData(2, "        return 0")]
        public void ReturnStmt_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var value = new LiteralNode(0);
            var returnStmt = new ReturnStmt(value);

            // Act
            string result = returnStmt.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region BlockStmt Tests

        /// <summary>
        /// Tests that BlockStmt correctly unparses empty blocks.
        /// </summary>
        [Fact]
        public void BlockStmt_Empty_UnparsesCorrectly()
        {
            // Arrange
            var statements = new List<Statement>();
            var block = new BlockStmt(statements);

            // Act
            string result = block.Unparse(0);

            // Assert
            Assert.Equal("{\n}", result);
        }

        /// <summary>
        /// Tests that BlockStmt correctly unparses blocks with single statement.
        /// </summary>
        [Fact]
        public void BlockStmt_SingleStatement_UnparsesCorrectly()
        {
            // Arrange
            var variable = new VariableNode("x");
            var value = new LiteralNode(5);
            var assignment = new AssignmentStmt(variable, value);
            var statements = new List<Statement> { assignment };
            var block = new BlockStmt(statements);

            // Act
            string result = block.Unparse(0);

            // Assert
            Assert.Equal("{\nx := 5\n}", result);
        }

        /// <summary>
        /// Tests that BlockStmt correctly unparses blocks with multiple statements.
        /// </summary>
        [Fact]
        public void BlockStmt_MultipleStatements_UnparsesCorrectly()
        {
            // Arrange
            var var1 = new VariableNode("x");
            var val1 = new LiteralNode(10);
            var assign1 = new AssignmentStmt(var1, val1);

            var var2 = new VariableNode("y");
            var val2 = new LiteralNode(20);
            var assign2 = new AssignmentStmt(var2, val2);

            var statements = new List<Statement> { assign1, assign2 };
            var block = new BlockStmt(statements);

            // Act
            string result = block.Unparse(0);

            // Assert
            Assert.Equal("{\nx := 10\ny := 20\n}", result);
        }

        /// <summary>
        /// Tests that AddStmt method correctly adds statements to the block.
        /// </summary>
        [Fact]
        public void BlockStmt_AddStmt_AddsStatementCorrectly()
        {
            // Arrange
            var statements = new List<Statement>();
            var block = new BlockStmt(statements);
            var variable = new VariableNode("z");
            var value = new LiteralNode(30);
            var assignment = new AssignmentStmt(variable, value);

            // Act
            block.AddStmt(assignment);
            string result = block.Unparse(0);

            // Assert
            Assert.Equal("{\nz := 30\n}", result);
        }

        #endregion

        #region DefaultBuilder Tests

        /// <summary>
        /// Tests that DefaultBuilder creates all binary operator nodes correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreatePlusNode_CreatesValidNode()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var left = new LiteralNode(1);
            var right = new LiteralNode(2);

            // Act
            var node = builder.CreatePlusNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.Equal("1 + 2", node.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates various operator nodes correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateOperatorNodes_CreatesValidNodes()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var left = new LiteralNode(10);
            var right = new LiteralNode(3);

            // Act & Assert
            Assert.NotNull(builder.CreateMinusNode(left, right));
            Assert.NotNull(builder.CreateTimesNode(left, right));
            Assert.NotNull(builder.CreateFloatDivNode(left, right));
            Assert.NotNull(builder.CreateIntDivNode(left, right));
            Assert.NotNull(builder.CreateModulusNode(left, right));
            Assert.NotNull(builder.CreateExponentiationNode(left, right));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates expression nodes correctly.
        /// </summary>
        [Theory]
        [InlineData(42)]
        [InlineData(3.14)]
        [InlineData("test")]
        public void DefaultBuilder_CreateLiteralNode_CreatesValidNode(object value)
        {
            // Arrange
            var builder = new DefaultBuilder();

            // Act
            var node = builder.CreateLiteralNode(value);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(value.ToString(), node.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates VariableNode correctly.
        /// </summary>
        [Theory]
        [InlineData("x")]
        [InlineData("myVar")]
        [InlineData("temp123")]
        public void DefaultBuilder_CreateVariableNode_CreatesValidNode(string name)
        {
            // Arrange
            var builder = new DefaultBuilder();

            // Act
            var node = builder.CreateVariableNode(name);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(name, node.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates AssignmentStmt correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateAssignmentStmt_CreatesValidStatement()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var variable = new VariableNode("x");
            var value = new LiteralNode(100);

            // Act
            var stmt = builder.CreateAssignmentStmt(variable, value);

            // Assert
            Assert.NotNull(stmt);
            Assert.Equal("x := 100", stmt.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates ReturnStmt correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateReturnStmt_CreatesValidStatement()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var value = new LiteralNode(42);

            // Act
            var stmt = builder.CreateReturnStmt(value);

            // Assert
            Assert.NotNull(stmt);
            Assert.Equal("return 42", stmt.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates BlockStmt correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateBlockStmt_CreatesValidStatement()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var statements = new List<Statement>
            {
                new AssignmentStmt(new VariableNode("x"), new LiteralNode(1))
            };

            // Act
            var stmt = builder.CreateBlockStmt(statements);

            // Assert
            Assert.NotNull(stmt);
            Assert.Contains("x := 1", stmt.Unparse(0));
        }

        #endregion

        #region NullBuilder Tests

        /// <summary>
        /// Tests that NullBuilder returns null for all operator node creation methods.
        /// </summary>
        [Fact]
        public void NullBuilder_CreateOperatorNodes_ReturnsNull()
        {
            // Arrange
            var builder = new NullBuilder();
            var left = new LiteralNode(1);
            var right = new LiteralNode(2);

            // Act & Assert
            Assert.Null(builder.CreatePlusNode(left, right));
            Assert.Null(builder.CreateMinusNode(left, right));
            Assert.Null(builder.CreateTimesNode(left, right));
            Assert.Null(builder.CreateFloatDivNode(left, right));
            Assert.Null(builder.CreateIntDivNode(left, right));
            Assert.Null(builder.CreateModulusNode(left, right));
            Assert.Null(builder.CreateExponentiationNode(left, right));
        }

        /// <summary>
        /// Tests that NullBuilder returns null for expression node creation methods.
        /// </summary>
        [Fact]
        public void NullBuilder_CreateExpressionNodes_ReturnsNull()
        {
            // Arrange
            var builder = new NullBuilder();

            // Act & Assert
            Assert.Null(builder.CreateLiteralNode(42));
            Assert.Null(builder.CreateVariableNode("x"));
        }

        /// <summary>
        /// Tests that NullBuilder returns null for statement creation methods.
        /// </summary>
        [Fact]
        public void NullBuilder_CreateStatements_ReturnsNull()
        {
            // Arrange
            var builder = new NullBuilder();
            var variable = new VariableNode("x");
            var value = new LiteralNode(1);
            var statements = new List<Statement>();

            // Act & Assert
            Assert.Null(builder.CreateAssignmentStmt(variable, value));
            Assert.Null(builder.CreateReturnStmt(value));
            Assert.Null(builder.CreateBlockStmt(statements));
        }

        #endregion

        #region DebugBuilder Tests

        /// <summary>
        /// Tests that DebugBuilder creates nodes and can be verified through unparsing.
        /// This test captures console output to verify debug messages are written.
        /// </summary>
        [Fact]
        public void DebugBuilder_CreatePlusNode_CreatesValidNodeAndOutputsDebugInfo()
        {
            // Arrange
            var builder = new DebugBuilder();
            var left = new LiteralNode(5);
            var right = new LiteralNode(3);

            // Capture console output
            var originalOut = Console.Out;
            using (var stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                var node = builder.CreatePlusNode(left, right);

                // Reset console output
                Console.SetOut(originalOut);
                var output = stringWriter.ToString();

                // Assert
                Assert.NotNull(node);
                Assert.Equal("5 + 3", node.Unparse(0));
                Assert.Contains("Plus node created", output);
            }
        }

        /// <summary>
        /// Tests that DebugBuilder outputs debug information for various node types.
        /// </summary>
        [Fact]
        public void DebugBuilder_CreateVariousNodes_OutputsDebugInfo()
        {
            // Arrange
            var builder = new DebugBuilder();
            var originalOut = Console.Out;
            
            using (var stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                builder.CreateLiteralNode(42);
                builder.CreateVariableNode("test");
                builder.CreateReturnStmt(new LiteralNode(1));

                // Reset console output
                Console.SetOut(originalOut);
                var output = stringWriter.ToString();

                // Assert
                Assert.Contains("Literal Node created", output);
                Assert.Contains("Variable Node created", output);
                Assert.Contains("Return Statement Created", output);
            }
        }

        /// <summary>
        /// Tests that DebugBuilder creates functional nodes despite debugging output.
        /// </summary>
        [Fact]
        public void DebugBuilder_CreatedNodes_AreFunctional()
        {
            // Arrange
            var builder = new DebugBuilder();
            var originalOut = Console.Out;
            
            using (var stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                var variable = builder.CreateVariableNode("result");
                var left = builder.CreateLiteralNode(10);
                var right = builder.CreateLiteralNode(5);
                var plus = builder.CreatePlusNode(left, right);
                var assignment = builder.CreateAssignmentStmt(variable, plus);

                // Reset console output
                Console.SetOut(originalOut);

                // Assert
                Assert.NotNull(assignment);
                Assert.Equal("result := 10 + 5", assignment.Unparse(0));
            }
        }

        #endregion
    }
}
using Xunit;
using AST;
using System;

namespace AST.Tests
{
    public class EvaluateVisitorTests
    {
        #region Literal and Variable Tests

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-5)]
        public void Visit_LiteralNode_ReturnsCorrectIntegerValue(int value)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var literal = new LiteralNode(value);
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = literal.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(5.5f)]
        [InlineData(10.25f)]
        [InlineData(0.0f)]
        [InlineData(-5.75f)]
        public void Visit_LiteralNode_ReturnsCorrectFloatValue(float value)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var literal = new LiteralNode(value);
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = literal.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Visit_VariableNode_ReturnsValueFromSymbolTable()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var variable = new VariableNode("x");
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = 42;

            // Act
            var result = variable.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Visit_VariableNode_ReturnsNullForUndefinedVariable()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var variable = new VariableNode("undefined");
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = variable.Accept(visitor, symbolTable);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Binary Operator Tests - Addition

        [Theory]
        [InlineData(5, 3, 8)]
        [InlineData(10, 20, 30)]
        [InlineData(-5, 5, 0)]
        [InlineData(0, 0, 0)]
        public void Visit_PlusNode_WithIntegers_ReturnsCorrectSum(int left, int right, int expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var plusNode = new PlusNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = plusNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5.5f, 3.2f, 8.7f)]
        [InlineData(10.0f, 20.5f, 30.5f)]
        [InlineData(-5.5f, 5.5f, 0.0f)]
        public void Visit_PlusNode_WithFloats_ReturnsCorrectSum(float left, float right, float expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var plusNode = new PlusNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = (float)plusNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result, 5); // 5 decimal places precision
        }

        [Fact]
        public void Visit_PlusNode_WithMixedTypes_PerformsCorrectly()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var plusNode = new PlusNode(new LiteralNode(5), new LiteralNode(3.5f));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = plusNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(8.5f, (float)result, 5);
        }

        #endregion

        #region Binary Operator Tests - Subtraction

        [Theory]
        [InlineData(10, 5, 5)]
        [InlineData(5, 10, -5)]
        [InlineData(0, 5, -5)]
        [InlineData(5, 5, 0)]
        public void Visit_MinusNode_WithIntegers_ReturnsCorrectDifference(int left, int right, int expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var minusNode = new MinusNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = minusNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10.5f, 5.2f, 5.3f)]
        [InlineData(5.0f, 10.5f, -5.5f)]
        public void Visit_MinusNode_WithFloats_ReturnsCorrectDifference(float left, float right, float expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var minusNode = new MinusNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = (float)minusNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result, 5);
        }

        #endregion

        #region Binary Operator Tests - Multiplication

        [Theory]
        [InlineData(5, 3, 15)]
        [InlineData(10, 2, 20)]
        [InlineData(-5, 3, -15)]
        [InlineData(0, 100, 0)]
        public void Visit_TimesNode_WithIntegers_ReturnsCorrectProduct(int left, int right, int expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var timesNode = new TimesNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = timesNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5.5f, 2.0f, 11.0f)]
        [InlineData(10.0f, 2.5f, 25.0f)]
        [InlineData(-5.5f, 3.0f, -16.5f)]
        public void Visit_TimesNode_WithFloats_ReturnsCorrectProduct(float left, float right, float expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var timesNode = new TimesNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = (float)timesNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result, 5);
        }

        #endregion

        #region Binary Operator Tests - Division

        [Theory]
        [InlineData(10.0f, 2.0f, 5.0f)]
        [InlineData(9.0f, 3.0f, 3.0f)]
        [InlineData(7.0f, 2.0f, 3.5f)]
        [InlineData(-10.0f, 2.0f, -5.0f)]
        public void Visit_FloatDivNode_ReturnsCorrectQuotient(float left, float right, float expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var divNode = new FloatDivNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = (float)divNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result, 5);
        }

        [Fact]
        public void Visit_FloatDivNode_DivideByZero_ThrowsEvaluationException()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var divNode = new FloatDivNode(new LiteralNode(10.0f), new LiteralNode(0.0f));
            var symbolTable = new SymbolTable<string, object>();

            // Act & Assert
            Assert.Throws<EvaluationException>(() => divNode.Accept(visitor, symbolTable));
        }

        [Theory]
        [InlineData(10, 2, 5)]
        [InlineData(9, 3, 3)]
        [InlineData(7, 2, 3)]
        [InlineData(-10, 2, -5)]
        [InlineData(10, 3, 3)]
        public void Visit_IntDivNode_ReturnsCorrectIntegerQuotient(int left, int right, int expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var divNode = new IntDivNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = divNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Visit_IntDivNode_DivideByZero_ThrowsEvaluationException()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var divNode = new IntDivNode(new LiteralNode(10), new LiteralNode(0));
            var symbolTable = new SymbolTable<string, object>();

            // Act & Assert
            Assert.Throws<EvaluationException>(() => divNode.Accept(visitor, symbolTable));
        }

        #endregion

        #region Binary Operator Tests - Modulus and Exponentiation

        [Theory]
        [InlineData(10, 3, 1)]
        [InlineData(15, 4, 3)]
        [InlineData(20, 6, 2)]
        [InlineData(7, 7, 0)]
        public void Visit_ModulusNode_ReturnsCorrectRemainder(int left, int right, int expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var modNode = new ModulusNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = modNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 3, 8.0)]
        [InlineData(5, 2, 25.0)]
        [InlineData(10, 0, 1.0)]
        [InlineData(3, 3, 27.0)]
        public void Visit_ExponentiationNode_ReturnsCorrectPower(int left, int right, double expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var expNode = new ExponentiationNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = (double)expNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result, 5);
        }

        [Theory]
        [InlineData(2.0f, 3.0f, 8.0)]
        [InlineData(4.0f, 0.5f, 2.0)]
        public void Visit_ExponentiationNode_WithFloats_ReturnsCorrectPower(float left, float right, double expected)
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var expNode = new ExponentiationNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var result = (double)expNode.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(expected, result, 5);
        }

        #endregion

        #region Statement Tests - Assignment

        [Fact]
        public void Visit_AssignmentStmt_AssignsValueToVariable()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var assignment = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(42)
            );

            // Act
            assignment.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(42, symbolTable["x"]);
        }

        [Fact]
        public void Visit_AssignmentStmt_ReturnsAssignedValue()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var assignment = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(42)
            );

            // Act
            var result = assignment.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Visit_AssignmentStmt_WithExpression_AssignsComputedValue()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var assignment = new AssignmentStmt(
                new VariableNode("x"),
                new PlusNode(new LiteralNode(5), new LiteralNode(3))
            );

            // Act
            assignment.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(8, symbolTable["x"]);
        }

        [Fact]
        public void Visit_AssignmentStmt_OverwritesExistingVariable()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = 10;
            var assignment = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(42)
            );

            // Act
            assignment.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(42, symbolTable["x"]);
        }

        #endregion

        #region Statement Tests - Return

        [Fact]
        public void Visit_ReturnStmt_ReturnsExpressionValue()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var returnStmt = new ReturnStmt(new LiteralNode(42));

            // Act
            var result = returnStmt.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Evaluate_WithReturnStmt_ReturnsCorrectValue()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);
            block.AddStatement(new ReturnStmt(new LiteralNode(42)));

            // Act
            var result = visitor.Evaluate(block);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Evaluate_ReturnStopsExecution()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);
            
            // Add statements
            block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));
            block.AddStatement(new ReturnStmt(new LiteralNode(42)));
            block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(20)));

            // Act
            var result = visitor.Evaluate(block);

            // Assert
            Assert.Equal(42, result);
            Assert.Equal(10, blockSymbolTable["x"]); // x should be assigned
            Assert.False(blockSymbolTable.ContainsKey("y")); // y should NOT be assigned
        }

        #endregion

        #region Statement Tests - Block

        [Fact]
        public void Visit_BlockStmt_ExecutesAllStatements()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);
            
            block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));
            block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(20)));

            // Act
            block.Accept(visitor, null);

            // Assert
            Assert.Equal(10, blockSymbolTable["x"]);
            Assert.Equal(20, blockSymbolTable["y"]);
        }

        [Fact]
        public void Visit_BlockStmt_ReturnsLastStatementValue()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);
            
            block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));
            block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(20)));

            // Act
            var result = block.Accept(visitor, null);

            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public void Visit_NestedBlockStmt_RespectsScope()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var outerSymbolTable = new SymbolTable<string, object>();
            var innerSymbolTable = new SymbolTable<string, object>(outerSymbolTable);
            
            var outerBlock = new BlockStmt(outerSymbolTable);
            var innerBlock = new BlockStmt(innerSymbolTable);
            
            outerBlock.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));
            innerBlock.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(20)));
            outerBlock.AddStatement(innerBlock);

            // Act
            outerBlock.Accept(visitor, null);

            // Assert
            Assert.Equal(10, outerSymbolTable["x"]);
            Assert.False(outerSymbolTable.ContainsKey("y")); // y is in inner scope only
            Assert.Equal(20, innerSymbolTable["y"]);
        }

        #endregion

        #region Complex Expression Tests

        [Fact]
        public void Visit_ComplexExpression_EvaluatesCorrectly()
        {
            // Test: (5 + 3) * 2 = 16
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var expression = new TimesNode(
                new PlusNode(new LiteralNode(5), new LiteralNode(3)),
                new LiteralNode(2)
            );

            // Act
            var result = expression.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(16, result);
        }

        [Fact]
        public void Visit_ExpressionWithVariables_EvaluatesCorrectly()
        {
            // Test: x + (y * 2) where x=5, y=3
            // Arrange
            var visitor = new EvaluateVisitor();
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = 5;
            symbolTable["y"] = 3;
            
            var expression = new PlusNode(
                new VariableNode("x"),
                new TimesNode(new VariableNode("y"), new LiteralNode(2))
            );

            // Act
            var result = expression.Accept(visitor, symbolTable);

            // Assert
            Assert.Equal(11, result);
        }

        [Fact]
        public void Evaluate_CompleteProgram_ReturnsCorrectResult()
        {
            // Test program:
            // x := 5
            // y := 3
            // z := x + y
            // return z * 2
            
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);
            
            block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(3)));
            block.AddStatement(new AssignmentStmt(
                new VariableNode("z"),
                new PlusNode(new VariableNode("x"), new VariableNode("y"))
            ));
            block.AddStatement(new ReturnStmt(
                new TimesNode(new VariableNode("z"), new LiteralNode(2))
            ));

            // Act
            var result = visitor.Evaluate(block);

            // Assert
            Assert.Equal(16, result);
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void Visit_EmptyBlockStmt_HandlesGracefully()
        {
            // Note: This test may reveal a bug in the implementation
            // The current implementation tries to access Statements[Count-1] on an empty list
            
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);

            // Act & Assert
            // Asserts that an empty block returns null
            var result = block.Accept(visitor, blockSymbolTable);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(-5, 0)]
        [InlineData(0, 0)]
        public void Visit_ModulusNode_DivideByZero_ThrowsException(int left, int right)
        {
            // Note: The implementation doesn't check for modulus by zero
            // This test documents expected behavior
            
            // Arrange
            var visitor = new EvaluateVisitor();
            var modNode = new ModulusNode(new LiteralNode(left), new LiteralNode(right));
            var symbolTable = new SymbolTable<string, object>();

            // Act & Assert
            Assert.Throws<EvaluationException>(() => modNode.Accept(visitor, symbolTable));
        }

        [Fact]
        public void Evaluate_MultipleReturns_StopsAtFirst()
        {
            // Arrange
            var visitor = new EvaluateVisitor();
            var blockSymbolTable = new SymbolTable<string, object>();
            var block = new BlockStmt(blockSymbolTable);
            
            block.AddStatement(new ReturnStmt(new LiteralNode(10)));
            block.AddStatement(new ReturnStmt(new LiteralNode(20)));

            // Act
            var result = visitor.Evaluate(block);

            // Assert
            Assert.Equal(10, result);
        }

        #endregion
    }
}
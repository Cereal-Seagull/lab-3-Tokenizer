using Xunit;
using AST;
using Tokenizer;

namespace AST.Tests
{
    public class UnparseVisitorTests
    {
        private readonly UnparseVisitor _visitor;

        public UnparseVisitorTests()
        {
            _visitor = new UnparseVisitor();
        }

        #region Literal Node Tests

        [Theory]
        [InlineData(5, "5")]
        [InlineData(42, "42")]
        [InlineData(0, "0")]
        [InlineData(-10, "-10")]
        public void Visit_LiteralNode_Integer_ReturnsCorrectString(int value, string expected)
        {
            // Arrange
            var node = new LiteralNode(value);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(3.14f, "3.14")]
        [InlineData(0.0f, "0")]
        [InlineData(2.5f, "2.5")]
        [InlineData(-1.5f, "-1.5")]
        public void Visit_LiteralNode_Float_ReturnsCorrectString(float value, string expected)
        {
            // Arrange
            var node = new LiteralNode(value);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Visit_LiteralNode_WithIndentation_IgnoresIndentation()
        {
            // Arrange
            var node = new LiteralNode(42);

            // Act
            var result = _visitor.Unparse(node, 3);

            // Assert
            // Literals should not include indentation in expressions
            Assert.Equal("42", result);
        }

        #endregion

        #region Variable Node Tests

        [Theory]
        [InlineData("x", "x")]
        [InlineData("variable", "variable")]
        [InlineData("myVar123", "myVar123")]
        [InlineData("_underscore", "_underscore")]
        public void Visit_VariableNode_ReturnsVariableName(string varName, string expected)
        {
            // Arrange
            var node = new VariableNode(varName);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Visit_VariableNode_WithIndentation_IgnoresIndentation()
        {
            // Arrange
            var node = new VariableNode("x");

            // Act
            var result = _visitor.Unparse(node, 2);

            // Assert
            Assert.Equal("x", result);
        }

        #endregion

        #region Binary Operator Tests

        [Fact]
        public void Visit_PlusNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(5);
            var right = new LiteralNode(3);
            var node = new PlusNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(5 + 3)", result);
        }

        [Fact]
        public void Visit_MinusNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(10);
            var right = new LiteralNode(4);
            var node = new MinusNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(10 - 4)", result);
        }

        [Fact]
        public void Visit_TimesNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(7);
            var right = new LiteralNode(6);
            var node = new TimesNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(7 * 6)", result);
        }

        [Fact]
        public void Visit_FloatDivNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(15);
            var right = new LiteralNode(3);
            var node = new FloatDivNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(15 / 3)", result);
        }

        [Fact]
        public void Visit_IntDivNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(17);
            var right = new LiteralNode(5);
            var node = new IntDivNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(17 // 5)", result);
        }

        [Fact]
        public void Visit_ModulusNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(17);
            var right = new LiteralNode(5);
            var node = new ModulusNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(17 % 5)", result);
        }

        [Fact]
        public void Visit_ExponentiationNode_ReturnsCorrectFormat()
        {
            // Arrange
            var left = new LiteralNode(2);
            var right = new LiteralNode(8);
            var node = new ExponentiationNode(left, right);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(2 ** 8)", result);
        }

        #endregion

        #region Nested Expression Tests

        [Fact]
        public void Visit_NestedExpression_SimpleNesting_ReturnsCorrectFormat()
        {
            // Arrange: (5 + (3 * 2))
            var innerLeft = new LiteralNode(3);
            var innerRight = new LiteralNode(2);
            var inner = new TimesNode(innerLeft, innerRight);
            var outer = new PlusNode(new LiteralNode(5), inner);

            // Act
            var result = _visitor.Unparse(outer);

            // Assert
            Assert.Equal("(5 + (3 * 2))", result);
        }

        [Fact]
        public void Visit_NestedExpression_DeepNesting_ReturnsCorrectFormat()
        {
            // Arrange: ((2 + 3) * (4 - 1))
            var leftSide = new PlusNode(new LiteralNode(2), new LiteralNode(3));
            var rightSide = new MinusNode(new LiteralNode(4), new LiteralNode(1));
            var node = new TimesNode(leftSide, rightSide);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("((2 + 3) * (4 - 1))", result);
        }

        [Fact]
        public void Visit_ComplexExpression_WithVariables_ReturnsCorrectFormat()
        {
            // Arrange: (x + (y * 2))
            var x = new VariableNode("x");
            var y = new VariableNode("y");
            var two = new LiteralNode(2);
            var mult = new TimesNode(y, two);
            var add = new PlusNode(x, mult);

            // Act
            var result = _visitor.Unparse(add);

            // Assert
            Assert.Equal("(x + (y * 2))", result);
        }

        [Fact]
        public void Visit_AllOperators_InSingleExpression_ReturnsCorrectFormat()
        {
            // Arrange: ((2 ** 3) % (4 // 2))
            var exp = new ExponentiationNode(new LiteralNode(2), new LiteralNode(3));
            var div = new IntDivNode(new LiteralNode(4), new LiteralNode(2));
            var mod = new ModulusNode(exp, div);

            // Act
            var result = _visitor.Unparse(mod);

            // Assert
            Assert.Equal("((2 ** 3) % (4 // 2))", result);
        }

        #endregion

        #region Assignment Statement Tests

        [Fact]
        public void Visit_AssignmentStmt_Simple_ReturnsCorrectFormat()
        {
            // Arrange: x := 5
            var variable = new VariableNode("x");
            var expression = new LiteralNode(5);
            var stmt = new AssignmentStmt(variable, expression);

            // Act
            var result = _visitor.Unparse(stmt);

            // Assert
            Assert.Equal("x := 5", result);
        }

        [Fact]
        public void Visit_AssignmentStmt_WithExpression_ReturnsCorrectFormat()
        {
            // Arrange: y := (3 + 4)
            var variable = new VariableNode("y");
            var expression = new PlusNode(new LiteralNode(3), new LiteralNode(4));
            var stmt = new AssignmentStmt(variable, expression);

            // Act
            var result = _visitor.Unparse(stmt);

            // Assert
            Assert.Equal("y := (3 + 4)", result);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "    ")]
        [InlineData(2, "        ")]
        [InlineData(3, "            ")]
        public void Visit_AssignmentStmt_WithIndentation_ReturnsCorrectIndentation(int level, string expectedIndent)
        {
            // Arrange: x := 5
            var variable = new VariableNode("x");
            var expression = new LiteralNode(5);
            var stmt = new AssignmentStmt(variable, expression);

            // Act
            var result = _visitor.Unparse(stmt, level);

            // Assert
            Assert.Equal($"{expectedIndent}x := 5", result);
        }

        [Fact]
        public void Visit_AssignmentStmt_VariableToVariable_ReturnsCorrectFormat()
        {
            // Arrange: x := y
            var variable = new VariableNode("x");
            var expression = new VariableNode("y");
            var stmt = new AssignmentStmt(variable, expression);

            // Act
            var result = _visitor.Unparse(stmt);

            // Assert
            Assert.Equal("x := y", result);
        }

        #endregion

        #region Return Statement Tests

        [Fact]
        public void Visit_ReturnStmt_Simple_ReturnsCorrectFormat()
        {
            // Arrange: return 42
            var expression = new LiteralNode(42);
            var stmt = new ReturnStmt(expression);

            // Act
            var result = _visitor.Unparse(stmt);

            // Assert
            Assert.Equal("return 42", result);
        }

        [Fact]
        public void Visit_ReturnStmt_WithExpression_ReturnsCorrectFormat()
        {
            // Arrange: return (x + 1)
            var x = new VariableNode("x");
            var one = new LiteralNode(1);
            var expression = new PlusNode(x, one);
            var stmt = new ReturnStmt(expression);

            // Act
            var result = _visitor.Unparse(stmt);

            // Assert
            Assert.Equal("return (x + 1)", result);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "    ")]
        [InlineData(2, "        ")]
        public void Visit_ReturnStmt_WithIndentation_ReturnsCorrectIndentation(int level, string expectedIndent)
        {
            // Arrange: return 10
            var expression = new LiteralNode(10);
            var stmt = new ReturnStmt(expression);

            // Act
            var result = _visitor.Unparse(stmt, level);

            // Assert
            Assert.Equal($"{expectedIndent}return 10", result);
        }

        [Fact]
        public void Visit_ReturnStmt_ComplexExpression_ReturnsCorrectFormat()
        {
            // Arrange: return ((a * b) + c)
            var a = new VariableNode("a");
            var b = new VariableNode("b");
            var c = new VariableNode("c");
            var mult = new TimesNode(a, b);
            var add = new PlusNode(mult, c);
            var stmt = new ReturnStmt(add);

            // Act
            var result = _visitor.Unparse(stmt);

            // Assert
            Assert.Equal("return ((a * b) + c)", result);
        }

        #endregion

        #region Block Statement Tests

        [Fact]
        public void Visit_BlockStmt_Empty_ReturnsCorrectFormat()
        {
            // Arrange
            var st = new SymbolTable<string, object>();
            var block = new BlockStmt(st);

            // Act
            var result = _visitor.Unparse(block);

            // Assert
            // Note: Implementation may have issues with newlines in empty blocks
            Assert.Contains("{", result);
            Assert.Contains("}", result);
        }

        [Fact]
        public void Visit_BlockStmt_SingleStatement_ReturnsCorrectFormat()
        {
            // Arrange
            var st = new SymbolTable<string, object>();
            var block = new BlockStmt(st);
            var assignment = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            block.AddStatement(assignment);

            // Act
            var result = _visitor.Unparse(block);

            // Assert
            Assert.Contains("{", result);
            Assert.Contains("    x := 5", result);
            Assert.Contains("}", result);
        }

        [Fact]
        public void Visit_BlockStmt_MultipleStatements_ReturnsCorrectFormat()
        {
            // Arrange
            var st = new SymbolTable<string, object>();
            var block = new BlockStmt(st);
            block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(10)));
            block.AddStatement(new ReturnStmt(new VariableNode("x")));

            // Act
            var result = _visitor.Unparse(block);

            // Assert
            Assert.Contains("x := 5", result);
            Assert.Contains("y := 10", result);
            Assert.Contains("return x", result);
            var lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(lines.Length >= 3); // At least the three statements
        }

        [Fact]
        public void Visit_BlockStmt_NestedBlocks_ReturnsCorrectIndentation()
        {
            // Arrange: Outer block with inner block
            var outerSt = new SymbolTable<string, object>();
            var outerBlock = new BlockStmt(outerSt);
            
            var innerSt = new SymbolTable<string, object>(outerSt);
            var innerBlock = new BlockStmt(innerSt);
            innerBlock.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(2)));
            
            outerBlock.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(1)));
            outerBlock.AddStatement(innerBlock);

            // Act
            var result = _visitor.Unparse(outerBlock);

            // Assert
            Assert.Contains("{", result);
            Assert.Contains("    x := 1", result);
            Assert.Contains("        y := 2", result); // Inner statement should be double-indented
            Assert.Contains("}", result);
        }

        [Fact]
        public void Visit_BlockStmt_WithIndentationLevel_ReturnsCorrectIndentation()
        {
            // Arrange
            var st = new SymbolTable<string, object>();
            var block = new BlockStmt(st);
            block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));

            // Act
            var result = _visitor.Unparse(block, 1);

            // Assert
            // Block at level 1 should have its opening brace indented
            Assert.Contains("    {", result);
            Assert.Contains("        x := 5", result); // Statement inside should be at level 2
        }

        [Fact]
        public void Visit_BlockStmt_DeeplyNested_ReturnsCorrectIndentation()
        {
            // Arrange: Three levels of nesting
            var st1 = new SymbolTable<string, object>();
            var block1 = new BlockStmt(st1);
            
            var st2 = new SymbolTable<string, object>(st1);
            var block2 = new BlockStmt(st2);
            
            var st3 = new SymbolTable<string, object>(st2);
            var block3 = new BlockStmt(st3);
            
            block3.AddStatement(new AssignmentStmt(new VariableNode("z"), new LiteralNode(3)));
            block2.AddStatement(block3);
            block1.AddStatement(block2);

            // Act
            var result = _visitor.Unparse(block1);

            // Assert
            // Check that we have increasing indentation levels
            Assert.Contains("            z := 3", result); // Level 3 indentation
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Visit_CompleteProgram_ReturnsCorrectFormat()
        {
            // Arrange: Full program with multiple statements
            var st = new SymbolTable<string, object>();
            var block = new BlockStmt(st);
            
            // x := 5
            block.AddStatement(new AssignmentStmt(
                new VariableNode("x"), 
                new LiteralNode(5)
            ));
            
            // y := (x + 10)
            block.AddStatement(new AssignmentStmt(
                new VariableNode("y"),
                new PlusNode(new VariableNode("x"), new LiteralNode(10))
            ));
            
            // return (y * 2)
            block.AddStatement(new ReturnStmt(
                new TimesNode(new VariableNode("y"), new LiteralNode(2))
            ));

            // Act
            var result = _visitor.Unparse(block);

            // Assert
            Assert.Contains("x := 5", result);
            Assert.Contains("y := (x + 10)", result);
            Assert.Contains("return (y * 2)", result);
        }

        [Fact]
        public void Visit_ProgramWithNestedBlocksAndComplexExpressions_ReturnsCorrectFormat()
        {
            // Arrange: Complex nested program
            var outerSt = new SymbolTable<string, object>();
            var outerBlock = new BlockStmt(outerSt);
            
            // a := ((2 + 3) * 5)
            var expr1 = new TimesNode(
                new PlusNode(new LiteralNode(2), new LiteralNode(3)),
                new LiteralNode(5)
            );
            outerBlock.AddStatement(new AssignmentStmt(new VariableNode("a"), expr1));
            
            // Nested block
            var innerSt = new SymbolTable<string, object>(outerSt);
            var innerBlock = new BlockStmt(innerSt);
            
            // b := (a // 2)
            innerBlock.AddStatement(new AssignmentStmt(
                new VariableNode("b"),
                new IntDivNode(new VariableNode("a"), new LiteralNode(2))
            ));
            
            // return (b % 3)
            innerBlock.AddStatement(new ReturnStmt(
                new ModulusNode(new VariableNode("b"), new LiteralNode(3))
            ));
            
            outerBlock.AddStatement(innerBlock);

            // Act
            var result = _visitor.Unparse(outerBlock);

            // Assert
            Assert.Contains("a := ((2 + 3) * 5)", result);
            Assert.Contains("b := (a // 2)", result);
            Assert.Contains("return (b % 3)", result);
        }

        #endregion

        #region Edge Cases and Special Scenarios

        [Fact]
        public void Visit_ExpressionWithOnlyVariables_ReturnsCorrectFormat()
        {
            // Arrange: (x + y)
            var x = new VariableNode("x");
            var y = new VariableNode("y");
            var node = new PlusNode(x, y);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(x + y)", result);
        }

        [Fact]
        public void Visit_ExpressionWithNegativeNumbers_ReturnsCorrectFormat()
        {
            // Arrange: (-5 + 3)
            var negFive = new LiteralNode(-5);
            var three = new LiteralNode(3);
            var node = new PlusNode(negFive, three);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Equal("(-5 + 3)", result);
        }

        [Fact]
        public void Visit_ExpressionWithFloats_ReturnsCorrectFormat()
        {
            // Arrange: (3.14 * 2.0)
            var pi = new LiteralNode(3.14f);
            var two = new LiteralNode(2.0f);
            var node = new TimesNode(pi, two);

            // Act
            var result = _visitor.Unparse(node);

            // Assert
            Assert.Contains("3.14", result);
            Assert.Contains("2", result);
            Assert.Contains("*", result);
        }

        [Fact]
        public void Visit_LongVariableName_ReturnsCorrectFormat()
        {
            // Arrange
            var longName = "thisIsAVeryLongVariableNameForTesting";
            var variable = new VariableNode(longName);

            // Act
            var result = _visitor.Unparse(variable);

            // Assert
            Assert.Equal(longName, result);
        }

        #endregion
    }
}
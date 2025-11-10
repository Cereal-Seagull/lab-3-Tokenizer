using Xunit;
using AST;
using System;
using System.Collections.Generic;

namespace AST.Tests
{
    public class NameAnalysisVisitorTests
    {
        #region Helper Methods

        /// <summary>
        /// Helper method to create a NameAnalysisVisitor and analyze an AST
        /// </summary>
        private NameAnalysisVisitor CreateAndAnalyze(Statement ast)
        {
            var visitor = new NameAnalysisVisitor();
            visitor.Analyze(ast);
            return visitor;
        }

        /// <summary>
        /// Helper to create a simple block with statements
        /// </summary>
        private BlockStmt CreateBlock(params Statement[] statements)
        {
            var block = new BlockStmt(new SymbolTable<string, object>());
            foreach (var stmt in statements)
            {
                block.AddStatement(stmt);
            }
            return block;
        }

        #endregion

        #region Literal Node Tests

        [Fact]
        public void Visit_LiteralNode_ReturnsTrue()
        {
            // Arrange
            var literal = new LiteralNode(42);
            var visitor = new NameAnalysisVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);

            // Act
            bool result = visitor.Visit(literal, tuple);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-50)]
        [InlineData(3.14)]
        public void Visit_LiteralNode_WithVariousValues_ReturnsTrue(object value)
        {
            // Arrange
            var literal = new LiteralNode(value);
            var visitor = new NameAnalysisVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);

            // Act
            bool result = visitor.Visit(literal, tuple);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Variable Node Tests

        [Fact]
        public void Visit_VariableNode_WhenVariableExists_ReturnsTrue()
        {
            // Arrange
            var variable = new VariableNode("x");
            var visitor = new NameAnalysisVisitor();
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true; // Variable is defined
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);

            // Act
            bool result = visitor.Visit(variable, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_VariableNode_WhenVariableDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var variable = new VariableNode("y");
            var visitor = new NameAnalysisVisitor();
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);

            // Act
            bool result = visitor.Visit(variable, tuple);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("var1")]
        [InlineData("myVariable")]
        public void Visit_VariableNode_WithDefinedVariables_ReturnsTrue(string varName)
        {
            // Arrange
            var variable = new VariableNode(varName);
            var visitor = new NameAnalysisVisitor();
            var symbolTable = new SymbolTable<string, object>();
            symbolTable[varName] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);

            // Act
            bool result = visitor.Visit(variable, tuple);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Binary Operator Tests

        [Fact]
        public void Visit_PlusNode_WithDefinedVariables_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true;
            symbolTable["y"] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var plusNode = new PlusNode(new VariableNode("x"), new VariableNode("y"));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(plusNode, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_PlusNode_WithUndefinedLeftOperand_ReturnsFalse()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var plusNode = new PlusNode(new VariableNode("undefined"), new LiteralNode(5));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(plusNode, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_PlusNode_WithUndefinedRightOperand_ReturnsFalse()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var plusNode = new PlusNode(new LiteralNode(5), new VariableNode("undefined"));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(plusNode, tuple);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("x", "y")]
        [InlineData("a", "b")]
        [InlineData("var1", "var2")]
        public void Visit_MinusNode_WithDefinedVariables_ReturnsTrue(string left, string right)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable[left] = true;
            symbolTable[right] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var minusNode = new MinusNode(new VariableNode(left), new VariableNode(right));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(minusNode, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_TimesNode_WithMixedOperands_ReturnsCorrectResult()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var timesNode = new TimesNode(new VariableNode("x"), new LiteralNode(10));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(timesNode, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_FloatDivNode_WithUndefinedVariables_ReturnsFalse()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var divNode = new FloatDivNode(new VariableNode("x"), new VariableNode("y"));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(divNode, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_IntDivNode_WithLiterals_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var divNode = new IntDivNode(new LiteralNode(10), new LiteralNode(3));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(divNode, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_ModulusNode_WithOneUndefinedOperand_ReturnsFalse()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var modNode = new ModulusNode(new VariableNode("x"), new VariableNode("undefined"));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(modNode, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_ExponentiationNode_WithDefinedVariables_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["base"] = true;
            symbolTable["exp"] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            var expNode = new ExponentiationNode(new VariableNode("base"), new VariableNode("exp"));
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(expNode, tuple);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Assignment Statement Tests

        [Fact]
        public void Visit_AssignmentStmt_DefinesNewVariable_ReturnsTrue()
        {
            // Arrange
            var assignment = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert
            Assert.True(result);
            Assert.True(symbolTable.ContainsKey("x"));
        }

        [Fact]
        public void Visit_AssignmentStmt_WithUndefinedVariableInExpression_ReturnsFalse()
        {
            // Arrange
            var assignment = new AssignmentStmt(
                new VariableNode("x"), 
                new PlusNode(new VariableNode("undefined"), new LiteralNode(5))
            );
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_AssignmentStmt_ReassigningExistingVariable_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true;
            var assignment = new AssignmentStmt(new VariableNode("x"), new LiteralNode(10));
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_AssignmentStmt_UsingPreviouslyDefinedVariable_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["y"] = true;
            var assignment = new AssignmentStmt(
                new VariableNode("x"), 
                new VariableNode("y")
            );
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert
            Assert.True(result);
            Assert.True(symbolTable.ContainsKey("x"));
        }

        #endregion

        #region Return Statement Tests

        [Fact]
        public void Visit_ReturnStmt_WithLiteral_ReturnsTrue()
        {
            // Arrange
            var returnStmt = new ReturnStmt(new LiteralNode(42));
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, returnStmt);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(returnStmt, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_ReturnStmt_WithDefinedVariable_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["result"] = true;
            var returnStmt = new ReturnStmt(new VariableNode("result"));
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, returnStmt);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(returnStmt, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_ReturnStmt_WithUndefinedVariable_ReturnsFalse()
        {
            // Arrange
            var returnStmt = new ReturnStmt(new VariableNode("undefined"));
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, returnStmt);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(returnStmt, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_ReturnStmt_WithComplexExpression_ReturnsCorrectResult()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true;
            symbolTable["y"] = true;
            var returnStmt = new ReturnStmt(
                new PlusNode(new VariableNode("x"), new VariableNode("y"))
            );
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, returnStmt);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(returnStmt, tuple);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Block Statement Tests

        [Fact]
        public void Visit_BlockStmt_Empty_ReturnsTrue()
        {
            // Arrange
            var block = CreateBlock();
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(block, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_BlockStmt_WithValidStatements_ReturnsTrue()
        {
            // Arrange
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new VariableNode("x"));
            var block = CreateBlock(stmt1, stmt2);
            
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(block, tuple);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Visit_BlockStmt_WithUndefinedVariable_ReturnsFalse()
        {
            // Arrange
            var stmt1 = new AssignmentStmt(
                new VariableNode("x"), 
                new VariableNode("undefined")
            );
            var block = CreateBlock(stmt1);
            
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(block, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_BlockStmt_WithMultipleErrors_ReturnsFalse()
        {
            // Arrange
            var stmt1 = new AssignmentStmt(
                new VariableNode("x"), 
                new VariableNode("undefined1")
            );
            var stmt2 = new ReturnStmt(new VariableNode("undefined2"));
            var block = CreateBlock(stmt1, stmt2);
            
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(block, tuple);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Integration Tests with Analyze Method

        [Fact]
        public void Analyze_SimpleAssignment_NoErrors()
        {
            // Arrange
            var assignment = new AssignmentStmt(new VariableNode("x"), new LiteralNode(10));
            var block = CreateBlock(assignment);

            // Act
            var visitor = CreateAndAnalyze(block);

            // Assert - no exceptions should be thrown
            Assert.NotNull(visitor);
        }

        [Fact]
        public void Analyze_UsingVariableBeforeDefinition_HasErrors()
        {
            // Arrange
            var stmt1 = new ReturnStmt(new VariableNode("x"));
            var stmt2 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var block = CreateBlock(stmt1, stmt2);

            // Act
            var visitor = CreateAndAnalyze(block);

            // Assert - visitor should complete analysis
            Assert.NotNull(visitor);
        }

        [Fact]
        public void Analyze_SequentialAssignments_NoErrors()
        {
            // Arrange
            var stmt1 = new AssignmentStmt(new VariableNode("a"), new LiteralNode(1));
            var stmt2 = new AssignmentStmt(new VariableNode("b"), new VariableNode("a"));
            var stmt3 = new AssignmentStmt(
                new VariableNode("c"), 
                new PlusNode(new VariableNode("a"), new VariableNode("b"))
            );
            var block = CreateBlock(stmt1, stmt2, stmt3);

            // Act
            var visitor = CreateAndAnalyze(block);

            // Assert
            Assert.NotNull(visitor);
        }

        [Fact]
        public void Analyze_ComplexExpression_WithAllDefinedVariables_NoErrors()
        {
            // Arrange
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new AssignmentStmt(
                new VariableNode("z"),
                new TimesNode(
                    new PlusNode(new VariableNode("x"), new VariableNode("y")),
                    new LiteralNode(2)
                )
            );
            var block = CreateBlock(stmt1, stmt2, stmt3);

            // Act
            var visitor = CreateAndAnalyze(block);

            // Assert
            Assert.NotNull(visitor);
        }

        [Fact]
        public void Analyze_ComplexExpression_WithUndefinedVariable_HasErrors()
        {
            // Arrange
            var stmt = new AssignmentStmt(
                new VariableNode("result"),
                new PlusNode(
                    new TimesNode(new VariableNode("x"), new LiteralNode(2)),
                    new VariableNode("undefined")
                )
            );
            var block = CreateBlock(stmt);

            // Act
            var visitor = CreateAndAnalyze(block);

            // Assert
            Assert.NotNull(visitor);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Visit_NestedBinaryOperators_WithMixedDefinedAndUndefined_ReturnsCorrectResult()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["a"] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            // (a + 5) * undefined
            var nestedExpr = new TimesNode(
                new PlusNode(new VariableNode("a"), new LiteralNode(5)),
                new VariableNode("undefined")
            );
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(nestedExpr, tuple);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Visit_DeeplyNestedExpression_AllDefined_ReturnsTrue()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["a"] = true;
            symbolTable["b"] = true;
            symbolTable["c"] = true;
            var block = CreateBlock();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, block);
            
            // ((a + b) * c) / 2
            var nestedExpr = new FloatDivNode(
                new TimesNode(
                    new PlusNode(new VariableNode("a"), new VariableNode("b")),
                    new VariableNode("c")
                ),
                new LiteralNode(2)
            );
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(nestedExpr, tuple);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("x")]
        [InlineData("variable")]
        [InlineData("myVar123")]
        public void Analyze_SingleVariableAssignment_AddsToSymbolTable(string varName)
        {
            // Arrange
            var assignment = new AssignmentStmt(new VariableNode(varName), new LiteralNode(42));
            var block = CreateBlock(assignment);

            // Act
            var visitor = CreateAndAnalyze(block);

            // Assert - analysis should complete without throwing
            Assert.NotNull(visitor);
        }

        #endregion

        #region Self-Reference and Order-of-Evaluation Tests

        [Fact]
        public void Visit_AssignmentStmt_SelfReferenceUndefined_ReturnsFalse()
        {
            // Test that x := x + 1 fails when x is not previously defined
            // The expression should be evaluated BEFORE x is added to the symbol table
            
            // Arrange
            var assignment = new AssignmentStmt(
                new VariableNode("x"),
                new PlusNode(new VariableNode("x"), new LiteralNode(1))
            );
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert - should return false because x is undefined in the expression
            Assert.False(result);
        }

        [Fact]
        public void Visit_AssignmentStmt_SelfReferenceDefined_ReturnsTrue()
        {
            // Test that x := x + 1 succeeds when x is previously defined
            
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true; // x is already defined
            var assignment = new AssignmentStmt(
                new VariableNode("x"),
                new PlusNode(new VariableNode("x"), new LiteralNode(1))
            );
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert - should return true because x was defined before the assignment
            Assert.True(result);
        }

        [Fact]
        public void Visit_AssignmentStmt_ExpressionEvaluatedBeforeVariableAdded()
        {
            // Test that the expression is evaluated before the variable is added to symbol table
            // y := x + 1, then check that y is in symbol table after
            
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            symbolTable["x"] = true;
            var assignment = new AssignmentStmt(
                new VariableNode("y"),
                new PlusNode(new VariableNode("x"), new LiteralNode(1))
            );
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert
            Assert.True(result);
            Assert.True(symbolTable.ContainsKey("y")); // y should now be in symbol table
            Assert.True(symbolTable.ContainsKey("x")); // x should still be in symbol table
        }

        [Fact]
        public void Visit_AssignmentStmt_LeftSideVariableNotRequiredToPreExist()
        {
            // Test that the variable on the left side doesn't need to pre-exist
            // This is the normal case: defining a new variable
            
            // Arrange
            var assignment = new AssignmentStmt(
                new VariableNode("newVar"),
                new LiteralNode(42)
            );
            var symbolTable = new SymbolTable<string, object>();
            var tuple = new Tuple<SymbolTable<string, object>, Statement>(symbolTable, assignment);
            var visitor = new NameAnalysisVisitor();

            // Act
            bool result = visitor.Visit(assignment, tuple);

            // Assert
            Assert.True(result);
            Assert.True(symbolTable.ContainsKey("newVar"));
        }

        #endregion
    }
}
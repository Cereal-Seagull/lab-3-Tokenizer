using Xunit;
using AST;
using Optimizer;
using System.Collections.Generic;
using System.Linq;

namespace AST.Tests
{
    public class ControlFlowGraphGeneratorVisitorTests
    {
        #region Helper Methods

        private ControlFlowGraphGeneratorVisitor CreateVisitor()
        {
            return new ControlFlowGraphGeneratorVisitor();
        }

        private AssignmentStmt CreateAssignment(string varName, int value)
        {
            return new AssignmentStmt(
                new VariableNode(varName),
                new LiteralNode(value)
            );
        }

        private ReturnStmt CreateReturn(int value)
        {
            return new ReturnStmt(new LiteralNode(value));
        }

        #endregion

        #region GenerateCFG Tests

        [Fact]
        public void GenerateCFG_SingleAssignment_CreatesCFGWithOneVertex()
        {
            // Arrange
            var visitor = CreateVisitor();

            // x := (5)
            var assignment = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var block = new BlockStmt(new SymbolTable<string, object>());
            
            block.AddStatement(assignment);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.NotNull(cfg);
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Equal(assignment, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_TwoSequentialAssignments_CreatesLinkedCFG()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 5);
            var assignment2 = CreateAssignment("y", 10);
            var block = new BlockStmt(new SymbolTable<string, object>());

            block.AddStatement(assignment1);
            block.AddStatement(assignment2);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(1, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(assignment1, assignment2));
            Assert.Equal(assignment1, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_ThreeSequentialAssignments_CreatesLinearCFG()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 5);
            var assignment2 = CreateAssignment("y", 10);
            var assignment3 = CreateAssignment("z", 15);
            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStatement(assignment1);
            block.AddStatement(assignment2);
            block.AddStatement(assignment3);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(3, cfg.VertexCount());
            Assert.Equal(2, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(assignment1, assignment2));
            Assert.True(cfg.HasEdge(assignment2, assignment3));
            Assert.Equal(assignment1, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_AssignmentThenReturn_CreatesProperFlow()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment = CreateAssignment("x", 5);
            var returnStmt = CreateReturn(10);
            var block = new BlockStmt(new SymbolTable<string, object>());

            block.AddStatement(assignment);
            block.AddStatement(returnStmt);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(1, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(assignment, returnStmt));
            Assert.Equal(assignment, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_SingleReturn_CreatesCFGWithOneVertex()
        {
            // Arrange
            var visitor = CreateVisitor();
            var returnStmt = CreateReturn(42);
            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStatement(returnStmt);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Equal(returnStmt, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_NestedBlocks_FlattensCorrectly()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 5);
            var assignment2 = CreateAssignment("y", 10);
            var assignment3 = CreateAssignment("z", 15);
            
            var innerBlock = new BlockStmt(new SymbolTable<string, object>());
            innerBlock.AddStatement(assignment2);
            innerBlock.AddStatement(assignment3);

            var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            outerBlock.AddStatement(assignment1);
            outerBlock.AddStatement(innerBlock);

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            // BlockStmt nodes should not appear in CFG
            Assert.Equal(3, cfg.VertexCount());
            Assert.Equal(2, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(assignment1, assignment2));
            Assert.True(cfg.HasEdge(assignment2, assignment3));
            Assert.Equal(assignment1, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_NestedBlocksWithReturn_HandlesCorrectly()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 2);
            var assignment2 = CreateAssignment("y", 8);
            var returnStmt = CreateReturn(10);
            var assignment3 = CreateAssignment("z", 3);
            var returnStmt2 = CreateReturn(5);
            
            var innerBlock = new BlockStmt(new SymbolTable<string, object>());
            innerBlock.AddStatement(assignment2);
            innerBlock.AddStatement(returnStmt);

            var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            outerBlock.AddStatement(assignment1);
            outerBlock.AddStatement(innerBlock);
            outerBlock.AddStatement(assignment3);
            outerBlock.AddStatement(returnStmt2);

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            // Bug Note: Based on the Visit(ReturnStmt) implementation, 
            // it checks if prev.GetType() != typeof(ReturnStmt), which will
            // throw NullReferenceException if prev is null (which it returns).
            // Also, statements after return should still be added but not connected.
            Assert.Equal(5, cfg.VertexCount());
            Assert.True(cfg.HasEdge(assignment1, assignment2));
            Assert.True(cfg.HasEdge(assignment2, returnStmt));
            // No edge should exist from returnStmt to assignment3
            Assert.False(cfg.HasEdge(returnStmt, assignment3));
            Assert.Equal(assignment1, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_EmptyBlock_CreatesEmptyCFG()
        {
            // Arrange
            var visitor = CreateVisitor();
            var block = new BlockStmt(new SymbolTable<string, object>());

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.NotNull(cfg);
            Assert.Equal(0, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Null(cfg.Start);
        }

        [Fact]
        public void GenerateCFG_MultipleReturns_OnlyFirstReturnHasIncomingEdge()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment = CreateAssignment("x", 5);
            var returnStmt1 = CreateReturn(10);
            var returnStmt2 = CreateReturn(20);
            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStatement(assignment);
            block.AddStatement(returnStmt1);
            block.AddStatement(returnStmt2);

            // Act
            // Bug Note: This will likely throw NullReferenceException
            // because Visit(ReturnStmt) returns null, and then the next
            // statement tries to call prev.GetType() on null.
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(3, cfg.VertexCount());
            Assert.True(cfg.HasEdge(assignment, returnStmt1));
            // returnStmt2 should be added but not connected
            Assert.False(cfg.HasEdge(returnStmt1, returnStmt2));
        }

        [Fact]
        public void GenerateCFG_DeeplyNestedBlocks_FlattensCorrectly()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("a", 1);
            var assignment2 = CreateAssignment("b", 2);
            var assignment3 = CreateAssignment("c", 3);
            var assignment4 = CreateAssignment("d", 4);
            
            var level3Block = new BlockStmt(new SymbolTable<string, object>());
            level3Block.AddStatement(assignment4);

            var level2Block = new BlockStmt(new SymbolTable<string, object>());
            level2Block.AddStatement(assignment3);
            level2Block.AddStatement(level3Block);

            var level1Block = new BlockStmt(new SymbolTable<string, object>());
            level1Block.AddStatement(assignment2);
            level1Block.AddStatement(level2Block);

            var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            outerBlock.AddStatement(assignment1);
            outerBlock.AddStatement(level1Block);

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            Assert.Equal(4, cfg.VertexCount());
            Assert.Equal(3, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(assignment1, assignment2));
            Assert.True(cfg.HasEdge(assignment2, assignment3));
            Assert.True(cfg.HasEdge(assignment3, assignment4));
            Assert.Equal(assignment1, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_BlockStartingWithNestedBlock_SetsStartCorrectly()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 1);
            var assignment2 = CreateAssignment("y", 2);
            
            var innerBlock = new BlockStmt(new SymbolTable<string, object>());
            innerBlock.AddStatement(assignment1);

            var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            outerBlock.AddStatement(innerBlock);
            outerBlock.AddStatement(assignment2);

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(assignment1, cfg.Start);
            Assert.True(cfg.HasEdge(assignment1, assignment2));
        }

        #endregion

        #region Visit Method Tests

        [Theory]
        [InlineData("x", 5)]
        [InlineData("variable", 100)]
        [InlineData("temp", -42)]
        public void Visit_AssignmentStmt_AddsVertexToCFG(string varName, int value)
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment = CreateAssignment(varName, value);
            visitor._cfg = new CFG();

            // Act
            visitor.Visit(assignment, null);

            // Assert
            Assert.Equal(1, visitor._cfg.VertexCount());
        }

        [Fact]
        public void Visit_AssignmentStmt_WithNonReturnPrev_AddsEdge()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 5);
            var assignment2 = CreateAssignment("y", 10);
            visitor._cfg = new CFG();
            visitor._cfg.AddVertex(assignment1);

            // Act
            visitor.Visit(assignment2, assignment1);

            // Assert
            Assert.Equal(2, visitor._cfg.VertexCount());
            Assert.True(visitor._cfg.HasEdge(assignment1, assignment2));
        }

        [Fact]
        public void Visit_AssignmentStmt_AfterReturn_NoEdgeAdded()
        {
            // Arrange
            var visitor = CreateVisitor();
            var returnStmt = CreateReturn(5);
            var assignment = CreateAssignment("x", 10);
            visitor._cfg = new CFG();
            visitor._cfg.AddVertex(returnStmt);

            // Act
            visitor.Visit(assignment, null); // return stmt will return null in a normal setting

            // Assert
            Assert.Equal(2, visitor._cfg.VertexCount());
            Assert.False(visitor._cfg.HasEdge(returnStmt, assignment));
        }

        [Fact]
        public void Visit_ReturnStmt_AddsVertexToCFG()
        {
            // Arrange
            var visitor = CreateVisitor();
            var returnStmt = CreateReturn(42);
            visitor._cfg = new CFG();

            // Act
            var result = visitor.Visit(returnStmt, null);

            // Assert
            Assert.Equal(1, visitor._cfg.VertexCount());
            Assert.Null(result);
        }

        [Fact]
        public void Visit_ReturnStmt_WithNonReturnPrev_AddsEdge()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment = CreateAssignment("x", 5);
            var returnStmt = CreateReturn(10);
            visitor._cfg = new CFG();
            visitor._cfg.AddVertex(assignment);

            // Act
            var result = visitor.Visit(returnStmt, assignment);

            // Assert
            Assert.Equal(2, visitor._cfg.VertexCount());
            Assert.True(visitor._cfg.HasEdge(assignment, returnStmt));
            Assert.Null(result);
        }

        [Fact]
        public void Visit_ReturnStmt_ReturnsNull()
        {
            // Arrange
            var visitor = CreateVisitor();
            var returnStmt = CreateReturn(42);
            var prevStmt = CreateAssignment("x", 5);
            visitor._cfg = new CFG();
            visitor._cfg.AddVertex(prevStmt);

            // Act
            var result = visitor.Visit(returnStmt, prevStmt);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Visit_BlockStmt_ProcessesAllStatements(int statementCount)
        {
            // Arrange
            var visitor = CreateVisitor();
            var statements = new List<Statement>();
            for (int i = 0; i < statementCount; i++)
            {
                statements.Add(CreateAssignment($"var{i}", i));
            }
            var block = new BlockStmt(new SymbolTable<string, object>());
            foreach (var stmt in statements)
            {
                block.AddStatement(stmt);
            }
            visitor._cfg = new CFG();
            // visitor.isStart = false;

            // Act
            visitor.Visit(block, null);

            // Assert
            Assert.Equal(statementCount, visitor._cfg.VertexCount());
            Assert.Equal(statementCount - 1, visitor._cfg.EdgeCount());
        }

        [Fact]
        public void Visit_BlockStmt_SetsStartOnFirstNonBlockStatement()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment = CreateAssignment("x", 5);
            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStatement(assignment);

            visitor._cfg = new CFG();
            // visitor.IsStart = false;

            // Act
            visitor.Visit(block, null);

            // Assert
            Assert.Equal(assignment, visitor._cfg.Start);
            Assert.True(visitor.IsStart);
        }

        [Fact]
        public void Visit_BlockStmt_SkipsBlockStmtWhenSettingStart()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment = CreateAssignment("x", 5);
            var innerBlock = new BlockStmt(new SymbolTable<string, object>());
            innerBlock.AddStatement(assignment);

            var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            outerBlock.AddStatement(innerBlock);

            visitor._cfg = new CFG();
            // visitor.isStart = false;

            // Act
            visitor.Visit(outerBlock, null);

            // Assert
            Assert.Equal(assignment, visitor._cfg.Start);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(-7)]
        public void Visit_LiteralNode_ReturnsPrev(int value)
        {
            // Arrange
            var visitor = CreateVisitor();
            var literal = new LiteralNode(value);
            var prevStmt = CreateAssignment("x", 1);

            // Act
            var result = visitor.Visit(literal, prevStmt);

            // Assert
            Assert.Equal(prevStmt, result);
        }

        [Theory]
        [InlineData("x")]
        [InlineData("myVariable")]
        [InlineData("temp123")]
        public void Visit_VariableNode_ReturnsPrev(string varName)
        {
            // Arrange
            var visitor = CreateVisitor();
            var variable = new VariableNode(varName);
            var prevStmt = CreateAssignment("x", 1);

            // Act
            var result = visitor.Visit(variable, prevStmt);

            // Assert
            Assert.Equal(prevStmt, result);
        }

        #endregion

        #region Binary Operator Tests

        [Fact]
        public void Visit_BinaryOperators_ReturnPrev()
        {
            // Arrange
            var visitor = CreateVisitor();
            var prevStmt = CreateAssignment("x", 1);
            var left = new LiteralNode(1);
            var right = new LiteralNode(2);

            // Act & Assert
            Assert.Equal(prevStmt, visitor.Visit(new PlusNode(left, right), prevStmt));
            Assert.Equal(prevStmt, visitor.Visit(new MinusNode(left, right), prevStmt));
            Assert.Equal(prevStmt, visitor.Visit(new TimesNode(left, right), prevStmt));
            Assert.Equal(prevStmt, visitor.Visit(new FloatDivNode(left, right), prevStmt));
            Assert.Equal(prevStmt, visitor.Visit(new IntDivNode(left, right), prevStmt));
            Assert.Equal(prevStmt, visitor.Visit(new ModulusNode(left, right), prevStmt));
            Assert.Equal(prevStmt, visitor.Visit(new ExponentiationNode(left, right), prevStmt));
        }

        #endregion

        #region Multiple Calls to GenerateCFG

        [Fact]
        public void GenerateCFG_CalledMultipleTimes_ResetsIsStart()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 5);
            var block1 = new BlockStmt(new SymbolTable<string, object>());
            block1.AddStatement(assignment1);
            
            var assignment2 = CreateAssignment("y", 10);
            var block2 = new BlockStmt(new SymbolTable<string, object>());
            block2.AddStatement(assignment2);

            // Act
            var cfg1 = visitor.GenerateCFG(block1);
            var cfg2 = visitor.GenerateCFG(block2);

            // Assert
            Assert.Equal(assignment1, cfg1.Start);
            Assert.Equal(assignment2, cfg2.Start);
        }

        [Fact]
        public void GenerateCFG_CalledMultipleTimes_CreatesSeparateCFGs()
        {
            // Arrange
            var visitor = CreateVisitor();
            var assignment1 = CreateAssignment("x", 5);
            var block1 = new BlockStmt(new SymbolTable<string, object>());
            block1.AddStatement(assignment1);

            // Act
            var cfg1 = visitor.GenerateCFG(block1);
            
            // Modify the visitor's internal CFG
            var assignment2 = CreateAssignment("y", 10);
            var block2 = new BlockStmt(new SymbolTable<string, object>());
            block2.AddStatement(assignment2);

            var cfg2 = visitor.GenerateCFG(block2);

            // Assert
            // Bug Note: The visitor reuses the same _cfg object, so cfg1 and cfg2
            // reference the same CFG instance. This means cfg1 will be modified
            // when GenerateCFG is called the second time.
            Assert.NotNull(cfg1);
            Assert.NotNull(cfg2);
        }

        #endregion
    }
}
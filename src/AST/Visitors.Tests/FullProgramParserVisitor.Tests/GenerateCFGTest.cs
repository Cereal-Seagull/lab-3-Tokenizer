using Xunit;
using AST;
using Optimizer;

namespace AST.Tests
{
    public class GenerateCFGTests
    {
        // Helper method to create a simple assignment statement
        private AssignmentStmt CreateAssignment(string varName, int value)
        {
            return new AssignmentStmt(
                new VariableNode(varName),
                new LiteralNode(value)
            );
        }

        // Helper method to create a return statement
        private ReturnStmt CreateReturn(string varName)
        {
            return new ReturnStmt(new VariableNode(varName));
        }

        private ReturnStmt CreateReturnLiteral(int value)
        {
            return new ReturnStmt(new LiteralNode(value));
        }

        [Fact]
        public void GenerateCFG_EmptyBlock_ReturnsEmptyCFG()
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var emptyBlock = new BlockStmt(new List<Statement>());

            // Act
            var cfg = visitor.GenerateCFG(emptyBlock);

            // Assert
            Assert.NotNull(cfg);
            Assert.Equal(0, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Null(cfg.Start);
        }

        [Fact]
        public void GenerateCFG_SingleAssignment_CreatesOneVertex()
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assignment = CreateAssignment("x", 5);
            var block = new BlockStmt(new List<Statement> { assignment });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Equal(assignment, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_SingleReturn_CreatesOneVertex()
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var returnStmt = CreateReturnLiteral(42);
            var block = new BlockStmt(new List<Statement> { returnStmt });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Equal(returnStmt, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_TwoSequentialAssignments_CreatesEdge()
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assign1 = CreateAssignment("x", 5);
            var assign2 = CreateAssignment("y", 10);
            var block = new BlockStmt(new List<Statement> { assign1, assign2 });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(1, cfg.EdgeCount());
            Assert.Equal(assign1, cfg.Start);
            Assert.True(cfg.HasEdge(assign1, assign2));
        }

        [Fact]
        public void GenerateCFG_AssignmentThenReturn_CreatesEdge()
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assignment = CreateAssignment("x", 5);
            var returnStmt = CreateReturn("x");
            var block = new BlockStmt(new List<Statement> { assignment, returnStmt });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(1, cfg.EdgeCount());
            Assert.Equal(assignment, cfg.Start);
            Assert.True(cfg.HasEdge(assignment, returnStmt));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void GenerateCFG_MultipleAssignments_CreatesLinearChain(int numAssignments)
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var statements = new List<Statement>();
            
            for (int i = 0; i < numAssignments; i++)
            {
                statements.Add(CreateAssignment($"var{i}", i));
            }
            
            var block = new BlockStmt(statements);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(numAssignments, cfg.VertexCount());
            Assert.Equal(numAssignments - 1, cfg.EdgeCount());
            Assert.Equal(statements[0], cfg.Start);
            
            // Verify chain connectivity
            for (int i = 0; i < numAssignments - 1; i++)
            {
                Assert.True(cfg.HasEdge(statements[i], statements[i + 1]));
            }
        }

        [Fact]
        public void GenerateCFG_ExampleFromPDF_Figure2()
        {
            // x := (5)
            // y := (10)
            // z := (x + y)
            // return (z)
            
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assign1 = CreateAssignment("x", 5);
            var assign2 = CreateAssignment("y", 10);
            var assign3 = new AssignmentStmt(
                new VariableNode("z"),
                new PlusNode(new VariableNode("x"), new VariableNode("y"))
            );
            var returnStmt = CreateReturn("z");
            
            var block = new BlockStmt(new List<Statement> 
            { 
                assign1, assign2, assign3, returnStmt 
            });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(4, cfg.VertexCount());
            Assert.Equal(3, cfg.EdgeCount());
            Assert.Equal(assign1, cfg.Start);
            Assert.True(cfg.HasEdge(assign1, assign2));
            Assert.True(cfg.HasEdge(assign2, assign3));
            Assert.True(cfg.HasEdge(assign3, returnStmt));
        }

        [Fact]
        public void GenerateCFG_NestedBlocks_FlattensCorrectly()
        {
            // {
            //     x := (2)
            //     {
            //         y := (2 * 4)
            //         return (x+y)
            //     }
            //     z := (3)
            //     return x
            // }
            
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assign1 = CreateAssignment("x", 2);
            var assign2 = new AssignmentStmt(
                new VariableNode("y"),
                new TimesNode(new LiteralNode(2), new LiteralNode(4))
            );
            var return1 = new ReturnStmt(
                new PlusNode(new VariableNode("x"), new VariableNode("y"))
            );
            var assign3 = CreateAssignment("z", 3);
            var return2 = CreateReturn("x");

            var innerBlock = new BlockStmt(new List<Statement> { assign2, return1 });
            var outerBlock = new BlockStmt(new List<Statement> 
            { 
                assign1, innerBlock, assign3, return2 
            });

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            Assert.Equal(4, cfg.VertexCount()); // Only non-block statements
            Assert.Equal(assign1, cfg.Start);
            
            // NOTE: Bug in implementation - after return1, there should be no edges
            // but the current implementation will try to connect return1 to assign3
            // The test reflects expected behavior per PDF (returns should not have outgoing edges)
            Assert.True(cfg.HasEdge(assign1, assign2));
            Assert.True(cfg.HasEdge(assign2, return1));
            
            // Bug: Implementation adds edge from return to next statement
            // Expected: no edge from return1 to assign3
            // Actual: edge exists (bug in Visit(ReturnStmt))
            // Testing what SHOULD happen, not the bug:
            Assert.False(cfg.HasEdge(return1, assign3));
        }

        [Fact]
        public void GenerateCFG_ReturnFollowedByStatement_NoEdgeFromReturn()
        {
            // This tests dead code after return
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assignment = CreateAssignment("x", 5);
            var returnStmt = CreateReturn("x");
            var deadCode = CreateAssignment("y", 10); // This is unreachable
            
            var block = new BlockStmt(new List<Statement> 
            { 
                assignment, returnStmt, deadCode 
            });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(3, cfg.VertexCount());
            Assert.Equal(assignment, cfg.Start);
            Assert.True(cfg.HasEdge(assignment, returnStmt));
            
            // NOTE: Bug in implementation - Visit(ReturnStmt) returns null
            // but Visit(AssignmentStmt) doesn't check for null prev
            // This will cause NullReferenceException or incorrect behavior
            // Expected: no edge from return to dead code
            Assert.False(cfg.HasEdge(returnStmt, deadCode));
        }

        [Fact]
        public void GenerateCFG_MultipleNestedBlocks_FlattensToStatements()
        {
            // { { x := 1 } { y := 2 } z := 3 }
            
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assign1 = CreateAssignment("x", 1);
            var assign2 = CreateAssignment("y", 2);
            var assign3 = CreateAssignment("z", 3);

            var block1 = new BlockStmt(new List<Statement> { assign1 });
            var block2 = new BlockStmt(new List<Statement> { assign2 });
            var outerBlock = new BlockStmt(new List<Statement> 
            { 
                block1, block2, assign3 
            });

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            Assert.Equal(3, cfg.VertexCount());
            Assert.Equal(2, cfg.EdgeCount());
            Assert.Equal(assign1, cfg.Start);
            Assert.True(cfg.HasEdge(assign1, assign2));
            Assert.True(cfg.HasEdge(assign2, assign3));
        }

        [Fact]
        public void GenerateCFG_OnlyNestedBlocks_FindsFirstNonBlockStatement()
        {
            // { { { x := 1 } } }
            
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assignment = CreateAssignment("x", 1);
            var innerBlock = new BlockStmt(new List<Statement> { assignment });
            var middleBlock = new BlockStmt(new List<Statement> { innerBlock });
            var outerBlock = new BlockStmt(new List<Statement> { middleBlock });

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Equal(assignment, cfg.Start);
        }

        [Fact]
        public void GenerateCFG_MultipleReturns_FirstReturnHasNoOutgoingEdges()
        {
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var return1 = CreateReturnLiteral(1);
            var return2 = CreateReturnLiteral(2);
            
            var block = new BlockStmt(new List<Statement> { return1, return2 });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(return1, cfg.Start);
            
            // No edge should exist from first return to second
            // NOTE: Bug - implementation checks prev.GetType() != typeof(ReturnStmt)
            // but doesn't handle when prev is null (returned by Visit(ReturnStmt))
            Assert.False(cfg.HasEdge(return1, return2));
        }

        [Fact]
        public void GenerateCFG_ComplexProgram_CorrectStructure()
        {
            // a := 1
            // b := 2
            // c := a + b
            // d := c * 2
            // return d
            
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var assign1 = CreateAssignment("a", 1);
            var assign2 = CreateAssignment("b", 2);
            var assign3 = new AssignmentStmt(
                new VariableNode("c"),
                new PlusNode(new VariableNode("a"), new VariableNode("b"))
            );
            var assign4 = new AssignmentStmt(
                new VariableNode("d"),
                new TimesNode(new VariableNode("c"), new LiteralNode(2))
            );
            var returnStmt = CreateReturn("d");
            
            var block = new BlockStmt(new List<Statement> 
            { 
                assign1, assign2, assign3, assign4, returnStmt 
            });

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(5, cfg.VertexCount());
            Assert.Equal(4, cfg.EdgeCount());
            Assert.Equal(assign1, cfg.Start);
            
            var neighbors1 = cfg.GetNeighbors(assign1);
            Assert.Single(neighbors1);
            Assert.Contains(assign2, neighbors1);
            
            var neighbors4 = cfg.GetNeighbors(assign4);
            Assert.Single(neighbors4);
            Assert.Contains(returnStmt, neighbors4);
            
            var neighborsReturn = cfg.GetNeighbors(returnStmt);
            Assert.Empty(neighborsReturn); // Return should have no outgoing edges
        }

        [Fact]
        public void GenerateCFG_BlockWithOnlyBlocks_StartIsNull()
        {
            // { { } { } }
            
            // Arrange
            var visitor = new ControlFlowGraphGeneratorVisitor();
            var emptyBlock1 = new BlockStmt(new List<Statement>());
            var emptyBlock2 = new BlockStmt(new List<Statement>());
            var outerBlock = new BlockStmt(new List<Statement> { emptyBlock1, emptyBlock2 });

            // Act
            var cfg = visitor.GenerateCFG(outerBlock);

            // Assert
            Assert.Equal(0, cfg.VertexCount());
            Assert.Null(cfg.Start);
        }
    }
}
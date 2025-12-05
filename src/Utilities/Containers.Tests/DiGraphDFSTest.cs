using Xunit;
using AST;
using Optimizer;
using System.Collections.Generic;
using System.Linq;

namespace DiGraphDFSTests
{
    /// <summary>
    /// Test suite for Depth-First Search functionality on CFGs generated from DEC programs.
    /// Tests verify correct DFS traversal order, finishing times via stack ordering,
    /// and proper handling of various program structures.
    /// </summary>
    public class DiGraphDFSTests
    {
        private ControlFlowGraphGeneratorVisitor visitor;

        public DiGraphDFSTests()
        {
            visitor = new ControlFlowGraphGeneratorVisitor();
        }

        #region Basic DFS Tests

        /// <summary>
        /// Tests DFS on an empty program returns an empty stack.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_EmptyProgram_ReturnsEmptyStack()
        {
            // Arrange
            string program =
            @"{
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests DFS on a single statement program returns stack with that statement.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_SingleStatement_ReturnsStackWithStatement()
        {
            // Arrange
            string program =
            @"{
                x := 5
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Single(result);
            Assert.Equal(block.Statements[0], result.Pop());
        }

        /// <summary>
        /// Tests DFS on a single return statement.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_SingleReturn_ReturnsStackWithStatement()
        {
            // Arrange
            string program =
            @"{
                return 42
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Single(result);
            Assert.Equal(typeof(ReturnStmt), result.Peek()?.GetType() ?? typeof(ReturnStmt));
            Assert.Equal(block.Statements[0], result.Pop());
        }

        #endregion

        #region Linear Chain Tests

        /// <summary>
        /// Tests DFS on a linear chain of two assignments produces correct finishing order.
        /// In a chain A->B, B finishes first, then A.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_TwoSequentialAssignments_CorrectFinishingOrder()
        {
            // Arrange
            string program =
            @"{
                x := 5
                y := 10
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(2, result.Count);
            // Stack is in reverse finishing order (last finished on top)
            Assert.Equal(block.Statements[0], result.Pop());
            Assert.Equal(block.Statements[1], result.Pop());
        }

        /// <summary>
        /// Tests DFS on the example from Assignment PDF Figure 4.
        /// Program: x := 5, y := 10, z := (x + y), return z
        /// </summary>
        [Fact]
        public void DepthFirstSearch_ExampleFromPDF_Figure4()
        {
            // Arrange
            string program =
            @"{
                x := 5
                y := 10
                z := (x + y)
                return z
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(4, result.Count);
            // Verify all statements are visited
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(4, statements.Count);
            Assert.Contains(block.Statements[0], statements);
            Assert.Contains(block.Statements[1], statements);
            Assert.Contains(block.Statements[2], statements);
            Assert.Contains(block.Statements[3], statements);
        }

        /// <summary>
        /// Tests DFS on a long linear chain of assignments.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_MultipleAssignments_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                x := 0
                y := 1
                z := 2
                a := 3
                b := 4
                c := 5
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(6, result.Count);
            // Verify all statements visited
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(6, statements.Count);
        }

        /// <summary>
        /// Tests DFS on assignment followed by return statement.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_AssignmentThenReturn_BothStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                x := 5
                return x
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(block.Statements[0], result.Pop());
            Assert.Equal(block.Statements[1], result.Pop());
        }

        #endregion

        #region Nested Block Tests

        /// <summary>
        /// Tests DFS on nested blocks, verifying all non-block statements are visited.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_NestedBlocks_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                x := 2
                {
                    y := (2 * 4)
                    return (x + y)
                }
                z := 3
                return x
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(5, result.Count); // Only non-block statements
            // Verify all are unique statements
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(5, statements.Count);
        }

        /// <summary>
        /// Tests DFS on multiple nested blocks at the same level.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_MultipleNestedBlocks_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                {
                    x := 1
                }
                {
                    y := 2
                }
                z := 3
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(3, result.Count);
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(3, statements.Count);
        }

        /// <summary>
        /// Tests DFS on deeply nested blocks.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_DeeplyNestedBlocks_StatementVisited()
        {
            // Arrange
            string program =
            @"{
                {
                    {
                        x := 1
                    }
                }
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Single(result);
        }

        #endregion

        #region Return Statement Tests

        /// <summary>
        /// Tests DFS when return statement is followed by unreachable code.
        /// All statements should still be in the CFG and visited by DFS.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_ReturnFollowedByStatement_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                x := 5
                return x
                y := 10
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(3, result.Count);
            // All statements should be in the result (even unreachable ones)
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(3, statements.Count);
        }

        /// <summary>
        /// Tests DFS on multiple return statements.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_MultipleReturns_AllReturnsVisited()
        {
            // Arrange
            string program =
            @"{
                return 1
                return 2
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(2, result.Count);
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(2, statements.Count);
        }

        #endregion

        #region Complex Program Tests

        /// <summary>
        /// Tests DFS on a more complex program with multiple assignments and expressions.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_ComplexProgram_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := (a + b)
                d := (c * 2)
                return d
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(5, result.Count);
            // Verify finishing order: first statement finishes last
            var finishingOrder = new List<Statement>();
            while (result.Count > 0)
            {
                finishingOrder.Add(result.Pop());
            }
            Assert.Equal(block.Statements[0], finishingOrder[0]);
        }

        /// <summary>
        /// Tests DFS on a program with nested blocks and multiple statement types.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_MixedStatementsAndBlocks_CorrectTraversal()
        {
            // Arrange
            string program =
            @"{
                x := 1
                {
                    y := 2
                    z := (x + y)
                }
                return z
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(4, result.Count);
            var statements = new List<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            // Each statement should appear exactly once
            Assert.Equal(statements.Count, statements.Distinct().Count());
        }

        #endregion

        #region Finishing Order Tests

        /// <summary>
        /// Tests that DFS respects the finishing order property.
        /// In a linear chain, the first statement should finish last (be on top of stack).
        /// </summary>
        [Fact]
        public void DepthFirstSearch_LinearChain_FirstStatementFinishesLast()
        {
            // Arrange
            string program =
            @"{
                x := 1
                y := 2
                z := 3
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(3, result.Count);
            // First statement in program should be on top of stack (finished last)
            Assert.Equal(block.Statements[0], result.Pop());
        }

        /// <summary>
        /// Tests that each statement appears exactly once in DFS result.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_AnyProgram_EachStatementOnce()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := (a + 1)
                c := (b + 1)
                d := (c + 1)
                e := (d + 1)
                return e
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            var statements = new List<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            // Check no duplicates
            Assert.Equal(statements.Count, statements.Distinct().Count());
        }

        #endregion

        #region Edge Cases

        /// <summary>
        /// Tests DFS on a program with only nested empty blocks.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_OnlyEmptyBlocks_ReturnsEmptyStack()
        {
            // Arrange
            string program =
            @"{
                {
                }
                {
                }
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests DFS on a program with complex nested structure.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_ComplexNesting_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                a := 1
                {
                    b := 2
                    {
                        c := 3
                    }
                    d := 4
                }
                e := 5
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(5, result.Count);
            var statements = new HashSet<Statement>();
            while (result.Count > 0)
            {
                statements.Add(result.Pop());
            }
            Assert.Equal(5, statements.Count);
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Tests DFS on a realistic DEC program with multiple operations.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_RealisticProgram_AllStatementsVisited()
        {
            // Arrange
            string program =
            @"{
                x := 10
                y := 20
                sum := (x + y)
                product := (x * y)
                difference := (y - x)
                result := ((sum + product) - difference)
                return result
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            Assert.Equal(7, result.Count);
            // Verify start statement is on top (finished last)
            Assert.Equal(cfg.Start, result.Pop());
        }

        /// <summary>
        /// Tests DFS correctly handles a program where some statements are unreachable.
        /// Note: DFS visits all vertices in the graph, even if they're unreachable from Start.
        /// </summary>
        [Fact]
        public void DepthFirstSearch_UnreachableStatements_AllVerticesInGraph()
        {
            // Arrange
            string program =
            @"{
                x := 5
                return x
                y := 10
                z := 20
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);
            var result = cfg.DepthFirstSearch();

            // Assert
            // DFS visits all vertices in the graph structure
            Assert.Equal(cfg.VertexCount(), result.Count);
        }

        #endregion
    }
}
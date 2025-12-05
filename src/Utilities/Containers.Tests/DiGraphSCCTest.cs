using Xunit;
using AST;
using Optimizer;
using System.Collections.Generic;
using System.Linq;

namespace DiGraphSCCTests
{
    /// <summary>
    /// Test suite for FindStronglyConnectedComponents functionality on CFGs generated from DEC programs.
    /// Tests verify that SCCs are correctly identified using Kosaraju's algorithm.
    /// In DEC programs (linear CFGs), each statement should form its own singleton SCC.
    /// </summary>
    public class DiGraphSCCTests
    {
        private ControlFlowGraphGeneratorVisitor visitor;

        public DiGraphSCCTests()
        {
            visitor = new ControlFlowGraphGeneratorVisitor();
        }

        #region Basic SCC Tests

        /// <summary>
        /// Tests SCC on an empty program returns empty list.
        /// </summary>
        [Fact]
        public void FindSCC_EmptyProgram_ReturnsEmptyList()
        {
            // Arrange
            string program =
            @"{
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Empty(sccs);
        }

        /// <summary>
        /// Tests SCC on a single statement returns one singleton SCC.
        /// </summary>
        [Fact]
        public void FindSCC_SingleStatement_ReturnsOneSingletonSCC()
        {
            // Arrange
            string program =
            @"{
                x := 5
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Single(sccs);
            Assert.Single(sccs[0]);
            Assert.Contains(block.Statements[0], sccs[0]);
        }

        /// <summary>
        /// Tests SCC on a single return statement.
        /// </summary>
        [Fact]
        public void FindSCC_SingleReturn_ReturnsOneSingletonSCC()
        {
            // Arrange
            string program =
            @"{
                return 42
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Single(sccs);
            Assert.Single(sccs[0]);
            Assert.Contains(block.Statements[0], sccs[0]);
        }

        #endregion

        #region Linear Chain Tests (Assignment Requirement)

        /// <summary>
        /// Tests SCC on two sequential assignments.
        /// Linear CFG: each statement is its own singleton SCC.
        /// </summary>
        [Fact]
        public void FindSCC_TwoSequentialAssignments_TwoSingletonSCCs()
        {
            // Arrange
            string program =
            @"{
                x := 5
                y := 10
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(2, sccs.Count);
            // Each SCC should be a singleton
            Assert.All(sccs, scc => Assert.Single(scc));
            // Verify all statements are accounted for
            var allStatements = sccs.SelectMany(scc => scc).ToHashSet();
            Assert.Equal(2, allStatements.Count);
        }

        /// <summary>
        /// Tests SCC on the example from Assignment PDF Figure 4.
        /// Program: x := 5, y := 10, z := (x + y), return z
        /// This is a linear CFG, so there should be 4 singleton SCCs.
        /// </summary>
        [Fact]
        public void FindSCC_ExampleFromPDF_Figure4_FourSingletonSCCs()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, sccs.Count);
            // Each SCC should be a singleton
            Assert.All(sccs, scc => Assert.Single(scc));
            // Verify all statements are present
            var allStatements = sccs.SelectMany(scc => scc).ToHashSet();
            Assert.Contains(block.Statements[0], allStatements);
            Assert.Contains(block.Statements[1], allStatements);
            Assert.Contains(block.Statements[2], allStatements);
            Assert.Contains(block.Statements[3], allStatements);
        }

        /// <summary>
        /// Tests SCC on a long linear chain of assignments.
        /// Each assignment should be its own singleton SCC.
        /// </summary>
        [Fact]
        public void FindSCC_MultipleAssignments_EachStatementIsSingletonSCC()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(6, sccs.Count);
            // Each SCC should be a singleton
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests SCC on assignment followed by return statement.
        /// </summary>
        [Fact]
        public void FindSCC_AssignmentThenReturn_TwoSingletonSCCs()
        {
            // Arrange
            string program =
            @"{
                x := 5
                return x
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(2, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        #endregion

        #region Nested Block Tests

        /// <summary>
        /// Tests SCC on nested blocks.
        /// All non-block statements should form singleton SCCs.
        /// </summary>
        [Fact]
        public void FindSCC_NestedBlocks_EachStatementIsSingletonSCC()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(5, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests SCC on multiple nested blocks at the same level.
        /// </summary>
        [Fact]
        public void FindSCC_MultipleNestedBlocks_ThreeSingletonSCCs()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(3, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests SCC on deeply nested blocks.
        /// </summary>
        [Fact]
        public void FindSCC_DeeplyNestedBlocks_OneSingletonSCC()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Single(sccs);
            Assert.Single(sccs[0]);
        }

        #endregion

        #region Unreachable Statement Tests

        /// <summary>
        /// Tests SCC on program with unreachable statements.
        /// All statements (reachable and unreachable) should form singleton SCCs.
        /// </summary>
        [Fact]
        public void FindSCC_ReturnFollowedByStatement_AllStatementsInSCCs()
        {
            // Arrange
            string program =
            @"{
                x := 5
                return x
                y := 10
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(3, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
            // Verify all statements are in some SCC
            var allStatements = sccs.SelectMany(scc => scc).ToHashSet();
            Assert.Contains(block.Statements[0], allStatements);
            Assert.Contains(block.Statements[1], allStatements);
            Assert.Contains(block.Statements[2], allStatements);
        }

        /// <summary>
        /// Tests SCC on multiple return statements (disconnected vertices).
        /// Each should be its own singleton SCC.
        /// </summary>
        [Fact]
        public void FindSCC_MultipleReturns_TwoSingletonSCCs()
        {
            // Arrange
            string program =
            @"{
                return 1
                return 2
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(2, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        #endregion

        #region Complex Program Tests

        /// <summary>
        /// Tests SCC on a complex program with multiple operations.
        /// Linear structure means each statement is a singleton SCC.
        /// </summary>
        [Fact]
        public void FindSCC_ComplexProgram_FiveSingletonSCCs()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(5, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests SCC on a realistic DEC program.
        /// </summary>
        [Fact]
        public void FindSCC_RealisticProgram_SevenSingletonSCCs()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(7, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        #endregion

        #region Verification Tests (Assignment Requirement)

        /// <summary>
        /// Tests that the number of SCCs equals the number of statements in a linear CFG.
        /// This verifies the linear nature of DEC programs as stated in the assignment.
        /// </summary>
        [Fact]
        public void FindSCC_LinearProgram_SCCCountEqualsStatementCount()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := 3
                d := 4
                e := 5
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            // Number of SCCs should equal number of statements
            Assert.Equal(block.Statements.Count, sccs.Count);
            Assert.Equal(cfg.VertexCount(), sccs.Count);
        }

        /// <summary>
        /// Tests that each statement appears in exactly one SCC.
        /// </summary>
        [Fact]
        public void FindSCC_AnyProgram_EachStatementInExactlyOneSCC()
        {
            // Arrange
            string program =
            @"{
                x := 1
                y := 2
                z := (x + y)
                return z
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            // Collect all statements from all SCCs
            var allStatements = sccs.SelectMany(scc => scc).ToList();
            // Each statement should appear exactly once
            Assert.Equal(allStatements.Count, allStatements.Distinct().Count());
            // All statements should be accounted for
            Assert.Equal(cfg.VertexCount(), allStatements.Count);
        }

        /// <summary>
        /// Tests that for assignment and return statements in DEC programs,
        /// the number of SCCs equals the total number of these statements.
        /// This directly tests the assignment requirement from the PDF.
        /// </summary>
        [Fact]
        public void FindSCC_AssignmentsAndReturns_SCCCountMatchesStatementCount()
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
            var cfg = visitor.GenerateCFG(block);

            // Count assignment and return statements
            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);
            int totalStatements = assignmentCount + returnCount;

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            // Number of SCCs should equal total number of assignment + return statements
            Assert.Equal(totalStatements, sccs.Count);
        }

        #endregion

        #region Edge Case Tests

        /// <summary>
        /// Tests SCC on program with only empty blocks.
        /// </summary>
        [Fact]
        public void FindSCC_OnlyEmptyBlocks_ReturnsEmptyList()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Empty(sccs);
        }

        /// <summary>
        /// Tests SCC on a program with complex nested structure.
        /// </summary>
        [Fact]
        public void FindSCC_ComplexNesting_AllSingletonSCCs()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(5, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        #endregion

        #region SCC Properties Tests

        /// <summary>
        /// Tests that no SCC is empty.
        /// </summary>
        [Fact]
        public void FindSCC_AnyProgram_NoEmptySCCs()
        {
            // Arrange
            string program =
            @"{
                x := 1
                y := 2
                z := 3
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.All(sccs, scc => Assert.NotEmpty(scc));
        }

        /// <summary>
        /// Tests that in a linear CFG, all SCCs are singletons.
        /// This is a fundamental property of DEC programs.
        /// </summary>
        [Fact]
        public void FindSCC_LinearCFG_AllSCCsAreSingletons()
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
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            // Every SCC should have exactly one statement
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests that the union of all SCCs equals the set of all vertices.
        /// </summary>
        [Fact]
        public void FindSCC_AnyProgram_SCCUnionEqualsVertexSet()
        {
            // Arrange
            string program =
            @"{
                x := 1
                y := 2
                z := 3
                w := 4
                return w
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);
            var allVertices = cfg.GetVertices().ToHashSet();

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            var allSCCStatements = sccs.SelectMany(scc => scc).ToHashSet();
            Assert.True(allVertices.SetEquals(allSCCStatements));
        }

        /// <summary>
        /// Tests that SCCs are disjoint (no statement appears in multiple SCCs).
        /// </summary>
        [Fact]
        public void FindSCC_AnyProgram_SCCsAreDisjoint()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := 3
                return c
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            var allStatements = sccs.SelectMany(scc => scc).ToList();
            var uniqueStatements = allStatements.Distinct().ToList();
            // If SCCs are disjoint, count should equal unique count
            Assert.Equal(allStatements.Count, uniqueStatements.Count);
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Tests that FindSCC works correctly after using DFS.
        /// </summary>
        [Fact]
        public void FindSCC_AfterDFS_ProducesCorrectResults()
        {
            // Arrange
            string program =
            @"{
                x := 1
                y := 2
                z := 3
                return z
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var dfsResult = cfg.DepthFirstSearch(); // Run DFS first
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests that FindSCC works correctly after using Transpose.
        /// </summary>
        [Fact]
        public void FindSCC_AfterTranspose_ProducesCorrectResults()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := 3
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var transposed = cfg.Transpose(); // Transpose first
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(3, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests the complete Kosaraju algorithm workflow.
        /// </summary>
        [Fact]
        public void FindSCC_CompleteKosarajuWorkflow_CorrectResults()
        {
            // Arrange
            string program =
            @"{
                x := 10
                y := 20
                sum := (x + y)
                return sum
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act - This internally uses DFS, Transpose, and DFS again
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
            
            // Verify all statements are accounted for
            var allSCCStatements = sccs.SelectMany(scc => scc).ToHashSet();
            Assert.Equal(cfg.VertexCount(), allSCCStatements.Count);
        }

        #endregion

        #region Mixed Statement Types Tests

        /// <summary>
        /// Tests SCC on program with mixed assignment and return statements.
        /// </summary>
        [Fact]
        public void FindSCC_MixedStatementTypes_EachFormsOwnSCC()
        {
            // Arrange
            string program =
            @"{
                x := 1
                return x
                y := 2
                return y
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests SCC on program with only assignments (no return).
        /// </summary>
        [Fact]
        public void FindSCC_OnlyAssignments_AllSingletonSCCs()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := 3
                d := 4
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        /// <summary>
        /// Tests SCC on program with nested blocks containing mixed statement types.
        /// </summary>
        [Fact]
        public void FindSCC_NestedMixedStatements_AllSingletonSCCs()
        {
            // Arrange
            string program =
            @"{
                x := 1
                {
                    y := 2
                    return y
                }
                z := 3
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, sccs.Count);
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        #endregion
    }
}
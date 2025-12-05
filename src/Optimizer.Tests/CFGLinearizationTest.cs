using Xunit;
using AST;
using Optimizer;
using System.Collections.Generic;
using System.Linq;

namespace CFGLinearizationTests
{
    /// <summary>
    /// Test suite for verifying the linear nature of DEC programs through SCC analysis.
    /// As stated in the assignment: "verify that the number of SCCs in a CFG representing 
    /// a DEC program corresponds directly to the total number of assignment statements and 
    /// return statements."
    /// </summary>
    public class CFGLinearizationTests
    {
        private ControlFlowGraphGeneratorVisitor visitor;

        public CFGLinearizationTests()
        {
            visitor = new ControlFlowGraphGeneratorVisitor();
        }

        #region PDF Example Tests

        /// <summary>
        /// Tests the exact example from the Assignment PDF Figure 4.
        /// Program: x := 5, y := 10, z := (x + y), return z
        /// This creates a linear CFG: [x := 5] → [y := 10] → [z := x + y] → [return z]
        /// As there are 4 statements, there will be 4 SCCs computed.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_PDFExample_FourStatementsEqualsFourSCCs()
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
            int statementCount = block.Statements.Count;
            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            // As there are 4 statements (3 assignments + 1 return), there will be 4 SCCs
            Assert.Equal(4, statementCount);
            Assert.Equal(3, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(4, sccs.Count);
            
            // Verify number of SCCs equals total number of assignment + return statements
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
            
            // Verify each SCC is a singleton (linear CFG property)
            Assert.All(sccs, scc => Assert.Single(scc));
        }

        #endregion

        #region Basic Linearization Tests

        /// <summary>
        /// Tests that a single assignment statement results in exactly one SCC.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_SingleAssignment_OneSCC()
        {
            // Arrange
            string program =
            @"{
                x := 5
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(1, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(1, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests that a single return statement results in exactly one SCC.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_SingleReturn_OneSCC()
        {
            // Arrange
            string program =
            @"{
                return 42
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(0, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(1, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests that two sequential assignments result in exactly two SCCs.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_TwoAssignments_TwoSCCs()
        {
            // Arrange
            string program =
            @"{
                x := 5
                y := 10
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(2, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(2, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests that one assignment followed by one return results in exactly two SCCs.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_AssignmentAndReturn_TwoSCCs()
        {
            // Arrange
            string program =
            @"{
                x := 5
                return x
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(1, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(2, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        #endregion

        #region Multiple Statement Tests

        /// <summary>
        /// Tests that five assignments result in exactly five SCCs.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_FiveAssignments_FiveSCCs()
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

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(5, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(5, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests that four assignments and one return result in exactly five SCCs.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_FourAssignmentsOneReturn_FiveSCCs()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := 3
                d := 4
                return d
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(5, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests a long linear chain of ten assignments.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_TenAssignments_TenSCCs()
        {
            // Arrange
            string program =
            @"{
                a := 0
                b := 1
                c := 2
                d := 3
                e := 4
                f := 5
                g := 6
                h := 7
                i := 8
                j := 9
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(10, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(10, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        #endregion

        #region Nested Block Tests

        /// <summary>
        /// Tests that nested blocks are flattened and SCCs correspond to statement count.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_NestedBlocks_SCCsEqualStatements()
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

            // Count non-block statements recursively
            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(3, assignmentCount);
            Assert.Equal(2, returnCount);
            Assert.Equal(5, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests multiple nested blocks at the same level.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_MultipleNestedBlocks_SCCsEqualStatements()
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

            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(3, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(3, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests deeply nested blocks.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_DeeplyNestedBlocks_SCCsEqualStatements()
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

            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(1, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(1, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests complex nested structure with mixed statement types.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_ComplexNesting_SCCsEqualStatements()
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

            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(5, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(5, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        #endregion

        #region Unreachable Code Tests

        /// <summary>
        /// Tests that unreachable statements are still counted in SCCs.
        /// In a linear CFG representation, even unreachable code forms singleton SCCs.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_UnreachableAfterReturn_SCCsIncludeAllStatements()
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

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(2, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(3, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests multiple returns (creating unreachable code).
        /// </summary>
        [Fact]
        public void VerifyLinearNature_MultipleReturns_SCCsEqualStatements()
        {
            // Arrange
            string program =
            @"{
                return 1
                return 2
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(0, assignmentCount);
            Assert.Equal(2, returnCount);
            Assert.Equal(2, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests unreachable code after return within nested blocks.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_UnreachableInNestedBlock_SCCsEqualStatements()
        {
            // Arrange
            string program =
            @"{
                x := 1
                {
                    return x
                    y := 2
                }
                z := 3
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(3, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(4, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        #endregion

        #region Complex Program Tests

        /// <summary>
        /// Tests a realistic DEC program with various expressions.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_RealisticProgram_SCCsEqualStatements()
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

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(6, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(7, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests a program with complex arithmetic expressions.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_ComplexExpressions_SCCsEqualStatements()
        {
            // Arrange
            string program =
            @"{
                a := 2
                b := 3
                c := ((a ** b) + (a * b))
                d := ((c / 2) - (a % b))
                return d
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(5, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests a program with nested blocks and complex expressions.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_NestedWithExpressions_SCCsEqualStatements()
        {
            // Arrange
            string program =
            @"{
                x := 5
                {
                    y := (x * 2)
                    z := (y + x)
                    {
                        result := ((z - y) + x)
                        return result
                    }
                }
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(4, assignmentCount);
            Assert.Equal(1, returnCount);
            Assert.Equal(5, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        #endregion

        #region Edge Cases

        /// <summary>
        /// Tests empty program (no statements).
        /// </summary>
        [Fact]
        public void VerifyLinearNature_EmptyProgram_ZeroSCCs()
        {
            // Arrange
            string program =
            @"{
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            int assignmentCount = block.Statements.Count(s => s is AssignmentStmt);
            int returnCount = block.Statements.Count(s => s is ReturnStmt);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(0, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(0, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        /// <summary>
        /// Tests program with only empty nested blocks.
        /// </summary>
        [Fact]
        public void VerifyLinearNature_OnlyEmptyBlocks_ZeroSCCs()
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

            int assignmentCount = CountStatements<AssignmentStmt>(block);
            int returnCount = CountStatements<ReturnStmt>(block);

            // Act
            var sccs = cfg.FindStronglyConnectedComponents();

            // Assert
            Assert.Equal(0, assignmentCount);
            Assert.Equal(0, returnCount);
            Assert.Equal(0, sccs.Count);
            Assert.Equal(assignmentCount + returnCount, sccs.Count);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Recursively counts statements of a specific type in a block and its nested blocks.
        /// </summary>
        /// <typeparam name="T">The type of statement to count (AssignmentStmt or ReturnStmt).</typeparam>
        /// <param name="block">The block statement to search.</param>
        /// <returns>The total count of statements of type T.</returns>
        private int CountStatements<T>(BlockStmt block) where T : Statement
        {
            int count = 0;

            foreach (var statement in block.Statements)
            {
                if (statement is T)
                {
                    count++;
                }
                else if (statement is BlockStmt nestedBlock)
                {
                    count += CountStatements<T>(nestedBlock);
                }
            }

            return count;
        }

        #endregion
    }
}
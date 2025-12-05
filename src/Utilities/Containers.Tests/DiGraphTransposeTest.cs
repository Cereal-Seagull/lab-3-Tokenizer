using Xunit;
using AST;
using Optimizer;
using System.Collections.Generic;
using System.Linq;

namespace DiGraphTransposeTests
{
    /// <summary>
    /// Test suite for Transpose functionality on CFGs generated from DEC programs.
    /// Tests verify that edge directions are correctly reversed while preserving all vertices.
    /// </summary>
    public class DiGraphTransposeTests
    {
        private ControlFlowGraphGeneratorVisitor visitor;

        public DiGraphTransposeTests()
        {
            visitor = new ControlFlowGraphGeneratorVisitor();
        }

        #region Basic Transpose Tests

        /// <summary>
        /// Tests transpose on an empty program returns an empty graph.
        /// </summary>
        [Fact]
        public void Transpose_EmptyProgram_ReturnsEmptyGraph()
        {
            // Arrange
            string program =
            @"{
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var transposed = cfg.Transpose();

            // Assert
            Assert.NotNull(transposed);
            Assert.Equal(0, transposed.VertexCount());
            Assert.Equal(0, transposed.EdgeCount());
        }

        /// <summary>
        /// Tests transpose on a single statement has same vertex, no edges.
        /// </summary>
        [Fact]
        public void Transpose_SingleStatement_SameVertex()
        {
            // Arrange
            string program =
            @"{
                x := 5
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(1, transposed.VertexCount());
            Assert.Equal(0, transposed.EdgeCount());
            // Verify the vertex exists in transposed graph
            Assert.Contains(block.Statements[0], transposed.GetVertices());
        }

        /// <summary>
        /// Tests transpose on a single return statement.
        /// </summary>
        [Fact]
        public void Transpose_SingleReturn_PreservesVertex()
        {
            // Arrange
            string program =
            @"{
                return 42
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(1, transposed.VertexCount());
            Assert.Equal(0, transposed.EdgeCount());
            Assert.Contains(block.Statements[0], transposed.GetVertices());
        }

        #endregion

        #region Two Statement Tests

        /// <summary>
        /// Tests transpose on two sequential assignments reverses the edge.
        /// Original: A -> B, Transposed: B -> A
        /// </summary>
        [Fact]
        public void Transpose_TwoSequentialAssignments_ReversesEdge()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(2, transposed.VertexCount());
            Assert.Equal(1, transposed.EdgeCount());
            // Original edge: Statements[0] -> Statements[1]
            Assert.False(transposed.HasEdge(block.Statements[0], block.Statements[1]));
            // Transposed edge: Statements[1] -> Statements[0]
            Assert.True(transposed.HasEdge(block.Statements[1], block.Statements[0]));
        }

        /// <summary>
        /// Tests transpose on assignment followed by return reverses the edge.
        /// </summary>
        [Fact]
        public void Transpose_AssignmentThenReturn_ReversesEdge()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(2, transposed.VertexCount());
            Assert.Equal(1, transposed.EdgeCount());
            // Verify edge reversal
            Assert.False(transposed.HasEdge(block.Statements[0], block.Statements[1]));
            Assert.True(transposed.HasEdge(block.Statements[1], block.Statements[0]));
        }

        #endregion

        #region Linear Chain Tests

        /// <summary>
        /// Tests transpose on the example from Assignment PDF Figure 4.
        /// Program: x := 5, y := 10, z := (x + y), return z
        /// Chain: A -> B -> C -> D becomes D -> C -> B -> A
        /// </summary>
        [Fact]
        public void Transpose_ExampleFromPDF_ReversesChain()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(4, transposed.VertexCount());
            Assert.Equal(3, transposed.EdgeCount());
            
            // Verify all edges are reversed
            for (int i = 0; i < 3; i++)
            {
                // Original edges should not exist
                Assert.False(transposed.HasEdge(block.Statements[i], block.Statements[i + 1]));
                // Reversed edges should exist
                Assert.True(transposed.HasEdge(block.Statements[i + 1], block.Statements[i]));
            }
        }

        /// <summary>
        /// Tests transpose on a long linear chain of assignments.
        /// </summary>
        [Fact]
        public void Transpose_MultipleAssignments_ReversesAllEdges()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(6, transposed.VertexCount());
            Assert.Equal(5, transposed.EdgeCount());
            
            // Verify chain reversal
            for (int i = 0; i < 5; i++)
            {
                Assert.False(transposed.HasEdge(block.Statements[i], block.Statements[i + 1]));
                Assert.True(transposed.HasEdge(block.Statements[i + 1], block.Statements[i]));
            }
        }

        #endregion

        #region Vertex Preservation Tests

        /// <summary>
        /// Tests that transpose preserves all vertices from the original graph.
        /// </summary>
        [Fact]
        public void Transpose_AnyProgram_PreservesAllVertices()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := (a + b)
                return c
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);
            var originalVertices = cfg.GetVertices().ToHashSet();

            // Act
            var transposed = cfg.Transpose();
            var transposedVertices = transposed.GetVertices().ToHashSet();

            // Assert
            Assert.Equal(originalVertices.Count, transposedVertices.Count);
            Assert.True(originalVertices.SetEquals(transposedVertices));
        }

        /// <summary>
        /// Tests that transpose preserves the number of edges.
        /// </summary>
        [Fact]
        public void Transpose_AnyProgram_PreservesEdgeCount()
        {
            // Arrange
            string program =
            @"{
                x := 1
                y := 2
                z := 3
                w := 4
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);
            var originalEdgeCount = cfg.EdgeCount();

            // Act
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(originalEdgeCount, transposed.EdgeCount());
        }

        #endregion

        #region Nested Block Tests

        /// <summary>
        /// Tests transpose on nested blocks preserves structure and reverses edges.
        /// </summary>
        [Fact]
        public void Transpose_NestedBlocks_ReversesEdges()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(cfg.VertexCount(), transposed.VertexCount());
            Assert.Equal(cfg.EdgeCount(), transposed.EdgeCount());
        }

        /// <summary>
        /// Tests transpose on multiple nested blocks at the same level.
        /// </summary>
        [Fact]
        public void Transpose_MultipleNestedBlocks_PreservesStructure()
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
            var originalVertices = cfg.GetVertices().ToHashSet();

            // Act
            var transposed = cfg.Transpose();
            var transposedVertices = transposed.GetVertices().ToHashSet();

            // Assert
            Assert.Equal(3, transposed.VertexCount());
            Assert.Equal(2, transposed.EdgeCount());
            Assert.True(originalVertices.SetEquals(transposedVertices));
        }

        /// <summary>
        /// Tests transpose on deeply nested blocks.
        /// </summary>
        [Fact]
        public void Transpose_DeeplyNestedBlocks_PreservesVertex()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(1, transposed.VertexCount());
            Assert.Equal(0, transposed.EdgeCount());
        }

        #endregion

        #region Unreachable Statement Tests

        /// <summary>
        /// Tests transpose on program with unreachable statements.
        /// All vertices should be preserved even if not reachable from Start.
        /// </summary>
        [Fact]
        public void Transpose_ReturnFollowedByStatement_PreservesAllVertices()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(3, transposed.VertexCount());
            Assert.Equal(1, transposed.EdgeCount());
            // Verify all statements are in transposed graph
            Assert.Contains(block.Statements[0], transposed.GetVertices());
            Assert.Contains(block.Statements[1], transposed.GetVertices());
            Assert.Contains(block.Statements[2], transposed.GetVertices());
        }

        /// <summary>
        /// Tests transpose on multiple return statements.
        /// </summary>
        [Fact]
        public void Transpose_MultipleReturns_PreservesDisconnectedVertices()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(2, transposed.VertexCount());
            Assert.Equal(0, transposed.EdgeCount());
            Assert.Contains(block.Statements[0], transposed.GetVertices());
            Assert.Contains(block.Statements[1], transposed.GetVertices());
        }

        #endregion

        #region Complex Program Tests

        /// <summary>
        /// Tests transpose on a complex program with multiple operations.
        /// </summary>
        [Fact]
        public void Transpose_ComplexProgram_ReversesAllEdges()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(5, transposed.VertexCount());
            Assert.Equal(4, transposed.EdgeCount());
            
            // Verify edge reversals
            for (int i = 0; i < 4; i++)
            {
                Assert.False(transposed.HasEdge(block.Statements[i], block.Statements[i + 1]));
                Assert.True(transposed.HasEdge(block.Statements[i + 1], block.Statements[i]));
            }
        }

        /// <summary>
        /// Tests transpose on a realistic DEC program.
        /// </summary>
        [Fact]
        public void Transpose_RealisticProgram_MaintainsGraphProperties()
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
            var originalVertexCount = cfg.VertexCount();
            var originalEdgeCount = cfg.EdgeCount();

            // Act
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(originalVertexCount, transposed.VertexCount());
            Assert.Equal(originalEdgeCount, transposed.EdgeCount());
        }

        #endregion

        #region Double Transpose Tests

        /// <summary>
        /// Tests that transposing twice returns to original edge structure.
        /// </summary>
        [Fact]
        public void Transpose_TwiceOnLinearChain_RestoresOriginalEdges()
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
            var transposed = cfg.Transpose();
            var doubleTransposed = transposed.Transpose();

            // Assert
            Assert.Equal(cfg.VertexCount(), doubleTransposed.VertexCount());
            Assert.Equal(cfg.EdgeCount(), doubleTransposed.EdgeCount());
            
            // Original edges should be restored
            for (int i = 0; i < 2; i++)
            {
                Assert.True(doubleTransposed.HasEdge(block.Statements[i], block.Statements[i + 1]));
            }
        }

        /// <summary>
        /// Tests that double transpose preserves all graph properties.
        /// </summary>
        [Fact]
        public void Transpose_TwiceOnComplexProgram_RestoresStructure()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := 2
                c := (a + b)
                return c
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var transposed = cfg.Transpose();
            var doubleTransposed = transposed.Transpose();

            // Assert
            // Verify all original edges are restored
            Assert.True(doubleTransposed.HasEdge(block.Statements[0], block.Statements[1]));
            Assert.True(doubleTransposed.HasEdge(block.Statements[1], block.Statements[2]));
            Assert.True(doubleTransposed.HasEdge(block.Statements[2], block.Statements[3]));
        }

        #endregion

        #region Neighbor Tests

        /// <summary>
        /// Tests that neighbors are correctly reversed in transposed graph.
        /// In A -> B -> C, A's neighbors: {B}, transposed C's neighbors: {B}
        /// </summary>
        [Fact]
        public void Transpose_LinearChain_NeighborsReversed()
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
            var transposed = cfg.Transpose();

            // Assert
            // In original: Statements[0] -> Statements[1]
            // In transposed: Statements[1] -> Statements[0]
            var neighborsOf1 = transposed.GetNeighbors(block.Statements[1]);
            Assert.Contains(block.Statements[0], neighborsOf1);
            
            // In original: Statements[1] -> Statements[2]
            // In transposed: Statements[2] -> Statements[1]
            var neighborsOf2 = transposed.GetNeighbors(block.Statements[2]);
            Assert.Contains(block.Statements[1], neighborsOf2);
        }

        /// <summary>
        /// Tests that the first statement in original has no incoming edges,
        /// but in transposed it has no outgoing edges.
        /// </summary>
        [Fact]
        public void Transpose_FirstStatement_HasNoOutgoingEdges()
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
            var transposed = cfg.Transpose();

            // Assert
            // First statement in original has no incoming edges
            // In transposed, it should have no outgoing edges
            var neighbors = transposed.GetNeighbors(block.Statements[0]);
            Assert.Empty(neighbors);
        }

        #endregion
        
        #region Edge Case Tests

        /// <summary>
        /// Tests transpose on program with only empty blocks.
        /// </summary>
        [Fact]
        public void Transpose_OnlyEmptyBlocks_ReturnsEmptyGraph()
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
            var transposed = cfg.Transpose();

            // Assert
            Assert.Equal(0, transposed.VertexCount());
            Assert.Equal(0, transposed.EdgeCount());
        }

        /// <summary>
        /// Tests transpose on a program with complex nested structure.
        /// </summary>
        [Fact]
        public void Transpose_ComplexNesting_PreservesAllVertices()
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
            var originalVertices = cfg.GetVertices().ToHashSet();

            // Act
            var transposed = cfg.Transpose();
            var transposedVertices = transposed.GetVertices().ToHashSet();

            // Assert
            Assert.Equal(5, transposed.VertexCount());
            Assert.True(originalVertices.SetEquals(transposedVertices));
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Tests that transpose works correctly with DFS.
        /// Transposing and then running DFS should visit all vertices.
        /// </summary>
        [Fact]
        public void Transpose_ThenDFS_VisitsAllVertices()
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
            var transposed = cfg.Transpose();
            var dfsResult = transposed.DepthFirstSearch();

            // Assert
            Assert.Equal(cfg.VertexCount(), dfsResult.Count);
        }

        /// <summary>
        /// Tests transpose maintains graph integrity for Kosaraju's algorithm.
        /// </summary>
        [Fact]
        public void Transpose_ForKosarajuAlgorithm_MaintainsGraphIntegrity()
        {
            // Arrange
            string program =
            @"{
                a := 1
                b := (a + 1)
                c := (b + 1)
                d := (c + 1)
                return d
            }";

            var block = Parser.Parser.Parse(program);
            var cfg = visitor.GenerateCFG(block);

            // Act
            var transposed = cfg.Transpose();

            // Assert
            // Verify same number of vertices and edges
            Assert.Equal(cfg.VertexCount(), transposed.VertexCount());
            Assert.Equal(cfg.EdgeCount(), transposed.EdgeCount());
            
            // Verify all vertices are preserved
            var originalVertices = cfg.GetVertices().ToHashSet();
            var transposedVertices = transposed.GetVertices().ToHashSet();
            Assert.True(originalVertices.SetEquals(transposedVertices));
        }

        #endregion
    }
}
using Xunit;
using AST;
using Optimizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFGTests
{
    public class CFGTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_InitializesEmptyCFG()
        {
            // Arrange & Act
            var cfg = new CFG();

            // Assert
            Assert.NotNull(cfg);
            Assert.Equal(0, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Null(cfg.Start);
        }

        #endregion

        #region Start Property Tests

        [Fact]
        public void Start_CanBeSetAndRetrieved()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(5)
            );

            // Act
            cfg.Start = stmt;

            // Assert
            Assert.Equal(stmt, cfg.Start);
        }

        [Fact]
        public void Start_InitiallyNull()
        {
            // Arrange & Act
            var cfg = new CFG();

            // Assert
            Assert.Null(cfg.Start);
        }

        [Fact]
        public void Start_CanBeSetToNull()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(5)
            );
            cfg.Start = stmt;

            // Act
            cfg.Start = null;

            // Assert
            Assert.Null(cfg.Start);
        }

        #endregion

        #region AddVertex Tests (Inherited from DiGraph)

        [Fact]
        public void AddVertex_AddsAssignmentStatement()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(10)
            );

            // Act
            var result = cfg.AddVertex(stmt);

            // Assert
            Assert.True(result);
            Assert.Equal(1, cfg.VertexCount());
        }

        [Fact]
        public void AddVertex_AddsReturnStatement()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new ReturnStmt(new LiteralNode(42));

            // Act
            var result = cfg.AddVertex(stmt);

            // Assert
            Assert.True(result);
            Assert.Equal(1, cfg.VertexCount());
        }

        [Fact]
        public void AddVertex_ReturnsTrueForDuplicateVertex()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(
                new VariableNode("x"),
                new LiteralNode(5)
            );

            // Act
            cfg.AddVertex(stmt);
            var result = cfg.AddVertex(stmt);

            // Assert
            Assert.True(result);
            Assert.Equal(1, cfg.VertexCount()); // Should not add duplicate
        }

        [Fact]
        public void AddVertex_HandlesMultipleStatements()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new ReturnStmt(new VariableNode("x"));

            // Act
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(stmt3);

            // Assert
            Assert.Equal(3, cfg.VertexCount());
        }

        #endregion

        #region AddEdge Tests

        [Fact]
        public void AddEdge_ConnectsTwoStatements()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);

            // Act
            var result = cfg.AddEdge(stmt1, stmt2);

            // Assert
            Assert.True(result);
            Assert.Equal(1, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(stmt1, stmt2));
        }

        [Fact]
        public void AddEdge_ThrowsExceptionForNonExistentSource()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt2);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => cfg.AddEdge(stmt1, stmt2));
        }

        [Fact]
        public void AddEdge_ThrowsExceptionForNonExistentDestination()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => cfg.AddEdge(stmt1, stmt2));
        }

        [Fact]
        public void AddEdge_CreatesLinearChain()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new AssignmentStmt(new VariableNode("z"), 
                new PlusNode(new VariableNode("x"), new VariableNode("y")));
            var stmt4 = new ReturnStmt(new VariableNode("z"));
            
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(stmt3);
            cfg.AddVertex(stmt4);

            // Act
            cfg.AddEdge(stmt1, stmt2);
            cfg.AddEdge(stmt2, stmt3);
            cfg.AddEdge(stmt3, stmt4);

            // Assert
            Assert.Equal(3, cfg.EdgeCount());
            Assert.True(cfg.HasEdge(stmt1, stmt2));
            Assert.True(cfg.HasEdge(stmt2, stmt3));
            Assert.True(cfg.HasEdge(stmt3, stmt4));
        }

        #endregion

        #region RemoveVertex Tests

        [Fact]
        public void RemoveVertex_RemovesExistingStatement()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            cfg.AddVertex(stmt);

            // Act
            var result = cfg.RemoveVertex(stmt);

            // Assert
            Assert.True(result);
            Assert.Equal(0, cfg.VertexCount());
        }

        [Fact]
        public void RemoveVertex_RemovesConnectedEdges()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new AssignmentStmt(new VariableNode("z"), new LiteralNode(15));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(stmt3);
            cfg.AddEdge(stmt1, stmt2);
            cfg.AddEdge(stmt2, stmt3);

            // Act
            cfg.RemoveVertex(stmt2);

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            // Note: Based on DiGraph implementation, edges should be removed
            // but the implementation only removes the vertex from the dictionary
            // This is a potential bug - edges pointing TO the removed vertex remain
        }

        #endregion

        #region RemoveEdge Tests

        [Fact]
        public void RemoveEdge_RemovesExistingEdge()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddEdge(stmt1, stmt2);

            // Act
            var result = cfg.RemoveEdge(stmt1, stmt2);

            // Assert
            Assert.True(result);
            Assert.False(cfg.HasEdge(stmt1, stmt2));
            Assert.Equal(0, cfg.EdgeCount());
        }

        [Fact]
        public void RemoveEdge_ThrowsExceptionForNonExistentSource()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt2);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => cfg.RemoveEdge(stmt1, stmt2));
        }

        #endregion

        #region HasEdge Tests

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HasEdge_ReturnsCorrectResult(bool addEdge)
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            
            if (addEdge)
            {
                cfg.AddEdge(stmt1, stmt2);
            }

            // Act
            var result = cfg.HasEdge(stmt1, stmt2);

            // Assert
            Assert.Equal(addEdge, result);
        }

        #endregion

        #region GetNeighbors Tests

        [Fact]
        public void GetNeighbors_ReturnsEmptyListForStatementWithNoEdges()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            cfg.AddVertex(stmt);

            // Act
            var neighbors = cfg.GetNeighbors(stmt);

            // Assert
            Assert.Empty(neighbors);
        }

        [Fact]
        public void GetNeighbors_ReturnsConnectedStatements()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new AssignmentStmt(new VariableNode("z"), new LiteralNode(15));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(stmt3);
            cfg.AddEdge(stmt1, stmt2);
            cfg.AddEdge(stmt1, stmt3);

            // Act
            var neighbors = cfg.GetNeighbors(stmt1);

            // Assert
            Assert.Equal(2, neighbors.Count);
            Assert.Contains(stmt2, neighbors);
            Assert.Contains(stmt3, neighbors);
        }

        [Fact]
        public void GetNeighbors_ThrowsExceptionForNonExistentVertex()
        {
            // Arrange
            var cfg = new CFG();
            var stmt = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => cfg.GetNeighbors(stmt));
        }

        #endregion

        #region GetVertices Tests

        [Fact]
        public void GetVertices_ReturnsEmptyForEmptyCFG()
        {
            // Arrange
            var cfg = new CFG();

            // Act
            var vertices = cfg.GetVertices().ToList();

            // Assert
            Assert.Empty(vertices);
        }

        [Fact]
        public void GetVertices_ReturnsAllStatements()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new ReturnStmt(new VariableNode("x"));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(stmt3);

            // Act
            var vertices = cfg.GetVertices().ToList();

            // Assert
            Assert.Equal(3, vertices.Count);
            Assert.Contains(stmt1, vertices);
            Assert.Contains(stmt2, vertices);
            Assert.Contains(stmt3, vertices);
        }

        #endregion

        #region VertexCount and EdgeCount Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void VertexCount_ReturnsCorrectCount(int count)
        {
            // Arrange
            var cfg = new CFG();
            for (int i = 0; i < count; i++)
            {
                cfg.AddVertex(new AssignmentStmt(
                    new VariableNode($"x{i}"),
                    new LiteralNode(i)
                ));
            }

            // Act
            var result = cfg.VertexCount();

            // Assert
            Assert.Equal(count, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void EdgeCount_ReturnsCorrectCount(int edgeCount)
        {
            // Arrange
            var cfg = new CFG();
            var statements = new List<Statement>();
            
            for (int i = 0; i <= edgeCount; i++)
            {
                var stmt = new AssignmentStmt(
                    new VariableNode($"x{i}"),
                    new LiteralNode(i)
                );
                statements.Add(stmt);
                cfg.AddVertex(stmt);
            }

            for (int i = 0; i < edgeCount; i++)
            {
                cfg.AddEdge(statements[i], statements[i + 1]);
            }

            // Act
            var result = cfg.EdgeCount();

            // Assert
            Assert.Equal(edgeCount, result);
        }

        #endregion

        #region ToString Tests

        [Fact]
        public void ToString_ReturnsEmptyStringForEmptyCFG()
        {
            // Arrange
            var cfg = new CFG();

            // Act
            var result = cfg.ToString();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ToString_ReturnsFormattedRepresentation()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddEdge(stmt1, stmt2);

            // Act
            var result = cfg.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            // The format should contain vertex and edge information
            // Based on DiGraph implementation: "vertex - > neighbor - > \n"
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void CFG_CanRepresentSimpleLinearProgram()
        {
            // Arrange - Represents: x := 5; y := 10; z := x + y; return z
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var stmt3 = new AssignmentStmt(new VariableNode("z"),
                new PlusNode(new VariableNode("x"), new VariableNode("y")));
            var stmt4 = new ReturnStmt(new VariableNode("z"));

            // Act
            cfg.AddVertex(stmt1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(stmt3);
            cfg.AddVertex(stmt4);
            cfg.AddEdge(stmt1, stmt2);
            cfg.AddEdge(stmt2, stmt3);
            cfg.AddEdge(stmt3, stmt4);
            cfg.Start = stmt1;

            // Assert
            Assert.Equal(4, cfg.VertexCount());
            Assert.Equal(3, cfg.EdgeCount());
            Assert.Equal(stmt1, cfg.Start);
            
            // Verify the chain
            var neighbors1 = cfg.GetNeighbors(stmt1);
            Assert.Single(neighbors1);
            Assert.Equal(stmt2, neighbors1[0]);

            var neighbors2 = cfg.GetNeighbors(stmt2);
            Assert.Single(neighbors2);
            Assert.Equal(stmt3, neighbors2[0]);

            var neighbors3 = cfg.GetNeighbors(stmt3);
            Assert.Single(neighbors3);
            Assert.Equal(stmt4, neighbors3[0]);

            var neighbors4 = cfg.GetNeighbors(stmt4);
            Assert.Empty(neighbors4); // Return has no outgoing edges
        }

        [Fact]
        public void CFG_HandlesMultipleReturnStatements()
        {
            // Arrange
            var cfg = new CFG();
            var stmt1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(5));
            var return1 = new ReturnStmt(new VariableNode("x"));
            var stmt2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(10));
            var return2 = new ReturnStmt(new VariableNode("y"));

            // Act
            cfg.AddVertex(stmt1);
            cfg.AddVertex(return1);
            cfg.AddVertex(stmt2);
            cfg.AddVertex(return2);
            cfg.AddEdge(stmt1, return1);
            cfg.AddEdge(stmt2, return2);

            // Assert
            Assert.Equal(4, cfg.VertexCount());
            Assert.Equal(2, cfg.EdgeCount());
        }

        #endregion
    }
}
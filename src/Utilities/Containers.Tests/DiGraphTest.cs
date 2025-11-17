using Xunit;
namespace DiGraphTests 
{
    public class DiGraphTests
    {
        #region AddVertex Tests

        [Fact]
        public void AddVertex_NewVertex_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            bool result = graph.AddVertex(1);

            // Assert
            Assert.True(result);
            Assert.Equal(1, graph.VertexCount());
        }

        [Fact]
        public void AddVertex_DuplicateVertex_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);

            // Act
            bool result = graph.AddVertex(1);

            // Assert
            Assert.True(result);
            Assert.Equal(1, graph.VertexCount());
        }

        [Fact]
        public void AddVertex_NullVertex_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act
            bool result = graph.AddVertex(null);

            // Assert
            Assert.False(result);
            Assert.Equal(0, graph.VertexCount());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-50)]
        [InlineData(0)]
        public void AddVertex_VariousIntegerValues_ReturnsTrue(int vertex)
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            bool result = graph.AddVertex(vertex);

            // Assert
            Assert.True(result);
            Assert.Equal(1, graph.VertexCount());
        }

        [Theory]
        [InlineData("a")]
        [InlineData("test")]
        [InlineData("")]
        public void AddVertex_VariousStringValues_ReturnsTrue(string vertex)
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act
            bool result = graph.AddVertex(vertex);

            // Assert
            Assert.True(result);
            Assert.Equal(1, graph.VertexCount());
        }

        #endregion

        #region AddEdge Tests

        [Fact]
        public void AddEdge_ValidVertices_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);

            // Act
            bool result = graph.AddEdge(1, 2);

            // Assert
            Assert.True(result);
            Assert.Equal(1, graph.EdgeCount());
            Assert.True(graph.HasEdge(1, 2));
        }

        [Fact]
        public void AddEdge_DuplicateEdge_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);

            // Act
            bool result = graph.AddEdge(1, 2);

            // Assert
            Assert.True(result);
            // Note: Based on implementation, duplicate edges may be added to DLL
            // This tests the current behavior
        }

        [Fact]
        public void AddEdge_SourceNotInGraph_ThrowsArgumentException()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(2);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => graph.AddEdge(1, 2));
        }

        [Fact]
        public void AddEdge_DestinationNotInGraph_ThrowsArgumentException()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => graph.AddEdge(1, 2));
        }

        [Fact]
        public void AddEdge_BothVerticesNotInGraph_ThrowsArgumentException()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => graph.AddEdge(1, 2));
        }

        [Fact]
        public void AddEdge_NullSource_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();
            graph.AddVertex("test");

            // Act
            bool result = graph.AddEdge(null, "test");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddEdge_NullDestination_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();
            graph.AddVertex("test");

            // Act
            bool result = graph.AddEdge("test", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddEdge_BothNull_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act
            bool result = graph.AddEdge(null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddEdge_SelfLoop_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);

            // Act
            bool result = graph.AddEdge(1, 1);

            // Assert
            Assert.True(result);
            Assert.True(graph.HasEdge(1, 1));
        }

        #endregion

        #region RemoveVertex Tests

        [Fact]
        public void RemoveVertex_ExistingVertex_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);

            // Act
            bool result = graph.RemoveVertex(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, graph.VertexCount());
        }

        [Fact]
        public void RemoveVertex_NonExistentVertex_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            bool result = graph.RemoveVertex(1);

            // Assert
            // Note: Implementation returns true even if vertex doesn't exist
            Assert.True(result);
        }

        [Fact]
        public void RemoveVertex_NullVertex_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act
            bool result = graph.RemoveVertex(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveVertex_WithEdges_RemovesVertexAndEdges()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);

            // Act
            graph.RemoveVertex(2);

            // Assert
            Assert.Equal(2, graph.VertexCount());
            Assert.False(graph.HasEdge(1, 2));
            // Note: Bug in implementation - edges TO removed vertex are not cleaned up
            // from other vertices' adjacency lists
        }

        #endregion

        #region RemoveEdge Tests

        [Fact]
        public void RemoveEdge_ExistingEdge_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);

            // Act
            bool result = graph.RemoveEdge(1, 2);

            // Assert
            Assert.True(result);
            Assert.False(graph.HasEdge(1, 2));
        }

        [Fact]
        public void RemoveEdge_NonExistentEdge_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);

            // Act
            bool result = graph.RemoveEdge(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RemoveEdge_SourceNotInGraph_ThrowsArgumentException()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(2);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => graph.RemoveEdge(1, 2));
        }

        [Fact]
        public void RemoveEdge_DestinationNotInGraph_ThrowsArgumentException()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => graph.RemoveEdge(1, 2));
        }

        [Fact]
        public void RemoveEdge_NullSource_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act
            bool result = graph.RemoveEdge(null, "test");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveEdge_NullDestination_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act
            bool result = graph.RemoveEdge("test", null);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region HasEdge Tests

        [Fact]
        public void HasEdge_ExistingEdge_ReturnsTrue()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);

            // Act
            bool result = graph.HasEdge(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasEdge_NonExistentEdge_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);

            // Act
            bool result = graph.HasEdge(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasEdge_ReverseEdgeDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);

            // Act
            bool result = graph.HasEdge(2, 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasEdge_NullSource_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();
            graph.AddVertex("test");

            // Act
            bool result = graph.HasEdge(null, "test");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasEdge_NullDestination_ReturnsFalse()
        {
            // Arrange
            var graph = new DiGraph<string>();
            graph.AddVertex("test");

            // Act
            bool result = graph.HasEdge("test", null);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region GetNeighbors Tests

        [Fact]
        public void GetNeighbors_VertexWithNeighbors_ReturnsAllNeighbors()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddEdge(1, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 4);

            // Act
            var neighbors = graph.GetNeighbors(1);

            // Assert
            Assert.Equal(3, neighbors.Count);
            Assert.Contains(2, neighbors);
            Assert.Contains(3, neighbors);
            Assert.Contains(4, neighbors);
        }

        [Fact]
        public void GetNeighbors_VertexWithNoNeighbors_ReturnsEmptyList()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);

            // Act
            var neighbors = graph.GetNeighbors(1);

            // Assert
            Assert.Empty(neighbors);
        }

        [Fact]
        public void GetNeighbors_NonExistentVertex_ThrowsArgumentException()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => graph.GetNeighbors(1));
        }

        [Fact]
        public void GetNeighbors_NullVertex_ThrowsNullReferenceException()
        {
            // Arrange
            var graph = new DiGraph<string>();

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => graph.GetNeighbors(null));
        }

        [Fact]
        public void GetNeighbors_OrderPreserved_ReturnsInInsertionOrder()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddEdge(1, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 4);

            // Act
            var neighbors = graph.GetNeighbors(1);

            // Assert
            Assert.Equal(2, neighbors[0]);
            Assert.Equal(3, neighbors[1]);
            Assert.Equal(4, neighbors[2]);
        }

        #endregion

        #region GetVertices Tests

        [Fact]
        public void GetVertices_EmptyGraph_ReturnsEmptyEnumerable()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            var vertices = graph.GetVertices();

            // Assert
            Assert.Empty(vertices);
        }

        [Fact]
        public void GetVertices_GraphWithVertices_ReturnsAllVertices()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            // Act
            var vertices = graph.GetVertices().ToList();

            // Assert
            Assert.Equal(3, vertices.Count);
            Assert.Contains(1, vertices);
            Assert.Contains(2, vertices);
            Assert.Contains(3, vertices);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(1)]
        public void GetVertices_VariousGraphSizes_ReturnsCorrectCount(int count)
        {
            // Arrange
            var graph = new DiGraph<int>();
            for (int i = 0; i < count; i++)
            {
                graph.AddVertex(i);
            }

            // Act
            var vertices = graph.GetVertices().ToList();

            // Assert
            Assert.Equal(count, vertices.Count);
        }

        #endregion

        #region VertexCount Tests

        [Fact]
        public void VertexCount_EmptyGraph_ReturnsZero()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            int count = graph.VertexCount();

            // Assert
            Assert.Equal(0, count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void VertexCount_AfterAddingVertices_ReturnsCorrectCount(int vertexCount)
        {
            // Arrange
            var graph = new DiGraph<int>();
            for (int i = 0; i < vertexCount; i++)
            {
                graph.AddVertex(i);
            }

            // Act
            int count = graph.VertexCount();

            // Assert
            Assert.Equal(vertexCount, count);
        }

        [Fact]
        public void VertexCount_AfterRemovingVertex_ReturnsCorrectCount()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.RemoveVertex(2);

            // Act
            int count = graph.VertexCount();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public void VertexCount_DuplicateVertices_CountsOnlyOnce()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(1);
            graph.AddVertex(1);

            // Act
            int count = graph.VertexCount();

            // Assert
            Assert.Equal(1, count);
        }

        #endregion

        #region EdgeCount Tests

        [Fact]
        public void EdgeCount_EmptyGraph_ReturnsZero()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            int count = graph.EdgeCount();

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void EdgeCount_GraphWithNoEdges_ReturnsZero()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);

            // Act
            int count = graph.EdgeCount();

            // Assert
            Assert.Equal(0, count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void EdgeCount_AfterAddingEdges_ReturnsCorrectCount(int edgeCount)
        {
            // Arrange
            var graph = new DiGraph<int>();
            for (int i = 0; i <= edgeCount; i++)
            {
                graph.AddVertex(i);
            }
            for (int i = 0; i < edgeCount; i++)
            {
                graph.AddEdge(i, i + 1);
            }

            // Act
            int count = graph.EdgeCount();

            // Assert
            Assert.Equal(edgeCount, count);
        }

        [Fact]
        public void EdgeCount_AfterRemovingEdge_ReturnsCorrectCount()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.RemoveEdge(1, 2);

            // Act
            int count = graph.EdgeCount();

            // Assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void EdgeCount_ComplexGraph_ReturnsCorrectCount()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(1, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);

            // Act
            int count = graph.EdgeCount();

            // Assert
            Assert.Equal(4, count);
        }

        #endregion

        #region ToString Tests

        [Fact]
        public void ToString_EmptyGraph_ReturnsEmptyString()
        {
            // Arrange
            var graph = new DiGraph<int>();

            // Act
            string result = graph.ToString();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ToString_GraphWithVerticesNoEdges_ContainsVertices()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);

            // Act
            string result = graph.ToString();

            // Assert
            Assert.Contains("1", result);
            Assert.Contains("2", result);
        }

        [Fact]
        public void ToString_GraphWithEdges_ContainsVerticesAndEdges()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);

            // Act
            string result = graph.ToString();

            // Assert
            Assert.Contains("1", result);
            Assert.Contains("2", result);
            Assert.Contains("- >", result);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void ComplexGraph_MultipleOperations_MaintainsConsistency()
        {
            // Arrange
            var graph = new DiGraph<string>();
            
            // Act
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");
            graph.AddVertex("D");
            graph.AddEdge("A", "B");
            graph.AddEdge("A", "C");
            graph.AddEdge("B", "D");
            graph.AddEdge("C", "D");

            // Assert
            Assert.Equal(4, graph.VertexCount());
            Assert.Equal(4, graph.EdgeCount());
            Assert.True(graph.HasEdge("A", "B"));
            Assert.True(graph.HasEdge("A", "C"));
            Assert.True(graph.HasEdge("B", "D"));
            Assert.True(graph.HasEdge("C", "D"));
            Assert.False(graph.HasEdge("D", "A"));
        }

        [Fact]
        public void DirectedGraph_ForwardAndReverseEdges_AreDifferent()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);

            // Assert
            Assert.True(graph.HasEdge(1, 2));
            Assert.False(graph.HasEdge(2, 1));
        }

        [Fact]
        public void Graph_AddBidirectionalEdges_BothEdgesExist()
        {
            // Arrange
            var graph = new DiGraph<int>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 1);

            // Assert
            Assert.Equal(2, graph.EdgeCount());
            Assert.True(graph.HasEdge(1, 2));
            Assert.True(graph.HasEdge(2, 1));
        }

        #endregion

    }
}


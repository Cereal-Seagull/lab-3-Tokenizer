using Xunit;
using AST;
using Optimizer;

namespace AST.Tests
{
    public class GenerateCFGTests
    {

        private ControlFlowGraphGeneratorVisitor visitor;

        public GenerateCFGTests()
        {
            visitor = new ControlFlowGraphGeneratorVisitor();
        }

        [Fact]
        public void GenerateCFG_EmptyBlock_ReturnsEmptyCFG()
        {
            // Arrange
            string program = 
            @"{
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);

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
            string program =
            @"{
                x := 5
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            // Verifies first statement is assigned as Start 
            Assert.Equal(block.Statements[0], cfg.Start);
            // Verifies first statement is AssignmentStmt
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
        }

        [Fact]
        public void GenerateCFG_SingleReturn_CreatesOneVertex()
        {
            // Arrange
            string program = 
            @"{
                return 42
            }";

            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            // Verifies first statement is assigned as Start
            Assert.Equal(block.Statements[0], cfg.Start);
            // Verifies first statement is ReturnStmt
            Assert.Equal(typeof(ReturnStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(ReturnStmt), cfg.Start.GetType());
        }

        [Fact]
        public void GenerateCFG_TwoSequentialAssignments_CreatesEdge()
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

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(1, cfg.EdgeCount());
            // First statement is start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            // AssignmentStmt 1 has edge to 2
            Assert.True(cfg.HasEdge(block.Statements[0], block.Statements[1]));
            // Verifies statements are AssignmentStmts
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), block.Statements[1].GetType());
        }

        [Fact]
        public void GenerateCFG_AssignmentThenReturn_CreatesEdge()
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

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            Assert.Equal(1, cfg.EdgeCount());
            Assert.Equal(block.Statements[0], cfg.Start);
            // First statement is an AssignmentStmt and is Start
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            // Edges exist
            Assert.True(cfg.HasEdge(block.Statements[0], block.Statements[1]));
        }

        [Fact]
        public void GenerateCFG_MultipleAssignments_CreatesLinearChain()
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

            // Assert
            Assert.Equal(6, cfg.VertexCount());
            Assert.Equal(5, cfg.EdgeCount());
            // First statement is AssignmentStmt and is Start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            
            // Verify chain connectivity
            for (int i = 0; i < 5; i++)
            {
                Assert.True(cfg.HasEdge(block.Statements[i], block.Statements[i + 1]));
            }
        }

        [Fact]
        public void GenerateCFG_ExampleFromPDF_Figure2()
        {
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

            // Assert
            Assert.Equal(4, cfg.VertexCount());
            Assert.Equal(3, cfg.EdgeCount());
            // First statement is AssignmentStmt and is Start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            
            // All edges exist
            for (int i = 0; i < 3; i++)
            {
                Assert.True(cfg.HasEdge(block.Statements[i], block.Statements[i + 1]));
            }
        }

        [Fact]
        public void GenerateCFG_NestedBlocks_FlattensCorrectly()
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

            // Assert
            Assert.Equal(5, cfg.VertexCount()); // Only non-block statements

            // First statement is AssignmentStmt and is Start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            // Correct edge count
            Assert.Equal(3, cfg.EdgeCount());

            // check edge (non-)existence, especially between return and z
        }

        [Fact]
        public void GenerateCFG_ReturnFollowedByStatement_NoEdgeFromReturn()
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

            // Assert
            Assert.Equal(3, cfg.VertexCount());
            // First statement is AssignmentStmt and is Start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            
            // Example: something like this but in multi-nest
            Assert.True(cfg.HasEdge(block.Statements[0], block.Statements[1]));
            Assert.False(cfg.HasEdge(block.Statements[1], block.Statements[2]));
        }

        [Fact]
        public void GenerateCFG_MultipleNestedBlocks_FlattensToStatements()
        {
            // Arrange
            string program = 
            @"{
                {
                    x := 1
                }
                {
                    y:= 2
                }
                z := 3
            }";
            
            var block = Parser.Parser.Parse(program);

            // Act
            var cfg = visitor.GenerateCFG(block);

            // Assert
            Assert.Equal(3, cfg.VertexCount());
            Assert.Equal(2, cfg.EdgeCount());

            // Start is AssignmentStmt
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
        }

        [Fact]
        public void GenerateCFG_OnlyNestedBlocks_FindsFirstNonBlockStatement()
        {
            // { { { x := 1 } } }
            
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

            // Assert
            Assert.Equal(1, cfg.VertexCount());
            Assert.Equal(0, cfg.EdgeCount());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
        }

        [Fact]
        public void GenerateCFG_MultipleReturns_FirstReturnHasNoOutgoingEdges()
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

            // Assert
            Assert.Equal(2, cfg.VertexCount());
            // First statement is ReturnStmt and is Start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(ReturnStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(ReturnStmt), cfg.Start.GetType());
            
            // No edge from returns
            Assert.False(cfg.HasEdge(block.Statements[0], block.Statements[1]));
        }

        [Fact]
        public void GenerateCFG_ComplexProgram_CorrectStructure()
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

            // Assert
            Assert.Equal(5, cfg.VertexCount());
            Assert.Equal(4, cfg.EdgeCount());
            // First statement is ReturnStmt and is Start
            Assert.Equal(block.Statements[0], cfg.Start);
            Assert.Equal(typeof(AssignmentStmt), block.Statements[0].GetType());
            Assert.Equal(typeof(AssignmentStmt), cfg.Start.GetType());
            // Edges exist
            for (int i = 0; i < 3; i++)
            {
                Assert.True(cfg.HasEdge(block.Statements[i], block.Statements[i+1]));
            }
        }

        [Fact]
        public void GenerateCFG_BlockWithOnlyBlocks_StartIsNull()
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

            // Assert
            Assert.Equal(0, cfg.VertexCount());
            Assert.Null(cfg.Start);
        }
    }
}
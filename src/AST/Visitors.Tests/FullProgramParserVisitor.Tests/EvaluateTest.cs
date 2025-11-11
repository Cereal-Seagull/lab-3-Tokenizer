using Xunit;
using AST;
using System;
using System.Data;

namespace AST.Tests
{


    public class EvaluateTests
    {
        private EvaluateVisitor visitor;

        public EvaluateTests()
        {
            visitor = new EvaluateVisitor();
        }

        #region Simple Assignment Tests

        [Fact]
        public void Evaluate_SimpleIntegerAssignment_ReturnsCorrectValue()
        {
            // x := 5
            string program =
            @"{
                x := 5
            }";

            var block = Parser.Parser.Parse(program);
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new LiteralNode(5)
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Evaluate_SimpleFloatAssignment_ReturnsCorrectValue()
        {
            // x := 3.14
            string program =
            @"{
                x := 3.14
            }";

            var block = Parser.Parser.Parse(program);
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new LiteralNode(3.14)
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal((float)3.14, result);
        }

        [Fact]
        public void Evaluate_MultipleAssignments_ReturnsLastValue()
        {
            // x := 5
            // y := 10
            string program =
            @"{
                x := 5
                y := 10
            }";

            var block = Parser.Parser.Parse(program);
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(10)));

            var result = visitor.Evaluate(block);

            Assert.Equal(10, result);
        }

        #endregion

        #region Arithmetic Operations Tests

        [Theory]
        [InlineData(5, 3, 8)]
        [InlineData(0, 0, 0)]
        // [InlineData(-5, 3, -2)]
        [InlineData(100, 200, 300)]
        public void Evaluate_Addition_ReturnsCorrectSum(int left, int right, int expected)
        {
            // x := left + right
            string program =
            $@"{{
                x := ({left} + {right})
            }}";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new PlusNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 3, 7)]
        [InlineData(0, 5, -5)]
        [InlineData(100, 100, 0)]
        public void Evaluate_Subtraction_ReturnsCorrectDifference(int left, int right, int expected)
        {
            // x := left - right
            string program =
            $@"{{
                x := ({left} - {right})
            }}";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new MinusNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 3, 15)]
        [InlineData(0, 100, 0)]
        // [InlineData(-4, 3, -12)]
        [InlineData(7, 7, 49)]
        public void Evaluate_Multiplication_ReturnsCorrectProduct(int left, int right, int expected)
        {
            // x := left * right
            string program =
            $@"{{
                x := ({left} * {right})
            }}";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new TimesNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 2, 5.0)]
        [InlineData(7, 2, 3.5)]
        [InlineData(0, 5, 0.0)]
        public void Evaluate_FloatDivision_ReturnsCorrectQuotient(int left, int right, float expected)
        {
            // x := left / right
            string program =
            $@"{{
                x := ({left} / {right})
            }}";
            
            var block = Parser.Parser.Parse(program);

            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new FloatDivNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 3, 3)]
        [InlineData(7, 2, 3)]
        [InlineData(15, 5, 3)]
        [InlineData(0, 5, 0)]
        public void Evaluate_IntegerDivision_ReturnsCorrectQuotient(int left, int right, int expected)
        {
            // x := left // right
            string program =
            $@"{{
                x := ({left} // {right})
            }}";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new IntDivNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 3, 1)]
        [InlineData(7, 2, 1)]
        [InlineData(15, 5, 0)]
        [InlineData(100, 7, 2)]
        public void Evaluate_Modulus_ReturnsCorrectRemainder(int left, int right, int expected)
        {
            // x := left % right
            string program =
            $@"{{
                x := ({left} % {right})
            }}";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new ModulusNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 3, 8.0)]
        [InlineData(5, 2, 25.0)]
        [InlineData(10, 0, 1.0)]
        [InlineData(3, 3, 27.0)]
        public void Evaluate_Exponentiation_ReturnsCorrectPower(int left, int right, double expected)
        {
            // x := left ** right
            string program =
            $@"{{
                x := ({left} ** {right})
            }}";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new ExponentiationNode(new LiteralNode(left), new LiteralNode(right))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(expected, result);
        }

        #endregion

        #region Complex Expression Tests

        [Fact]
        public void Evaluate_NestedArithmetic_ReturnsCorrectValue()
        {
            // x := ((5 + 3) * 2)
            string program =
            @"{
                x := ((5 + 3) * 2)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var innerExpr = new PlusNode(new LiteralNode(5), new LiteralNode(3));
            // var outerExpr = new TimesNode(innerExpr, new LiteralNode(2));
            // var assignment = new AssignmentStmt(new VariableNode("x"), outerExpr);
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(16, result);
        }

        [Fact]
        public void Evaluate_DeeplyNestedExpression_ReturnsCorrectValue()
        {
            // x := (((10 - 2) * 3) + 5)
            string program =
            @"{
                x := (((10 - 2) * 3) + 5)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var innermost = new MinusNode(new LiteralNode(10), new LiteralNode(2));
            // var middle = new TimesNode(innermost, new LiteralNode(3));
            // var outer = new PlusNode(middle, new LiteralNode(5));
            // var assignment = new AssignmentStmt(new VariableNode("x"), outer);
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal(29, result);
        }

        #endregion

        #region Variable Usage Tests

        [Fact]
        public void Evaluate_AssignmentUsingPreviousVariable_ReturnsCorrectValue()
        {
            // x := 5
            // y := x + 3
            string program =
            @"{
                x := (5)
                y := (x + 3)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));

            // var expr = new PlusNode(new VariableNode("x"), new LiteralNode(3));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), expr));

            var result = visitor.Evaluate(block);

            Assert.Equal(8, result);
        }

        [Fact]
        public void Evaluate_ChainedVariableAssignments_ReturnsCorrectValue()
        {
            // x := 10
            // y := x * 2
            // z := y - 5
            string program =
            @"{
                x := (10)
                y := (x * 2)
                z := (y - 5)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));

            // var expr1 = new TimesNode(new VariableNode("x"), new LiteralNode(2));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), expr1));

            // var expr2 = new MinusNode(new VariableNode("y"), new LiteralNode(5));
            // block.AddStatement(new AssignmentStmt(new VariableNode("z"), expr2));

            var result = visitor.Evaluate(block);

            Assert.Equal(15, result);
        }

        [Fact]
        public void Evaluate_VariableReassignment_ReturnsUpdatedValue()
        {
            // x := 5
            // x := x + 10
            string program =
            @"{
                x := (5)
                x := (x + 10)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));

            // var expr = new PlusNode(new VariableNode("x"), new LiteralNode(10));
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), expr));

            var result = visitor.Evaluate(block);

            Assert.Equal(15, result);
        }

        #endregion

        #region Return Statement Tests

        [Fact]
        public void Evaluate_SimpleReturn_ReturnsValue()
        {
            // return 42
            string program =
            @"{
                return (42)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var returnStmt = new ReturnStmt(new LiteralNode(42));
            // block.AddStatement(returnStmt);

            var result = visitor.Evaluate(block);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Evaluate_ReturnWithExpression_ReturnsEvaluatedValue()
        {
            // return 5 + 3
            string program =
            @"{
                return (5 + 3)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var expr = new PlusNode(new LiteralNode(5), new LiteralNode(3));
            // var returnStmt = new ReturnStmt(expr);
            // block.AddStatement(returnStmt);

            var result = visitor.Evaluate(block);

            Assert.Equal(8, result);
        }

        [Fact]
        public void Evaluate_ReturnWithVariable_ReturnsVariableValue()
        {
            // x := 100
            // return x
            string program =
            @"{
                x := (100)
                return (x)
            }";
            
            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(100)));
            // block.AddStatement(new ReturnStmt(new VariableNode("x")));

            var result = visitor.Evaluate(block);

            Assert.Equal(100, result);
        }

        [Fact]
        public void Evaluate_ReturnStopsExecution_IgnoresSubsequentStatements()
        {
            // x := 5
            // return x
            // x := 10  (should not execute)
            // Note: This test assumes return stops execution
            string program =
            @"{
                x := (5)
                return (x)
                x := (10)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new ReturnStmt(new VariableNode("x")));
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));

            var result = visitor.Evaluate(block);

            Assert.Equal(5, result);
        }

        #endregion

        #region Nested Block Tests

        [Fact]
        public void Evaluate_NestedBlock_EvaluatesInnerStatements()
        {
            // x := 5
            // {
            //     y := 10
            // }

            string program =
            @"{
                x := (5)
                {
                    y := (10)
                }
            }";
            
            var outerBlock = Parser.Parser.Parse(program);
            // var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            // outerBlock.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));

            // var innerBlock = new BlockStmt(new SymbolTable<string, object>());
            // innerBlock.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(10)));

            // outerBlock.AddStatement(innerBlock);

            var result = visitor.Evaluate(outerBlock);

            Assert.Equal(10, result);
        }

        [Fact]
        public void Evaluate_NestedBlockWithReturn_ReturnsFromInnerBlock()
        {
            // x := 5
            // {
            //     return 20
            // }
            // x := 100 (should not execute)
            string program =
            @"{
                x := (5)
                {
                    return (20)
                }
                x := (100)
            }";

            var outerBlock = Parser.Parser.Parse(program);
            
            // var outerBlock = new BlockStmt(new SymbolTable<string, object>());
            // outerBlock.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));

            // var innerBlock = new BlockStmt(new SymbolTable<string, object>());
            // innerBlock.AddStatement(new ReturnStmt(new LiteralNode(20)));

            // outerBlock.AddStatement(innerBlock);
            // outerBlock.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(100)));

            var result = visitor.Evaluate(outerBlock);

            Assert.Equal(20, result);
        }

        #endregion

        #region Edge Cases and Error Conditions

        [Fact]
        public void Evaluate_EmptyBlock_ReturnsNull()
        {
            // Empty block
            string program =
            @"{
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());

            var result = visitor.Evaluate(block);

            Assert.Null(result);
        }

        [Fact]
        public void Evaluate_DivisionByZero_ThrowsException()
        {
            // x := 10 / 0
            string program =
            @"{
                x := (10 / 0)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new FloatDivNode(new LiteralNode(10), new LiteralNode(0))
            // );
            // block.AddStatement(assignment);

            Assert.Throws<EvaluationException>(() => visitor.Evaluate(block));
        }

        [Fact]
        public void Evaluate_IntegerDivisionByZero_ThrowsException()
        {
            // x := 10 // 0
            string program =
            @"{
                x := (10 // 0)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new IntDivNode(new LiteralNode(10), new LiteralNode(0))
            // );
            // block.AddStatement(assignment);

            Assert.Throws<EvaluationException>(() => visitor.Evaluate(block));
        }

        [Fact]
        public void Evaluate_ModulusByZero_ThrowsException()
        {
            // x := 10 % 0
            string program =
            @"{
                x := (10 % 0)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new ModulusNode(new LiteralNode(10), new LiteralNode(0))
            // );
            // block.AddStatement(assignment);

            Assert.Throws<EvaluationException>(() => visitor.Evaluate(block));
        }

        [Fact]
        public void Evaluate_UndefinedVariable_ThrowsException()
        {
            // y := x (x not defined)
            string program =
            @"{
                y := (x)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("y"),
            //     new VariableNode("x")
            // );
            // block.AddStatement(assignment);

            Assert.Throws<EvaluationException>(() => visitor.Evaluate(block));
        }

        #endregion

        #region Mixed Type Operations

        [Fact]
        public void Evaluate_IntegerAndFloatAddition_ReturnsFloat()
        {
            // x := 5 + 3.5
            string program =
            @"{
                x := (5 + 3.5)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new PlusNode(new LiteralNode(5), new LiteralNode((float)3.5))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal((float)8.5, result);
        }

        [Fact]
        public void Evaluate_MixedTypeMultiplication_ReturnsFloat()
        {
            // x := 4 * 2.5
            string program =
            @"{
                x := (4 * 2.5)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new TimesNode(new LiteralNode(4), new LiteralNode((float)2.5))
            // );
            // block.AddStatement(assignment);

            var result = visitor.Evaluate(block);

            Assert.Equal((float)10.0, result);
        }

        #endregion
        
        #region Dr. Alvin's Complexity
        [Fact]
        public void TestComplexProgram()
        {
            // Test a more complex program that calculates factorial
            string program =
            @"{
                n := (5)
                result := (1)
                counter := (1)
                {
                    result := (result * counter)
                    counter := (counter + 1)
                    {
                        temp := (counter)
                    }
                    result := (result * counter)
                    counter := (counter + 1)
                    result := (result * counter)
                    counter := (counter + 1)
                    result := (result * counter)
                    counter := (counter + 1)
                    result := (result * counter)
                    counter := (counter + 1)
                    return (result)
                }
            }";

            BlockStmt ast = Parser.Parser.Parse(program);
            object result = visitor.Evaluate(ast);

            // This calculates 5! = 5*4*3*2*1 = 120
            Assert.Equal(120, result);
        }

        [Fact]
        public void TestDivisionByZero()
        {
            // Test division by zero exception
            string program = @"{
                return (10 / 0)
            }";

            BlockStmt ast = Parser.Parser.Parse(program);
        
            var exception = Assert.Throws<EvaluationException>(() => visitor.Evaluate(ast));
            Assert.Contains("cannot divide by 0", exception.Message);
        }
        #endregion
    }
}
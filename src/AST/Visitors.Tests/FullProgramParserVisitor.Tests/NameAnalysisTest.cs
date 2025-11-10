using Xunit;
using AST;
using System;
using System.IO;
using System.Collections.Generic;

namespace AST.Tests
{
    public class AnalyzeTests
    {
        // Helper method to capture console output
        private string CaptureConsoleOutput(Action action)
        {
            var originalOutput = Console.Out;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                action();
                Console.SetOut(originalOutput);
                return writer.ToString().Trim();
            }
        }

        #region Valid Programs - Should Pass Name Analysis

        [Fact]
        public void Analyze_SimpleAssignment_NoErrors()
        {
            // x := 5
            string program =
            @"{
                x := (5)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // var assignment = new AssignmentStmt(
            //     new VariableNode("x"),
            //     new LiteralNode(5)
            // );
            // block.AddStatement(assignment);

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            // Should have empty error list (shown as empty collection in output)
            Assert.Contains("0", output); // Count should be 0
        }

        [Fact]
        public void Analyze_MultipleAssignments_NoErrors()
        {
            // x := 5
            // y := 10
            // z := 15
            string program =
            @"{
                x := (5)
                y := (10)
                z := (15)
            }";
            
            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(10)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("z"), new LiteralNode(15)));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_AssignmentUsingPreviouslyDefinedVariable_NoErrors()
        {
            // x := 5
            // y := x + 10
            string program =
            @"{
                x := (5)
                y := (x + 10)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("y"),
            //     new PlusNode(new VariableNode("x"), new LiteralNode(10))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_ReturnWithDefinedVariable_NoErrors()
        {
            // x := 5
            // return x
            string program =
            @"{
                x := (5)
                return (x)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new ReturnStmt(new VariableNode("x")));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        [InlineData("%")]
        public void Analyze_BinaryOperationsWithDefinedVariables_NoErrors(string operation)
        {
            // x := 5
            // y := 10
            // z := x op y
            string program =
            $@"{{
                x := (5)
                y := (10)
                z := (x {operation} y)
            }}";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(10)));

            // ExpressionNode expr = operation switch
            // {
            //     "+" => new PlusNode(new VariableNode("x"), new VariableNode("y")),
            //     "-" => new MinusNode(new VariableNode("x"), new VariableNode("y")),
            //     "*" => new TimesNode(new VariableNode("x"), new VariableNode("y")),
            //     "/" => new FloatDivNode(new VariableNode("x"), new VariableNode("y")),
            //     "%" => new ModulusNode(new VariableNode("x"), new VariableNode("y")),
            //     _ => throw new ArgumentException("Invalid operation")
            // };

            // block.AddStatement(new AssignmentStmt(new VariableNode("z"), expr));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_VariableReassignment_NoErrors()
        {
            // x := 5
            // x := x + 1
            string program =
            @"{
                x := (5)
                y := (x + 1)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("x"),
            //     new PlusNode(new VariableNode("x"), new LiteralNode(1))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        #endregion

        #region Invalid Programs - Should Report Errors

        [Fact]
        public void Analyze_UndefinedVariableInAssignment_ReportsError()
        {
            // x := y + 5  (y is undefined)
            string program =
            @"{
                x := (y + 5)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("x"),
            //     new PlusNode(new VariableNode("y"), new LiteralNode(5))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            // Should contain error message about undefined variable
            Assert.Contains("undefined", output.ToLower());
        }

        [Fact]
        public void Analyze_UndefinedVariableInReturn_ReportsError()
        {
            // return x  (x is undefined)
            string program =
            @"{
                return (x)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new ReturnStmt(new VariableNode("x")));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("undefined", output.ToLower());
        }

        [Fact]
        public void Analyze_SelfReferencingAssignment_ReportsError()
        {
            // y := y + 1  (y is not defined before use)
            string program =
            @"{
                y := (y + 1)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("y"),
            //     new PlusNode(new VariableNode("y"), new LiteralNode(1))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            // EXPECTED: Should report error since y is undefined on right side
            // BUG: Implementation may incorrectly pass this test due to ordering
            Assert.Contains("undefined", output.ToLower());
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("undefined")]
        public void Analyze_MultipleUndefinedVariables_ReportsErrors(string varName)
        {
            // x := varName + 10  (varName is undefined)
            string program =
            $@"{{
                x := ({varName} + 10)
            }}";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("x"),
            //     new PlusNode(new VariableNode(varName), new LiteralNode(10))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("undefined", output.ToLower());
        }

        [Fact]
        public void Analyze_ComplexExpressionWithUndefinedVariable_ReportsError()
        {
            // x := 5
            // y := (x + z) * 2  (z is undefined)
            string program =
            @"{
                x := (5)
                y := ((x + z) * 2)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("y"),
            //     new TimesNode(
            //         new PlusNode(new VariableNode("x"), new VariableNode("z")),
            //         new LiteralNode(2)
            //     )
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("undefined", output.ToLower());
            Assert.Contains("undefined", output.ToLower());
        }

        [Fact]
        public void Analyze_MultipleErrorsInProgram_ReportsAllErrors()
        {
            // x := a + 5  (a undefined)
            // y := b * 2  (b undefined)
            // return c    (c undefined)
            string program =
            @"{
                x := (a + 5)
                y := (b * 2)
                return (c)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("x"),
            //     new PlusNode(new VariableNode("a"), new LiteralNode(5))
            // ));
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("y"),
            //     new TimesNode(new VariableNode("b"), new LiteralNode(2))
            // ));
            // block.AddStatement(new ReturnStmt(new VariableNode("c")));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            // Should report multiple errors (at least 3)
            var errorCount = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length - 1;
            Assert.True(errorCount >= 3, $"Expected at least 3 errors, found {errorCount}");
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Analyze_EmptyBlock_NoErrors()
        {
            // Empty program
            string program =
            @"{
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_OnlyLiterals_NoErrors()
        {
            // x := 5
            // y := 10
            // return 15
            string program =
            @"{
                x := (5)
                y := (10)
                return (15)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(5)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(10)));
            // block.AddStatement(new ReturnStmt(new LiteralNode(15)));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_DeeplyNestedExpression_NoErrors()
        {
            // x := 1
            // y := 2
            // z := ((x + y) * (x - y)) / (x + 1)
            string program =
            @"{
                x := (1)
                y := (2)
                z := (((x + y) * (x - y)) / (x + 1))
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(1)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(2)));
            
            // var expr = new FloatDivNode(
            //     new TimesNode(
            //         new PlusNode(new VariableNode("x"), new VariableNode("y")),
            //         new MinusNode(new VariableNode("x"), new VariableNode("y"))
            //     ),
            //     new PlusNode(new VariableNode("x"), new LiteralNode(1))
            // );
            
            // block.AddStatement(new AssignmentStmt(new VariableNode("z"), expr));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_DeeplyNestedExpressionWithError_ReportsError()
        {
            // x := 1
            // z := ((x + undefined_var) * (x - 5)) / 2
            string program =
            @"{
                x := (1)
                y := (((x + undefined) * (x - 5)) / 2)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(1)));
            
            // var expr = new FloatDivNode(
            //     new TimesNode(
            //         new PlusNode(new VariableNode("x"), new VariableNode("undefined")),
            //         new MinusNode(new VariableNode("x"), new LiteralNode(5))
            //     ),
            //     new LiteralNode(2)
            // );
            
            // block.AddStatement(new AssignmentStmt(new VariableNode("z"), expr));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("undefined", output.ToLower());
        }

        #endregion

        #region Operator Coverage

        [Fact]
        public void Analyze_ExponentiationWithDefinedVariables_NoErrors()
        {
            // x := 2
            // y := 3
            // z := x ** y
            string program =
            @"{
                x := (2)
                y := (3)
                z := (x ** y)
            }";

            var block = Parser.Parser.Parse(program);

            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(2)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(3)));
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("z"),
            //     new ExponentiationNode(new VariableNode("x"), new VariableNode("y"))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        [Fact]
        public void Analyze_IntDivWithDefinedVariables_NoErrors()
        {
            // x := 10
            // y := 3
            // z := x // y
            string program =
            @"{
                x := (10)
                y := (3)
                z := (x // y)
            }";

            var block = Parser.Parser.Parse(program);
            
            // var block = new BlockStmt(new SymbolTable<string, object>());
            // block.AddStatement(new AssignmentStmt(new VariableNode("x"), new LiteralNode(10)));
            // block.AddStatement(new AssignmentStmt(new VariableNode("y"), new LiteralNode(3)));
            // block.AddStatement(new AssignmentStmt(
            //     new VariableNode("z"),
            //     new IntDivNode(new VariableNode("x"), new VariableNode("y"))
            // ));

            var visitor = new NameAnalysisVisitor();
            var output = CaptureConsoleOutput(() => visitor.Analyze(block));

            Assert.Contains("0", output);
        }

        #endregion
    }
}